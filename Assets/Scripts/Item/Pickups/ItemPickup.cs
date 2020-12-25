using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
	[SerializeField] private ItemData itemToGive;
	[SerializeField] private int amount;

	private GameManager gameManager;


	private void OnEnable()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			OnPickup();
		}
	}

	private void OnPickup()
	{
		int remaining = gameManager.GiveItemToPlayer(itemToGive, amount);
		if(remaining == amount)
		{

		}
		if (remaining != 0)
		{
			amount = remaining;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
