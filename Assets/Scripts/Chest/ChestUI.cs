using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : InventoryUI
{
	private int currentSlotCount = 18;

	private static ChestUI instance;
	public static new ChestUI Instance 
	{
		get
		{
			return instance;
		}
	}

	protected override void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		rectTransform = GetComponent<RectTransform>();
		InitInventorySlotsUI();
	}

	protected override void OnEnable()
	{
		if(Inventory == null)
		{
			InventoryWindow.SetActive(false);
			return;
		}
		if (currentSlotCount > Inventory.InventorySlotCount)
		{
			for (int i = Inventory.InventorySlotCount; i < currentSlotCount; i++)
			{
				Destroy(InventorySlotsUI[i].transform.parent.gameObject);
			}
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
		
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.CeilToInt((float)currentSlotCount / 3) * 120);
		UpdateInventorySlotsUI();
		Inventory.SlotUpdatedEvent += OnSlotUpdatedEvent;
	}

	protected override void OnDisable()
	{
		if (Inventory == null) 
		{
			return;
		}
		Inventory.SlotUpdatedEvent -= OnSlotUpdatedEvent;
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
		int remaining = GameManager.Instance.GiveItemToPlayer(tempSlot.Item, tempSlot.Amount);
		Inventory.RemoveItemFromIndex(slotIndex, amountToAdd - remaining);
	}
}
