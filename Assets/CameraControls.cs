using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
	private Transform tf;

	public float translationSpeed = 1;
	public float rotationSpeed = 0.5f;

	private Vector3 movement;
	private Vector2 mouseStart;
	private Vector2 mouseCurrent;

    void Start()
    {
		tf = transform;
    }

    void Update()
    {
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");
		movement.z = Input.GetAxis("Z-Plane");
		if(movement.magnitude != 0)
		{
			Vector3 newPos = tf.position;
			Vector3 forward = tf.forward * movement.y;
			Vector3 right = tf.right * movement.x;
			Vector3 up = tf.up * movement.z;
			newPos += (forward + right + up) * translationSpeed;
			tf.position = newPos;
		}

		bool rmbDown = Input.GetMouseButtonDown(1);
		bool rmb = Input.GetMouseButton(1);
		if(rmbDown)
		{
			mouseStart = Input.mousePosition;
		}
		if(rmb)
		{
			mouseCurrent = Input.mousePosition;
			Vector2 mouseDiff = mouseCurrent - mouseStart;
			mouseStart = mouseCurrent;
			Vector3 newRot = tf.eulerAngles;
			newRot.x -= mouseDiff.y * rotationSpeed;
			newRot.y += mouseDiff.x * rotationSpeed;
			tf.eulerAngles = newRot;
		}
    }
}
