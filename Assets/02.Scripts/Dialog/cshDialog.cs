using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cshDialog : MonoBehaviour
{
    public GameObject Player;

    public GameObject DialogPanel;
    public GameObject DialogManager;
    public GameObject NamePanel;

    public GameObject DialogName;
    public GameObject DialogSpeach;
    public GameObject[] Dialog;

    public GameObject CurrentDialog;

    public GameObject LayserPoint;
    public GameObject NPC;
    public string NPCName;
    public GameObject FireWood;
    public GameObject Trickery;
    public GameObject Neol;

    public GameObject[] Board;
    public Sprite MapImage;
    public bool map=false;
    public bool nextDialog=false;
    public string QuestName = null, QuestContents = null;

    public int TalkWithPeople=0;
    public int TalkWithMerchant=0;
    // Start is called before the first frame update
    void Start()
    {
        Board = new GameObject[3];
        Board[0] = GameObject.Find("저잣거리 지도1");
        Board[1] = GameObject.Find("저잣거리 지도2");
        Board[2] = GameObject.Find("저잣거리 지도3");

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentDialog == Dialog[2])
        {
            CurrentQuestContents("쪼개진 나무 " + (FireWood.GetComponent<cshFireWood>().firewoodPileIndex * 4).ToString()+"/24");
        }
    }

    public void ShowDialog()
    {
        Player = LayserPoint.GetComponent<LayserPoint>().Player;

        NPC = LayserPoint.GetComponent<LayserPoint>().NPC;
        NPCName = LayserPoint.GetComponent<LayserPoint>().NPCName;

        FireWood = LayserPoint.GetComponent<LayserPoint>().firewood;
        Trickery = LayserPoint.GetComponent<LayserPoint>().trickery;
        Neol = LayserPoint.GetComponent<LayserPoint>().neol;

        if (NPCName.Equals("순복"))
        {
            if (CurrentDialog == null)
                DialogPanelLocation(NPC, 0);
            else if (CurrentDialog == Dialog[1])
                DialogPanelLocation(NPC, 2);
            else if (FireWood.GetComponent<cshFireWood>().firewoodPileIndex * 4 >=24 && CurrentDialog == Dialog[2])
                DialogPanelLocation(NPC, 3);
        }
        else if (NPCName.Equals("중노미"))
        {
            if (CurrentDialog == Dialog[0])
                DialogPanelLocation(NPC, 1);
        }
        else if (NPCName.Equals("낡은 게시판"))
        {
            if (!map)
            {
                DialogPanelLocation(NPC, 4);
                CurrentDialog = null;
            }
            else DialogPanelLocation(NPC, 5);
        }

        else if (NPCName.Equals("팽삼"))
        {
            if (CurrentDialog == Dialog[5])
                DialogPanelLocation(NPC, 6);
            if (CurrentDialog == Dialog[11])
            {
                DialogPanelLocation(NPC, 12);
                LayserPoint.GetComponent<LayserPoint>().StartTrickery();
            }
            else if (CurrentDialog == Dialog[12])
            {
                if(Trickery.GetComponent<cshTrickery>().correctCup==Trickery.GetComponent<cshTrickery>().chooseCup)
                    DialogPanelLocation(NPC, 13);
                else
                    DialogPanelLocation(NPC, 14);
            }
            else if (CurrentDialog == Dialog[18])
            {
                CurrentQuestNULL();
                DialogPanelLocation(NPC, 19);
            }
            else if (CurrentDialog == Dialog[19])
            {
                DialogPanelLocation(NPC, 20);
            }
        }
        else if (NPCName.Equals("구경꾼"))
        {
            if (CurrentDialog == Dialog[6])
            {
                DialogPanelLocation(NPC, 7);
                CurrentQuestContents("구경중인 사람들 1/4");
            }
            else if (CurrentDialog == Dialog[7])
            {
                DialogPanelLocation(NPC, 8);
                CurrentQuestContents("구경중인 사람들 2/4");
            }
            else if (CurrentDialog == Dialog[8])
            {
                DialogPanelLocation(NPC, 9);
                CurrentQuestContents("구경중인 사람들 3/4");
            }
            else if (CurrentDialog == Dialog[9])
            {
                CurrentQuestContents("구경중인 사람들 4/4\n바람잡이에게 말 걸어보자.");
                DialogPanelLocation(NPC, 10);
            }
        }
        else if (NPCName.Equals("바람잡이"))
        {
            if (CurrentDialog == Dialog[10])
            {
                DialogPanelLocation(NPC, 11);
                CurrentQuestContents("팽삼과 대화하기");
            }
        }
        else if (NPCName.Equals("울상짓는 남성"))
        {
            if (CurrentDialog == Dialog[13] || CurrentDialog == Dialog[14])
            {
                DialogPanelLocation(NPC, 16);
            }
            if (CurrentDialog == Dialog[17])
            {
                DialogPanelLocation(NPC, 18);
                CurrentQuest("팽삼과 대화하기");
                CurrentQuestContents("팽삼과 대화하기");
            }
        }
        else if (NPCName.Equals("Bowl"))
        {
            if (CurrentDialog == Dialog[16])
            {
                DialogPanelLocation(NPC, 17);
            }
        }
        else if (NPCName.Equals("외지인"))
        {
            if (CurrentDialog == Dialog[20])
            {
                DialogPanelLocation(NPC, 21);
            }
            else if (CurrentDialog == Dialog[22])
            {
                DialogPanelLocation(NPC, 23);
            }
            else if (CurrentDialog == Dialog[23]||Player.GetComponent<cshPlayerInteraction>().NeolBest>=25)
            {
                DialogPanelLocation(NPC, 24);
            }
            else if (CurrentDialog == Dialog[24])
            {
                DialogPanelLocation(NPC, 25);
            }
        }
        else if (NPCName.Equals("삼란"))
        {
            if (CurrentDialog == Dialog[21])
            {
                DialogPanelLocation(NPC, 22);
            }
        }
        else if (NPCName.Equals("세숙"))
        {
            if (CurrentDialog == Dialog[25])
            {
                DialogPanelLocation(NPC, 0);
            }
            else if ((CurrentDialog == Dialog[1] || CurrentDialog == Dialog[2] || CurrentDialog == Dialog[3]) && TalkWithPeople==3)
            {
                DialogPanelLocation(NPC, 4);
            }
            else if ((CurrentDialog == Dialog[5] || CurrentDialog == Dialog[6]) && TalkWithMerchant == 2)
            {
                DialogPanelLocation(NPC, 7);
            }
        }
        else if (NPCName.Equals("쉬고있는주민"))
        {
            if (CurrentDialog == Dialog[0] || CurrentDialog == Dialog[2] || CurrentDialog == Dialog[3])
            {
                DialogPanelLocation(NPC, 1);
                TalkWithPeople++;

            }
        }
        else if (NPCName.Equals("가판대를 가만히 응시하던 주민"))
        {
            if (CurrentDialog == Dialog[0] || CurrentDialog == Dialog[1] || CurrentDialog == Dialog[3])
            {
                DialogPanelLocation(NPC, 2);
                TalkWithPeople++;
            }
        }
        else if (NPCName.Equals("당황한주민"))
        {
            if(CurrentDialog == Dialog[0] || CurrentDialog==Dialog[1]|| CurrentDialog == Dialog[2])
            {
                DialogPanelLocation(NPC, 3);
                TalkWithPeople++;
            }
        }
        else if (NPCName.Equals("지게를 메고 쉬고있는 보상"))
        {
            if (CurrentDialog == Dialog[4] || CurrentDialog == Dialog[6])
            {
                DialogPanelLocation(NPC, 5);
                TalkWithMerchant++;
            }
        }
        else if (NPCName.Equals("짐을 정리중이던 보상"))
        {
            if (CurrentDialog == Dialog[4] || CurrentDialog == Dialog[5])
            {
                DialogPanelLocation(NPC, 6);
                TalkWithMerchant++;
            }
        }
        else if (NPCName.Equals("계덕"))
        {
            if (CurrentDialog == Dialog[7])
            {
                DialogPanelLocation(NPC, 8);
            }
        }
        else if (NPCName.Equals("병돌"))
        {
            if (CurrentDialog == Dialog[8])
            {
                DialogPanelLocation(NPC, 9);
            }
            else if (CurrentDialog == Dialog[9])
            {
                DialogPanelLocation(NPC, 10);
                LayserPoint.GetComponent<LayserPoint>().JoinQuiz();
            }
            else if (CurrentDialog == Dialog[10])
            {
                if (Player.GetComponent<cshPlayerInteraction>().QuizScore > 3)
                    DialogPanelLocation(NPC, 11);
                else
                    DialogPanelLocation(NPC, 12);
            }
            else if (CurrentDialog == Dialog[11] || CurrentDialog == Dialog[12])
                DialogPanelLocation(NPC, 13);
        }
        else if (NPCName.Equals("계분"))
        {
            if (CurrentDialog == Dialog[13])
            {
                DialogPanelLocation(NPC, 14);
            }
            else if (CurrentDialog == Dialog[14])
            {
                DialogPanelLocation(NPC, 15);
            }
            else if (CurrentDialog == Dialog[15])
            {
                DialogPanelLocation(NPC, 16);
            }
        }
    }
    public void DialogPanelLocation(GameObject NPC, int DialogNum)
    {
        DialogManager.GetComponent<ConversationManager>().StartConversation(Dialog[DialogNum].GetComponent<NPCConversation>());
        CurrentDialog = Dialog[DialogNum];
    }
    public void DialogPanelLocation(GameObject NPC)
    {
        DialogPanel.transform.position = NPC.transform.position + new Vector3(0, 2, 0.5f);
    }
    public void DialogPanelLocation(int DialogNum)
    {
        DialogManager.GetComponent<ConversationManager>().StartConversation(Dialog[DialogNum].GetComponent<NPCConversation>());
        CurrentDialog = Dialog[DialogNum];
    }
    public void DialogColor()
    {
        DialogName.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        DialogSpeach.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
    }
    public void QuestColor()
    {
        DialogName.GetComponent<TextMeshProUGUI>().color = new Color32(255, 188, 93, 255);
        DialogSpeach.GetComponent<TextMeshProUGUI>().color = new Color32(255, 188, 93, 255);
    }
    public void RewordColor()
    {
        DialogSpeach.GetComponent<TextMeshProUGUI>().color = new Color32(178, 178, 178, 255);
    }
    public void InfoColor()
    {
        DialogSpeach.GetComponent<TextMeshProUGUI>().color = new Color32(178, 178, 178, 255);
    }
    public void ShowNamePanel()
    {
        NamePanel.SetActive(true);
    }
    public void CloseNamePanel()
    {
        NamePanel.SetActive(false);
    }
    public void CurrentQuest(string questName)
    {
        QuestName = questName;
    }
    public void CurrentQuestContents(string questContents)
    {
        QuestContents = questContents;
    }
    public void CurrentQuestNULL()
    {
        QuestName = "";
        QuestContents = "";
    }
    public void ChangeMapImage()
    {
        map = true;
        Board[0].gameObject.GetComponent<SpriteRenderer>().sprite = MapImage;
        Board[1].gameObject.GetComponent<SpriteRenderer>().sprite = MapImage;
        Board[2].gameObject.GetComponent<SpriteRenderer>().sprite = MapImage;
    }
    public void NextDialog()
    {
        nextDialog = true;
    }
}