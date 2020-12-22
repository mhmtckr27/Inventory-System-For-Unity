using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindowRoot;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
		{
            inventoryWindowRoot.SetActive(!inventoryWindowRoot.activeInHierarchy);
		}
    }
}
