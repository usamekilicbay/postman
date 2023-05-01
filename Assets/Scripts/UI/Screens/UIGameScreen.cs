using TMPro;
using UnityEngine;

public class UIGameScreen : UIScreenBase
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI remaningCardCountText;

    public void UpdateMoneyText(int money)
    {
        moneyText.SetText($"${money}");
    }

    public void UpdateRemaningCardCountText(int remaningCardCount)
    {
        remaningCardCountText.SetText($"Remaining Cards: {remaningCardCount}");
    }
}
