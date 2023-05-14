using UnityEngine;
using Zenject;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int startMoney = 100000;

    public static int CurrentMoney { get; private set; }

    private UIGameScreen _uiGameScreen;

    [Inject]
    public void Construct(UIGameScreen uiGameScreen)
    {
        _uiGameScreen = uiGameScreen;
        CurrentMoney = startMoney;
    }

    public bool BuyItem(int amount)
    {
        if (CurrentMoney < amount)
        {
            Debug.LogWarning($"Not enough money! Required: ${amount}, Owned: ${CurrentMoney}");
            return false;
        }

        CurrentMoney -= amount;
        _uiGameScreen.UpdateMoneyText(CurrentMoney);

        return true;
    }

    public bool SellItem(int amount)
    {
        Debug.LogWarning($"Item sold for: ${amount}, Owned: ${CurrentMoney}");

        CurrentMoney += amount;
        _uiGameScreen.UpdateMoneyText(CurrentMoney);

        return true;
    }
}
