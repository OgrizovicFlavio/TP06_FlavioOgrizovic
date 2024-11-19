using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float parallaxEffect;

    private float startPosition;
    private float length;

    private void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update()
    {
        float distance = mainCamera.transform.position.x * parallaxEffect;
        float movement = mainCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (movement > startPosition + length)
        {
            startPosition += length;
        }
        else if (movement < startPosition - length)
        {
            startPosition -= length;
        }
    }
}
