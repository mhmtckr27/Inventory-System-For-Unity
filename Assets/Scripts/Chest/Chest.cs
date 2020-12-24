using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour, IInteractable 
{
	[SerializeField] private int slotCount;
	
	private InventorySlot[] chestSlots;

	public Chest()
	{
		chestSlots = new InventorySlot[slotCount];
		for (int i = 0; i < slotCount; i++)
		{
			chestSlots[i] = new InventorySlot();
		}
	}


	public void Interact()
	{
        Debug.Log("Opening chest");
	}

	public void UpdateSlot(ItemData itemToAdd, int Amount)
	{

	}
}
