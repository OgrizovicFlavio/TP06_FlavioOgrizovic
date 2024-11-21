using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private BossSO bossSO;
    [SerializeField] private Image healthBar;
    [SerializeField] private ParticleSystem smokeParticlesPrefab;

    private AudioManager audioManager;
    private Animator bossAnimator;
    private float hurtStateDuration = 0.5f;
    private float timeToDestroy = 1f;
    private float health;
    private bool isDying = false;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        bossAnimator = GetComponent<Animator>();
        health = bossSO.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDying)
        {
            return;
        }

        audioManager.Stop("BossWalk");
        audioManager.Play("BossHurt");
        health -= amount;
        healthBar.fillAmount = (health / 200);
        bossAnimator.SetBool("IsHurt", true);

        StartCoroutine(ExitHurtState());
        if (health < 1)
        {
            health = 0;
            StartCoroutine(Die());
        }
    }

    private IEnumerator ExitHurtState()
    {
        yield return new WaitForSeconds(hurtStateDuration);
        bossAnimator.SetBool("IsHurt", false);
    }

    private IEnumerator Die()
    {
        isDying = true;
        bossAnimator.SetBool("IsHurt", false);
        bossAnimator.SetTrigger("Die");
        audioManager.Stop("BossWalk");
        audioManager.Play("BossDie");
        ParticleSystem smokeParticles = Instantiate(smokeParticlesPrefab, transform.position, Quaternion.identity);
        smokeParticles.Play();
        StartCoroutine(DestroyParticle(smokeParticles));

        yield return new WaitForSeconds(bossAnimator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    private IEnumerator DestroyParticle(ParticleSystem smokeParticles)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(smokeParticles.gameObject);
    }
}
