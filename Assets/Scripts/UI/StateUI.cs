using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SimpleJSON;

public class StateUI : MonoBehaviour
{

    public GameObject ResourceItem;
	
    private bool mPendingPopulate = true;
    private int mGoldQuantity = 0;



    void Update()
    {
        if (SampleState.Instance != null)
        {
            if (mPendingPopulate)
            {
                mPendingPopulate = false;

                JSONClass resources = SampleState.Instance.mState["resources"] as JSONClass;

                foreach (KeyValuePair<string, JSONNode> stateObj in resources)
                {
                    JSONNode node = stateObj.Value;
                    string textOut = stateObj.Key + ", Quantity: " + (string)node["quantity"] + ", Cost: " + (string)node["cost"] + "\n";
                    GameObject test3 = GameObject.Instantiate(ResourceItem);
                    test3.transform.FindChild("Text").GetComponent<Text>().text = textOut;
                    test3.transform.SetParent(this.gameObject.transform.FindChild("ResourceListComponent").transform.FindChild("ResourceContainer").transform);
                }
            }

            JSONClass gold = SampleState.Instance.mState["gold"] as JSONClass;
            if ( mGoldQuantity != gold["quantity"].AsInt) {
                mGoldQuantity = gold["quantity"].AsInt;
                GameObject.Find("Gold").GetComponent<Text>().text = "Gold: " + mGoldQuantity;
            }

     
        }
    }

    public void HandleBuyRequest(Text textUI)
    {
        Match match = Regex.Match(textUI.text, @"(\S*), .*");
        if (match.Success)
        {
            string resourceName = match.Groups[1].Value.ToString();
            JSONNode resourceNode = SampleState.Instance.mState["resources"][resourceName];
            
            JSONClass gold = SampleState.Instance.mState["gold"] as JSONClass;
            int newGoldQuantity = gold["quantity"].AsInt - resourceNode["cost"].AsInt;
            if (newGoldQuantity >= 0) {
                gold["quantity"] = new JSONData(newGoldQuantity);
        
                int newQuantity = resourceNode["quantity"].AsInt + 1;
                resourceNode["quantity"] = new JSONData(newQuantity);
                textUI.text = resourceName + ", Quantity: " + (string)resourceNode["quantity"] + ", Cost: " + (string)resourceNode["cost"] + "\n";
                string transactionMessage = "buy 1 " + resourceName;
                ExecuteEvents.Execute<IStateTransactionMessageTarget>(GameObject.Find("StateTransaction"), null, (x, y) => x.TransactionMessage(transactionMessage));
            }
        }
    }

    public  void HandleSellRequest(Text textUI)
    {
        Match match = Regex.Match(textUI.text, @"(\S*), .*");
        if (match.Success)
        {
            string resourceName = match.Groups[1].Value.ToString();
            JSONNode resourceNode = SampleState.Instance.mState["resources"][resourceName];

 
            int newResourceQuantity = resourceNode["quantity"].AsInt - 1;

            if (newResourceQuantity >= 0) {
                JSONClass gold = SampleState.Instance.mState["gold"] as JSONClass;
                int newGoldQuantity = gold["quantity"].AsInt + resourceNode["cost"].AsInt;

                gold["quantity"] = new JSONData( newGoldQuantity);
                resourceNode["quantity"] = new JSONData( newResourceQuantity);
                textUI.text = resourceName + ", Quantity: " + (string)resourceNode["quantity"] + ", Cost: " + (string)resourceNode["cost"] + "\n";
                string transactionMessage = "sell 1 " + resourceName;
                ExecuteEvents.Execute<IStateTransactionMessageTarget>(GameObject.Find("StateTransaction"), null, (x, y) => x.TransactionMessage( transactionMessage));
            }
            
        }
    }

    public  void HandleCheatQuantity(Text textUI)
    {
        Match match = Regex.Match(textUI.text, @"(\S*), .*");
        if (match.Success)
        {
            string resourceName = match.Groups[1].Value.ToString();
            JSONNode resourceNode = SampleState.Instance.mState["resources"][resourceName];

            int newQuantity = resourceNode["quantity"].AsInt + 1;
            resourceNode["quantity"] = new JSONData(newQuantity);
            textUI.text = resourceName + ", Quantity: " + (string)resourceNode["quantity"] + ", Cost: " + (string)resourceNode["cost"] + "\n";
         }
    }


    public  void HandleTransmitState()
    {
        ExecuteEvents.Execute<IStateTransactionMessageTarget>(GameObject.Find("StateTransaction"), null, (x, y) => x.RequestTransmitState());
    }
}