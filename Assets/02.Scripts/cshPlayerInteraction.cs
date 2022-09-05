using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class cshPlayerInteraction : MonoBehaviour
{
    public GameObject seesaw;
    public GameObject quiz;
    public GameObject checkBar;
    public GameObject Bar;
    public GameObject neolInteraction;
    public GameObject quizInteraction;
    public GameObject JumpScore;
    public int NeolBest;
    public int QuizScore;
    public GameObject npcInteraction;
    public bool isOnSeesaw = false;

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        quiz = GameObject.FindGameObjectWithTag("quiz");
        seesaw = GameObject.FindGameObjectWithTag("neol");
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnSeesaw)
        {
            JumpScore.SetActive(true);
            JumpScore.transform.Find("Check").transform.Find("CurrentHeight").GetComponent<TextMeshProUGUI>().text = ((int)(gameObject.transform.position.y)-16).ToString();
            JumpScore.transform.Find("Check").transform.Find("BestScore").GetComponent<TextMeshProUGUI>().text = ((seesaw.GetComponent<CharacterJumpWithSeesaw>().JumpHeight)-16).ToString();
            NeolBest = (seesaw.GetComponent<CharacterJumpWithSeesaw>().JumpHeight) - 16;
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                PV.RPC("outNeol", RpcTarget.AllBufferedViaServer, null);
            }
        }
        else if (!isOnSeesaw)
        {
            JumpScore.SetActive(false);
        }
    }
    public void onNeol()
    {
        PV.RPC("isNeol", RpcTarget.AllBufferedViaServer, null);
    }
    [PunRPC]
    void isNeol()
    {
        if (!isOnSeesaw)
        {
            seesaw.GetComponent<CharacterJumpWithSeesaw>().PlayerOn(gameObject);
            isOnSeesaw = true;
        }
        else
        {
            seesaw.GetComponent<CharacterJumpWithSeesaw>().PlayerOff(gameObject);
            isOnSeesaw = false;
        }
    }
    [PunRPC]
    public void RotateBar(float time)
    {
        StartCoroutine(RotateCheckBar(Bar.transform,time));
    }

    [PunRPC]
    public IEnumerator RotateCheckBar(Transform transform, float timeToRotate)
    {
        var t = 0f;
        checkBar.SetActive(true);
        var currentPos = transform.GetComponent<RectTransform>().anchoredPosition;
        Vector3 position = new Vector3 (0,0,0);
        while (t < 1)
        {
            t += Time.deltaTime / timeToRotate;
            transform.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(currentPos, position, t);
            if (OVRInput.GetDown(OVRInput.Button.One))//Input.GetKeyDown(KeyCode.Space))
            {
                checkBar.SetActive(false);
                if (0.875f < t && t < 1f)
                {
                    PV.RPC("addJumpPower",RpcTarget.AllBufferedViaServer, 6f);
                }
                else if (0.75f < t && t < 0.875f)
                {
                    PV.RPC("addJumpPower", RpcTarget.AllBufferedViaServer, 5.5f);
                }
                else
                {
                    PV.RPC("addJumpPower", RpcTarget.AllBufferedViaServer, -1.0f);
                }
                transform.GetComponent<RectTransform>().anchoredPosition = currentPos;
                yield return null;
            }
            //PV.RPC("addJumpPower", RpcTarget.AllBufferedViaServer, -5.0f);
            yield return null;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = currentPos;
        checkBar.SetActive(false);
    }

    [PunRPC]
    void joinNeol()
    {

    }
    [PunRPC]
    void outNeol()
    {
        seesaw.GetComponent<CharacterJumpWithSeesaw>().PlayerOff(gameObject);
        isOnSeesaw = false;
    }
    [PunRPC]
    void addJumpPower(float power)
    {
        seesaw.GetComponent<CharacterJumpWithSeesaw>().jumpforce(power);
    }
}
