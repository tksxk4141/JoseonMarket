using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckArrowIn : MonoBehaviour
{
    public int arrowScore = 0;
    Canvas canvas;
    Transform pannel;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pannel = canvas.transform.Find("Pannel");


    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            arrowScore++;
        }
    }




}
