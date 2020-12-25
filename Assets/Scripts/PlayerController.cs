using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private GameObject chestWindow;
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private Text interactText;

    private bool inRangeOfInteractable = false;
    private Chest interactableChestInRange;

	public bool InRangeOfInteractable
    {
        get
        {
            return inRangeOfInteractable;
        }
        set
        {
            inRangeOfInteractable = value;
        }
    }
	private void OnEnable()
	{
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventoryWindow.activeInHierarchy)
		{
            CloseInventory();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (chestWindow.activeInHierarchy)
		{
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
		}
        else if (Input.GetKeyDown(KeyCode.I) && !inventoryWindow.activeInHierarchy)
        {
            OpenInventory();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        if (Input.GetMouseButtonDown(0) && Cursor.visible && !inventoryWindow.activeInHierarchy && !chestWindow.activeInHierarchy)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!inventoryWindow.activeInHierarchy && !chestWindow.activeInHierarchy)
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
			if (Input.GetKeyDown(KeyCode.E))
			{
                if (interactableChestInRange != null)
                {
                    interactableChestInRange.Interact();
                }
            }
        }
    }

    public void CloseInventory()
	{
        inventoryWindow.SetActive(false);
    }

    public void OpenInventory()
	{
        inventoryWindow.SetActive(true);
    }
    
    public void CloseChest()
	{
        chestWindow.SetActive(false);
    }

	private void OnTriggerEnter(Collider other)
	{
        Chest chest = other.gameObject.GetComponent<Chest>();
        if(chest != null)
		{
            interactText.gameObject.SetActive(true);
            InRangeOfInteractable = true;
            interactableChestInRange = other.gameObject.GetComponent<Chest>();
        }
    }

	private void OnTriggerExit(Collider other)
	{
        Chest chest = other.gameObject.GetComponent<Chest>();
        if(chest != null)
		{
            interactText.gameObject.SetActive(false);
            InRangeOfInteractable = false;
            interactableChestInRange = null;
        }
    }
}
