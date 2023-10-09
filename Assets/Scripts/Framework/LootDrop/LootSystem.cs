using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
public class LootSystem : GenericSingleton<LootSystem>
{
    private LootTable _lootTable;
    private LootForce _lootForce;
    private Transform _lootTransform;
    private bool _isPlaying, _isTest; 
    private List<GameObject> roll = new List<GameObject>();
    public void ChooseLoot(LootTable lootTable,LootForce lootForce,Transform lootTransform)
    {
        _lootTable = lootTable;
        _lootForce = lootForce;
        _lootTransform = lootTransform;
        _isTest = false;
        DropRequiredItems();
        DropChanceItems();
    }

    public GameObject[] RollLoot(LootTable lootTable,LootForce lootForce,Transform lootTransform)
    {
        roll.Clear();
        _lootTable = lootTable;
        _lootForce = lootForce;
        _lootTransform = lootTransform;
        _isTest = true;
        DropRequiredItems();
        DropChanceItems();
        return roll.ToArray();
    }
    
    private void DropRequiredItems()
    {
        if (!_lootTable.HasRequiredDrops) {return;}
        foreach (var elements in _lootTable.requiredDrops)
        {
            for (var i = 0; i < elements.dropAmount; i++)
            {
                    DropItem(elements.item);
            }
        }

    }
    
    private void DropChanceItems( )
    {
        if (!MayChanceItemsDrop(_lootTable.chanceDropRate/100) || !_lootTable.HasChanceDrops){return;}

        var randomChance = CalculateDropChance();
        
        var element = GetElement(randomChance);
        
        PickRandomItems(element.items,element.dropItemsFromList);
    }
    
    private bool MayChanceItemsDrop(float dropChance)
    {
        return Random.value > dropChance;
    }
    
    private float CalculateDropChance()
    {
        return Random.Range(0, CalculateTotalDropChance());
    }
    private float CalculateTotalDropChance()
    {
        float i = 0;
        foreach (var elements in _lootTable.chanceDrops)
        {
             i += elements.dropChance;
        }

        return i;
    }
    
    private void PickRandomItems(IReadOnlyList<GameObject> targetItems, int maxItems)
    {
        for (var i = 0; i < maxItems; i++)
        {
            var item = targetItems[Random.Range(0, targetItems.Count)];
            DropItem(item);
        }
    }
    private LootTable.ChanceDrops GetElement(float randomNumber)
    {
        foreach (var element in _lootTable.chanceDrops)
        {
            if (randomNumber <= element.dropChance)
            {
                return element;
            }

            randomNumber -= element.dropChance;
        }

        return default;
    }


    private void DropItem(GameObject item)
    {
        if(_isTest)roll.Add(item);
        if (Application.isPlaying)InstantiateItem(item);
    }
    
    private void InstantiateItem(GameObject item)
    {
        var spawnedItem = Instantiate(item, _lootTransform.transform.position,quaternion.identity);
        spawnedItem.AddComponent<ForceBody>();
        var force = _lootForce.itemDropForce.Clone();
        if (_lootTransform != null)
            force.Direction = new Vector3(
                Random.Range(_lootForce.minForceDirection.x, _lootForce.maxForceDirection.x) * (_lootTransform.localScale.x / _lootForce.baseSize.x),
                Random.Range(_lootForce.minForceDirection.y, _lootForce.maxForceDirection.y) * (_lootTransform.localScale.y / _lootForce.baseSize.y),
                0);
        spawnedItem.GetComponent<ForceBody>().Add(force, new CallbackConfig());
    }

}