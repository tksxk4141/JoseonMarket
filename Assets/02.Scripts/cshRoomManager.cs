using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;//Path������� ���

public class cshRoomManager : MonoBehaviourPunCallbacks//�ٸ� ���� ���� �޾Ƶ��̱�
{
    public static cshRoomManager Instance;//Room Manager ��ũ��Ʈ�� �޼���� ����ϱ� ���� ����
    public int selectSpwanPoint = 0;
    public int storyFlag = 0;

    public bool isFireExists = false;

    void Awake()
    {
        if (Instance)//�ٸ� ��Ŵ��� ����Ȯ��
        {
            Destroy(gameObject);//������ �ı�
            return;
        }
        DontDestroyOnLoad(gameObject);//��Ŵ��� ��ȥ�ڸ� �״�� 
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Ȱ��ȭ�Ǹ� �� �Ŵ����� OnSceneLoaded�� ü���� �Ǵ�.
        // ���� �ٲ𶧸��� �۵���
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        // ��Ȱ��ȭ�Ǹ� �� �Ŵ����� ü���� �����.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode load)
    {
        if (scene.buildIndex == 2)//���Ӿ��̸�. 0�� ���� ���۸޴� ���̴�. 
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
