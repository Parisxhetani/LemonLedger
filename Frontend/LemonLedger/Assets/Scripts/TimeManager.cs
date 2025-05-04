using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    private int gameUnits = 0;

    [Header("Canvas Background Swap")]
    public Image  canvasBgImage;
    public Sprite morningBackground;
    public Sprite noonBackground;
    public Sprite nightBackground;

    [Header("TMP Texts to Recolor")]
    public TextMeshProUGUI[] uiTexts;

    [Header("Night Summary Panel")]
    [Tooltip("Drag your NightPanel GameObject here")]
    public GameObject nightPanel;

    private int currentBgIndex = 0;
    private int day  => GameData.DayCount;
    private float earn=> GameData.DailyEarnings;
    private float spend=> GameData.DailyExpenses;

    private TextMeshProUGUI balanceText;
    private TextMeshProUGUI loanText;

    void Start()
    {
        // ensure hidden at start
        if (nightPanel != null) nightPanel.SetActive(false);

        InvokeRepeating(nameof(GameTick),         30f, 30f);
        InvokeRepeating(nameof(ChangeBackground), 10f, 10f);
    }

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

    void GameTick()
    {
        gameUnits++;
        Debug.Log($"[GameTime] unit = {gameUnits}");
    }

/// <summary>
    /// Call this once when night arrives to roll earnings, expenses and profit.
    /// </summary>
    private void GenerateDailySummary()
    {
        // 1) Roll a random earning between 50 and 100
        float earnings = Random.Range(50f, 100f);

        // 2) (Optional) round to nearest .5
        earnings = Mathf.Round(earnings * 2f) / 2f;

        // 3) Calculate expenses (25%)
        float expenses = earnings * 0.25f;
        expenses = Mathf.Round(expenses * 2f) / 2f;  // optional

        // 4) Calculate profit
        float profit = earnings - expenses;
        profit = Mathf.Round(profit * 2f) / 2f;      // optional

        // 5) Store for your popup
        GameData.DailyEarnings = earnings;
        GameData.DailyExpenses = expenses;
        GameData.DailyProfit   = profit;
    }

    void ChangeBackground()
    {
        // advance 0→1→2→0…
        currentBgIndex = (currentBgIndex + 1) % 3;

        // swap sprite
        switch (currentBgIndex)
        {
            case 0: canvasBgImage.sprite = morningBackground; break;
            case 1: canvasBgImage.sprite = noonBackground;    break;
            case 2: canvasBgImage.sprite = nightBackground;   break;
        }

        // recolor texts
        Color col = (currentBgIndex == 2) ? Color.white : Color.black;
        foreach (var t in uiTexts)
            if (t != null) t.color = col;

        // **SHOW/HIDE NIGHT PANEL**
        if (nightPanel != null)
            nightPanel.SetActive(currentBgIndex == 2);

        // optionally, populate your panel’s texts when it appears
        if (currentBgIndex == 2)
        {
            // generate fresh numbers
            GenerateDailySummary();

            // then show your panel & populate it
            if (nightPanel != null)
            {
                PopulateNightPanel();
                nightPanel.SetActive(true);
            }
        }
        else if (nightPanel != null)
        {
            nightPanel.SetActive(false);
        }
    }

    void PopulateNightPanel()
    {
        // assume you have references via child look-ups, e.g.:
        // DayNr, Income, Expense, Profit under nightPanel
        Transform np = nightPanel.transform;
        np.Find("DayNr").GetComponent<TextMeshProUGUI>().text     = $"{day}";
        np.Find("Income").GetComponent<TextMeshProUGUI>().text    = $"{earn}XCR";
        np.Find("Expense").GetComponent<TextMeshProUGUI>().text   = $"{spend}XCR";
        np.Find("Profit").GetComponent<TextMeshProUGUI>().text    = $"{earn - spend}XCR";

        // Add it to the balance and refresh the on-screen text        
        GameData.Balance += earn;
         balanceText.text = GameData.Balance.ToString() + "XCR";
        loanText.text    = GameData.Loan.ToString() + "XCR";
    }
}
