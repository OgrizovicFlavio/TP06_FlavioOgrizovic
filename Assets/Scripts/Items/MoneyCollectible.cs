using UnityEngine;

public class MoneyCollectible : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private GameObject pickUpEffect;

    void Start()
    {
        //audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utilities.CheckLayerInMask(playerLayerMask, other.gameObject.layer))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Instantiate(pickUpEffect, transform.position, transform.rotation);
                //audioManager.Play("PowerUp");             
            }
        }
    }
}
