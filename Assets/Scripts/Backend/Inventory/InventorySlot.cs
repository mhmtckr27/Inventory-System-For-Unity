using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
	[SerializeField] private ItemData item;
	[SerializeField] private int amount;

	public ItemData Item { get => item; set => item = value; }
	public int Amount { get => amount; set => amount = value; }
}
