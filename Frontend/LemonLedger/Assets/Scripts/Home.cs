using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    private TextMeshProUGUI balanceText;
    private TextMeshProUGUI loanText;

    void Awake()
    {
        // tries to find GameObjects called exactly "BalanceText" / "LoanText"
        var btGO = GameObject.Find("BalanceText");
        var ltGO = GameObject.Find("LoanText");

        if (btGO != null) balanceText = btGO.GetComponent<TextMeshProUGUI>();
        if (ltGO != null) loanText = ltGO.GetComponent<TextMeshProUGUI>();

        if (balanceText == null || loanText == null)
            Debug.LogError("Home: Could not auto-find TMP objects—check names or assign manually.");
    }

    void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        balanceText.text = GameData.Balance.ToString() + "XCR";
        loanText.text    = GameData.Loan.ToString() + "XCR";
    }

    /// <summary>
    /// Increases the runtime balance by `amount` and immediately updates the display.
    /// </summary>
    public void IncreaseBalance()
    {        
        RefreshUI();
    }

    public void OnReturnToMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
