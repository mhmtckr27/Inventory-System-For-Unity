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
    [SerializeField] private float interactRange = 3f;

    private bool inRangeOfInteractable = false;
    private Chest interactableChestInRange;
    float mouseX;
    float mouseY;

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
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
       // transform.forward = Camera.main.transform.forward;
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
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(Camera.main.transform.forward.x, 0,Camera.main.transform.forward.z) * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z) * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-Camera.main.transform.right.x, 0, -Camera.main.transform.right.z) * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * moveSpeed * Time.deltaTime);
            }
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if ((hit.collider != null) && hit.collider.GetComponent<Chest>() && (hit.distance < interactRange))
			{
                interactText.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
				{
                    interactableChestInRange = hit.collider.GetComponent<Chest>();
                    interactableChestInRange.Interact();
                }

            }
			else
			{
                interactText.gameObject.SetActive(false);
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
}
