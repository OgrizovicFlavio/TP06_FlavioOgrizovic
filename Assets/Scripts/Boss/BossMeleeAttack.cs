using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    [SerializeField] private BossSO bossSO;

    private AudioManager audioManager;
    private Vector3 attackOffset;
    private float attackDamage;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        attackDamage = bossSO.attackDamage;
    }
    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        audioManager.Play("BossAttack");

        Collider2D colInfo = Physics2D.OverlapCircle(pos, bossSO.meleeRange, bossSO.playerLayerMask);
        if (colInfo != null)
        {
            if (Utilities.CheckLayerInMask(bossSO.playerLayerMask, colInfo.gameObject.layer))
            {
                PlayerController player = colInfo.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(bossSO.attackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, bossSO.meleeRange);
    }

    public float GetDamage()
    {
        return attackDamage;
    }
}
