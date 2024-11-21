using UnityEngine;

public class BossWalkAttack : StateMachineBehaviour
{
    [SerializeField] private BossSO bossSO;

    private BossTarget bossTarget;
    private Transform player;
    private Rigidbody2D bossRb2D;
    private AudioManager audioManager;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (Utilities.CheckLayerInMask(bossSO.playerLayerMask, obj.layer) && obj.transform.parent == null)
            {
                player = obj.transform;
                break;
            }
        }
        bossRb2D = animator.GetComponent<Rigidbody2D>();
        bossTarget = animator.GetComponent<BossTarget>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null || !animator.GetBool("IsMoving"))
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        bossTarget.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, bossRb2D.position.y);
        Vector2 newPosition = Vector2.MoveTowards(bossRb2D.position, target, bossSO.movementSpeed * Time.deltaTime);
        bossRb2D.MovePosition(newPosition);

        if (Vector2.Distance(player.position, bossRb2D.position) <= bossSO.meleeRange)
        {
            animator.SetTrigger("MeleeAttack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("MeleeAttack");
        animator.SetBool("IsMoving", false);
        audioManager.Stop("BossWalk");
    }
}
