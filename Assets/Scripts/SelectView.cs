using UnityEngine;

[RequireComponent(typeof(MouseOrbitOld))]
public class SelectView : MonoBehaviour
{
	Transform targetCamPos;
	Transform targetFocusPos;

	public Transform[] viewsPos;
	public Transform[] viewFocus;

	MouseOrbitOld mouseOrbit;

	private void Awake()
	{
		mouseOrbit = GetComponent<MouseOrbitOld>();
		//currentCamPos = viewFocus[0].transform.position;
		SetViewFront(0);
	}

	public void SetViewFront(int index)
	{
		targetCamPos = viewsPos[index];
		targetFocusPos = viewFocus[index];
		//mouseOrbit.traveling = true;
	}
}
