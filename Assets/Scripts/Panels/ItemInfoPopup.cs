using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPopup : MonoBehaviour
{
	[SerializeField] private Text itemName;
	[SerializeField] private Text itemDesc;
	[SerializeField] private Text sellPrice;

	private GameObject itemInfoPopup;
	private InventoryUI chestUI;

	private void Awake()
	{
		itemInfoPopup = transform.GetChild(0).gameObject;
		chestUI = FindObjectOfType<ChestUI>();
	}

	private void OnEnable()
	{
		chestUI.ActiveSlotModifiedEvent += UpdateItemInfo;
	}

	private void OnDisable()
	{
		chestUI.ActiveSlotModifiedEvent -= UpdateItemInfo;
	}

	public void UpdateItemInfo(int slotIndex)
	{
		if(slotIndex == -1)
		{
			ClosePopup();
			return;
		}
		ItemData itemToShow = chestUI.Inventory.GetSlotAtIndex(slotIndex).Item;
		if (itemToShow == null)
		{
			ClosePopup();
		}
		else
		{
			UpdatePopup(itemToShow);
			ShowPopup();
		}
	}

	private void UpdatePopup(ItemData itemToShow)
	{
		itemName.text = itemToShow.ItemName;
		itemDesc.text = itemToShow.Description;
		sellPrice.text = itemToShow.ToStringProperty(0);
	}

	private void ShowPopup()
	{
		itemInfoPopup.SetActive(true);
	}

	private void ClosePopup()
	{
		itemInfoPopup.SetActive(false);
	}
}
