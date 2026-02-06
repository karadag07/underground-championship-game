using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI goldText;
    public Button startButton;
    public Button upgradesButton;
    public Button resetButton;
    public Button quitButton;

    [Header("Panels")]
    public GameObject resetPanel;

    void Start()
    {
        Debug.Log("✅ Menu Controller Start!");

        // Buton listener'ları
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (upgradesButton != null)
            upgradesButton.onClick.AddListener(OnUpgradesClicked);

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        // Reset panelini kapat
        if (resetPanel != null)
            resetPanel.SetActive(false);

        // Altını güncelle
        UpdateGoldDisplay();
    }

    void Update()
    {
        // Her frame altını güncelle
        UpdateGoldDisplay();
    }

    // ============================================
    // BUTON FONKSİYONLARI
    // ============================================

    void OnStartClicked()
    {
        Debug.Log("🎮 START clicked!");
        SceneManager.LoadScene("Map Scene");
    }

    void OnUpgradesClicked()
    {
        Debug.Log("⬆️ UPGRADES clicked!");
        SceneManager.LoadScene("UpgradeScene");
    }

    void OnResetClicked()
    {
        Debug.Log("🗑️ RESET clicked!");
        if (resetPanel != null)
            resetPanel.SetActive(true);
    }

    void OnQuitClicked()
    {
        Debug.Log("👋 QUIT clicked!");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // ============================================
    // RESET ONAY SİSTEMİ
    // ============================================

    public void ConfirmReset()
    {
        Debug.Log("✅ Reset confirmed!");

        // GameManager'dan reset
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetProgress();
        }

        // 🔥 ALTINI SIFIRLA
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetCurrency();
        }

        // Panel'i kapat
        if (resetPanel != null)
            resetPanel.SetActive(false);

        // Altını güncelle
        UpdateGoldDisplay();

        Debug.Log("🎉 Game reset complete!");
    }

    public void CancelReset()
    {
        Debug.Log("❌ Reset cancelled!");
        if (resetPanel != null)
            resetPanel.SetActive(false);
    }

    // ============================================
    // ALTIN GÖSTERME
    // ============================================

    void UpdateGoldDisplay()
    {
        if (goldText == null)
            return;

        // 🔥 ARTIK GERÇEK ALTINI GÖSTER
        if (CurrencyManager.Instance != null)
        {
            int gold = CurrencyManager.Instance.GetGold();
            goldText.text = $"{gold} G";
        }
        else
        {
            goldText.text = "0 G";
        }
    }
}