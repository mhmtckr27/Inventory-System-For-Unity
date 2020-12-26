using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Text pickupMessage;
	
	private Inventory playerInventory;

	private static GameManager instance;
	public static GameManager Instance { get => instance; }

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		playerInventory = Inventory.Instance;
	}

	public int GiveItemToPlayer(ItemData itemToGive, int amount)
	{
		if(itemToGive == null)
		{
			return -1;
		}
		int remaining = playerInventory.AddItemDull(itemToGive, amount);

		if(remaining == amount)
		{
			StartCoroutine("OnPickupCoroutine", "Inventory full!");
		}
		else
		{
			StartCoroutine("OnPickupCoroutine", "Added " + (amount - remaining) + " " + itemToGive.ItemName);
		}
		return remaining;
	}

	IEnumerator OnPickupCoroutine(string msg)
	{
		pickupMessage.text = msg;
		pickupMessage.gameObject.SetActive(true);
		yield return new WaitForSeconds(1f);
		pickupMessage.text = "";
		pickupMessage.gameObject.SetActive(false);
	}
}
