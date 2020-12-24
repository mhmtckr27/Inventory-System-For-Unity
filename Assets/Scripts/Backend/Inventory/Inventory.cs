using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
	[SerializeField] private int inventorySlotCount;
	[SerializeField] private int equipmentSlotCount;
    [SerializeField] private int maxStackAmount;
	[SerializeField] private InventoryUI inventoryUI;

	public int InventorySlotCount { get => inventorySlotCount; set => inventorySlotCount = value; }
	public InventorySlot[] InventorySlots { get; set; }
	public int EquipmentSlotCount { get => equipmentSlotCount; set => equipmentSlotCount = value; }

	public event Action<int> SlotUpdatedEvent;

    private void Awake()
	{
		InitInventorySlots();
	}

	private void InitInventorySlots()
	{
		InventorySlots = new InventorySlot[InventorySlotCount + EquipmentSlotCount];
		for (int i = 0; i < InventorySlotCount; i++)
		{
			InventorySlots[i] = new InventorySlot();
		}
		for(int i = InventorySlotCount; i < InventorySlotCount + EquipmentSlotCount; i++)
		{
			InventorySlots[i] = new EquipmentSlot((ItemCategory)i - InventorySlotCount);
		}
	}

	private void OnEnable()
	{
		inventoryUI.UseItemEvent += UseItemAtIndex;
		inventoryUI.DropItemEvent += RemoveItemFromIndex;
		inventoryUI.SplitStackEvent += SplitStack;
		inventoryUI.SwapSlotsEvent += SwapSlots;
		inventoryUI.CombineStacksEvent += CombineStacks;
	}
	private void OnDisable()
	{
		inventoryUI.UseItemEvent -= UseItemAtIndex;
		inventoryUI.DropItemEvent -= RemoveItemFromIndex;
		inventoryUI.SplitStackEvent -= SplitStack;
		inventoryUI.SwapSlotsEvent -= SwapSlots;
		inventoryUI.CombineStacksEvent -= CombineStacks;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			AddItemDull(itemDatabase.FindItem("Diamond Ore"), 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			AddItemDull(itemDatabase.FindItem("New Axe"), 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			AddItemDull(itemDatabase.FindItem("Diamond Sword"), 1);
		}
	}

	//returns true if slot at index is empty.
	public bool SlotIsEmpty(int index)
	{
		return InventorySlots[index].Amount == 0;
	}

	//returns the amount at index.
	private int GetAmountAtIndex(int index)
	{
		return InventorySlots[index].Amount;
	}

	public InventorySlot GetSlotAtIndex(int index)
	{
		return InventorySlots[index];
	}

	//removes the given amount from given index. returns true if success.
	public void RemoveItemFromIndex(int index, int amount)
	{
		//check if slot is empty or not
		if (SlotIsEmpty(index) == false)
		{
			//slot is not empty.
			//check if we should remove all amount.
			if (amount >= GetAmountAtIndex(index))
			{
				//we want to remove all amount from index.			
				UpdateSlot(null, 0, index);
			}
			else
			{
				//we dont want to remove all of the amount.
				UpdateSlot(InventorySlots[index].Item, InventorySlots[index].Amount - amount, index);
			}
		}
	}

	private void SwapSlots(int firstIndex, int secondIndex)
    {
		if(firstIndex > InventorySlotCount - 1)
		{
			if (((EquipmentSlot)InventorySlots[firstIndex]).SlotCategory != InventorySlots[secondIndex].Item.Category)
			{
				inventoryUI.InventorySlotsUI[secondIndex].SetSlotActive();
				return;
			}
		}
		else if(secondIndex > InventorySlotCount - 1)
		{
			if(InventorySlots[firstIndex].Item != null)
			{
				if (((EquipmentSlot)InventorySlots[secondIndex]).SlotCategory != InventorySlots[firstIndex].Item.Category)
				{
					inventoryUI.InventorySlotsUI[secondIndex].SetSlotActive();
					return;
				}
			}
		}

		InventorySlot tmpSlot = new InventorySlot();
		tmpSlot.Item = InventorySlots[firstIndex].Item;
		tmpSlot.Amount = InventorySlots[firstIndex].Amount;
		UpdateSlot(InventorySlots[secondIndex].Item, InventorySlots[secondIndex].Amount, firstIndex);
		UpdateSlot(tmpSlot.Item, tmpSlot.Amount, secondIndex);
		inventoryUI.InventorySlotsUI[firstIndex].SetSlotActive();
	}

	private void SplitStack(int index, int amount)
	{
		int emptySlotIndex = 0;
		if (EmptySlotExists(ref emptySlotIndex))
		{
			UpdateSlot(InventorySlots[index].Item, amount, emptySlotIndex);
			RemoveItemFromIndex(index, amount);
		}
	}


	private void CombineStacks(int firstIndex, int secondIndex)
	{
		int amountToMove;
		if (InventorySlots[firstIndex].Amount + InventorySlots[secondIndex].Amount > maxStackAmount)
		{
			amountToMove = maxStackAmount - InventorySlots[secondIndex].Amount;
		}
		else
		{
			amountToMove = InventorySlots[firstIndex].Amount;
		}
		UpdateSlot(InventorySlots[secondIndex].Item, InventorySlots[secondIndex].Amount + amountToMove, secondIndex);
		RemoveItemFromIndex(firstIndex, amountToMove);
		inventoryUI.InventorySlotsUI[secondIndex].SetSlotActive();
	}

	private void UseItemAtIndex(int index)
    {
		ItemData itemToUse = InventorySlots[index].Item;
		if (SlotIsEmpty(index) == true)
		{
			return;
		}

		if (itemToUse.CanBeUsed)
        {
			if(itemToUse.Category == ItemCategory.Consumable)
            {
				RemoveItemFromIndex(index, 1);
            }
			itemToUse.OnUse();
        }
		if (itemToUse.CanBeEquipped)
		{
			if(index > InventorySlotCount - 1)
			{
				int localFoundIndex = 0;
				if(EmptySlotExists(ref localFoundIndex))
				{
					SwapSlots(localFoundIndex, index);
				}
			}
			else
			{
				SwapSlots(InventorySlotCount + (int)itemToUse.Category, index);
			}
		}
    }

	//returns true if there is an empty slot.
	private bool EmptySlotExists(ref int index)
	{
		for (int i = 0; i < InventorySlotCount; i++)
		{
			if (SlotIsEmpty(i))
			{
				index = i;
				return true;
			}
		}
		return false;
	}

	//returns true if there is a stack which is not full.
	private bool NotFullStackExists(ItemData item, ref int index)
	{
		for (int i = 0; i < InventorySlotCount; i++)
		{
			if (SlotIsEmpty(i) == false)
			{
				if (maxStackAmount > InventorySlots[i].Amount && item.ItemId == InventorySlots[i].Item.ItemId)
				{
					index = i;
					return true;
				}
			}
		}
		return false;
	}

	//updates slot at index with given item and amount.
	private void UpdateSlot(ItemData item, int amount, int index)
	{
		InventorySlots[index].Item = item;
		InventorySlots[index].Amount = amount;
		if (SlotUpdatedEvent != null)
		{
			SlotUpdatedEvent.Invoke(index);
		}
	}

	private int AddItemDull(ItemData itemToAdd, int amountToAdd)
	{
		int remaining = amountToAdd;
		return AddItem(itemToAdd, amountToAdd, ref remaining);
	}
	private int AddItem(ItemData itemToAdd, int amountToAdd, ref int remaining)
	{
		ItemData localItem = itemToAdd;
		int localFoundIndex = 0;
		int localAmount = amountToAdd;

		if (localItem.CanBeStacked == true)
		{
			//item can be stacked. first look for available stack, if no found, look for free slot to create a new stack.
			if (NotFullStackExists(itemToAdd, ref localFoundIndex))
			{
				//there is a stack that is not full, add to that stack.

				//check if stack can take all we want to add.
				if (InventorySlots[localFoundIndex].Amount + localAmount > maxStackAmount)
				{
					//stack CAN NOT take all we want to add. Add until full, then look for another stack or slot to add remaining.

					localAmount = InventorySlots[localFoundIndex].Amount + localAmount - maxStackAmount;

					UpdateSlot(itemToAdd, maxStackAmount, localFoundIndex);

					return AddItem(localItem, localAmount, ref remaining);
					//return true;
				}
				else
				{
					//stack can take all we want to add. simply add them all.
					UpdateSlot(itemToAdd, InventorySlots[localFoundIndex].Amount + localAmount, localFoundIndex);
					remaining = 0;
					return remaining;
				}

			}
			else
			{
				//no stack available, search for an empty slot.
				if (EmptySlotExists(ref localFoundIndex))
				{
					if (localAmount > maxStackAmount)
					{
						//exceeds maxstackamount, fill slot and search for another empty slot for remaining.
						UpdateSlot(itemToAdd, maxStackAmount, localFoundIndex);
						return AddItem(localItem, localAmount - maxStackAmount, ref remaining);
						//return true;
					}
					else
					{
						//slot can take all amount.
						UpdateSlot(itemToAdd, localAmount, localFoundIndex);
						remaining = 0;
						return remaining;
					}
				}
				else
				{
					//no slot available, can not add item.
					remaining = localAmount;
					return remaining;
				}
			}
		}
		else
		{
			//item can not be stacked. look for a free slot.
			int index = 0;

			if (EmptySlotExists(ref index))
			{
				//empty slot exists. add item.
				localFoundIndex = index;
				UpdateSlot(itemToAdd, 1, localFoundIndex);

				if (localAmount > 1)
				{
					//more than 1 unstackable item. we should call this function recursively until no remaining or no empty slot.
					return AddItem(localItem, localAmount - 1, ref remaining);
					//return true;
				}
				else
				{
					//just 1 item so we added.
					remaining = 0;
					return remaining;
				}
			}
			else
			{
				//no empty slot. can not add item.
				remaining = localAmount;
				return remaining;
			}
		}

	}
}
