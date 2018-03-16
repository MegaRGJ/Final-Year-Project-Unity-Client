using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    GameObject CHARACTER;

    Vector2 MOUSELOOK;
    Vector2 SMOOTH;

    public float SENSITIVITY = 5.0f;
    public float SMOOTHING = 2.0f;

	void Start ()
    {
        CHARACTER = transform.parent.gameObject;
    }
	
	void Update ()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(SENSITIVITY * SMOOTHING, SENSITIVITY * SMOOTHING));

        SMOOTH.x = Mathf.Lerp(SMOOTH.x, mouseDelta.x, 1f / SMOOTHING);
        SMOOTH.y = Mathf.Lerp(SMOOTH.y, mouseDelta.y, 1f / SMOOTHING);
        MOUSELOOK += SMOOTH;

        MOUSELOOK.y = Mathf.Clamp(MOUSELOOK.y, -90.0f, 90.0f); // Stops the camera from going to far up or down.

        transform.localRotation = Quaternion.AngleAxis(-MOUSELOOK.y, Vector3.right);
        CHARACTER.transform.localRotation = Quaternion.AngleAxis(MOUSELOOK.x, CHARACTER.transform.up);
    }
}
