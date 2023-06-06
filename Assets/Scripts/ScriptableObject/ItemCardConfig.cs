using Merchant.Card;
using System.Collections.Generic;
using UnityEngine;

namespace Merchant.Config.Item
{
    [CreateAssetMenu(fileName = "New Item Card Config", menuName = "Configs/Item Card Config")]
    public class ItemCardConfig : ScriptableObject
    {
        [ReadOnly] public string Id;

        [SerializeField] private Sprite artwork;

        [SerializeField] private string name;

        [SerializeField] private string description;

        [SerializeField] private int price;

        [SerializeField] private int value;
        
        [SerializeField] private bool[] acquireMethods;

        [SerializeField] private Rarity rarity;

        [SerializeField] private float weight;

        [SerializeField] private Dictionary<int, string> curseList;

        [SerializeField] private Dictionary<int, string> blessList;

        [SerializeField, Range(0f, 100f)] private float cursePossibility;

        [SerializeField, Range(0f, 100f)] private float blessPossibility;

        public Sprite Artwork => artwork;
        public string Name => name;
        public string Description => description;
        public int Value => value;
        public int Price => price;
        public Rarity Rarity => rarity;
        public float Weight => weight;
        public bool[] AcquireMethods => acquireMethods;

        private void OnEnable()
        {
            // Generate a new ID when the ScriptableObject is created for the first time
            if (string.IsNullOrEmpty(Id))
                Id = System.Guid.NewGuid().ToString();
        }
    }
}
