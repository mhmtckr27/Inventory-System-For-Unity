using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSlotBorderHandler : MonoBehaviour
{
	[SerializeField] private InventoryUI inventoryUI;

	private IEnumerator cor;

	private void Start()
	{
		cor = MoveToSlotDelayed();
		StartCoroutine(cor);
	}
	private void OnEnable()
	{
		inventoryUI.ActiveSlotModifiedEvent += MoveToSlot;
	}
	private void OnDisable()
	{
		inventoryUI.ActiveSlotModifiedEvent -= MoveToSlot;
	}
	private void MoveToSlot(int slotIndex)
	{
		transform.position = inventoryUI.GetSlotPosition(slotIndex);
		transform.SetParent(inventoryUI.InventorySlotsUI[slotIndex].transform, true);
	}

	IEnumerator MoveToSlotDelayed()
	{
		yield return null;
		transform.position = inventoryUI.GetSlotPosition(0);
	}
}
