using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private Text interactText;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private Chest testChest;

    private GameObject inventoryWindow;
    private GameObject chestWindow;
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

    private void Start()
    {
        chestWindow = ChestWindow.Instance.gameObject;
        inventoryWindow = InventoryWindow.Instance.gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventoryWindow.activeInHierarchy)
		{
            InventoryWindowRoot.Instance.ShowInventory(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (chestWindow.activeInHierarchy)
		{
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
		}
        else if (Input.GetKeyDown(KeyCode.I) && !inventoryWindow.activeInHierarchy)
        {
            InventoryWindowRoot.Instance.ShowInventory(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
        else if (chestWindow.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
		{
            ChestWindowRoot.Instance.ShowChest(false); 
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //giving item to player
        if (Input.GetKey(KeyCode.Alpha1))
        {
            GameManager.Instance.GiveItemToPlayer(Inventory.Instance.ItemDatabase.FindItem("Diamond Ore"), 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.GiveItemToPlayer(Inventory.Instance.ItemDatabase.FindItem("Silver Pickaxe"), 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.GiveItemToPlayer(Inventory.Instance.ItemDatabase.FindItem("Diamond Sword"), 1);
        }

        //giving item to chest
        if (Input.GetKey(KeyCode.Alpha4))
        {
            testChest.AddItemDull(testChest.ItemDatabase.FindItem("Diamond Ore"), 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            testChest.AddItemDull(testChest.ItemDatabase.FindItem("Silver Pickaxe"), 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            testChest.AddItemDull(testChest.ItemDatabase.FindItem("Diamond Sword"), 1);
        }
    }
}
