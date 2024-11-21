using UnityEngine;

public class BossTarget : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Canvas canvas;

    private bool isFlipped = false;

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            canvas.transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            canvas.transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
}
