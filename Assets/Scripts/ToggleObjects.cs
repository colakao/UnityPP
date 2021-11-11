using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ToggleObjects : MonoBehaviour
{
	public bool setEnabled;
	public List<GameObject> objects = new List<GameObject>();


	public void FindObjects()
	{
		//Find objects in scene to add to list
	}

	public void EnableGameObjects(bool activate)
	{
		foreach (GameObject go in objects)
		{
			go.SetActive(activate);
		}
	}

	public void ToggleGameObjects()
	{
		foreach (GameObject go in objects)
		{
			go.SetActive(!go.activeInHierarchy);
		}
	}
}