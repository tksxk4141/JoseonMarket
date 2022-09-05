using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class cshFireWoodAxe : MonoBehaviour
{
    public GameObject FireWoodGame;

    public GameObject firewood;
    public bool isfirewood = false;
    public bool isPlayerOn = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        firewood = FireWoodGame.GetComponent<cshFireWood>().firewood;
        isfirewood = FireWoodGame.GetComponent<cshFireWood>().isfirewood;
        isPlayerOn = FireWoodGame.GetComponent<cshFireWood>().isPlayerOn;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isPlayerOn&&!isfirewood && collider.gameObject.CompareTag("firewood"))
        {
            FireWoodGame.GetComponent<cshFireWood>().isfirewood = true;
            isfirewood = true;
            firewood.GetComponent<RayfireRigid>().Initialize();
            StartCoroutine(FireWoodGame.GetComponent<cshFireWood>().WaitToWood(2f));
        }
    }
}