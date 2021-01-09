using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Chest : Inventory, IInteractable 
{
	private GameObject chestWindow;

	protected override void Awake()
	{
		InitInventorySlots();
	}
	private void Start()
	{
		InventoryUI = ChestUI.Instance;
		chestWindow = ChestWindow.Instance.gameObject;

		InventoryUI.SwapSlotsEvent += SwapSlots;
		InventoryUI.CombineStacksEvent += CombineStacks;
	}

	public void Interact()
	{
		InventoryUI.Inventory = this;
		chestWindow.SetActive(!chestWindow.activeInHierarchy);
	}

	protected override void InitInventorySlots()
	{
		InventorySlots = new InventorySlot[InventorySlotCount];
		for (int i = 0; i < InventorySlotCount; i++)
		{
			InventorySlots[i] = new InventorySlot();
		}
	}
}
