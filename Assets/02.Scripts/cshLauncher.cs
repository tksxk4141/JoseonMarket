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
        Instance = this;//메서드로 사용
    }
    void Start()
    {
        Debug.Log("Connecting to Master");
        if (PhotonNetwork.IsConnected == false)
            PhotonNetwork.ConnectUsingSettings();//설정한 포톤 서버에 때라 마스터 서버에 연결
    }

    void Update()
    {

    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();//설정한 포톤 서버에 때라 마스터 서버에 연결
    }

    public override void OnConnectedToMaster()//마스터서버에 연결시 작동됨
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();//마스터 서버 연결시 로비로 연결
        PhotonNetwork.AutomaticallySyncScene = true;//자동으로 모든 사람들의 scene을 통일 시켜준다. 
    }

    public override void OnJoinedLobby()//로비에 연결시 작동
    {
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = cshLoginValue.username;
        userName.text = PhotonNetwork.NickName;
    }
    public void CreateRoom()//방만들기
    {
        int rnd = Random.Range(0,100);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(rnd.ToString(), roomOptions); //포톤 네트워크기능으로 roomNameInputField.text의 이름으로 방을 만든다.
        Debug.Log("Create Room");
    }

    public override void OnJoinedRoom()//방에 들어갔을때 작동
    {
        Debug.Log("Joined Room");
        Instantiate(RoomManager);
        StartGame();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)//방장이 나가서 방장이 바뀌었을때
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);//방장만 게임시작 버튼 누르기 가능
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//방 만들기 실패시 작동
    {
        errorText.text = "Room Creation Failed: " + message;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(2);//1인 이유는 빌드에서 scene 번호가 1번씩이기 때문이다. 0은 초기 씬.
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();//방떠나기 포톤 네트워크 기능
        GameObject.Find("Top Panel").transform.Find("Button List").gameObject.SetActive(true);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//포톤 네트워크의 JoinRoom기능 해당이름을 가진 방으로 접속한다.
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnLeftRoom()//방을 떠나면 호출
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(1);
            return;
        }
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//포톤의 룸 리스트 기능
    {
        foreach (Transform trans in roomListContent)//존재하는 모든 roomListContent
        {
            Destroy(trans.gameObject);//룸리스트 업데이트가 될때마다 싹지우기
        }
        for (int i = 0; i < roomList.Count; i++)//방갯수만큼 반복
        {
            if (roomList[i].RemovedFromList)//사라진 방은 취급 안한다. 
                continue;
            //Instantiate(roomListItemPrefab, roomListContent).GetComponent<cshRoomList>().SetUp(roomList[i]);
            //instantiate로 prefab을 roomListContent위치에 만들어주고 그 프리펩은 i번째 룸리스트가 된다. 
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)//다른 플레이어가 방에 들어오면 작동
    {
        //Instantiate(playerListItemPrefab, playerListContent).GetComponent<cshPlayerList>().SetUp(newPlayer);
        //instantiate로 prefab을 playerListContent위치에 만들어주고 그 프리펩을 이름 받아서 표시.
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