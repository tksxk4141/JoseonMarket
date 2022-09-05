using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterJumpWithSeesaw : MonoBehaviour
{
    public GameObject otherCharacter;
    public GameObject[] players = new GameObject[2];
    public Rigidbody[] Rb = new Rigidbody[2];

    string[] tag = { "leftCharacter", "rightCharacter", "Player" };

    public GameObject[] gameObjectPoint = new GameObject[2];

    Quaternion firstAngle = Quaternion.identity;
    Quaternion secondAngle = Quaternion.identity;

    int index = 0;
    int collideCharacterFlag = 0;
    public int flag = 0;

    public float JumpPower = 5.0f;
    public int JumpHeight = 0;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        firstAngle = Quaternion.Euler(0, 0, -10);
        secondAngle = Quaternion.Euler(0, 0, 10);
    }

    void Update()
    {
        if (flag == 2)
        {
            PV.RPC("Score",RpcTarget.AllBufferedViaServer,null);
        }
    }
    public void PlayerOn(GameObject player) {
        players[index] = player;
        players[index].transform.position = gameObjectPoint[index].transform.position;
        players[index].tag = tag[index];
        Rb[index] = player.GetComponent<Rigidbody>();
        index++;
        flag++;
    }
    public void PlayerOff(GameObject player)
    {
        for (int i = 0; i < players.Length; i++) {
            if (player.tag == "leftCharacter") {
                players[i] = null;
                Rb[i] = null;
                player.tag = tag[2];
                player.transform.position = gameObjectPoint[i].transform.position;
            }
        }
        index--;
        flag--;
    }
    [PunRPC]
    public void Score()
    {
        if(JumpHeight < (int)players[0].GetComponent<Transform>().position.y)
            JumpHeight = (int)players[0].GetComponent<Transform>().position.y;
        else if (JumpHeight < (int)players[1].GetComponent<Transform>().position.y)
            JumpHeight = (int)players[1].GetComponent<Transform>().position.y;
    }
    //내가 널뛰기에 닿았을 때(착지했을 때) 반대편 플레이어를 점프시키는 코드
    private void OnCollisionEnter(Collision collision)
    {
        if (flag == 2) {//두명 다 널뛰기에 탑승했을 때부터 코드를 실행하기 위해 int형의 flag를 줌
            if (collision.gameObject.tag == "leftCharacter")
            {
                Debug.Log("LeftCharacter");
                if (collideCharacterFlag == 0)  
                    //플레이어가 널뛰기에 착지했을 때, 널뛰기가 아래로 내려가기 시작하는데,
                    //이때 플레이어도 중력을 받아 내려오며 내려가는 널뛰기와 여러번 충돌 이벤트 발생,
                    //따라서 맨 처음 착지했을 시점에 단 한번만 반대편 플레이어에게 AddForce를 하기 위해 collideCharacterFlag 줌.
                    //널뛰기가 최종 각도에 도달하면(널뛰기 한쪽끝이 바닥에 닿았을 때) collideCharacterFlag가 0으로 초기화된다. 
                {
                    Rb[1].AddForce(Vector3.up * JumpPower * 1f, ForceMode.Impulse); //내가 아닌 반대편 플레이어에게 AddForce
                    PV.RPC("PunSwingLeft",RpcTarget.AllBufferedViaServer,null); //내가 널뛰기에 착지한 순간 내 쪽 널뛰기가 아래로 내려가는 코루틴 함수 실행
                    PV.RPC("playerCheckBar", RpcTarget.AllBufferedViaServer, 0); //플레이어 위에 게이지바를 생성하는 코드.//players[0].GetComponent<cshPlayerInteraction>().StartCoroutine(players[0].GetComponent<cshPlayerInteraction>().RotateCheckBar(players[0].GetComponent<cshPlayerInteraction>().checkBar.transform.Find("CheckBar").gameObject.transform, 2*(JumpPower)/9.8f));
                                                                                 //알맞은 타이밍에 오른쪽 컨트롤러의 A버튼을 누르면 더 높이 점프한다. 
                    collideCharacterFlag++;
                }
            }
            if (collision.gameObject.tag == "rightCharacter")
            {
                Debug.Log("RightCharacter");
                if (collideCharacterFlag == 0)
                {
                    Rb[0].AddForce(Vector3.up * JumpPower * 1f, ForceMode.Impulse);
                    PV.RPC("PunSwingRight", RpcTarget.AllBufferedViaServer, null);
                    //PV.RPC("playerCheckBar", RpcTarget.AllBufferedViaServer, 1);//players[1].GetComponent<cshPlayerInteraction>().StartCoroutine(players[1].GetComponent<cshPlayerInteraction>().RotateCheckBar(players[1].GetComponent<cshPlayerInteraction>().checkBar.transform.Find("CheckBar").gameObject.transform, 1f));
                    collideCharacterFlag++;
                }
            }
        }
    }
    [PunRPC]
    public void jumpforce(float force)
    {
        JumpPower += force;
    }
    [PunRPC]
    void playerCheckBar(int playernum)
    {
        players[playernum].GetComponent<cshPlayerInteraction>().RotateBar(2 * (JumpPower) / 9.8f);//.StartCoroutine(players[playernum].GetComponent<cshPlayerInteraction>().RotateCheckBar(players[playernum].GetComponent<cshPlayerInteraction>().Bar.transform, 2 * (JumpPower) / 9.8f)) ;
    }
    [PunRPC]
    void PunSwingLeft()
    {
        StartCoroutine("SwingLeft");

    }
    [PunRPC]
    void PunSwingRight()
    {
        StartCoroutine("SwingRight");

    }
    IEnumerator SwingLeft()
    {   
        while (true)
        {
            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, secondAngle, Time.deltaTime * 3f);
            yield return null;

            if ((gameObject.transform.eulerAngles.z >= 0 && gameObject.transform.eulerAngles.z <= 9.8) 
                || (gameObject.transform.eulerAngles.z >= 350.2 && gameObject.transform.eulerAngles.z <= 360))
            {
                continue;
            }
            else
            {
                Debug.Log(gameObject.transform.eulerAngles.z);
            }
            collideCharacterFlag = 0;
            Debug.Log("Left Break");
            yield break;
        }
    }
    IEnumerator SwingRight()
    {        
        while (true)
        {
            gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, firstAngle, Time.deltaTime * 3f);
            yield return null;

            if ((gameObject.transform.eulerAngles.z >= 0 && gameObject.transform.eulerAngles.z <= 9.8) 
                || (gameObject.transform.eulerAngles.z >= 350.2 && gameObject.transform.eulerAngles.z <= 360))
            {
                continue;
            }
            else
            {
                Debug.Log(gameObject.transform.eulerAngles.z);
            }
            collideCharacterFlag = 0;
            Debug.Log("Right Break");
            yield break;
        }
    }
}