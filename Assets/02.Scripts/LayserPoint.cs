using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using TMPro;
using Michsky.UI.Shift;
using PathCreation.Examples;

public class LayserPoint : MonoBehaviour
{
    public GameObject Player;

    private LineRenderer layser;        // 레이저
    private RaycastHit Collided_object; // 충돌된 객체
    private GameObject currentObject;   // 가장 최근에 충돌한 객체를 저장하기 위한 객체

    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";

    public float raycastDistance = 10f; // 레이저 포인터 감지 거리

    public Material mat;
    
    PhotonView PV;

    public GameObject firewood;
    public GameObject neol;
    public GameObject quiz;
    public GameObject trickery;

    GameObject frameMan;

    public GameObject DialogManager;
    public GameObject DialogManager2;
    public GameObject DialogManager3;

    cshDialog cshDialog;
    public GameObject NPC;
    public string NPCName;


    void Start()
    {
        PV = Player.GetComponent<PhotonView>();

        firewood = GameObject.FindGameObjectWithTag("firewood");
        trickery = GameObject.FindGameObjectWithTag("trickery");
        neol = GameObject.FindGameObjectWithTag("neol");
        quiz = GameObject.FindGameObjectWithTag("quiz");


        // 스크립트가 포함된 객체에 라인 렌더러라는 컴포넌트를 넣고있다.
        layser = this.gameObject.AddComponent<LineRenderer>();

        // 라인이 가지개될 색상 표현
        Material material = mat;
        material.color = new Color(255, 188, 93, 0.5f);
        layser.material = material;
        // 레이저의 꼭지점은 2개가 필요 더 많이 넣으면 곡선도 표현 할 수 있다.
        layser.positionCount = 2;
        // 레이저 굵기 표현
        layser.startWidth = 0.01f;
        layser.endWidth = 0.01f;

        //키보드
        overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        if (overlayKeyboard != null)
            inputText = overlayKeyboard.text;

        cshDialog = DialogManager.GetComponent<cshDialog>();
        DialogManager = DialogManager2;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogManager.GetComponent<cshDialog>().nextDialog)
            DialogManager = DialogManager3;

        layser.SetPosition(0, transform.position); // 첫번째 시작점 위치 업데이트에 넣어 줌으로써, 플레이어가 이동하면 이동을 따라가게 된다. 선 만들기(충돌 감지를 위한)
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // 충돌 감지 시
        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, raycastDistance))
        {
            layser.SetPosition(1, Collided_object.point);

            // 충돌 객체의 태그가 Button인 경우
            if (Collided_object.collider.gameObject.CompareTag("Button"))
            {

                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    //CilckButton();
                    // 버튼에 등록된 onClick 메소드를 실행한다.
                    Collided_object.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                    //PV.RPC("CilckButton", RpcTarget.AllBufferedViaServer, null)
                }
                else
                {
                    Collided_object.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);
                    currentObject = Collided_object.collider.gameObject;
                }
            }
            if (Collided_object.collider.gameObject.CompareTag("NPC"))
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Player.GetComponent<cshPlayerInteraction>().npcInteraction.SetActive(true);
                }
                else
                {
                    currentObject = Collided_object.collider.gameObject;
                    NPC = currentObject;
                    NPCName = currentObject.name;
                }
            }
            if (Collided_object.collider.gameObject.CompareTag("TextField"))
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Collided_object.collider.gameObject.GetComponent<OnScreenKeyboardInputfield>().OnPointerDown(null);
                    Collided_object.collider.gameObject.GetComponent<InputField>().Select();
                    Collided_object.collider.gameObject.GetComponent<InputField>().ActivateInputField();
                }
                else
                {
                    Collided_object.collider.gameObject.GetComponent<InputField>().OnPointerClick(null);
                    currentObject = Collided_object.collider.gameObject;
                }
            }
            if (Collided_object.collider.gameObject.CompareTag("Cup"))
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    trickery.GetComponent<cshTrickery>().CheckAnswer(Collided_object.collider.gameObject);
                }
                else
                {
                    currentObject = Collided_object.collider.gameObject;
                }
            }
            if (Collided_object.collider.gameObject.CompareTag("seesaw"))
            {
                //Player.GetComponent<cshPlayerInteraction>().seesaw = neol;
                Player.GetComponent<cshPlayerInteraction>().neolInteraction.SetActive(true);

                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Collided_object.collider.gameObject.GetComponent<CharacterJumpWithSeesaw>().PlayerOn(Player);
                }
                else
                {
                    Collided_object.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);
                    currentObject = Collided_object.collider.gameObject;
                }
            }
        }
        else
        {
            // 레이저에 감지된 것이 없기 때문에 레이저 초기 설정 길이만큼 길게 만든다.
            layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            // 최근 감지된 오브젝트가 Button인 경우
            // 버튼은 현재 눌려있는 상태이므로 이것을 풀어준다.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }
        }
    }
    private void LateUpdate()
    {
        // 버튼을 누를 경우        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //layser.material.color = new Color(255, 255, 255, 0.5f);
        }

        // 버튼을 뗄 경우          
        else if (OVRInput.GetUp(OVRInput.Button.One))
        {
            //layser.material.color = new Color(255, 188, 93, 0.5f);
        }
    }
    [PunRPC]
    void CilckButton()
    {
        Collided_object.collider.gameObject.GetComponent<Button>().onClick.Invoke();
    }
    [PunRPC]
    public void onNeol()
    {
        Player.GetComponent<cshPlayerInteraction>().onNeol();//.isOnSeesaw = true;
        //neol.GetComponent<CharacterJumpWithSeesaw>().PlayerOn(Player);
    }
    [PunRPC]
    public void offNeol()
    {
        neol.GetComponent<CharacterJumpWithSeesaw>().PlayerOff(Player);
        Player.GetComponent <cshPlayerInteraction>().isOnSeesaw = false;
    }
    [PunRPC]
    public void JoinQuiz()
    {
       quiz.GetComponent<cshQuiz>().SetUpPlayer(Player);
    }
    [PunRPC]
    public void StartTrickery()
    {
        trickery.GetComponent<cshTrickery>().startGame(Player);
    }
}