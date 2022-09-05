using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyLanternController1 : MonoBehaviour
{
    public bool EndOfDay;

    bool flag = true;

    float nextTime;

    void Start()
    {

    }


    void Update()
    {
        if (EndOfDay) //밤이되면 풍등날리기 (TimeOfDay > 1300)
        {
            nextTime += Time.deltaTime;
        }
        if (nextTime >= 4.0f && flag)
        {
            flag = false;
            StartCoroutine(FlySkyLantern());
        }
    }

    public IEnumerator FlySkyLantern()
    {
        while (true)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 1f);
            yield return null;

            //밤이 끝나가면 풍등날리기 종료 (TimeOfDay > 1420일때 endofday false)
            if (!EndOfDay)
            {
                flag = true;
                nextTime = 0.0f;
                transform.localPosition = new Vector3(0, -1.1f, 0);
                yield break;
            }
        }
    }

}
