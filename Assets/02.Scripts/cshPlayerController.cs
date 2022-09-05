using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class cshPlayerController : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 2.0F;
    private Vector3 moveDirection = Vector3.zero;
    float MovSpeed = 10f;


   private void Start()
    {

    }
    void Update()
    { 
        move();
    }
    void move()
    {
        Vector2 mov2d = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 mov = new Vector3(mov2d.x * MovSpeed, 0, mov2d.y * MovSpeed);
        transform.Translate(mov * Time.deltaTime);

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Debug.Log("Jump");
            moveDirection.y = jumpSpeed;
            this.GetComponent<Rigidbody>().AddForce(0f, 0.5f, 0f, ForceMode.Impulse);
        }
    }
}
