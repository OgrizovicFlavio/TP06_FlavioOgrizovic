using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    public float minY;
    public float maxY;
    public float minX;
    public float maxX;

    private void Update()
    {
        float targetX = Mathf.Clamp(target.position.x, minX, maxX);
        float targetY = Mathf.Clamp(target.position.y + yOffset, minY, maxY);
        Vector3 newPos = new Vector3(targetX, targetY, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
