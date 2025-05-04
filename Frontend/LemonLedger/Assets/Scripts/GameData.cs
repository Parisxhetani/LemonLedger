using UnityEngine;

public static class GameData 
{
    public static float Balance = 0;
    public static float Loan = 300;

    // New for daily summary:
    public static float DailyEarnings = 0f;
    public static float DailyExpenses = 0f;
    public static float DailyProfit   = 0f;
    public static int   DayCount      = 1;

    public static void AddEarnings(int amount)
        => DailyEarnings += amount;
    public static void AddExpense(int amount)
        => DailyExpenses += amount;
}
