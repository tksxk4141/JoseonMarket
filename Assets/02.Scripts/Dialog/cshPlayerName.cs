using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cshPlayerName : MonoBehaviour
{
    public GameObject NamePrefab;

    List<GameObject> npc = new List<GameObject>();
    List<GameObject> npcName = new List<GameObject>();
    GameObject[] npcs;

    List<GameObject> player = new List<GameObject>();
    List<GameObject> playerName = new List<GameObject>();
    GameObject[] players;

    void Awake()
    {
        SearchNPC();
        SearchPlayer();
    }

    void Start()
    {
        DisplayNPCName();
        DisplayPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < npcName.Count; i++)
        {
            npcName[i].transform.rotation = this.transform.rotation;
        }
        for (int i = 0; i < playerName.Count; i++)
        {
            playerName[i].transform.rotation = this.transform.rotation;
        }
    }

    void SearchNPC()
    {
        npcs = GameObject.FindGameObjectsWithTag("NPC");
        npc.Clear();
        for (int i = 0; i < npcs.Length; i++)
        {
            npc.Add(npcs[i]);
        }
    }
    void DisplayNPCName()
    {
        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].name.Contains("ÆØ»ï")|| npc[i].name.Contains("¼øº¹")|| npc[i].name.Contains("¼¼¼÷"))
                npcName.Add(Instantiate(NamePrefab, npc[i].GetComponent<cshHead>().Head));
            else if(npc[i].name.Contains("³°Àº °Ô½ÃÆÇ"))
            {
                npcName.Add(Instantiate(NamePrefab, npc[i].GetComponent<cshHead>().Head));
                npcName[i].transform.localScale = new Vector3(1f,1f,1f);
            }
            else
               npcName.Add(Instantiate(NamePrefab, npc[i].transform));

            npcName[i].GetComponentInChildren<TextMeshProUGUI>().text = npc[i].name;
            npcName[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
        }
    }
    public void SearchPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        player.Clear();
        if (players.Length != 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                player.Add(players[i]);
            }
        }
    }
    public void DisplayPlayerName()
    {
        for (int i = 0; i < player.Count; i++)
        {
            playerName.Add(Instantiate(NamePrefab, player[i].transform));
            playerName[i].GetComponentInChildren<TextMeshProUGUI>().text = player[i].name;
        }
    }
}
