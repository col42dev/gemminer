using UnityEngine;
using SimpleJSON;

using System;
using System.Collections;
using System.Reflection;

public class RulesDataFileSysLoader : MonoBehaviour {

    #region Private Members
    private bool mbLoadSuccess = true;
    private int mDataSetLoadedTally = 0;
    private bool mbPendingLoad = false;

    #endregion

    #region Public Methods
    public void Start()
    {
        mbLoadSuccess = true;
        mDataSetLoadedTally = 0;
        mbPendingLoad = true;
    }
    public void Update()
    {
        if (mbPendingLoad)
        {
                    mbPendingLoad = false;

                    // Get data request
                    IEnumerator dataSetsEnumerator = RulesDataReference.Instance.GetEnumerator();
                    while (dataSetsEnumerator.MoveNext())
                    {
                            RulesDataReference.RulesDataSet rds = dataSetsEnumerator.Current as RulesDataReference.RulesDataSet;
                            string jsonfilepath = "RulesData/51viy";
                            TextAsset fileContents = (TextAsset)Resources.Load(jsonfilepath);
                            JSONClass jsondata = JSON.Parse(fileContents.text) as JSONClass;
                            Type myType = rds.GetType().GetGenericArguments()[0];
                            Activator.CreateInstance(myType, jsondata);

                            /*
                            MethodInfo methodInfo = myType.GetMethod("PushRawDataToView");
                            if (methodInfo != null)
                            {
                                string ejs = jsondata.ToString();
                                ejs = ejs.Replace('\n', ' ');
                                methodInfo.Invoke(null, new object[] { ejs });
                            }*/

                            mDataSetLoadedTally++;
                    }
        }
    }
    #endregion

    #region Private Methods
  
    #endregion

}
