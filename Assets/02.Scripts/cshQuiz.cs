using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class cshQuiz : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject TimePanel;
    public GameObject QuizPanel;
    public GameObject[] AnswerPanel = new GameObject[2];

    public string[] trueQuiz = new string[10];
    public string[] falseQuiz = new string[10];
    
    Dictionary<int, string> QuizDictIndex = new Dictionary<int, string>();
    Dictionary<string, bool> QuizDict = new Dictionary<string, bool>();
    List<string> sortedQuiz = new List<string>();

    private bool Answer;
    private int quizIndex = 0;
    private bool Timer = false;
    private bool checkTime = false;
    public float quizTime = 10.0f;
    public float waitTime = 3.0f;

    public struct QuizPlayer
    {
        public GameObject Player;
        public int score;
        public bool selectTrue;
        public bool selectFalse;
        public QuizPlayer(GameObject player)
        {
            this.Player = player;
            score = 0;
            selectTrue = false;
            selectFalse = false;
        }
    }
    QuizPlayer[] quizPlayers = new QuizPlayer[10];
    private int quizPlayerIndex = 0;

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        trueQuiz[0] = "부부 사이는 1촌이다.";
        trueQuiz[1] = "조선시대 국가 공인 상점의 이름은 육의전이다.";
        trueQuiz[2] = "금난전권은 정조 때의 신해통공으로 폐지되었다.";
        falseQuiz[0] = "조선 후기에 이르되 양반의 수는 크게 줄었다.";
        falseQuiz[1] = "조선시대에도 이혼 제도가 있었다.";
        falseQuiz[2] = "대동법 이후 수공업 생산량이 감소하였다.";
    }

    // Update is called once per frame
    void Update()
    {
        if (quizPlayerIndex == 1)
        {
            PV.RPC("StartQuiz", RpcTarget.AllBufferedViaServer, null);// Countdown();
        }
        if (Timer)
            PV.RPC("Countdown",RpcTarget.AllBufferedViaServer, null);// Countdown();
    }
    [PunRPC]
    private void SetUpQuiz()
    {
        quizIndex = 0;
        for (int i = 0; i < trueQuiz.Length; i++)
        {
            QuizDict.Add(trueQuiz[i], true);
            QuizDictIndex.Add(i, trueQuiz[i]);
        }
        for (int i = 0; i < falseQuiz.Length; i++)
        {
            QuizDict.Add(falseQuiz[i], false);
            QuizDictIndex.Add(i + trueQuiz.Length, falseQuiz[i]);
        }
    }
    [PunRPC]
    public void SortQuiz()
    {
        int rnd = Random.Range(0, falseQuiz.Length + trueQuiz.Length);
        string currentQuiz = "";
        for (int i = 0; i < QuizDict.Count; i++)
        {
            do
            {
                rnd = Random.Range(0, falseQuiz.Length + trueQuiz.Length);
                currentQuiz = QuizDictIndex[rnd];
            }
            while (sortedQuiz.Contains(currentQuiz));
            sortedQuiz.Add(QuizDictIndex[rnd]);
        }
    }
    [PunRPC]
    void PrintQuiz()
    {
        if (AnswerPanel[0].activeSelf)
            AnswerPanel[0].SetActive(false);
        if(AnswerPanel[1].activeSelf)
            AnswerPanel[1].SetActive(false);

        TextMeshPro text = QuizPanel.GetComponent<TextMeshPro>();
        text.text = sortedQuiz[quizIndex];
        quizTime = 5.0f;
        waitTime = 3.0f;
    }
    public void CheckAnswer()
    {
        foreach (KeyValuePair<string, bool> item in QuizDict)
        {
            if (item.Key.Equals(sortedQuiz[quizIndex]))
            {
                Answer = item.Value;
            }
        }
        if (checkTime)
        {
            if (Answer)
            {
                TextMeshPro text = QuizPanel.GetComponent<TextMeshPro>();
                //text.text = "O";
                AnswerPanel[0].SetActive(true);
                for (int i = 0; i < quizPlayers.Length; i++)
                {
                    if (quizPlayers[i].selectTrue&&!quizPlayers[i].selectFalse)
                    {
                        quizPlayers[i].score++;
                        Debug.Log("정답");
                    }
                    else if (quizPlayers[i].selectFalse&&!quizPlayers[i].selectTrue)
                    {
                        Debug.Log("오답");
                    }
                    else
                    {
                        Debug.Log("오답");
                    }
                }
            }
            else if (!Answer)
            {
                TextMeshPro text = QuizPanel.GetComponent<TextMeshPro>();
                //text.text = "X";
                AnswerPanel[1].SetActive(true);
                for (int i = 0; i < quizPlayers.Length; i++)
                {
                    if (quizPlayers[i].selectTrue&&!quizPlayers[i].selectFalse)
                    {
                        Debug.Log("오답");
                    }
                    else if (quizPlayers[i].selectFalse&&!quizPlayers[i].selectTrue)
                    {
                        quizPlayers[i].score++;
                        Debug.Log("정답");
                    }
                    else
                    {
                        Debug.Log("오답");
                    }
                }
            }
            quizIndex++;
            if (quizIndex == QuizDict.Count)
            {
                PV.RPC("CheckScore",RpcTarget.AllBufferedViaServer,null);// CheckScore();
                PV.RPC("EndQuiz", RpcTarget.AllBufferedViaServer, null);// CheckScore();
            }
            checkTime = false;
        }
    }

    [PunRPC]
    void EndQuiz()
    {
        Timer = false;
        checkTime = false;
        sortedQuiz = new List<string>();
        QuizDictIndex = new Dictionary<int, string>();
        QuizDict = new Dictionary<string, bool>();
        quizPlayerIndex = 0;
        quizIndex = 0;
    }

    public void SetUpPlayersAnswer(GameObject select, GameObject player)
    {
        if (select.tag == "true")
        {
            for(int i = 0; i < quizPlayers.Length; i++)
            {
                if(quizPlayers[i].Player == player)
                {
                    quizPlayers[i].selectTrue = true;
                    quizPlayers[i].selectFalse = false;
                }
            }
        }
        if (select.tag == "false")
        {
            for (int i = 0; i < quizPlayers.Length; i++)
            {
                if (quizPlayers[i].Player == player)
                {
                    quizPlayers[i].selectFalse = true;
                    quizPlayers[i].selectTrue = false;
                }
            }
        }
    }
    public void SetUpPlayersAnswer(GameObject player)
    {
        for (int i = 0; i < quizPlayers.Length; i++)
        {
            if (quizPlayers[i].Player == player)
            {
                quizPlayers[i].selectFalse = false;
                quizPlayers[i].selectTrue = false;
            }
        }
    }
    [PunRPC]
    public void SetUpPlayer(GameObject player)
    {
        player.tag = "quizPlayer";
        QuizPlayer quizPlayer = new QuizPlayer(player);
        quizPlayers[quizPlayerIndex] = quizPlayer;
        quizPlayerIndex++;
    }
    public void StartQuiz(GameObject player)
    {
        SetUpPlayer(player);
        PV.RPC("SetUpQuiz", RpcTarget.AllBufferedViaServer, null);//SetUpQuiz();
        PV.RPC("SortQuiz",RpcTarget.AllBufferedViaServer,null);//SortQuiz();
        PV.RPC("PrintQuiz",RpcTarget.AllBufferedViaServer,null); // PrintQuiz();
        Timer = true;
    }
    [PunRPC]
    public void StartQuiz()
    {
        PV.RPC("SetUpQuiz", RpcTarget.AllBufferedViaServer, null);//SetUpQuiz();
        PV.RPC("SortQuiz", RpcTarget.AllBufferedViaServer, null);//SortQuiz();
        PV.RPC("PrintQuiz", RpcTarget.AllBufferedViaServer, null); // PrintQuiz();
        quizPlayerIndex = 0;
        Timer = true;
    }
    [PunRPC]
    void Countdown()
    {
        if (quizTime > 0)
            quizTime -= Time.deltaTime;
        if (quizTime <= 0 && waitTime>=3.0f)
        {
            checkTime = true;
            CheckAnswer();
            waitTime -= Time.deltaTime;
        }
        if (quizTime <= 0 && waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        if (waitTime<=0)
        {
            PV.RPC("PrintQuiz", RpcTarget.AllBufferedViaServer, null); //PrintQuiz();
        }
        TimePanel.GetComponent<TextMeshPro>().text = "저잣거리 상식대회 "+Mathf.Round(quizTime).ToString()+"초";
    }
    [PunRPC]
    void CheckScore()
    {
        if (AnswerPanel[0].activeSelf)
            AnswerPanel[0].SetActive(false);
        if (AnswerPanel[1].activeSelf)
            AnswerPanel[1].SetActive(false);

        TextMeshPro text = QuizPanel.GetComponent<TextMeshPro>();
        string scoretext = "";
        int score = 0;
        for(int i = 0; i < quizPlayers.Length; i++)
        {
            if (quizPlayers[i].score > score)
            {
                score = quizPlayers[i].score;
                quizPlayers[i].Player.GetComponent<cshPlayerInteraction>().QuizScore = quizPlayers[i].score;
                scoretext = "장원 급제 " + quizPlayers[i].score.ToString() + "/" + QuizDict.Count.ToString();
            }
            text.text = scoretext;
        }
    }
}