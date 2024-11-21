using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem smokeParticlesPrefab;
    [SerializeField] private float timeToDestroy = 1f;

    private void Awake()
    {
        EnemyController.onEnemyDeath += Enemy_onEnemyDie;
    }

    private void OnDestroy()
    {
        EnemyController.onEnemyDeath -= Enemy_onEnemyDie;
    }

    private void Enemy_onEnemyDie(Vector3 pos)
    {
        ParticleSystem smokeParticles = Instantiate(smokeParticlesPrefab, pos, Quaternion.identity);
        smokeParticles.Play();
        StartCoroutine(DestroyParticle(smokeParticles));
    }

    private IEnumerator DestroyParticle(ParticleSystem smokeParticles)
    {
        yield return new WaitForSeconds(timeToDestroy);

        Destroy(smokeParticles.gameObject);
    }
}
