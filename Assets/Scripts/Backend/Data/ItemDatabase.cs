using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item Database", order = 2)]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> items = new List<ItemData>();

    public ItemData FindItem(int id)
	{
        return items.Find(item => item.ItemId == id);
	}
    public ItemData FindItem(string name)
	{
        return items.Find(item => item.ItemName == name);
	}

}
