using UnityEngine;

public enum ActionType
{
    LightInsult,
    HeavyInsult,
    Ignore,
}

public class FighterBase : MonoBehaviour
{
    [SerializeField] private FigtherConfig _figtherConfig;

    private BattleManager _battleManager;

    private int _honor;
    private int _stamina;

    private void Awake()
    {
        _honor = _figtherConfig.MaxHonor;
        _stamina = _figtherConfig.MaxStamina;
    }

    public void TakeTurn()
    {
        _stamina += _figtherConfig.StaminaRecoverAmount;
    }

    public void Execute(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.LightInsult:
                _stamina -= _figtherConfig.LightInsultRequiredStamina;
                _battleManager.Insult(this, _figtherConfig.LightInsultDamage, _figtherConfig.LightInsultText);  
                break;
            case ActionType.HeavyInsult:
                _stamina -= _figtherConfig.HeavyInsultRequiredStamina;
                _battleManager.Insult(this, _figtherConfig.HeavyInsultDamage, _figtherConfig.HeavyInsultText);  
                break;
            case ActionType.Ignore:
                _honor += _figtherConfig.HonorRecoverAmount;
                break;
        }
    }

    public void GetOffend(int insultDamage)
    {
        _honor -= insultDamage;

        if (_honor < 0)
        {
            // TODO: Battlee Over
        }
    }
}
