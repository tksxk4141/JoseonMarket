using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshLobbyManager : MonoBehaviour
{
    public static cshLobbyManager Instance;//�ٸ� class������ ȣ�Ⱑ��

    [SerializeField] cshLobby[] menus;//SerializedField�� ����ϸ� �츮�� publicó�� �� �� ������  public�� �ƴϿ��� �ܺο����� ������.

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Enter();
    }

    private void Enter()
    {
        if (menus[2] == true) { if (Input.GetKeyDown(KeyCode.Return)) cshLauncher.Instance.CreateRoom(); }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)//string�� �޾Ƽ� �ش��̸� ���� �޴��� ���� ��ũ��Ʈ
            {
                menus[i].Open();//���� �޴�(��Ʈ��)�� �ִ� for���� ���� �޴�(�޴�)���� �Ȱ��� �־ �ߺ��� ���ϰ��� �ڵ� ����.  
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(cshLobby menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(cshLobby menu)
    {
        menu.Close();
    }
}