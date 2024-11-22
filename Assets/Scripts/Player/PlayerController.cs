using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private Image healthBar;
    [SerializeField] private PlayerState playerState;
    [SerializeField] private PlayerState lastPlayerState;

    private float health;
    private bool hasDoubleJumped = false;
    private bool isHurt = false;
    private bool isInvulnerable = false;
    private AudioManager audioManager;
    private PlayerMovement playerMovement;
    private Weapon weapon;
    private Coroutine powerUpCoroutine;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        playerMovement = GetComponent<PlayerMovement>();
        weapon = GetComponent<Weapon>();
        playerMovement.playerSO = playerSO;
        health = playerSO.maxHealth;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        playerMovement.Move(0);

        switch (playerState)
        {
            case PlayerState.Idle:
                HandleIdleState(moveInput);
                break;
            case PlayerState.Run:
                HandleRunState(moveInput);
                break;
            case PlayerState.Attack:
                HandleAttackState();
                break;
            case PlayerState.Jump:
                HandleJumpState(moveInput);
                break;
            case PlayerState.DoubleJump:
                HandleDoubleJumpState(moveInput);
                break;
            case PlayerState.Hurt:
                HandleHurtState(moveInput);
                break;
            case PlayerState.Die:
                HandleDieState();
                break;
            default:
                playerState = PlayerState.Idle;
                Debug.LogWarning("¡DEFAULT STATE!");
                break;
        }
    }

    private void HandleIdleState(float moveInput)
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 0);
            audioManager.Stop("PlayerRun");
            isHurt = false;
        }


        if (moveInput != 0)
        {
            playerState = PlayerState.Run;
        }
        else if (Input.GetButtonDown("Jump") && playerMovement.IsGrounded())
        {
            playerState = PlayerState.Jump;
            hasDoubleJumped = false;
        }
        else if (Input.GetButtonDown("Fire1") && playerMovement.IsGrounded())
        {
            playerState = PlayerState.Attack;
        }
    }

    private void HandleRunState(float moveInput)
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 1);
        }

        playerMovement.Move(moveInput);

        if (Mathf.Abs(moveInput) < 0.01f)
        {
            playerState = PlayerState.Idle;
        }
        else if (Input.GetButtonDown("Jump") && playerMovement.IsGrounded())
        {
            playerState = PlayerState.Jump;
            hasDoubleJumped = false;
        }

        if (playerState != PlayerState.Run)
        {
            audioManager.Stop("PlayerRun");
        }
    }

    private void HandleAttackState()
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 2);
            weapon.Shoot();
            Invoke(nameof(ChangeToIdle), 0.5f);
        }
    }

    private void HandleJumpState(float moveInput)
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 3);
            playerMovement.Jump();
        }

        playerMovement.Move(moveInput);

        if (playerMovement.IsGrounded())
        {
            if (moveInput != 0)
            {
                playerState = PlayerState.Run;
            }
            else
            {
                playerState = PlayerState.Idle;
            }
            hasDoubleJumped = false;
        }
        else if (Input.GetButtonDown("Jump") && !hasDoubleJumped)
        {
            playerState = PlayerState.DoubleJump;
            hasDoubleJumped = true;
        }
    }

    private void HandleDoubleJumpState(float moveInput)
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 4);
            playerMovement.DoubleJump();           
        }

        playerMovement.Move(moveInput);

        if (playerMovement.IsGrounded())
        {
            if (moveInput != 0)
            {
                playerState = PlayerState.Run;
            }
            else
            {
                playerState = PlayerState.Idle;
            }
            hasDoubleJumped = false;
        }
    }

    private void HandleHurtState(float moveInput)
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 5);
            if (!isHurt)
            {
                Invoke(nameof(ChangeToIdle), 0.5f);
                isHurt = true;
            }
        }
        playerMovement.Move(moveInput / 2);
    }

    private void HandleDieState()
    {
        if (playerState != lastPlayerState)
        {
            lastPlayerState = playerState;
            playerAnimator.SetInteger("State", 6);
        }
    }

    private void ChangeToIdle()
    {
        playerState = PlayerState.Idle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Utilities.CheckLayerInMask(playerSO.enemyLayerMask, other.gameObject.layer))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                Vector2 hitDirection = (transform.position - other.transform.position).normalized;
                TakeDamage(enemy.GetDamage());
            }
        }

        if (Utilities.CheckLayerInMask(playerSO.bossLayerMask, other.gameObject.layer))
        {
            BossMeleeAttack boss = other.gameObject.GetComponent<BossMeleeAttack>();

            if (boss != null)
            {
                Vector2 hitDirection = (transform.position - other.transform.position).normalized;
                TakeDamage(boss.GetDamage());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utilities.CheckLayerInMask(playerSO.moneyLayerMask, other.gameObject.layer))
        {
            Destroy(other.gameObject);
            moneyManager.moneyCounter++;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable)
        {
            return;
        }

        if (playerState != PlayerState.Hurt || !isHurt)
        {
            playerState = PlayerState.Hurt;
            audioManager.Stop("PlayerRun");
            audioManager.Play("PlayerHurt");
            health -= amount;
            healthBar.fillAmount = (health / 100);

            if (health < 1)
            {
                health = 0;
                Die();
            }
        }
    }

    public void Die()
    {
        audioManager.Stop("PlayerRun");
        audioManager.Play("PlayerDie");
        playerState = PlayerState.Die;
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);

        Time.timeScale = 0;
        FindObjectOfType<PauseManager>().GameOver();
    }

    public void Heal(float amount)
    {
        health += amount;
        healthBar.fillAmount = (health / 100);

        if (health > 100)
        {
            health = 100;
        }
    }

    public void BulletPowerUp(float duration)
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        powerUpCoroutine = StartCoroutine(PickUp(duration));
    }

    private IEnumerator PickUp(float duration)
    {
        if (weapon != null)
        {
            weapon.SetBulletScaleMultiplier(2f);
            weapon.SetBulletDamageMultiplier(2f);
        }

        yield return new WaitForSeconds(duration);

        if (weapon != null)
        {
            weapon.SetBulletScaleMultiplier(1f);
            weapon.SetBulletDamageMultiplier(1f);
        }

        powerUpCoroutine = null;
    }

    public void ActivateInvulnerability(float duration)
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        powerUpCoroutine = StartCoroutine(InvulnerabilityEffect(duration));
    }

    private IEnumerator InvulnerabilityEffect(float duration)
    {
        isInvulnerable = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.cyan;
        }

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        powerUpCoroutine = null;
    }

    public void PlayRunSound()
    {
        audioManager.Play("PlayerRun");
    }    
}

