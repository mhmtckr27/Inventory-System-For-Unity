using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
	[SerializeField] private int itemId;
	[SerializeField] protected Image itemImage;
	[SerializeField] protected Image borderImage;
	[SerializeField] private Text countText;
	[SerializeField] private Sprite emptySprite;
	[SerializeField] private Button borderButton;

	public int SlotIndex { get; set; }
	public InventoryUI InventoryUI { get; set; }
	public Image BorderImage { get => borderImage; set => borderImage = value; }
	public Text CountText { get => countText; set => countText = value; }

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		if(SlotUIIsEmpty() == false)
		{
			itemImage.transform.SetParent(InventoryUI.DraggingItem.transform);
			itemImage.transform.localPosition = Vector3.zero;
		}
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		InventoryUI.DraggingItem.transform.position = eventData.position;
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		itemImage.transform.SetParent(borderImage.transform);
		itemImage.transform.localPosition = Vector3.zero;
		if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)InventoryUI.InventoryWindow.transform, eventData.position))
		{
			SetSlotActive();
			InventoryUI.OnDropButton();
		}
	}

	public virtual void OnDrop(PointerEventData eventData)
	{
		int draggingItemId = eventData.selectedObject.GetComponent<InventorySlotUI>().itemId;
		if (RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, Input.mousePosition) == true && draggingItemId != -1)
		{
			if (itemId == draggingItemId)
			{
				InventoryUI.CombineStacksUI(eventData.selectedObject.GetComponent<InventorySlotUI>().SlotIndex, SlotIndex); 
			}
			else
			{
				InventoryUI.SwapSlotsUI(SlotIndex, eventData.selectedObject.GetComponent<InventorySlotUI>().SlotIndex);
			}
			//moved to inventory script. (inside combinestacks and swapslots functions)
			//SetSlotActive();
		}
		//to remove focus from slot, otherwise you cannot highlight it without clicking somewhere else
		eventData.selectedObject = null;
	}

	public virtual void UpdateSlotUI(InventorySlot inventorySlot)
	{
		if (inventorySlot.Item == null)
		{
			itemId = -1;
			itemImage.sprite = emptySprite;
			CountText.text = "";
		}
		else
		{
			itemId = inventorySlot.Item.ItemId;
			itemImage.sprite = inventorySlot.Item.Icon;
			if (inventorySlot.Item.CanBeStacked)
			{
				CountText.text = "x" + inventorySlot.Amount;
			}
			else
			{
				CountText.text = "";
			}
		}
	}

	public virtual bool SlotUIIsEmpty()
	{
		return itemId == -1;
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		SetSlotActive();
		if ((eventData.button == PointerEventData.InputButton.Right) || ((eventData.button == PointerEventData.InputButton.Left) && (eventData.clickCount == 2)))
		{
			InventoryUI.UseItemUI();
			eventData.clickCount = 0;
		}
		eventData.selectedObject = null;
	}

	public virtual void SetSlotActive()
	{
		InventoryUI.ActiveSlot = this;
		borderButton.Select();
	}
}
