using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SampleState {


    #region Private Members
    private static SampleState instance = null;
    #endregion

    #region Public Properties
    public static SampleState Instance
    {
        get { return instance; }
    }
    public JSONClass mState = new JSONClass();
    #endregion


    #region Constructor
    public SampleState(JSONNode json)
    {
        instance = this;

        this.mState = json["data"] as JSONClass;

    }
    #endregion
}
