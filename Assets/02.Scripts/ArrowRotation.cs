using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRotateArrow()
    {
        StartCoroutine(RotateArrow());
    }

    public IEnumerator RotateArrow()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        while (true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), Mathf.Tan(Time.deltaTime));

            yield return null;

            if(transform.eulerAngles.x >= 88)
            {
                yield break;
            }
        }

        
    }


    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
    }
}
