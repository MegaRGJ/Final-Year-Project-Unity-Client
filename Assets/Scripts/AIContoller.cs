using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIContoller : MonoBehaviour
{
    bool ISAIENABLED = true;
    float SPEED = 15.0f;
    private float DELTATIME;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        DELTATIME = Time.deltaTime;

        MoveForward();
        Turn(50);
    }

    void MoveForward()
    {
        float translation = 1 * SPEED;
        float strafe = 0 * SPEED;

        translation *= DELTATIME;
        strafe *= DELTATIME;

        transform.Translate(strafe, 0, translation);
    }

    void MoveBackwards()
    {

    }

    //void TurnRight(float rotationAmount)
    //{
    //    float angle;
    //    Vector3 way;

    //    transform.rotation.ToAngleAxis(out angle, out way);

    //    if (angle > (360 - rotationAmount)) // caused some odd bug without
    //    {
    //        angle = 0; // 0 == 360
    //    }

    //    transform.localRotation = Quaternion.AngleAxis(angle + rotationAmount, way);
    //}

    /// <summary>
    /// - == Left | + == Right
    /// </summary>
    /// <param name="rotationAmount"></param>
    void Turn(float rotationAmount) 
    {
        transform.Rotate(Vector3.up, rotationAmount * DELTATIME);
    }

    float RotationAmountRNG()
    {
        return Random.Range(-500.0f, 500.0f);
    }

    float StateTimeAmountRNG()
    {
        return Random.Range(0.3f, 4.0f);
    }
}
