using UnityEngine;
using NTC.MonoCache;
using System.Collections.Generic;
using DG.Tweening;

public class AutoZavod : MonoCache
{
    public int spawnPerSecond = 3;
    public int itemsMaxSpawn = 10;
    public Item spawnItem;
    public ItemType setItemType;
    public Transform spawnPoint;

    [Space(2)]
    [Header("Have items")]
    public List<Item> items;

    private float timer = 0;
    private bool spawnPaused = false;

    protected override void Run()
    {
        spawnPaused = items.Count >= itemsMaxSpawn;

        if (!spawnPaused)
        {
            timer += Time.deltaTime;
            if (timer >= spawnPerSecond)
            {
                SpawnItem();
                timer = 0;
            }
        }
    }

    private void SpawnItem()
    {
        var instantiate = Instantiate(spawnItem.gameObject, spawnPoint.position, Quaternion.identity);
        Item item = instantiate.GetComponent<Item>();
        item.itemType = setItemType;
        items.Add(item);
        item.SetParent(spawnPoint.transform);
        item.UnFreezeRigibody();
    }

    public void GetItem()
    {
        if (items.Count > 0)
        {
            var item = items[^1];
            items.Remove(item);
        }
    }
}
