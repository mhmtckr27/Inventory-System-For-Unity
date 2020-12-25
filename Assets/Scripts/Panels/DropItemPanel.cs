using UnityEngine;
using UnityEngine.UI;

public class DropItemPanel : PanelBase
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;
    [SerializeField] private Slider amountSlider;
    [SerializeField] private InputField inputField;
    [SerializeField] private InventoryUI inventoryUI;

    private GameObject dropItemContent;

	private void Awake()
	{
        panelType = PanelType.DropItem;	
        dropItemContent = transform.GetChild(0).gameObject;
    }

	private void Start()
	{
       
    }
	private void OnEnable()
	{
        inventoryUI.RequestDropItemPanelEvent += OnRequestDropItemPanelEvent;
        inventoryUI.DisablePanelEvent += OnDisablePanelEvent;
        inventoryUI.ActiveSlotModifiedEvent += OnCancelButton;
    }
	private void OnDisable()
	{
        inventoryUI.RequestDropItemPanelEvent -= OnRequestDropItemPanelEvent;
        inventoryUI.DisablePanelEvent -= OnDisablePanelEvent;
        inventoryUI.ActiveSlotModifiedEvent += OnCancelButton;
    }

    public void OnRequestDropItemPanelEvent(InventorySlot slot)
    {
        if(slot.Item == null)
		{
            //return;
		}
        amountSlider.gameObject.SetActive(slot.Item.CanBeStacked && slot.Amount > 1);
        inputField.gameObject.SetActive(slot.Item.CanBeStacked && slot.Amount > 1);
        itemIcon.sprite = slot.Item.Icon;
        itemName.text = slot.Item.ItemName;
        amountSlider.maxValue = slot.Amount;
        amountSlider.value = 1;
        dropItemContent.SetActive(true);

        inventoryUI.ActivePanelType = panelType;
    }

    public void OnDropButton()
    {
        inventoryUI.DropItemUI((int)amountSlider.value);
        OnDisablePanelEvent();
    }

    public void OnDisablePanelEvent()
    {
        dropItemContent.SetActive(false);
    }

    public void OnCancelButton(int dummy)
	{
        OnDisablePanelEvent();
        inventoryUI.ActivePanelType = PanelType.Empty;
	}

    public void OnAmountChanged(float newValue)
    {
        if(inputField.text != "")
        {
            inputField.text = newValue.ToString();
        }
    }
    public void OnAmountChanged(string newValue)
    {
        if(newValue != "")
        {
            amountSlider.value = int.Parse(newValue);
        }
        else
        {
            amountSlider.value = amountSlider.minValue;
        }
    }

    public void OnTextFieldEndEdit()
    {
        if(inputField.text == "")
        {
            inputField.text = amountSlider.minValue.ToString();
        }
    }
}
