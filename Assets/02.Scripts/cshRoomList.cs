using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cshRoomList : MonoBehaviour
{
    [SerializeField] TMP_Text RoomName;
    [SerializeField] TMP_Text PlayerCount;


    public RoomInfo info;//포톤 리얼타임의 방정보 기능. 퍼블릭으로 선언해서 다른곳에서 접근 가능하도록 수정. 
    public void SetUp(RoomInfo _info)//방정보 받아오기
    {
        info = _info;
        RoomName.text = _info.Name;
        PlayerCount.text = _info.PlayerCount +"/"+_info.MaxPlayers;
    }

    public void OnClick()
    {
        cshLauncher.Instance.JoinRoom(info);//런처스크립트 메서드로 JoinRoom실행
    }
}
