using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public static event Action<Vector3> onEnemyDeath;

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Image healthBar;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private EnemyState lastEnemyState;

    private AudioManager audioManager;
    private EnemyPatrol enemyPatrol;
    private Animator enemyAnimator;
    private float health;
    private float damage;
    private bool isDying = false;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyAnimator = GetComponent<Animator>();
        health = enemySO.maxHealth;
        damage = enemySO.baseDamage;
        Invoke(nameof(StartPatrol), 1f);
    }

    private void Update()
    {
        if (Time.timeScale == 0 || isDying)
        {
            return;
        }

        switch (enemyState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Patrol:
                HandlePatrolState();
                break;
            case EnemyState.Hurt:
                HandleHurtState();
                break;
            case EnemyState.Die:
                break;
            default:
                enemyState = EnemyState.Idle;
                Debug.LogWarning("¡DEFAULT STATE!");
                break;
        }
    }

    private void StartPatrol() 
    { 
        enemyState = EnemyState.Patrol; 
    }

    private void HandleIdleState()
    {
        if (enemyState != lastEnemyState)
        {
            lastEnemyState = enemyState;
            enemyAnimator.SetInteger("State", 0);
            enemyPatrol.Stop();
        }
    }

    private void HandlePatrolState()
    {
        if (enemyState != lastEnemyState)
        {
            lastEnemyState = enemyState;
            enemyAnimator.SetInteger("State", 1);
        }
        enemyPatrol.Patrol();
    }

    private void HandleHurtState()
    {
        if (enemyState != lastEnemyState)
        {
            lastEnemyState = enemyState;
            enemyAnimator.SetInteger("State", 2);            
            Invoke(nameof(StartPatrol), 0.5f);
        }
        enemyPatrol.Patrol();
    }

    private void HandleDieState()
    {
        if (enemyState != lastEnemyState)
        {
            lastEnemyState = enemyState;
            enemyAnimator.SetInteger("State", 3);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDying)
        {
            return;
        }

        enemyState = EnemyState.Hurt;
        audioManager.Play("EnemyHurt");
        health -= amount;
        healthBar.fillAmount = (health / 100);

        if (health < 1)
        {
            health = 0;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDying = true;
        audioManager.Play("EnemyDie");
        enemyState = EnemyState.Die;
        HandleDieState();
        onEnemyDeath?.Invoke(transform.position);

        yield return new WaitForSeconds(enemyAnimator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}
