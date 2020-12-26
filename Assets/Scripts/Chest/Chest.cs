using UnityEngine;

public class Chest : Inventory, IInteractable 
{
	[SerializeField] private GameObject chestWindow;

	protected override void Awake()
	{
		InitInventorySlots();
	}
	private void Start()
	{
		InventoryUI = ChestUI.Instance;
	}
	protected override void Update()
	{
		if (Input.GetKey(KeyCode.Alpha4))
		{
			AddItemDull(ItemDatabase.FindItem("Diamond Ore"), 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			AddItemDull(ItemDatabase.FindItem("New Axe"), 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			AddItemDull(ItemDatabase.FindItem("Diamond Sword"), 1);
		}
	}
	public void Interact()
	{
		InventoryUI.Inventory = this;
		chestWindow.SetActive(!chestWindow.activeInHierarchy);

		Debug.Log("Opening chest");
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
