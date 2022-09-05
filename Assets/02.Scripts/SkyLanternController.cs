using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyLanternController : MonoBehaviour
{
    public bool EndOfDay;
    public GameObject myLanternPrefab;
    public Transform myLanternSpawnPoint;
    bool flag = true;

    GameObject myLantern;
    //float nextTime;

    void Start()
    {

    }


    void Update()
    {
        if (EndOfDay && flag) //���̵Ǹ� ǳ����� (TimeOfDay > 1300)
        {
            StartCoroutine(FlySkyLantern());
            flag = false;
            //nextTime += Time.deltaTime;
        }
    }

    public IEnumerator FlySkyLantern()
    {
        myLantern = Instantiate(myLanternPrefab, myLanternSpawnPoint.position, Quaternion.Euler(90, 0, 0));
        myLantern.transform.SetParent(this.gameObject.transform);


        while (true)
        {
            
            transform.Translate(Vector3.up * Time.deltaTime * 1f);
            yield return null;

            //���� �������� ǳ����� ���� (TimeOfDay > 1420�϶� endofday false)
            if (!EndOfDay)
            {
                flag = true;
                transform.localPosition = new Vector3(0, 0, 0);
                yield break;
            }
        }
    }

}
