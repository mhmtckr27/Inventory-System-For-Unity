using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : InventoryUI
{
	private GameManager gameManager;
	private int currentSlotCount = 18;

	protected override void Awake()
	{
		//We don't have a chest in the beginning. When player interacts with a chest,
		//that chest's interact function sets CurrentChest as itself.
		//Inventory = FindObjectOfType<Chest>();
		gameManager = FindObjectOfType<GameManager>();
		InitInventorySlotsUI();
		DraggingItem = new GameObject("DraggingItem");
		DraggingItem.transform.parent = transform;
		//ActiveSlot = InventorySlotsUI[10];
	}

	protected override void OnEnable()
	{
		if (currentSlotCount > Inventory.InventorySlotCount)
		{
			InventorySlotsUI.RemoveRange(Inventory.InventorySlotCount, currentSlotCount - Inventory.InventorySlotCount);
		}
		else if (currentSlotCount < Inventory.InventorySlotCount)
		{
			for (int i = currentSlotCount; i < Inventory.InventorySlotCount; i++)
			{
				InventorySlotsUI.Add(Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponentInChildren<InventorySlotUI>());
				InventorySlotsUI[i].SlotIndex = i;
				InventorySlotsUI[i].InventoryUI = this;
			}
		}
		currentSlotCount = Inventory.InventorySlotCount;
		for (int i = 0; i < currentSlotCount; i++)
		{
			UpdateInventorySlotsUI();
		}
		Inventory.SlotUpdatedEvent += OnSlotUpdatedEvent;
	}

	protected override void InitInventorySlotsUI()
	{
		InventorySlotsUI = new List<InventorySlotUI>();
		for (int i = 0; i < currentSlotCount; i++)
		{
			InventorySlotsUI.Add(Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponentInChildren<InventorySlotUI>());
			InventorySlotsUI[i].SlotIndex = i;
			InventorySlotsUI[i].InventoryUI = this;
		}
	}

	private void UpdateInventorySlotsUI()
	{
		for (int i = 0; i < currentSlotCount; i++)
		{
			InventorySlotsUI[i].UpdateSlotUI(Inventory.InventorySlots[i]);
		}
	}

	public void GiveItemToPlayer(int slotIndex)
	{
		InventorySlot tempSlot = Inventory.GetSlotAtIndex(slotIndex);
		int amountToAdd = tempSlot.Amount;
		int remaining = gameManager.GiveItemToPlayer(tempSlot.Item, tempSlot.Amount);
		Inventory.RemoveItemFromIndex(slotIndex, amountToAdd - remaining);
	}
}
