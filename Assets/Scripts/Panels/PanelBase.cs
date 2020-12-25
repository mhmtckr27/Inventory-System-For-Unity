using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
	protected PanelType panelType;
}

public enum PanelType
{
	ItemInfo,
	DropItem,
	SplitStack,
	Empty
}