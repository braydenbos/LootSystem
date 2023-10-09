using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New loot table")]
public class LootTable : ScriptableObject
{
    [Range(0f,100f)] public float chanceDropRate;
    public List<RequiredDrops> requiredDrops;
    public List<ChanceDrops> chanceDrops;

    [Serializable]
    public struct RequiredDrops
    {
        public GameObject item;
        public  int dropAmount;
        
    }
    
    [Serializable]
    public struct ChanceDrops
    {
        public GameObject[] items;
        public int dropItemsFromList;
        public float dropChance;
    }
    
    public bool HasRequiredDrops => requiredDrops.Count > 0;
    public bool HasChanceDrops => chanceDrops.Count > 0;

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        Debug.Log("Shoot");
    }

}
