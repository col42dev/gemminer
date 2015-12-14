using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System.Text;


public interface IStateTransactionMessageTarget : IEventSystemHandler
{
    void TransactionMessage(string message);
    void RequestTransmitState();
}

public class StateTransaction : MonoBehaviour, IStateTransactionMessageTarget
{
    #region Private Members
    private List<string> mTransactions;
	private string url = "http://ec2-54-201-237-107.us-west-2.compute.amazonaws.com:8080/unity";

    #endregion

    // Use this for initialization
    void Start () {
        mTransactions = new List<string>();

		Debug.Log("Transmiting Start State");
		
		WWWForm form = new WWWForm();
		form.AddField("event", "launch");

		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
    }

	void  OnApplicationQuit() {
		Debug.Log("Transmiting Quit State");
		
		WWWForm form = new WWWForm();	
		form.AddField("event", "kill");

		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));

		System.Threading.Thread.Sleep(1000);
	}

    public void TransactionMessage(string message)
    {
        Debug.Log("Message 1 received:" + message);
        mTransactions.Add(message);
    }

    public void RequestTransmitState()
    {
        Debug.Log("Transmiting State");

        WWWForm form = new WWWForm();
        string messageBody = "transactions:";
        foreach (string transactionMessage in mTransactions)
        {
            messageBody += transactionMessage + "<br/>";
        }
		form.AddField("event", messageBody);

        mTransactions.Clear();

  
		WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: ");
            Debug.Log(www.text);

            GameObject.Find("TransactionResponse").GetComponent<Text>().text = www.text.Replace("<br/>", "\n");
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    } 

}


