using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestWindow : MonoBehaviour
{
	private static ChestWindow instance;
	public static ChestWindow Instance
	{
		get
		{
			return instance;
		}
	}

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
}
