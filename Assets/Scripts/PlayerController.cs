using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float SPEED = 15.0f;
    float SHIFTSPEED = 100.0f;
    AIContoller AICONTOLLER;

    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        AICONTOLLER = GetComponent<AIContoller>();
	}
	
	void Update ()
    {
        float speed = SPEED;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = SHIFTSPEED;
        }

         float dtime = Time.deltaTime;
         float translation = Input.GetAxis("Vertical") * speed;
         float strafe = Input.GetAxis("Horizontal") * speed;
         
         translation *= dtime;
         strafe *= dtime;
         
         transform.Translate(strafe, 0, translation);
        

        if (Input.GetKeyDown("f"))
        {
            AICONTOLLER.IS_AI_ENABLED ^= true;
        }

        if (Input.GetKeyDown("q"))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
