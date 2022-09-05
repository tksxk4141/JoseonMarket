using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshNPCName : MonoBehaviour
{
    public GameObject NamePrefab;
    
    List<GameObject> npc = new List<GameObject> ();
    List<GameObject> npcName = new List<GameObject>();
    GameObject[] npcs;

    void Awake()
    {
        npcs = GameObject.FindGameObjectsWithTag("NPC");
        for (int i = 0; i < npcs.Length; i++)
        {
            npc.Add(npcs[i]);
        }
    }

    void Start()
    {
        for (int i = 0; i < npc.Count; i++)
        {
            npcName.Add(Instantiate(NamePrefab, npc[i].transform));
            npcName[i].GetComponent<TextMesh>().text = npc[i].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < npcName.Count; i++)
        {
            npcName[i].transform.rotation = this.transform.rotation;
        }
    }
}
