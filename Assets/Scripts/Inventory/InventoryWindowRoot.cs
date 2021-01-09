using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindowRoot : MonoBehaviour
{
	private static InventoryWindowRoot instance;
	public static InventoryWindowRoot Instance
	{
		get
		{
			return instance;
		}
	}

	private GameObject inventoryWindow;

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
	}

	private void Start()
	{
		inventoryWindow = InventoryWindow.Instance.gameObject;
	}
	public void ShowInventory(bool show)
	{
		inventoryWindow.SetActive(show);
	}
}
