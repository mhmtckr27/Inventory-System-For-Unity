using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private float moveSpeed = 8;

	private void OnEnable()
	{
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
		{
            CloseInventory();
		}
		if (!inventoryWindow.activeInHierarchy)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis("Mouse X"), transform.eulerAngles.z);
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            if (Input.GetMouseButtonDown(0) && Cursor.visible)
            {
                Cursor.visible = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape) && Cursor.visible)
            {
                Cursor.visible = true;
            }
        }  
    }

    public void CloseInventory()
	{
        inventoryWindow.SetActive(!inventoryWindow.activeInHierarchy);
    }
}
