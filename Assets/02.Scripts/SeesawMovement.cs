using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawMovement : MonoBehaviour
{
    public GameObject seesaw;
    Quaternion firstAngle = Quaternion.identity;
    Quaternion secondAngle = Quaternion.identity;
    float seesawFlag = 0;
    int seesawCount = 0;
    float firstAngleX;
    float firstAngleY;
    float firstAngleZ;

    float secondAngleX;

    float lerpTime;

    void Start()
    {

        firstAngleX = this.transform.rotation.x;
        firstAngleY = this.transform.rotation.y;
        firstAngleZ = this.transform.rotation.z;
        lerpTime = 0.0f;
    }


    void Update()
    {
        

        firstAngle = Quaternion.Euler(firstAngleX, firstAngleY, firstAngleZ-18);

        secondAngle = Quaternion.Euler(firstAngleX, firstAngleY, firstAngleZ+18);

        if (seesawCount == 0)
        {
            seesaw.transform.localRotation = Quaternion.Slerp(seesaw.transform.localRotation, secondAngle, Time.deltaTime * 1f);
            seesawFlag += Mathf.Lerp(0, (firstAngleZ + 18), (Time.deltaTime * 1f));
            if (seesawFlag >= firstAngleZ + 18) {
                seesawCount++;
            }
        }
        if (seesawCount == 1)
        {
            seesaw.transform.localRotation = Quaternion.Slerp(seesaw.transform.localRotation, firstAngle, Time.deltaTime * 1f);
            seesawFlag -= Mathf.Lerp(0, (firstAngleZ + 18), (Time.deltaTime * 1f));
            if (seesawFlag <= firstAngleZ - 18)
            {
                seesawCount=0;
            }
        }
        /*
        if (Input.GetKey(KeyCode.Space)){
            //StartCoroutine("swing");

            if (seesawFlag == 0)
            {
                seesaw.transform.localRotation = Quaternion.Slerp(seesaw.transform.localRotation, secondAngle, Time.deltaTime * 0.1f);
                seesawFlag++;
            }
            if (seesawFlag == 1)
            {
                seesaw.transform.localRotation = Quaternion.Slerp(seesaw.transform.localRotation, firstAngle, Time.deltaTime);
                seesawFlag = 0;
            }
        }*/
        


    }


    IEnumerator swing()
    {
        while (true)
        {
            if (lerpTime > 1.0f) { yield break; }
            lerpTime += Time.deltaTime;

            seesaw.transform.localRotation = Quaternion.Slerp(seesaw.transform.localRotation, secondAngle, Time.deltaTime * 0.1f);
            yield return null;

        }
    }
}
