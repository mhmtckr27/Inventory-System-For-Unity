using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
	private int equipmentSlotCount = 3;
    private EquipmentSlot[] equipmentSlots;

	private void Awake()
	{
		InitEquipmentSlots();
	}

	private void InitEquipmentSlots()
	{
		equipmentSlots = new EquipmentSlot[equipmentSlotCount];
		equipmentSlots[0] = new EquipmentSlot(ItemCategory.Armor);
		equipmentSlots[1] = new EquipmentSlot(ItemCategory.Weapon);
		equipmentSlots[2] = new EquipmentSlot(ItemCategory.Quest);
	}
}
