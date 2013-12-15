using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public float MoveSpeed = 100;
	public float RotationSpeed = 200;

	void Update ()
	{
		Debug.DrawRay(Map.Get2DMapCenter(), Vector3.up * 100);

		// TRANSLATION
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, MoveSpeed * Time.deltaTime);
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
			transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.right, MoveSpeed * Time.deltaTime);
		else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
			transform.position = Vector3.MoveTowards(transform.position, transform.position - Vector3.Cross(Vector3.up, transform.right), MoveSpeed * Time.deltaTime);
		else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.Cross(Vector3.up, transform.right), MoveSpeed * Time.deltaTime);

		// ZOOM
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
			Camera.main.fieldOfView = Mathf.Min(Camera.main.fieldOfView+2, 90);
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
			Camera.main.fieldOfView = Mathf.Max(Camera.main.fieldOfView-2, 10);

		// ROTATION
		if (Input.GetKey(KeyCode.E))
			RotateAroundMapCenter(-45);
	    else if (Input.GetKey(KeyCode.A))
			RotateAroundMapCenter(45);
	}

	private void RotateAroundMapCenter(float angle)
	{
		Vector3 lookAt = Map.Get2DMapCenter();
		transform.RotateAround(lookAt, Vector3.up, Time.deltaTime * angle);
	}
}