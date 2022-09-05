using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cshRoomList : MonoBehaviour
{
    [SerializeField] TMP_Text RoomName;
    [SerializeField] TMP_Text PlayerCount;


    public RoomInfo info;//���� ����Ÿ���� ������ ���. �ۺ����� �����ؼ� �ٸ������� ���� �����ϵ��� ����. 
    public void SetUp(RoomInfo _info)//������ �޾ƿ���
    {
        info = _info;
        RoomName.text = _info.Name;
        PlayerCount.text = _info.PlayerCount +"/"+_info.MaxPlayers;
    }

    public void OnClick()
    {
        cshLauncher.Instance.JoinRoom(info);//��ó��ũ��Ʈ �޼���� JoinRoom����
    }
}
