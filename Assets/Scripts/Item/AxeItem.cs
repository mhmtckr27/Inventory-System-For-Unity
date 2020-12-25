using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Item/Axe")]

public class AxeItem : ItemData
{
	public override void OnUse()
	{
		Debug.Log("This is an axe!");
	}
}
