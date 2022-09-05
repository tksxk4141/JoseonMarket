using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshCheckQuiz : MonoBehaviour
{
    public GameObject quiz;
    Collision other;
    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //quiz = GameObject.FindGameObjectWithTag("quiz");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        this.other = other;
        //quiz.GetComponent<cshQuiz>().SetUpPlayersAnswer(this.gameObject, other.gameObject);
        PV.RPC("enter",RpcTarget.All,null);
    }
    void OnCollisionExit(Collision other)
    {
        this.other = other;
        PV.RPC("exit", RpcTarget.All, null);
        //quiz.GetComponent<cshQuiz>().SetUpPlayersAnswer(other.gameObject);
    }
    [PunRPC]
    void enter()
    {
        quiz.GetComponent<cshQuiz>().SetUpPlayersAnswer(this.gameObject, other.gameObject);
    }
    [PunRPC]
    void exit()
    {
        quiz.GetComponent<cshQuiz>().SetUpPlayersAnswer(other.gameObject);
    }
}
