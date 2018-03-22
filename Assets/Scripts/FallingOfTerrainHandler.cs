using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOfTerrainHandler : MonoBehaviour
{
    public GameObject ObjectToSpawnOn;
    public float FallDistance = -10;

    Vector3 ObjectToSpawnOnSize;
    Vector3 ObjectToSpawnOnPosition;
    Vector3 Size;

    void Start ()
    {
        if (ObjectToSpawnOn == null)
        {
            ObjectToSpawnOn = GameObject.Find("Teleport Cube");
        }

        ObjectToSpawnOnSize = ObjectToSpawnOn.GetComponent<BoxCollider>().bounds.size;
        ObjectToSpawnOnPosition = ObjectToSpawnOn.GetComponent<Transform>().position;
        Size = GetComponent<BoxCollider>().bounds.size;
    }
	
	void Update ()
    {
        if (transform.position.y < FallDistance)
        {
            float y = (ObjectToSpawnOnSize.y / 2) + (Size.y / 2);

            transform.position = new Vector3(ObjectToSpawnOnPosition.x, y, ObjectToSpawnOnPosition.z);
        }
	}
}
