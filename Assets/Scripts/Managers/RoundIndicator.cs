using UnityEngine;
using UnityEngine.UI;

public class RoundIndicator : MonoBehaviour
{
    [Header("Round Noktaları")]
    public Image round1Indicator; // Soldan 1. nokta
    public Image round2Indicator; // Soldan 2. nokta
    public Image round3Indicator; // Soldan 3. nokta

    [Header("Renkler")]
    public Color activeColor = new Color(1f, 0.5f, 0f); // Turuncu
    public Color inactiveColor = Color.gray; // Gri

    void Start()
    {
        UpdateIndicators();
    }

    void Update()
    {
        UpdateIndicators();
    }

    void UpdateIndicators()
    {
        if (GameManager.Instance == null)
            return;

        int currentRound = GameManager.Instance.currentRound;

        // Round 1
        if (round1Indicator != null)
        {
            round1Indicator.color = (currentRound >= 1) ? activeColor : inactiveColor;
        }

        // Round 2
        if (round2Indicator != null)
        {
            round2Indicator.color = (currentRound >= 2) ? activeColor : inactiveColor;
        }

        // Round 3
        if (round3Indicator != null)
        {
            round3Indicator.color = (currentRound >= 3) ? activeColor : inactiveColor;
        }
    }
}