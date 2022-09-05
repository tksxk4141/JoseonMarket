using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndThrow : MonoBehaviour
{
    List<GameObject> objects = new List<GameObject>();

    bool isHolding = false;

    
    void Start()
    {
        
    }


    void Update()
    {
        if(OVRInput.Get(OVRInput.RawButton.RHandTrigger) && !isHolding)
        {
            isHolding = true;
            foreach (GameObject go in objects)
            {
                go.transform.parent = transform;
                if (go.GetComponent<Rigidbody>())
                {
                    Rigidbody rb = go.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }
        } else if(!OVRInput.Get(OVRInput.RawButton.RHandTrigger) && isHolding)
        {
            isHolding = false;
            foreach (GameObject go in objects)
            {
                if (go.GetComponent<Rigidbody>())
                {
                    Rigidbody rb = go.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    //이거 안되면 addforce 하기
                    rb.velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);

                    go.GetComponent<ArrowRotation>().StartRotateArrow();

                }
            }

            transform.DetachChildren();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        objects.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        objects.Remove(other.gameObject);
    }
}
