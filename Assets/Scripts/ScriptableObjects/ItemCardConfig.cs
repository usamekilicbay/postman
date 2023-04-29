using UnityEngine;

[CreateAssetMenu(fileName = "New Item Card Config", menuName = "Configs/Item Card Config")]
public class ItemCardConfig : ScriptableObject
{
    public Sprite Artwork;
    public string Name;
    public string Description;
    public string Price;
    public Rarity Rarity;
    public float Weight;
}
