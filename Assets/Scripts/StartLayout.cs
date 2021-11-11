using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLayout : MonoBehaviour
{
	public Transform Canvas;
	public GameObject startPanel;

	private void Awake()
	{
		foreach(Transform child in Canvas)
		{
			child.gameObject.SetActive(false);
		}
		startPanel.SetActive(true);
	}
}
