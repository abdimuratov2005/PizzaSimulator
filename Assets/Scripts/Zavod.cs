using NTC.MonoCache;
using System.Collections.Generic;
using UnityEngine;

public class Zavod : MonoCache
{
    public ItemType requireItemType;
    public List<Item> getItemsList;
    public Transform getItemPoint;
    public int getMaxItem = 5;
    public bool getItemPaused = false;

    [Space(2)]
    [Header("Spawn New Item")]
    public Item spawnItem;
    public ItemType spawnItemType;
    public Transform spawnItemPoint;
    public List<Item> spawnedItemsList;
    public int spawnedMaxItem = 5;
    public int spawnPerSecond = 3;
    public bool spawnItemPaused = false;

    private Item lastChild;

    private float timer = 0;

    protected override void Run()
    {
        getItemPaused = getItemsList.Count >= getMaxItem;
        spawnItemPaused = spawnedItemsList.Count >= spawnedMaxItem;

        if (getItemsList.Count > 0)
        {
            lastChild = getItemsList[^1];
            if (!spawnItemPaused)
            {
                timer += Time.deltaTime;
                if (timer >= spawnPerSecond)
                {
                    SpawnItem();
                    timer = 0;
                }
            }
        }
    }

    private void SpawnItem()
    {
        getItemsList.Remove(lastChild);
        Destroy(lastChild.gameObject);
        var instantiate = Instantiate(spawnItem.gameObject, spawnItemPoint.position, Quaternion.identity);
        Item item = instantiate.GetComponent<Item>();
        item.itemType = spawnItemType;
        spawnedItemsList.Add(item);
        item.transform.SetParent(spawnItemPoint.transform);
        item.UnFreezeRigibody();
    }
}
