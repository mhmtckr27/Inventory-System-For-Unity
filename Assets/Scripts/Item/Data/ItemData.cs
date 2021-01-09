using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Default Item")]
public class ItemData : ScriptableObject
{
	[SerializeField] private int itemId;
	[SerializeField] private string itemName;
	[SerializeField] private string description;
	[SerializeField] private string useText;
	[SerializeField] private ItemCategory category;
	[SerializeField] private Sprite icon;
	[SerializeField] private bool canBeStacked;
	[SerializeField] private bool canBeUsed;
	[SerializeField] private bool canBeEquipped;
	[SerializeField] private int sellPrice;
	[SerializeField] private int buyPrice;
	[SerializeField] private StringIntDictionary itemProperties;


	public int ItemId { get => itemId; set => itemId = value; }
	public string ItemName { get => itemName; set => itemName = value; }
	public string Description { get => description; set => description = value; }
	public string UseText { get => useText; set => useText = value; }
	public ItemCategory Category { get => category; set => category = value; }
	public Sprite Icon { get => icon; set => icon = value; }
	public bool CanBeStacked { get => canBeStacked; set => canBeStacked = value; }
	public bool CanBeUsed { get => canBeUsed; set => canBeUsed = value; }
	public StringIntDictionary ItemProperties { get => itemProperties; set => itemProperties = value; }
	public int SellPrice { get => sellPrice; set => sellPrice = value; }
	public int BuyPrice { get => buyPrice; set => buyPrice = value; }
	public bool CanBeEquipped { get => canBeEquipped; set => canBeEquipped = value; }

	public virtual void OnUse() { }

	public string ToStringProperty(int opt)
	{
		if (opt == 0)
		{
			return "Sell price: " + sellPrice;
		}
		else
		{
			return "Buy price: " + buyPrice;
		}
	}
}
