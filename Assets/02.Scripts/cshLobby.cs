using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class cshLobby : MonoBehaviour
{
    public string menuName;
    public bool open;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);//특정 메뉴 켜지기
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}