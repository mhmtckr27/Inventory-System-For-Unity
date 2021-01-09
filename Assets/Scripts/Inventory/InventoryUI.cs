using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	#region Inspector attached fields
	[SerializeField] protected GameObject inventorySlotPrefab;
	[SerializeField] private GameObject equipmentSlotPrefab;
	[SerializeField] private GameObject inventoryWindow;
	[SerializeField] private Transform equipmentPanelContentTransform;
	[SerializeField] private GameObject draggingItem;
	#endregion

	#region Singleton
	private static InventoryUI instance;
	public static InventoryUI Instance
	{
		get
		{
			return instance;
		}
	}
	#endregion

	#region Public properties
	public Inventory Inventory { get => inventory; set => inventory = value; }
	public GameObject InventoryWindow { get => inventoryWindow; set => inventoryWindow = value; }
	public GameObject DraggingItem { get => draggingItem; set => draggingItem = value; }
	public List<InventorySlotUI> InventorySlotsUI { get => inventorySlotsUI; set => inventorySlotsUI = value; }
	#endregion

	#region Private fields
	private List<InventorySlotUI> inventorySlotsUI;
	protected RectTransform rectTransform;
	private Inventory inventory;
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
			if(ActiveSlotModifiedEvent != null)
			{
				ActiveSlotModifiedEvent.Invoke(value != null ? value.SlotIndex : -1);
			}
		}
	}
	#endregion

	#region ActivePanelType Property and Event
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
				if(RequestItemInfoPanelEvent != null)
				{
					RequestItemInfoPanelEvent.Invoke();
				}
			}
			activePanelType = value;
			if(ActivePanelTypeChangedEvent != null)
			ActivePanelTypeChangedEvent.Invoke(value);
		}
	}
	#endregion


	protected virtual void Awake()
	{
		#region Singleton
		if (instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
		#endregion

		rectTransform = GetComponent<RectTransform>();
	}

	protected virtual void OnEnable()
	{
		if(inventory == null)
		{
			inventory = Inventory.Instance;
			InitInventorySlotsUI();
			activePanelType = PanelType.ItemInfo;
			ActiveSlot = InventorySlotsUI[0];
			InventoryWindow.SetActive(false);
		}
		AdjustScrollableContentSize();
		for (int i = 0; i < Inventory.InventorySlotCount + Inventory.EquipmentSlotCount; i++)
		{
			OnSlotUpdatedEvent(i);
		}
		Inventory.SlotUpdatedEvent += OnSlotUpdatedEvent;
	}

	private void AdjustScrollableContentSize()
	{
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.CeilToInt((float)Inventory.InventorySlotCount / 3) * 120);
	}

	protected virtual void OnDisable()
	{
		Inventory.SlotUpdatedEvent -= OnSlotUpdatedEvent;
	}

	protected virtual void InitInventorySlotsUI()
	{
		InventorySlotsUI = new List<InventorySlotUI>();
		for (int i = 0; i < Inventory.InventorySlotCount; i++)
		{
			InventorySlotsUI.Add(Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, gameObject.transform).GetComponentInChildren<InventorySlotUI>());
			InventorySlotsUI[i].SlotIndex = i;
			InventorySlotsUI[i].InventoryUI = this;
			InventorySlotsUI[i].UpdateSlotUI(Inventory.InventorySlots[i]);

		}

		for (int i = Inventory.InventorySlotCount; i < Inventory.InventorySlotCount + Inventory.EquipmentSlotCount; i++)
		{
			InventorySlotsUI.Add(Instantiate(equipmentSlotPrefab, Vector3.zero, Quaternion.identity, equipmentPanelContentTransform).GetComponentInChildren<EquipmentSlotUI>());
			InventorySlotsUI[i].SlotIndex = i;
			InventorySlotsUI[i].InventoryUI = this;
			InventorySlotsUI[i].UpdateSlotUI(Inventory.InventorySlots[i]);
		}
	}

	public Vector3 GetSlotPosition(int index)
	{
		return InventorySlotsUI[index].transform.position;
	}
	
	#region Reflects backend inventory slot changes to ui slots
	public virtual void OnSlotUpdatedEvent(int index)
	{
		InventorySlotsUI[index].UpdateSlotUI(Inventory.GetSlotAtIndex(index));
		if((activeSlot != null) && (index == activeSlot.SlotIndex) && (ActiveSlotModifiedEvent != null))
		{
			ActiveSlotModifiedEvent.Invoke(index);
		}
	}
	#endregion
	
	#region Displaying panels on iteminfo region

	public virtual void OnUseButton()
	{
		UseItemUI();
	}

	public virtual void OnDropButton()
	{
		DisablePanelEvent.Invoke();
		RequestDropItemPanelEvent.Invoke(Inventory.GetSlotAtIndex(ActiveSlot.SlotIndex));
	}

	public virtual void OnDropButton(InventorySlotUI slotToDrop)
	{
		DisablePanelEvent.Invoke();
		RequestDropItemPanelEvent.Invoke(Inventory.GetSlotAtIndex(slotToDrop.SlotIndex));
	}

	public virtual void OnSplitButton()
	{
		DisablePanelEvent.Invoke();
		RequestSplitStackPanelEvent.Invoke(Inventory.GetSlotAtIndex(ActiveSlot.SlotIndex));
	}
	#endregion

	#region Reflecting UI changes to backend
	public virtual void UseItemUI()
	{
		UseItemEvent.Invoke(ActiveSlot.SlotIndex);
		ActiveSlotModifiedEvent.Invoke(ActiveSlot.SlotIndex);
	}

	public virtual void DropItemUI(int amountToRemove)
	{
		DropItemEvent.Invoke(ActiveSlot.SlotIndex, amountToRemove);
		ActiveSlotModifiedEvent.Invoke(ActiveSlot.SlotIndex);
	}

	public virtual void SplitStackUI(int amountToSplit)
	{
		SplitStackEvent.Invoke(ActiveSlot.SlotIndex, amountToSplit);
		ActiveSlotModifiedEvent.Invoke(ActiveSlot.SlotIndex);
	}

	public virtual void SwapSlotsUI(int firstIndex, int secondIndex)
	{
		SwapSlotsEvent.Invoke(firstIndex, secondIndex);
		ActiveSlotModifiedEvent.Invoke(ActiveSlot.SlotIndex);
	}

	public virtual void CombineStacksUI(int firstIndex, int secondIndex)
	{
		CombineStacksEvent.Invoke(firstIndex, secondIndex);
		ActiveSlotModifiedEvent.Invoke(ActiveSlot.SlotIndex);
	}
	#endregion
}
