using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] private ItemDatabase itemDatabase;
	[SerializeField] private int inventorySlotCount;
	[SerializeField] private int equipmentSlotCount;
	[SerializeField] private int maxStackAmount;
	[SerializeField] private InventoryUI inventoryUI;

	#region Properties
	public ItemDatabase ItemDatabase { get => itemDatabase; set => itemDatabase = value; }
	public int InventorySlotCount { get => inventorySlotCount; set => inventorySlotCount = value; }
	public int EquipmentSlotCount { get => equipmentSlotCount; set => equipmentSlotCount = value; }
	public int MaxStackAmount { get => maxStackAmount; set => maxStackAmount = value; }
	public InventoryUI InventoryUI { get => inventoryUI; set => inventoryUI = value; }
	#endregion

	#region Auto Properties
	public InventorySlot[] InventorySlots { get; set; }
	#endregion

	#region Events
	public event Action<int> SlotUpdatedEvent;
	#endregion

	protected virtual void Awake()
	{
		InitInventorySlots();
	}

	protected virtual void OnEnable()
	{
		InventoryUI.UseItemEvent += UseItemAtIndex;
		InventoryUI.DropItemEvent += RemoveItemFromIndex;
		InventoryUI.SplitStackEvent += SplitStack;
		InventoryUI.SwapSlotsEvent += SwapSlots;
		InventoryUI.CombineStacksEvent += CombineStacks;
	}
	protected virtual void OnDisable()
	{
		InventoryUI.UseItemEvent -= UseItemAtIndex;
		InventoryUI.DropItemEvent -= RemoveItemFromIndex;
		InventoryUI.SplitStackEvent -= SplitStack;
		InventoryUI.SwapSlotsEvent -= SwapSlots;
		InventoryUI.CombineStacksEvent -= CombineStacks;
	}

	protected virtual void Update()
	{
		if (Input.GetKey(KeyCode.Alpha1))
		{
			AddItemDull(ItemDatabase.FindItem("Diamond Ore"), 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			AddItemDull(ItemDatabase.FindItem("New Axe"), 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			AddItemDull(ItemDatabase.FindItem("Diamond Sword"), 1);
		}
	}

	protected virtual void InitInventorySlots()
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

	//returns true if slot at index is empty.
	public virtual bool SlotIsEmpty(int index)
	{
		return InventorySlots[index].Amount == 0;
	}

	//returns the amount at index.
	protected virtual int GetAmountAtIndex(int index)
	{
		return InventorySlots[index].Amount;
	}

	public virtual InventorySlot GetSlotAtIndex(int index)
	{
		return InventorySlots[index];
	}

	//removes the given amount from given index. returns true if success.
	public virtual void RemoveItemFromIndex(int index, int amount)
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

	protected virtual void SwapSlots(int firstIndex, int secondIndex)
	{
		if (firstIndex > InventorySlotCount - 1)
		{
			if (((EquipmentSlot)InventorySlots[firstIndex]).SlotCategory != InventorySlots[secondIndex].Item.Category)
			{
				InventoryUI.InventorySlotsUI[secondIndex].SetSlotActive();
				return;
			}
		}
		else if (secondIndex > InventorySlotCount - 1)
		{
			if (InventorySlots[firstIndex].Item != null)
			{
				if (((EquipmentSlot)InventorySlots[secondIndex]).SlotCategory != InventorySlots[firstIndex].Item.Category)
				{
					InventoryUI.InventorySlotsUI[secondIndex].SetSlotActive();
					return;
				}
			}
		}

		InventorySlot tmpSlot = new InventorySlot();
		tmpSlot.Item = InventorySlots[firstIndex].Item;
		tmpSlot.Amount = InventorySlots[firstIndex].Amount;
		UpdateSlot(InventorySlots[secondIndex].Item, InventorySlots[secondIndex].Amount, firstIndex);
		UpdateSlot(tmpSlot.Item, tmpSlot.Amount, secondIndex);
		InventoryUI.InventorySlotsUI[firstIndex].SetSlotActive();
	}

	protected virtual void SplitStack(int index, int amount)
	{
		int emptySlotIndex = 0;
		if (EmptySlotExists(ref emptySlotIndex))
		{
			UpdateSlot(InventorySlots[index].Item, amount, emptySlotIndex);
			RemoveItemFromIndex(index, amount);
		}
	}

	protected virtual void CombineStacks(int firstIndex, int secondIndex)
	{
		int amountToMove;
		if (InventorySlots[firstIndex].Amount + InventorySlots[secondIndex].Amount > MaxStackAmount)
		{
			amountToMove = MaxStackAmount - InventorySlots[secondIndex].Amount;
		}
		else
		{
			amountToMove = InventorySlots[firstIndex].Amount;
		}
		UpdateSlot(InventorySlots[secondIndex].Item, InventorySlots[secondIndex].Amount + amountToMove, secondIndex);
		RemoveItemFromIndex(firstIndex, amountToMove);
		InventoryUI.InventorySlotsUI[secondIndex].SetSlotActive();
	}

	protected virtual void UseItemAtIndex(int index)
	{
		ItemData itemToUse = InventorySlots[index].Item;
		if (SlotIsEmpty(index) == true)
		{
			return;
		}

		if (itemToUse.CanBeUsed)
		{
			if (itemToUse.Category == ItemCategory.Consumable)
			{
				RemoveItemFromIndex(index, 1);
			}
			itemToUse.OnUse();
		}
		if (itemToUse.CanBeEquipped)
		{
			if (index > InventorySlotCount - 1)
			{
				int localFoundIndex = 0;
				if (EmptySlotExists(ref localFoundIndex))
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
	protected virtual bool EmptySlotExists(ref int index)
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
	protected virtual bool NotFullStackExists(ItemData item, ref int index)
	{
		for (int i = 0; i < InventorySlotCount; i++)
		{
			if (SlotIsEmpty(i) == false)
			{
				if (MaxStackAmount > InventorySlots[i].Amount && item.ItemId == InventorySlots[i].Item.ItemId)
				{
					index = i;
					return true;
				}
			}
		}
		return false;
	}

	//updates slot at index with given item and amount.
	protected virtual void UpdateSlot(ItemData item, int amount, int index)
	{
		InventorySlots[index].Item = item;
		InventorySlots[index].Amount = amount;
		if (SlotUpdatedEvent != null)
		{
			SlotUpdatedEvent.Invoke(index);
		}
	}

	public virtual int AddItemDull(ItemData itemToAdd, int amountToAdd)
	{
		int remaining = amountToAdd;
		return AddItem(itemToAdd, amountToAdd, ref remaining);
	}
	protected virtual int AddItem(ItemData itemToAdd, int amountToAdd, ref int remaining)
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
				if (InventorySlots[localFoundIndex].Amount + localAmount > MaxStackAmount)
				{
					//stack CAN NOT take all we want to add. Add until full, then look for another stack or slot to add remaining.

					localAmount = InventorySlots[localFoundIndex].Amount + localAmount - MaxStackAmount;

					UpdateSlot(itemToAdd, MaxStackAmount, localFoundIndex);

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
					if (localAmount > MaxStackAmount)
					{
						//exceeds maxstackamount, fill slot and search for another empty slot for remaining.
						UpdateSlot(itemToAdd, MaxStackAmount, localFoundIndex);
						return AddItem(localItem, localAmount - MaxStackAmount, ref remaining);
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
