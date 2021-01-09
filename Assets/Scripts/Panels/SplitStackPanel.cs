using UnityEngine;
using UnityEngine.UI;

public class SplitStackPanel : PanelBase
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;
    [SerializeField] private Slider amountSlider;
    [SerializeField] private InputField inputField;
    [SerializeField] private InventoryUI inventoryUI;

    private GameObject splitStackContent;

	private void Awake()
	{
        panelType = PanelType.SplitStack;
        splitStackContent = transform.GetChild(0).gameObject;
    }
    
	private void OnEnable()
	{
        inventoryUI.RequestSplitStackPanelEvent += OnRequestSplitStackPanelEvent;
        inventoryUI.DisablePanelEvent += OnDisablePanelEvent;
        inventoryUI.ActiveSlotModifiedEvent += OnCancelButton;
    }
	private void OnDisable()
	{
        inventoryUI.RequestSplitStackPanelEvent -= OnRequestSplitStackPanelEvent;
        inventoryUI.DisablePanelEvent -= OnDisablePanelEvent;
        inventoryUI.ActiveSlotModifiedEvent -= OnCancelButton;
	}

	public void OnRequestSplitStackPanelEvent(InventorySlot slot)
    {
        itemIcon.sprite = slot.Item.Icon;
        itemName.text = slot.Item.ItemName;
        amountSlider.maxValue = slot.Amount - 1;
        amountSlider.value = 1;
        splitStackContent.SetActive(true);

        inventoryUI.ActivePanelType = panelType;
    }

    public void OnSplitButton()
    {
        inventoryUI.SplitStackUI((int)amountSlider.value);
        OnDisablePanelEvent();
    }

    private void OnDisablePanelEvent()
    {
        splitStackContent.SetActive(false);
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
