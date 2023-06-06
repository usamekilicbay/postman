using UnityEngine;
using System.Collections.Generic;
using Merchant.Card;

namespace Merchant.Config.Item
{
    public class ItemBaseConfig : ScriptableObject
    {
        [SerializeField, ReadOnly, InspectorName("Id")] private string _id;

        [SerializeField, Header("Artwork")]
        protected Sprite artwork;

        [SerializeField]
        protected string name;

        [SerializeField, TextArea, Header("Description")]
        protected string description;

        [SerializeField, Header("Weight")]
        protected float weight;

        [SerializeField, Header("Value")]
        protected int value;

        [SerializeField, Header("Price")]
        protected int price;

        [SerializeField, Header("Acquire Methods")]
        protected bool[] acquireMethods;

        [SerializeField, Header("Curse List")]
        protected Dictionary<int, string> curseList;

        [SerializeField, Header("Bless List")]
        protected Dictionary<int, string> blessList;

        [SerializeField, Range(0f, 100f), Header("Curse Possibility")]
        protected float cursePossibility;

        [SerializeField, Range(0f, 100f), Header("Bless Possibility")]
        protected float blessPossibility;

        [SerializeField, Range(0f, 100f), Header("Encounter Possibility")]
        protected float encounterPossibility;

        [SerializeField, Header("Rarity")]
        protected Rarity rarity;

        public string Id => _id;
        public string Name => name;
        public Sprite Artwork => artwork;
        public string Description => description;
        public float Weight => weight;
        public int Value => value;
        public int Price => price;
        public bool[] AcquireMethods => acquireMethods;
        public Dictionary<int, string> CurseList => curseList;
        public Dictionary<int, string> BlessList => blessList;
        public float CursePossibility => cursePossibility;
        public float BlessPossibility => blessPossibility;
        public float EncounterPossibility => encounterPossibility;
        public Rarity Rarity => rarity;

        protected void OnEnable()
        {
            // Generate a new ID when the ScriptableObject is created for the first time
            if (string.IsNullOrEmpty(_id))
                _id = System.Guid.NewGuid().ToString();
        }
    }
}
