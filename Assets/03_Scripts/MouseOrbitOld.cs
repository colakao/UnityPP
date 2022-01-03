using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class MouseOrbitOld : MonoBehaviour
{
	Transform targetCamTransform;
	Transform targetFocusTransform;

	private Vector3 currentCamPos;
	private Vector3 currentFocusPos;

	public float maxOffsetDistance = 2000f;
	public float orbitSpeed = 15f;
	public float panSpeed = .5f;
	public float zoomSpeed = 10f;


	public Transform[] viewsPos;
	public Transform[] viewFocus;
	public float transSpeed = 1;

	bool traveling;

	// touch info

	public float distance = 5.0f;
	public float maxDistance = 200;
	public float minDistance = 10;
	public float touchOrbSpeed = 0.03f;
	public int yMinLimit = -60;
	public int yMaxLimit = 60;
	public float touchZoomRate = 0.001f;
	public float panSpeedTouch = 0.01f;
	public float zoomDampening = 5.0f;

	//private float xDeg = 0.0f;
	//private float yDeg = 0.0f;

	Vector3 touchStart;

	private float firstClickTime, timeBetweenClicks;
	private bool coroutineAllowed;
	private int clickCounter;

	Transform puntoTarget;
	public Transform camNewTarget;
	float panSpeedScaled;
	float camDist;

	bool draggingUI;
	public bool simularTouchConMouse;

	Vector3 desiredDistance;
	Vector3 currentDistance;
	Vector3 lastOffset;
	Vector3 targetOffset;
	float delta;
	public bool debug;

	void Start()
	{
		Initialize();
		currentCamPos = viewFocus[0].transform.position;
		SetViewFront(0);

		if (debug) 
		{
			foreach (Transform t in viewFocus)
			{
				Debug.Log(t.position);
			}
		}
	}

	private void Initialize()
	{
		firstClickTime = 0;
		timeBetweenClicks = 0.2f;
		clickCounter = 0;
		coroutineAllowed = true;
	}

	void Update()
	{
		CamDistance();
		//CheckDoubleclick();
		DraggingUI();
		if (!draggingUI)
			MouseControls();
		//TouchControls();
	}


	private void CamDistance()
	{
		camDist = Vector3.Distance(transform.position, targetFocusTransform.position);
		//print(camDist);
		//if (camDist > maxDistance)
		//{
		//	var offset = camDist - maxDistance;
		//	transform.position = -offset * transform.forward;
		//}
		//else if (camDist < minDistance)
		//{
		//	var offset = minDistance - camDist;
		//	transform.position = offset * transform.forward;
		//}
	}

	private void DraggingUI()
	{
		Input.simulateMouseWithTouches = simularTouchConMouse;
		if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject())
			draggingUI = true;
		else if (Input.GetMouseButton(1) && EventSystem.current.IsPointerOverGameObject())
			draggingUI = true;
		else if (Input.mouseScrollDelta.magnitude > 0.1f && EventSystem.current.IsPointerOverGameObject())
			draggingUI = true;
		else if (!EventSystem.current.IsPointerOverGameObject() && !Input.anyKey)
				draggingUI = false;
		//else
		//	draggingUI = false;
	}

	void CheckDoubleclick()
	{
		if (Input.GetMouseButtonUp(0))
		{
			clickCounter += 1;
		}
		if (clickCounter == 1 && coroutineAllowed)
		{
			firstClickTime = Time.time;
			StartCoroutine(DoubleClickDetection());
		}
	}

	private IEnumerator DoubleClickDetection()
	{
		coroutineAllowed = false;
		while (Time.time < firstClickTime + timeBetweenClicks)
		{
			if (clickCounter == 2)
			{
				CloseUpView();
				break;
			}
			yield return new WaitForEndOfFrame();
		}

		clickCounter = 0;
		firstClickTime = 0f;
		coroutineAllowed = true;
	}

	private void LateUpdate()
	{
		if (traveling)		
			GoToView();
	}

	public void SetViewFront(int index)
	{
		//currentCamPos = viewsPos[index].position;
		targetCamTransform = viewsPos[index];
		targetFocusTransform = viewFocus[index];
		traveling = true;
	}

	private void GoToView()
	{
		transform.position = Vector3.Lerp(transform.position, targetCamTransform.position, Time.deltaTime * transSpeed);
		currentCamPos = Vector3.Lerp(currentCamPos, targetFocusTransform.position, Time.deltaTime * transSpeed * 2);
		transform.LookAt(currentCamPos);
	}

	public void CloseUpView()
	{
		print("CloseUpView called.");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300f) && hit.collider)
		{
			print("Point: " + hit.point);
			print("Transform: " + hit.transform.position);
			targetFocusTransform.position = hit.point;
			Vector3 auxPos = Vector3.Lerp(transform.position, targetFocusTransform.position, 0.85f);
			targetCamTransform.position = auxPos;
			traveling = true;
		}
		//transform.position =  Vector3.Lerp(transform.position, targetFocusPos.position, 0.9f);

	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}

	public void SetTraveling(bool isTravelling)
	{
		traveling = isTravelling;
	}

	#region Touch

	private void TouchControls()
	{

		if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider != null)
					{
						//hit.collider.GetComponent<DisplayUI>().SelectPiece();
					}
				}
			}
		}
		//Zoom

		if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Moved && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			traveling = false;
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
			float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

			float targetDist = Vector3.Distance(transform.position, targetFocusTransform.position);

			if (targetDist > minDistance)
			{
				transform.position += transform.forward * deltaMagDiff * touchZoomRate * targetDist;
			}

			Vector2 centerPos = touchOnePreviousPosition - touchZeroPreviousPosition;
			Vector2 centerDelta = touchOne.deltaPosition;

			Vector2 touchMovem = touchZero.deltaPosition + touchOne.deltaPosition;

			Vector3 camMove = transform.right * touchMovem.x * panSpeedTouch + transform.up * touchMovem.y * panSpeedTouch;
			transform.position += camMove * 0.1f;
			targetFocusTransform.position += camMove * 0.1f;

			if (Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				return;
			}
		}


		//  ORBIT
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			Vector2 touchposition = Input.GetTouch(0).deltaPosition;
			//xDeg += touchposition.x * 0.01f;
			//yDeg -= touchposition.y * 0.01f;
			//yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

			float xAng = Input.GetTouch(0).deltaPosition.x;
			float yAng = Input.GetTouch(0).deltaPosition.y;
			float absXAng = Math.Abs(xAng);
			if (absXAng > 0.1)
			{
				traveling = false;
			}

			transform.RotateAround(targetFocusTransform.position, Vector3.down, xAng * touchOrbSpeed);
			float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);
			float pitchDelta = -yAng * touchOrbSpeed;
			float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 10f, 170f);
			pitchDelta = newAngle - pitchAngle;
			transform.RotateAround(currentCamPos, -transform.right, pitchDelta);
		}
	}
	#endregion

	#region Mouse
	private void MouseControls()
	{
		// Left Mouse to Orbit
		#region Orbit
		if (Input.GetMouseButton(0))
		{
			float mouseMove = Math.Abs(Input.GetAxis("Mouse X"));
			if (mouseMove > 0.02f)
			{
				traveling = false;
			}
			transform.RotateAround(currentCamPos, Vector3.up, Input.GetAxis("Mouse X") * orbitSpeed);
			float pitchAngle = Vector3.Angle(Vector3.up, transform.forward);

			float pitchDelta = -Input.GetAxis("Mouse Y") * orbitSpeed;

			float newAngle = Mathf.Clamp(pitchAngle + pitchDelta, 0f, 180f);
			pitchDelta = newAngle - pitchAngle;
			transform.RotateAround(currentCamPos, transform.right, pitchDelta);
			//transform.RotateAround(targetFocusPos.position, transform.right, pitchDelta);

		}
		#endregion

		#region Panning
		// Right Mouse To Pan
		if (Input.GetMouseButton(1))
		{
			traveling = false;
			panSpeedScaled = panSpeed * camDist * 0.05f;

			Vector3 offset = transform.right * -Input.GetAxis("Mouse X") * panSpeedScaled + transform.up * -Input.GetAxis("Mouse Y") * panSpeedScaled;

			transform.position += offset;
			currentCamPos += offset;
		}
		#endregion

		#region Zoom
		// Scroll to Zoom        
		if (camDist >= minDistance)
		{
			transform.position += camDist / 2 * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * transform.forward;

			if (Input.mouseScrollDelta.y > 0.01f || Input.mouseScrollDelta.y < -0.01f)
				traveling = false;
			
		}
		else
			transform.position += camDist * Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel"), Mathf.NegativeInfinity, 0) * zoomSpeed * transform.forward;

		#endregion
	}
	#endregion
}