using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;//Path사용위에 사용

public class cshRoomManager : MonoBehaviourPunCallbacks//다른 포톤 반응 받아들이기
{
    public static cshRoomManager Instance;//Room Manager 스크립트를 메서드로 사용하기 위해 선언
    public int selectSpwanPoint = 0;
    public int storyFlag = 0;

    public bool isFireExists = false;

    void Awake()
    {
        if (Instance)//다른 룸매니저 존재확인
        {
            Destroy(gameObject);//있으면 파괴
            return;
        }
        DontDestroyOnLoad(gameObject);//룸매니저 나혼자면 그대로 
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        // 활성화되면 씬 매니저의 OnSceneLoaded에 체인을 건다.
        // 씬이 바뀔때마다 작동됨
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        // 비활성화되면 씬 매니저의 체인을 지운다.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode load)
    {
        if (scene.buildIndex == 2)//게임씬이면. 0은 현재 시작메뉴 씬이다. 
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
