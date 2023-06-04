using Merchant.Card;
using UnityEngine;

namespace Merchant.Config
{
    [CreateAssetMenu(fileName = "New Item Card Config", menuName = "Configs/Item Card Config")]
    public class ItemCardConfig : ScriptableObject
    {
        [ReadOnly]
        public string Id;

        [SerializeField]
        private Sprite artwork;

        [SerializeField]
        private string name;

        [SerializeField]
        private string description;

        [SerializeField]
        private int price;

        [SerializeField]
        private Rarity rarity;

        [SerializeField]
        private float weight;

        public Sprite Artwork { get { return artwork; } }
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public int Price { get { return price; } }
        public Rarity Rarity { get { return rarity; } }
        public float Weight { get { return weight; } }

        private void OnEnable()
        {
            // Generate a new ID when the ScriptableObject is created for the first time
            if (string.IsNullOrEmpty(Id))
                Id = System.Guid.NewGuid().ToString();
        }
    }
}
