using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyCounterText;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Collider2D doorCollider2D;

    private AudioManager audioManager;
    public int moneyCounter;
    private bool isDoorOpen = false;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        moneyCounterText.text = ": " + moneyCounter.ToString();

        if (moneyCounter >= 10 && !isDoorOpen)
        {           
            moneyCounter -= 10;
            OpenBossGate();
        }
    }

    private void OpenBossGate()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenBossGate");
            audioManager.Play("BossGate");

            StartCoroutine(StopSoundAfterDelay(0.4f));

            audioManager.PlayBossMusic();
        }

        if (doorCollider2D != null)
        {
            doorCollider2D.enabled = false;
        }

        isDoorOpen = true;
    }

    private IEnumerator StopSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioManager.Stop("BossGate");

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("IdleBossGate");
        }
    }

}
