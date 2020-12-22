using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	#region Serialized Fields
	[SerializeField] private GameObject inventorySlotPrefab;
	[SerializeField] private GameObject equipmentSlotPrefab;
	[SerializeField] private GameObject inventoryInterface;
	[SerializeField] private Transform equipmentPanelContentTransform;
	#endregion

	#region Public properties
	public Inventory Inventory { get; set; }
	public GameObject DraggingItem { get; set; }
	public List<InventorySlotUI> InventorySlotsUI { get => inventorySlotsUI; set => inventorySlotsUI = value; }
	#endregion

	#region Private fields
	private List<InventorySlotUI> inventorySlotsUI;
	private List<EquipmentSlotUI> equipmentSlotsUI;
	#endregion

	#region Events used for reflecting ui inventory changes to backend inventory script.
	//inventory is responsible for registering/unregistering to these event
	public event Action<int> UseItemEvent;
	public event Action<int, int> SplitStackEvent;
	public event Action<int, int> DropItemEvent;
	public event Action<int, int> SwapSlotsEvent;
	public event Action<int, int> CombineStacksEvent;
	public event Action<int> ActiveSlotModifiedEvent;
	#endregion

	#region Events for requesting panels such as iteminfo, dropitem, splitstack, disablepanel
	//Each panel is responsible for registering/unregistering to these events and displaying themselves accordingly.
	//C# events are used for better performance over unityevents
	public event Action DisablePanelEvent;
	public event Action RequestItemInfoPanelEvent;
	public event Action<InventorySlot> RequestDropItemPanelEvent;
	public event Action<InventorySlot> RequestSplitStackPanelEvent;
	#endregion

	#region ActiveSlot Property
	//currently selected inventory slot
	private InventorySlotUI activeSlot;
	public InventorySlotUI ActiveSlot
	{
		get => activeSlot;
		set
		{
			activeSlot = value;
			ActiveSlotModifiedEvent.Invoke(value.SlotIndex);
		}
	}
	#endregion

	public event Action<PanelType> ActivePanelTypeChangedEvent;
	private PanelType activePanelType;
	public PanelType ActivePanelType
	{
		get => activePanelType;
		set
		{
			if(value == PanelType.Empty)
			{
				value = PanelType.ItemInfo;
				RequestItemInfoPanelEvent.Invoke();
			}
			activePanelType = value;
			ActivePanelTypeChangedEvent.Invoke(value);
		}
	}

	public GameObject InventoryInterface { get => inventoryInterface; set => inventoryInterface = value; }

	private void Awake()
	{
		Inventory = FindObjectOfType<Inventory>();
		InitInventorySlotsUI();
		DraggingItem = new GameObject("DraggingItem");
		DraggingItem.transform.parent = transform;
		activePanelType = PanelType.ItemInfo;
	}

	private void InitInventorySlotsUI()
	{
		InventorySlotsUI = new List<InventorySlotUI>();
		for (int i = 0; i < Inventory.InventorySlotCount; i++)
		{
			InventorySlotsUI.Add(Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponentInChildren<InventorySlotUI>());
			InventorySlotsUI[i].SlotIndex = i;
			InventorySlotsUI[i].InventoryUI = this;
			InventorySlotsUI[i].UpdateSlotUI(Inventory.InventorySlots[i]);
		}

		equipmentSlotsUI = new List<EquipmentSlotUI>();
		for(int i = Inventory.InventorySlotCount; i < Inventory.InventorySlotCount + Inventory.EquipmentSlotCount; i++)
		{
			InventorySlotsUI.Add(Instantiate(equipmentSlotPrefab, Vector3.zero, Quaternion.identity, equipmentPanelContentTransform).GetComponentInChildren<EquipmentSlotUI>());
			InventorySlotsUI[i].SlotIndex = i;
			InventorySlotsUI[i].InventoryUI = this;
			InventorySlotsUI[i].UpdateSlotUI(Inventory.InventorySlots[i]);
		}
	}

	private void Start()
	{
		ActiveSlot = InventorySlotsUI[0];
	}
	private void OnEnable()
	{
		Inventory.SlotUpdatedEvent += OnSlotUpdatedEvent;
	}
	private void OnDisable()
	{
		Inventory.SlotUpdatedEvent -= OnSlotUpdatedEvent;
	}

	public Vector3 GetSlotPosition(int index)
	{
		return InventorySlotsUI[index].transform.position;
	}


	#region Reflects backend inventory slot changes to ui slots
	private void OnSlotUpdatedEvent(int index)
	{
		InventorySlotsUI[index].UpdateSlotUI(Inventory.GetSlotAtIndex(index));
	}
	#endregion

	#region Displaying panels on iteminfo region

	public void OnUseButton()
	{
		UseItemUI();
	}

	public void OnDropButton()
	{
		DisablePanelEvent.Invoke();
		RequestDropItemPanelEvent.Invoke(Inventory.GetSlotAtIndex(activeSlot.SlotIndex));
	}

	public void OnDropButton(InventorySlotUI slotToDrop)
	{
		DisablePanelEvent.Invoke();
		RequestDropItemPanelEvent.Invoke(Inventory.GetSlotAtIndex(slotToDrop.SlotIndex));
	}

	public void OnSplitButton()
	{
		DisablePanelEvent.Invoke();
		RequestSplitStackPanelEvent.Invoke(Inventory.GetSlotAtIndex(activeSlot.SlotIndex));
	}
	#endregion


	#region Reflecting UI changes to backend
	public void UseItemUI()
	{
		UseItemEvent.Invoke(ActiveSlot.SlotIndex);
		ActiveSlotModifiedEvent.Invoke(ActiveSlot.SlotIndex);
	}

	public void DropItemUI(int amountToRemove)
	{
		DropItemEvent.Invoke(ActiveSlot.SlotIndex, amountToRemove);
		ActiveSlotModifiedEvent.Invoke(activeSlot.SlotIndex);
	}

	public void SplitStackUI(int amountToSplit)
	{
		SplitStackEvent.Invoke(ActiveSlot.SlotIndex, amountToSplit);
		ActiveSlotModifiedEvent.Invoke(activeSlot.SlotIndex);
	}

	public void SwapSlotsUI(int firstIndex, int secondIndex)
	{
		SwapSlotsEvent.Invoke(firstIndex, secondIndex);
		ActiveSlotModifiedEvent.Invoke(activeSlot.SlotIndex);
	}

	public void CombineStacksUI(int firstIndex, int secondIndex)
	{
		CombineStacksEvent.Invoke(firstIndex, secondIndex);
		ActiveSlotModifiedEvent.Invoke(activeSlot.SlotIndex);
	}
	#endregion
}
