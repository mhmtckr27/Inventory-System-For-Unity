using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private Inventory playerInventory;

	private void Awake()
	{
		playerInventory = FindObjectOfType<Inventory>();
	}

	public int GiveItemToPlayer(ItemData itemToGive, int amount)
	{
		return playerInventory.AddItemDull(itemToGive, amount);
	}
}
