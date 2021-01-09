using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestWindowRoot : MonoBehaviour
{
	private static ChestWindowRoot instance;
	public static ChestWindowRoot Instance
	{
		get
		{
			return instance;
		}
	}

	private GameObject chestWindow;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		chestWindow = ChestWindow.Instance.gameObject;
	}

	public void ShowChest(bool show)
	{
		chestWindow.SetActive(show);
	}
}
