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
    public bool playerStaying = false;

    protected override void Run()
    {
    }
}
