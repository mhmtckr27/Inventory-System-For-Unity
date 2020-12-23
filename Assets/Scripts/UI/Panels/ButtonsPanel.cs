using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsPanel : MonoBehaviour
{
	[SerializeField] private ButtonHandler useButton;
	[SerializeField] private ButtonHandler splitButton;
	[SerializeField] private ButtonHandler dropButton;

	private InventoryUI inventoryUI;

	private void Awake()
	{
		inventoryUI = FindObjectOfType<InventoryUI>();
		//inventoryUI.ActiveSlotModifiedEvent += UpdateButtons;
		//inventoryUI.ActivePanelTypeChangedEvent += OnActivePanelTypeChanged;
	}
	private void OnEnable()
	{
		inventoryUI.ActiveSlotModifiedEvent += UpdateButtons;
		inventoryUI.ActivePanelTypeChangedEvent += OnActivePanelTypeChanged;
		UpdateButtons(inventoryUI.ActiveSlot.SlotIndex);
	}
	private void OnDisable()
	{
		inventoryUI.ActiveSlotModifiedEvent -= UpdateButtons;
		inventoryUI.ActivePanelTypeChangedEvent -= OnActivePanelTypeChanged;
	}

	private void UpdateButtons(int slotIndex)
	{
		ItemData tempData = inventoryUI.Inventory.GetSlotAtIndex(slotIndex).Item;
		if (tempData)
		{
			useButton.SetInteractable(tempData.CanBeUsed);
			useButton.SetText(tempData.CanBeUsed ? ((slotIndex > inventoryUI.Inventory.InventorySlotCount - 1) ? "Unequip" : tempData.UseText) : "Use");

			splitButton.SetInteractable(tempData.CanBeStacked && inventoryUI.Inventory.GetSlotAtIndex(slotIndex).Amount > 1);

			dropButton.SetInteractable(tempData.Category != ItemCategory.Quest);
		}
		else
		{
			LockAllButtons();
		}
	}

	private void LockAllButtons()
	{
		useButton.SetInteractable(false);
		splitButton.SetInteractable(false);
		dropButton.SetInteractable(false);
	}

	private void OnActivePanelTypeChanged(PanelType newPanelType)
	{
		if(newPanelType != PanelType.ItemInfo)
		{
			LockAllButtons();
		}
		else
		{
			UpdateButtons(inventoryUI.ActiveSlot.SlotIndex);
		}
	}
}
