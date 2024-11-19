using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyCounterText;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Collider2D doorCollider;

    public int moneyCounter;

    private void Update()
    {
        moneyCounterText.text = ": " + moneyCounter.ToString();

        if (moneyCounter >= 10)
        {
            OpenBossGate();
        }
    }

    private void OpenBossGate()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenBossGate");
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }
}
