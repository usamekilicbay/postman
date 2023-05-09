using System;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Card Config", menuName = "Configs/Item Card Config")]
public class ItemCardConfig : ScriptableObject
{
    public string Id { get; private set; }

    public string IdInfo;
    public Sprite Artwork;
    public string Name;
    public string Description;
    public int Price;
    public Rarity Rarity;
    public float Weight;

    //[SerializeField, HideInInspector] private bool _hasBeenInitialised;  
    public bool _hasBeenInitialised;  

    [Conditional("UNITY_EDITOR")]
    private void OnValidate() 
    {
        if (!_hasBeenInitialised)
        {
            Id = Guid.NewGuid().ToString();
            _hasBeenInitialised = true;
        }

        IdInfo = Id;
    }


    [ContextMenu("Reset Config")]
    public void ResetConfig()
    {
        _hasBeenInitialised = false;
    }

    [ContextMenu("Generate New Id")]
    public void GenerateNewId()
    {
        Id = Guid.NewGuid().ToString();
        IdInfo = Id;
    }
}
