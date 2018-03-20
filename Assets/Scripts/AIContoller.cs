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
        //StateMachine();

    }

    void StateMachine()
    {
        Turn(50);
    }

    /// <summary>
    /// -1 == Backwards | +1 == Forwards
    /// </summary>
    /// <param name="heading"></param>
    void Heading(float heading)
    {
        float translation = heading * SPEED;
        float strafe = 0 * SPEED;

        translation *= DELTATIME;
        strafe *= DELTATIME;

        transform.Translate(strafe, 0, translation);
    }

    /// <summary>
    /// -1 == Left | +1 == Right
    /// </summary>
    /// <param name="heading"></param>
    void Strafing(float heading)
    {
        float translation = heading * SPEED;
        float strafe = 0 * SPEED;

        translation *= DELTATIME;
        strafe *= DELTATIME;

        transform.Translate(translation, 0, strafe);
    }

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
