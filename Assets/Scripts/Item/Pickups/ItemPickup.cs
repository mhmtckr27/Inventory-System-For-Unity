using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
	[SerializeField] private ItemData itemToGive;
	[SerializeField] private int amount;


	private void OnEnable()
	{
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
		int remaining = GameManager.Instance.GiveItemToPlayer(itemToGive, amount);

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
