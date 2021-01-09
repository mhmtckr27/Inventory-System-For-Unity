using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private GameObject inventoryWindow;
    float mouseX;
    float mouseY;

	private void Start()
	{
        inventoryWindow = InventoryUI.Instance.InventoryWindow;
    }

	void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

	private void LateUpdate()
	{
        if(!inventoryWindow.activeInHierarchy && (ChestUI.Instance != null) && !ChestUI.Instance.InventoryWindow.activeInHierarchy)
		{
            transform.eulerAngles = new Vector3(transform.eulerAngles.x - mouseY, transform.eulerAngles.y + mouseX, transform.eulerAngles.z);
        }
    }
}
