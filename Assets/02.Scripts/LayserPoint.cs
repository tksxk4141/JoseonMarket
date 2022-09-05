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

    private LineRenderer layser;        // ������
    private RaycastHit Collided_object; // �浹�� ��ü
    private GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü

    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";

    public float raycastDistance = 10f; // ������ ������ ���� �Ÿ�

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


        // ��ũ��Ʈ�� ���Ե� ��ü�� ���� ��������� ������Ʈ�� �ְ��ִ�.
        layser = this.gameObject.AddComponent<LineRenderer>();

        // ������ �������� ���� ǥ��
        Material material = mat;
        material.color = new Color(255, 188, 93, 0.5f);
        layser.material = material;
        // �������� �������� 2���� �ʿ� �� ���� ������ ��� ǥ�� �� �� �ִ�.
        layser.positionCount = 2;
        // ������ ���� ǥ��
        layser.startWidth = 0.01f;
        layser.endWidth = 0.01f;

        //Ű����
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

        layser.SetPosition(0, transform.position); // ù��° ������ ��ġ ������Ʈ�� �־� �����ν�, �÷��̾ �̵��ϸ� �̵��� ���󰡰� �ȴ�. �� �����(�浹 ������ ����)
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, raycastDistance))
        {
            layser.SetPosition(1, Collided_object.point);

            // �浹 ��ü�� �±װ� Button�� ���
            if (Collided_object.collider.gameObject.CompareTag("Button"))
            {

                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    //CilckButton();
                    // ��ư�� ��ϵ� onClick �޼ҵ带 �����Ѵ�.
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
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
            layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            // �ֱ� ������ ������Ʈ�� Button�� ���
            // ��ư�� ���� �����ִ� �����̹Ƿ� �̰��� Ǯ���ش�.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }
        }
    }
    private void LateUpdate()
    {
        // ��ư�� ���� ���        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //layser.material.color = new Color(255, 255, 255, 0.5f);
        }

        // ��ư�� �� ���          
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