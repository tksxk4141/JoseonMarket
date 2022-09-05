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
    //���� �ζٱ⿡ ����� ��(�������� ��) �ݴ��� �÷��̾ ������Ű�� �ڵ�
    private void OnCollisionEnter(Collision collision)
    {
        if (flag == 2) {//�θ� �� �ζٱ⿡ ž������ ������ �ڵ带 �����ϱ� ���� int���� flag�� ��
            if (collision.gameObject.tag == "leftCharacter")
            {
                Debug.Log("LeftCharacter");
                if (collideCharacterFlag == 0)  
                    //�÷��̾ �ζٱ⿡ �������� ��, �ζٱⰡ �Ʒ��� �������� �����ϴµ�,
                    //�̶� �÷��̾ �߷��� �޾� �������� �������� �ζٱ�� ������ �浹 �̺�Ʈ �߻�,
                    //���� �� ó�� �������� ������ �� �ѹ��� �ݴ��� �÷��̾�� AddForce�� �ϱ� ���� collideCharacterFlag ��.
                    //�ζٱⰡ ���� ������ �����ϸ�(�ζٱ� ���ʳ��� �ٴڿ� ����� ��) collideCharacterFlag�� 0���� �ʱ�ȭ�ȴ�. 
                {
                    Rb[1].AddForce(Vector3.up * JumpPower * 1f, ForceMode.Impulse); //���� �ƴ� �ݴ��� �÷��̾�� AddForce
                    PV.RPC("PunSwingLeft",RpcTarget.AllBufferedViaServer,null); //���� �ζٱ⿡ ������ ���� �� �� �ζٱⰡ �Ʒ��� �������� �ڷ�ƾ �Լ� ����
                    PV.RPC("playerCheckBar", RpcTarget.AllBufferedViaServer, 0); //�÷��̾� ���� �������ٸ� �����ϴ� �ڵ�.//players[0].GetComponent<cshPlayerInteraction>().StartCoroutine(players[0].GetComponent<cshPlayerInteraction>().RotateCheckBar(players[0].GetComponent<cshPlayerInteraction>().checkBar.transform.Find("CheckBar").gameObject.transform, 2*(JumpPower)/9.8f));
                                                                                 //�˸��� Ÿ�ֿ̹� ������ ��Ʈ�ѷ��� A��ư�� ������ �� ���� �����Ѵ�. 
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