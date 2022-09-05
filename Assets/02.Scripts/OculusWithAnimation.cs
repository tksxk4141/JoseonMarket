using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusWithAnimation : MonoBehaviour
{

    public Animator anim;


    bool isGrounded = false;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        MovePlayer();
        CheckGround();
    }


    void MovePlayer()
    {


        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            var absX = Mathf.Abs(coord.x);
            var absY = Mathf.Abs(coord.y);

            anim.SetBool("isMoveAt", true);


            if (absX > absY)
            {
                //Right
                if (coord.x > 0)
                {
                    anim.SetFloat("MoveAt", 1.0f);
                }
                //Left
                else
                {
                    anim.SetFloat("MoveAt", 0.0f);
                }
            }
            else
            {
                //Forward
                if (coord.y > 0)
                {
                    anim.SetFloat("MoveAt", 2.0f);
                }
                //BackWard
                else
                {
                    anim.SetFloat("MoveAt", 3.0f);
                }
            }


        }
        else
        {
            anim.SetBool("isMoveAt", false);

        }

        //Jump
        //X¹öÆ°
        if (OVRInput.GetDown(OVRInput.Button.Three) && isGrounded)
        {
            this.GetComponent<Rigidbody>().AddForce(0f, 5f, 0f, ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("isjump", true);
        }
        if(OVRInput.GetUp(OVRInput.Button.Three))
        {
            anim.SetBool("isjump", false);
        }


    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .8f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

}
