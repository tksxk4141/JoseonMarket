using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshTrickery : MonoBehaviour
{
    public GameObject[] Cups;
    public GameObject gameBall;

    public GameObject CupPosition;

    List<Vector3> CupsPosition = new List<Vector3>();

    public GameObject DialogManager;
    public GameObject correctCup;
    public GameObject chooseCup;

    Vector3 cuppos1, cuppos2;

    bool ongame = false;
    bool isshuffle = false;
    bool ischeck = false;
    public bool GameEnd=false;
    int shuffleCount = 0;

    public GameObject Player;

    struct Cupsets
    {
        public GameObject cup1, cup2;
    }
    List<Cupsets> cupsetslist = new List<Cupsets>();

    // Start is called before the first frame update
    void Start()
    {
        Cupsets cupset1 = new Cupsets();
        cupset1.cup1 = Cups[0];
        cupset1.cup2 = Cups[1];
        Cupsets cupset2 = new Cupsets();
        cupset2.cup1 = Cups[1];
        cupset2.cup2 = Cups[2];
        Cupsets cupset3 = new Cupsets();
        cupset3.cup1 = Cups[0];
        cupset3.cup2 = Cups[2];

        cupsetslist.Add(cupset1);
        cupsetslist.Add(cupset2);
        cupsetslist.Add(cupset3);

        CupsPosition.Add(Cups[0].transform.position);
        CupsPosition.Add(Cups[1].transform.position);
        CupsPosition.Add(Cups[2].transform.position);

        //DialogManager = GameObject.FindGameObjectWithTag("dialog");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startGame(GameObject player)
    {
        Player = player;
        SetCups();
        SetBall();
        shuffleCount = 0;
        ongame = true;
        for (int i = 0; i < Cups.Length; i++)
        {
            StartCoroutine(MoveToPosition(Cups[i], new Vector3(0, 1, 0), 1));
        }
    }

    void SetCups()
    {
        for (int i = 0; i < Cups.Length; i++)
        {
            Cups[i].transform.position = CupsPosition[i];
        }
    }

    void SetBall()
    {
        int rand = Random.Range(0, Cups.Length);
        correctCup = Cups[rand];
        gameBall.transform.position = Cups[rand].transform.position;
    }

    void ShuffleCup()
    {
        int rand = Random.Range(0, Cups.Length);
        StartCoroutine(RotationMoveCup(cupsetslist[rand].cup1, cupsetslist[rand].cup2, 1f));
        shuffleCount++;
    }

    public void CheckAnswer(GameObject cup)
    {
        StartCoroutine(MoveToPosition(cup, new Vector3(0, 1, 0), 1));
        chooseCup = cup;
    }
    void CheckCups()
    {
        for (int i = 0; i < Cups.Length; i++)
        {
            StartCoroutine(MoveToPosition(Cups[i], new Vector3(0, 1, 0), 1));
        }
        ongame = false;
    }
    public IEnumerator MoveToPosition(GameObject cup, Vector3 position, float timeToMove)
    {
        var currentPos = cup.transform.position;
        var targetPos = currentPos + position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            cup.transform.position = Vector3.Lerp(currentPos, targetPos, t);
            yield return null;
        }
        cup.transform.position = targetPos;
        StartCoroutine(ReternToPosition(cup, -position, timeToMove));
    }
    public IEnumerator ReternToPosition(GameObject cup, Vector3 position, float timeToMove)
    {
        var currentPos = cup.transform.position;
        var targetPos = currentPos + position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            cup.transform.position = Vector3.Lerp(currentPos, targetPos, t);
            yield return null;
        }
        cup.transform.position = targetPos;
        if (!isshuffle && !ischeck && ongame)
        {
            gameBall.transform.SetParent(correctCup.transform);
            ShuffleCup();
        }
        else if (!isshuffle && ischeck && ongame)
        {
            CheckCups();
            ischeck = false;
        }
        else if(!isshuffle && !ischeck && !ongame)
        {
            GameEnd=true;
        }
    }
    public IEnumerator RotationMoveCup(GameObject cup1, GameObject cup2, float timeToRotate)
    {
        cuppos1 = cup1.transform.position;
        cuppos2 = cup2.transform.position;
        Vector3 pos = (cuppos1 + cuppos2) / 2;

        var t = 0f;
        while (t < 1)
        {
            isshuffle = true;
            t += Time.deltaTime / timeToRotate;
            cup1.transform.RotateAround(pos, Vector3.down, Time.deltaTime * 180);
            cup2.transform.RotateAround(pos, Vector3.down, Time.deltaTime * 180);
            yield return null;
        }
        cup1.transform.position = cuppos2;
        cup2.transform.position = cuppos1;
        if (shuffleCount != 10)
        {
            ShuffleCup();
        }
        else if (shuffleCount == 10)
        {
            isshuffle = false;
            ischeck = true;
            gameBall.transform.SetParent(this.transform);
        }
    }
}