using UnityEngine;

[CreateAssetMenu(fileName = "Fighter Config", menuName = "Fighter Configs/New Fighter Config")]
public class FigtherConfig : ScriptableObject
{
    public int LightInsultDamage;
    public int HeavyInsultDamage;
    [Space(10)]
    public int HonorRecoverAmount;
    public int StaminaRecoverAmount;
    [Space(10)]
    public int LightInsultRequiredStamina;
    public int HeavyInsultRequiredStamina;
    [Space(10)]
    public int CriticalDamageChance;
    public int IgnoreChance;
    [Space(10)]
    public int MaxStamina;
    public int MaxHonor;
    [Space(20)]
    [Header("Insults")]
    public string LightInsultText;
    public string HeavyInsultText;
}
