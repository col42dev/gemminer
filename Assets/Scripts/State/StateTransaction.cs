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
    #endregion

    // Use this for initialization
    void Start () {
        mTransactions = new List<string>();
    }

    public void TransactionMessage(string message)
    {
        Debug.Log("Message 1 received:" + message);
        mTransactions.Add(message);
    }

    public void RequestTransmitState()
    {
        // 
        Debug.Log("Transmiting State");

        //string url = "http://localhost:3000/transactions/";
		string url = "http://localhost:8080/";
        WWWForm form = new WWWForm();

        string messageBody = "";
        foreach (string transactionMessage in mTransactions)
        {
            messageBody += transactionMessage + '\n';
        }
        form.AddField("message", messageBody);

        mTransactions.Clear();

        //Hashtable headers = new Hashtable();
        //headers.Add("Content-Type", "application/text");
        byte[] bytes = Encoding.ASCII.GetBytes(messageBody.ToCharArray());//new byte[messageBody.Length * sizeof(char)];

        //System.Buffer.BlockCopy(messageBody.ToCharArray(), 0, bytes, 0, bytes.Length);
        WWW www = new WWW(url, bytes);
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


