using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using TMPro;

public class cshPlayFabValue : MonoBehaviour
{
    public GameObject DialogManager;
    public GameObject DialogManager2;
    public GameObject DialogManager3;

    public GameObject QuestWindow;
    public GameObject QuestNameWindow;
    public GameObject QuestContentsWindow;

    public GameObject Inventory;
    public GameObject JNValue;
    public GameObject PNValue;

    public string QuestName;
    public string QuestContents;

    public GameObject SelectedItem;

    public GameObject[] InventoryButton;
    public GameObject[] InventoryButtonImage;
    public Sprite[] ItemSprite;

    public GameObject Skylight;
    public GameObject SkylightPrefab;
    public Transform SkylightPos;

    struct SelectedItemStruct
    {
        public string SelectedItemName;
        public int SelectedItemCost;
        public string SelectedItemCurrency;
        public SelectedItemStruct(string selecteditemname, int selecteditemcost, string selecteditemcurrency) {
            SelectedItemName = selecteditemname;
            SelectedItemCost = selecteditemcost;
            SelectedItemCurrency = selecteditemcurrency;
        }
    }
    SelectedItemStruct CurrentItem;
    
    List<SelectedItemStruct> SelectedItems = new List<SelectedItemStruct>();

    private void Start()
    {
        GetVirtualCurrencies();

        SelectedItemStruct BW = new SelectedItemStruct("BW", 5, "PN");
        SelectedItemStruct OV = new SelectedItemStruct("OV", 5, "PN");
        SelectedItemStruct PF = new SelectedItemStruct("PF", 2, "PN");
        SelectedItemStruct ST = new SelectedItemStruct("ST", 1, "PN");
        
        SelectedItemStruct FR = new SelectedItemStruct("FR", 5, "PN");
        SelectedItemStruct BM = new SelectedItemStruct("BM", 4, "PN");
        SelectedItemStruct CP = new SelectedItemStruct("CP", 5, "PN");
        SelectedItemStruct CC = new SelectedItemStruct("CC", 3, "PN");
        SelectedItemStruct CO = new SelectedItemStruct("CO", 1, "PN");

        SelectedItemStruct SG = new SelectedItemStruct("SG", 0, "PN");
        SelectedItemStruct MP = new SelectedItemStruct("MP", 0, "PN");

        SelectedItems.Add(BW);
        SelectedItems.Add(OV);
        SelectedItems.Add(PF);
        SelectedItems.Add(ST);
        SelectedItems.Add(FR);
        SelectedItems.Add(BM);
        SelectedItems.Add(CP);
        SelectedItems.Add(CC);
        SelectedItems.Add(CO);
        SelectedItems.Add(SG);
        SelectedItems.Add(MP);

        DialogManager = DialogManager2;

        SkylightPos = GameObject.FindGameObjectWithTag("lanternpos").transform;

    }
    void Update()
    {
        if (DialogManager.GetComponent<cshDialog>().nextDialog)
            DialogManager = DialogManager3;
        //    ConsumeItem("DDDC50A8F03ED1AD");
        CurrentQuest();

        if (OVRInput.GetDown(OVRInput.Button.Four)&& !Inventory.activeSelf)
        {
            Inventory.SetActive(true);
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four) && Inventory.activeSelf)
        {
            Inventory.SetActive(false);
        }

    }

    public void ItemButton(GameObject button)
    {
        if (button.name == "Ç³µî")
        {
            Skylights();
        }
    }
    public void Skylights()
    {
        Skylight = Instantiate(SkylightPrefab, SkylightPos.position, Quaternion.Euler(90, 0, 0));
        Skylight.transform.SetParent(SkylightPos, false);
        Skylight.transform.position = SkylightPos.position;
    }

    public void AddPN()
    {
        AddVirtualCurrency("PN", 5);
    }
    public void SubPN()
    {
        SubtractVirtualCurrency("PN", 5);
    }
    public void Soup()
    {
        AddVirtualCurrency("PN", 5);
        PurchaseItem("v1.0.0", "Main", "SG", 0, "PN");
    }
    public void Map()
    {
        PurchaseItem("v1.0.0", "Main", "MP", 0, "PN");
    }
    public void Lights()
    {
        PurchaseItem("v1.0.0", "Main", "SL", 0, "PN");
    }

    public void BuyItem()
    {
        PurchaseItem("v1.0.0", "Main", CurrentItem.SelectedItemName, CurrentItem.SelectedItemCost, CurrentItem.SelectedItemCurrency);
    }

    public void SelectItem(GameObject selecteditem)
    {
        SelectedItem = selecteditem;
        if (SelectedItem != null)
        {
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                if (SelectedItems[i].SelectedItemName == SelectedItem.gameObject.name)
                    CurrentItem = SelectedItems[i];
            }
        }
    }
    void CurrentQuest()
    {
        QuestName = DialogManager.GetComponent<cshDialog>().QuestName;
        QuestContents = DialogManager.GetComponent<cshDialog>().QuestContents;

        if (QuestName != "")
        {
            QuestWindow.SetActive(true);
            QuestNameWindow.GetComponent<TextMeshProUGUI>().text = QuestName;
            QuestContentsWindow.GetComponent<TextMeshProUGUI>().text = QuestContents;
        }
        else if (QuestName == "")
        {
            QuestWindow.SetActive(false);
        }
    }
    void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"Ancestor", "Arthur"},
            {"Successor", "Fred"}
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("Ancestor")) Debug.Log("No Ancestor");
            else Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),OnGetUserInvectorySuccess,OnError);
    }
    public void OnGetUserInvectorySuccess(GetUserInventoryResult result)
    {
        int PN = result.VirtualCurrency["PN"];
        PNValue.GetComponent<TextMeshProUGUI>().text = PN.ToString()+"Ç¬";

        int JN = result.VirtualCurrency["JN"];
        JNValue.GetComponent<TextMeshProUGUI>().text = JN.ToString()+"Àü";

        if (result.Inventory.Count == 0)
        {
        }
        else
        {
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                var items = result.Inventory[i];
                for(int j = 0; j < ItemSprite.Length; j++)
                {
                    if (items.DisplayName == ItemSprite[j].name)
                    {
                        InventoryButton[i].name = items.DisplayName;
                        InventoryButtonImage[i].GetComponent<Image>().sprite = ItemSprite[j];
                        InventoryButtonImage[i].GetComponent<Image>().color = new Color32(255,255,255,255);
                        InventoryButton[i].GetComponentInChildren<TextMeshProUGUI>().text = items.RemainingUses.ToString();
                    }
                }
            }
        }
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }

    public void AddVirtualCurrency(string key, int num)
    {
        var request = new AddUserVirtualCurrencyRequest() { VirtualCurrency = key, Amount = num };
        PlayFabClientAPI.AddUserVirtualCurrency(request,
        (result) => GetVirtualCurrencies(),
        (error) => Debug.Log(error.ErrorMessage));
    }
    public void SubtractVirtualCurrency(string key, int num)
    {
        var request = new SubtractUserVirtualCurrencyRequest() { VirtualCurrency = key, Amount = num };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request,
        (result) => GetVirtualCurrencies(),
        (error) => Debug.Log(error.ErrorMessage));
    }

    public void PurchaseItem(string catalogVersion, string storeId, string itemId, int price, string virtualCurrency)
    {
        var request = new PurchaseItemRequest()
        {
            CatalogVersion = catalogVersion,
            StoreId = storeId,
            ItemId = itemId,
            Price = price,
            VirtualCurrency = virtualCurrency
        };
        PlayFabClientAPI.PurchaseItem(request,
            result =>
            {
                GetVirtualCurrencies();
                Debug.Log("Purchase Item Success!");
            }, OnError);
    }
    public void ConsumeItem(string itemInstanceId)
    {
        var request = new ConsumeItemRequest { ConsumeCount = 1 , ItemInstanceId = itemInstanceId};
        PlayFabClientAPI.ConsumeItem(request, (result) => GetVirtualCurrencies(), OnError);
    }
}
