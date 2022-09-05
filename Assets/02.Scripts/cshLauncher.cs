using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using TMPro;

public class cshLauncher : MonoBehaviourPunCallbacks
{
    public static cshLauncher Instance;

    [SerializeField] TMP_Text errorText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject startGameButton;
    public GameObject Screen;
    [SerializeField] GameObject RoomManager;
    [SerializeField] TMP_Text userName;

    public GameObject CharacterImageHolder;
    public Sprite[] playerCharacterImage;
    int chrindex=0;

    void Awake()
    {
        Instance = this;//�޼���� ���
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        if (PhotonNetwork.IsConnected == false)
            PhotonNetwork.ConnectUsingSettings();//������ ���� ������ ���� ������ ������ ����
    }

    void Update()
    {

    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();//������ ���� ������ ���� ������ ������ ����
    }

    public override void OnConnectedToMaster()//�����ͼ����� ����� �۵���
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();//������ ���� ����� �κ�� ����
        PhotonNetwork.AutomaticallySyncScene = true;//�ڵ����� ��� ������� scene�� ���� �����ش�. 
    }

    public override void OnJoinedLobby()//�κ� ����� �۵�
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = cshLoginValue.username;
        userName.text = PhotonNetwork.NickName;
    }
    public void CreateRoom()//�游���
    {
        int rnd = Random.Range(0,100);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(rnd.ToString(), roomOptions); //���� ��Ʈ��ũ������� roomNameInputField.text�� �̸����� ���� �����.
        Debug.Log("Create Room");
    }

    public override void OnJoinedRoom()//�濡 ������ �۵�
    {
        Debug.Log("Joined Room");
        Instantiate(RoomManager);
        StartGame();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)//������ ������ ������ �ٲ������
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//���常 ���ӽ��� ��ư ������ ����
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//�� ����� ���н� �۵�
    {
        errorText.text = "Room Creation Failed: " + message;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(2);//1�� ������ ���忡�� scene ��ȣ�� 1�����̱� �����̴�. 0�� �ʱ� ��.
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
        GameObject.Find("Top Panel").transform.Find("Button List").gameObject.SetActive(true);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//���� ��Ʈ��ũ�� JoinRoom��� �ش��̸��� ���� ������ �����Ѵ�.
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnLeftRoom()//���� ������ ȣ��
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(1);
            return;
        }
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//������ �� ����Ʈ ���
    {
        foreach (Transform trans in roomListContent)//�����ϴ� ��� roomListContent
        {
            Destroy(trans.gameObject);//�븮��Ʈ ������Ʈ�� �ɶ����� �������
        }
        for (int i = 0; i < roomList.Count; i++)//�氹����ŭ �ݺ�
        {
            if (roomList[i].RemovedFromList)//����� ���� ��� ���Ѵ�. 
                continue;
            //Instantiate(roomListItemPrefab, roomListContent).GetComponent<cshRoomList>().SetUp(roomList[i]);
            //instantiate�� prefab�� roomListContent��ġ�� ������ְ� �� �������� i��° �븮��Ʈ�� �ȴ�. 
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)//�ٸ� �÷��̾ �濡 ������ �۵�
    {
        //Instantiate(playerListItemPrefab, playerListContent).GetComponent<cshPlayerList>().SetUp(newPlayer);
        //instantiate�� prefab�� playerListContent��ġ�� ������ְ� �� �������� �̸� �޾Ƽ� ǥ��.
    }

    public void ChangeCharacter()
    {
        if (chrindex == playerCharacterImage.Length-1)
        {
            chrindex = 0; // playerCharacterImage.Length;
        }
        else
        {
            chrindex++;
        }
        CharacterImageHolder.GetComponent<Image>().sprite = playerCharacterImage[chrindex];
        cshLoginUserValue.usernum = chrindex+1;

    }
    public void ChangeCharacterReverse()
    {
        if (chrindex == 0)
        {
            chrindex = playerCharacterImage.Length-1;
        }
        else
        {
            chrindex--;
        }
        CharacterImageHolder.GetComponent<Image>().sprite = playerCharacterImage[chrindex];
        cshLoginUserValue.usernum = chrindex+1;
    }
}