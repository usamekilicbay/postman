using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int StartMoney = 1000;

    public static int CurrentMoney {get;private set;}

    private void Awake()
    {
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
        return true;
    }
}
