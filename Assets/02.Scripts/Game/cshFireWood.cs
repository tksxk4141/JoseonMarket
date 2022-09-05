using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshFireWood : MonoBehaviour
{
    public GameObject firewoodPrefab;
    public Transform firewoodSpawnPoint;
    public GameObject[] firewoodPile;

    public GameObject firewood;

    public int firewoodPileIndex = 0;
    public bool isfirewood = false;
    public bool isPlayerOn = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerOn(GameObject player)
    {
        isPlayerOn = true;
    }

    void SpawnFireWood()
    {
        firewood = Instantiate(firewoodPrefab, firewoodSpawnPoint.position, Quaternion.Euler(-90, 0, 0));
        firewood.transform.SetParent(firewoodSpawnPoint, false);
        firewood.transform.position = firewoodSpawnPoint.position;
    }

    public IEnumerator WaitToWood(float timeToWait)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToWait;
            yield return null;
        }
        Destroy(firewood);
        isfirewood = false;
        if (firewoodPileIndex == 6)
        {
            firewoodPileIndex = 0;
        }
        else
        {
            firewoodPile[firewoodPileIndex].SetActive(true);
            firewoodPileIndex++;
        }
        SpawnFireWood();
    }
}
