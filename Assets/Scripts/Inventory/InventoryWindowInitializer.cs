using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindowInitializer : MonoBehaviour
{
	private GameObject inventoryWindow;

	private void Awake()
	{
		inventoryWindow = transform.GetChild(0).gameObject;

	}
	private void Start()
	{
		inventoryWindow.SetActive(true);
		Invoke("DisableWindow", Time.deltaTime * 2);
	}

	private void DisableWindow()
	{
		inventoryWindow.SetActive(false);
	}
}
