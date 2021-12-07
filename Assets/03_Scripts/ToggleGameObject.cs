using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
	public void ToggleActive(GameObject gameObject)
	{
		gameObject.SetActive(!gameObject.activeInHierarchy);
	}
}
