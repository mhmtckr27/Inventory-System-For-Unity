using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerUpHandler
{
	public Button Button { get; set; }
	public Text ButtonText { get; set; }

	private void Awake()
	{
		Button = GetComponent<Button>();
		ButtonText = GetComponentInChildren<Text>();
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		eventData.selectedObject = null;
	}

	public void SetText(string newText)
	{
		ButtonText.text = newText;
	}

	public void SetInteractable(bool newVal)
	{
		Button.interactable = newVal;
	}
}