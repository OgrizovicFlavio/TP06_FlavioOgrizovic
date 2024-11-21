using UnityEngine;

public class BossIdle : StateMachineBehaviour
{
    [SerializeField] private BossSO bossSO;

    private Transform player;
    private Rigidbody2D bossRb2D;

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
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) 
        {
            animator.SetBool("IsMoving", false);
            return; 
        }

        float distanceToPlayer = Vector2.Distance(bossRb2D.position, player.position);
        bool playerDetected = distanceToPlayer <= bossSO.sightRange;
        animator.SetBool("IsMoving", playerDetected);
    }
}
