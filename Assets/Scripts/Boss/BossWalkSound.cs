using UnityEngine;

public class BossWalkSound : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void PlayWalkSound()
    {
        audioManager.Play("BossWalk");
    }
}
