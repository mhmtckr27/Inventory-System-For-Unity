using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : PanelBase
{
	[SerializeField] private Text itemName;
	[SerializeField] private Image itemIcon;
	[SerializeField] private Text itemDesc;
	[SerializeField] private Text itemCategory;
	[SerializeField] private Text sellPrice;
	[SerializeField] private InventoryUI inventoryUI;

	private Sprite initalIcon;
	private GameObject itemInfoContent;

	private void Awake()
	{
		initalIcon = itemIcon.sprite;
		itemInfoContent = transform.GetChild(0).gameObject;
		panelType = PanelType.ItemInfo;
	}
	private void Start()
	{

	}
	private void OnEnable()
	{
		inventoryUI.ActiveSlotModifiedEvent += UpdatePanel;
		inventoryUI.DisablePanelEvent += OnDisablePanelEvent;
		inventoryUI.RequestItemInfoPanelEvent += OnRequestItemInfoPanelEvent;
		UpdatePanel(inventoryUI.ActiveSlot.SlotIndex);
	}

	private void OnDisable()
	{
		inventoryUI.ActiveSlotModifiedEvent -= UpdatePanel;
		inventoryUI.DisablePanelEvent -= OnDisablePanelEvent;
		inventoryUI.RequestItemInfoPanelEvent -= OnRequestItemInfoPanelEvent;
	}

	private void OnDisablePanelEvent()
	{
		itemInfoContent.SetActive(false);
	}

	private void OnRequestItemInfoPanelEvent()
	{
		itemInfoContent.SetActive(true);
		inventoryUI.ActivePanelType = panelType;
	}

	private void UpdatePanel(int slotIndex)
	{
		ShowItemInfo(inventoryUI.Inventory.GetSlotAtIndex(slotIndex).Item);
	}

	public void ShowItemInfo(ItemData itemToShow)
	{
		if (itemToShow)
		{
			itemName.text = itemToShow.ItemName;
			itemIcon.sprite = itemToShow.Icon;
			itemDesc.text = itemToShow.Description;
			itemCategory.text = itemToShow.Category.ToString();
			sellPrice.text = itemToShow.ToStringProperty(0);
		}
		else
		{
			ClearPanel();
		}
	}

	internal void ClearPanel()
	{
		itemIcon.sprite = initalIcon;
		itemName.text = "";
		itemDesc.text = "";
		itemCategory.text = "";
		sellPrice.text = "";
	}
}
