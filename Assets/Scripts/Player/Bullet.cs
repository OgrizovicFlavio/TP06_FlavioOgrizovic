using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletSO bulletSO;
    [SerializeField] private Rigidbody2D bulletRb2D;

    private float damage;
    private float damageMultiplier = 1f;

    void Start()
    {
        bulletRb2D.velocity = transform.right * bulletSO.speed;
        Destroy(gameObject, bulletSO.lifetime);
        damage = bulletSO.baseDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }

        if (Utilities.CheckLayerInMask(bulletSO.enemyLayerMask, other.gameObject.layer))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                float finalDamage = damage * damageMultiplier;
                enemy.TakeDamage(finalDamage);
            }
            Destroy(gameObject);
        }
        else if (Utilities.CheckLayerInMask(bulletSO.bossLayerMask, other.gameObject.layer))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                float finalDamage = damage * damageMultiplier;
                bossHealth.TakeDamage(finalDamage);
            }
            Destroy(gameObject);
        }
        else if (Utilities.CheckLayerInMask(bulletSO.worldLayerMask, other.gameObject.layer))
        {           
            Destroy(gameObject);
        }
    }
    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }
}
