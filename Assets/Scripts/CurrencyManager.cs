using UnityEngine;
using Zenject;

public class CurrencyManager : MonoBehaviour
{
    private int StartMoney = 1000;

    public static int CurrentMoney {get;private set;}

    private UIGameScreen _uiGameScreen;

    [Inject]
    public void Construct(UIGameScreen uiGameScreen)
    {
        _uiGameScreen = uiGameScreen;
        CurrentMoney = StartMoney;
    }

    public bool BuyItem(int amount)
    {
        if (CurrentMoney < amount)
        {
            Debug.LogWarning($"Not enough money! Required: {amount}, Owned: {CurrentMoney}");
            return false;
        }

        CurrentMoney -= amount;
        _uiGameScreen.UpdateMoneyText(CurrentMoney);
        return true;
    }
}
