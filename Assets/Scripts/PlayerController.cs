using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindowRoot;
    [SerializeField] private FreeFlyCamera freeFlyCamera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
		{
            CloseInventory();
		}
    }

    public void CloseInventory()
	{
        inventoryWindowRoot.SetActive(!inventoryWindowRoot.activeInHierarchy);
        freeFlyCamera._active = !inventoryWindowRoot.activeInHierarchy;
    }
}
