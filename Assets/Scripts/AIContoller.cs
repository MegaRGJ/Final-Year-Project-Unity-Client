using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Actions
{
    public bool forward;
    public bool backwards;
    public bool left;
    public bool right;
    public bool rotateLeft;
    public bool rotateRight;

    public Actions(bool defaults)
    {
        forward = defaults;
        backwards = defaults;
        left = defaults;
        right = defaults;
        rotateLeft = defaults;
        rotateRight = defaults;
    }
}

public class AIContoller : MonoBehaviour
{

    bool IS_AI_ENABLED = true;
    float SPEED = 15.0f;
    private float DELTATIME;
    Actions ACTIONS;

    float TIME_BETWEEN_MOVEMENT;
    float LAST_MOVEMENT_DECISION;
    float TIME_BETWEEN_ROTATION;
    float LAST_ROTATION_DECISION;

    float ROTATE_LEFT_SPEED;
    float ROTATE_RIGHT_SPEED;

    void Start ()
    {
        ACTIONS = new Actions(false);
    }
	
	void Update ()
    {
        if (IS_AI_ENABLED)
        {
            DELTATIME = Time.deltaTime;

            ACTIONS = ActionDecider(ACTIONS);
            ActionProcesser(ACTIONS);
        }
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
    /// -1 == Left | +1 == Right
    /// </summary>
    /// <param name="rotationAmount"></param>
    void Rotate(float rotationAmount) 
    {
        transform.Rotate(Vector3.up, rotationAmount * DELTATIME);
    }

    float RNGRotationSpeed()
    {
        return Random.Range(10.0f, 300.0f);
    }

    float RNGActionTime()
    {
        return Random.Range(0.3f, 4.0f);
    }
    
    float RNGRotationTime()
    {
        return Random.Range(0.5f, 1.0f);
    }

    Actions ActionDecider(Actions actions)
    {

        if (LAST_MOVEMENT_DECISION + TIME_BETWEEN_MOVEMENT <= Time.time)
        {
            LAST_MOVEMENT_DECISION = Time.time;
            TIME_BETWEEN_MOVEMENT = RNGActionTime();

            if (Chance(80))
            {
                actions.forward = true;
            }
            else
            {
                actions.forward = false;
            }
        }

        if (LAST_ROTATION_DECISION + TIME_BETWEEN_ROTATION <= Time.time)
        {
            LAST_ROTATION_DECISION = Time.time;
            TIME_BETWEEN_ROTATION = RNGActionTime();

            if (Chance(30))
            {
                if (Chance(50))
                {
                    actions.rotateRight = false;
                    actions.rotateLeft = true;
                    ROTATE_LEFT_SPEED = RNGRotationSpeed();
                }
                else
                {
                    actions.rotateLeft = false;
                    actions.rotateRight = true;
                    ROTATE_RIGHT_SPEED = RNGRotationSpeed();
                }
            }
            else
            {
                actions.rotateLeft = false;
                actions.rotateRight = false;
            }
        }

        return actions;
    }

    bool Chance(float chance)
    {
        int num = Random.Range(1, 100);

        if (num <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ActionProcesser(Actions actions)
    {
        if (actions.forward)
        {
            Heading(1);
        }
        if (actions.backwards)
        {
            Heading(-1);
        }
        if (actions.left)
        {
            Strafing(-1);
        }
        if (actions.right)
        {
            Strafing(1);
        }
        if (actions.rotateLeft)
        {
            Rotate(-ROTATE_LEFT_SPEED);
        }
        if (actions.rotateRight)
        {
            Rotate(ROTATE_RIGHT_SPEED);
        }
    }
}
