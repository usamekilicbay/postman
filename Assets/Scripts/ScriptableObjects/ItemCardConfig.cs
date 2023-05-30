using UnityEngine;

[CreateAssetMenu(fileName = "New Item Card Config", menuName = "Configs/Item Card Config")]
public class ItemCardConfig : ScriptableObject
{
    [SerializeField] private string id;
    public string Id { get { return id; } }

    public Sprite Artwork;
    public string Name;
    public string Description;
    public int Price;
    public Rarity Rarity;
    public float Weight;

    private void OnEnable()
    {
        // Generate a new ID when the ScriptableObject is created for the first time
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
            // Make the ID immutable
            hideFlags = HideFlags.NotEditable;
        }
    }
}
