using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Canvas canvas;

    public EnemySO enemySO;
    private Rigidbody2D enemyRb2D;
    private Vector3 currentPoint;
    private bool isWaiting = false;

    void Start()
    {
        enemyRb2D = GetComponent<Rigidbody2D>();
        if (Random.value > 0.5f)
        {
            currentPoint = pointA.position;
        }
        else
        {
            currentPoint = pointB.position;
        }

        UpdateSpriteOrientation();
    }

    public void Patrol()
    {
        if (isWaiting)
        {
            return;
        }    

        Vector2 direction = (currentPoint - transform.position).normalized;
        enemyRb2D.velocity = new Vector2(direction.x * enemySO.movementSpeed, enemyRb2D.velocity.y);

        if (Vector2.Distance(transform.position, currentPoint) < 0.1f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    public void Stop()
    {
        enemyRb2D.velocity = Vector2.zero;
    }

    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        Stop();

        yield return new WaitForSeconds(enemySO.waitTime);

        if (currentPoint == pointA.position)
        {
            currentPoint = pointB.position;
        }
        else
        {
            currentPoint = pointA.position;
        }

        UpdateSpriteOrientation();
        isWaiting = false;
    }

    private void UpdateSpriteOrientation()
    {
        // Determinar la dirección actual del movimiento
        float direction = (currentPoint.x - transform.position.x);

        // Cambiar orientación del sprite según la dirección del movimiento
        if (direction < 0 && transform.localScale.x > 0 || direction > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        canvas.transform.localScale = new Vector3(-canvas.transform.localScale.x, canvas.transform.localScale.y, canvas.transform.localScale.z);
    }
}
