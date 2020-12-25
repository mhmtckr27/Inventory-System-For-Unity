using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindowInitializer : MonoBehaviour
{
	private GameObject inventoryWindow;

	private void Awake()
	{
		inventoryWindow = transform.GetChild(0).gameObject;
		//inventoryWindow.SetActive(true);
		//inventoryWindow.SetActive(false);
	}
}
