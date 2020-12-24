using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
	#region Serialized Fields
	[SerializeField] private GameObject inventorySlotPrefab;
	#endregion

	#region Public properties
	public Chest Chest{ get => chest; set => chest = value; }
	public GameObject DraggingItem { get; set; }
	public List<InventorySlotUI> InventorySlotsUI { get => inventorySlotsUI; set => inventorySlotsUI = value; }
	#endregion

	#region Private fields
	private List<InventorySlotUI> inventorySlotsUI;
	private Chest chest;
	#endregion

}
