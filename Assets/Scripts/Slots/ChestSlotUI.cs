using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestSlotUI : InventorySlotUI, IPointerEnterHandler, IPointerExitHandler
{
	public override void OnEndDrag(PointerEventData eventData)
	{
		itemImage.transform.SetParent(borderImage.transform);
		itemImage.transform.localPosition = Vector3.zero;
		eventData.selectedObject = null;
	}

	public override void OnDrop(PointerEventData eventData)
	{
		base.OnDrop(eventData);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		//dont set slot as active because we dont want to highlight selected slot, we will show item info as mouse hovers.
		//SetSlotActive();

		//if right click OR left double-click
		if ((eventData.button == PointerEventData.InputButton.Right) || ((eventData.button == PointerEventData.InputButton.Left) && (eventData.clickCount == 2)))
		{
			//try to give item to player
			(InventoryUI as ChestUI).GiveItemToPlayer(SlotIndex);
			UpdateSlotUI(InventoryUI.Inventory.GetSlotAtIndex(SlotIndex));
			eventData.clickCount = 0;
		}
		eventData.selectedObject = null;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		InventoryUI.ActiveSlot = this;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		InventoryUI.ActiveSlot = null;
	}
}
