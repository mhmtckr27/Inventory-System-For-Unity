using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : InventorySlot
{
	[SerializeField] private ItemCategory slotCategory;

	public EquipmentSlot(ItemCategory slotCategory)
	{
		this.SlotCategory = slotCategory;
	}

	public ItemCategory SlotCategory { get => slotCategory; set => slotCategory = value; }
}
