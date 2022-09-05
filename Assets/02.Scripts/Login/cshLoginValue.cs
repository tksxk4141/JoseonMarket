using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using TMPro;

public class cshLoginValue : MonoBehaviour
{
    public static string username = "";
    public static int usernum = 0;
    public static List<string> friendList = null;
    public GameObject goldValues;
    public GameObject skyLights;

    private void Start()
    {
        GetVirtualCurrencies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddVirtualCurrency("GD", 100);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SubtractVirtualCurrency("GD", 100);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PurchaseItem("v1.0.0", "Main", "SL",100, "GD");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ConsumeItem("DDDC50A8F03ED1AD");
        }
    }

    void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),OnGetUserInvectorySuccess,OnError);
    }
    public void OnGetUserInvectorySuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency["GD"];
        goldValues.GetComponent<TextMeshPro>().text = coins.ToString();

        if (result.Inventory.Count == 0)
        {
            skyLights.GetComponent<TextMeshPro>().text ="-";
        }
        else
        {
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                var skylights = result.Inventory[i];
                skyLights.GetComponent<TextMeshPro>().text = skylights.DisplayName.ToString() + " : " + skylights.RemainingUses.ToString() + " °³";
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
