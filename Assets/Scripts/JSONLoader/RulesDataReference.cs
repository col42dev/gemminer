using System.Collections.Generic;
using System.Collections;

public class RulesDataReference  {

    public abstract class RulesDataSet
    {
        public abstract string GetMyJSONURL();
    }

    public class RulesDataSet<T> : RulesDataSet
    {
        private string mURL;

        public RulesDataSet(string URL)
        {
            mURL = URL;
        }

        public override string GetMyJSONURL()
        {
            return mURL;
        }
    }

    #region Private Members
    private static RulesDataReference instance = null;
    #endregion

    #region Public Properties
    public static RulesDataReference Instance {
        get {
            if (instance == null)
            {
                instance = new RulesDataReference();
            }
            return instance;
        }
    }
    public IEnumerator GetEnumerator() { return Instance.mRulesDataSets.GetEnumerator();  }
    public int DataSetCount() { return Instance.mRulesDataSets.Count; }
    #endregion

    #region Private Members
    private List<RulesDataSet> mRulesDataSets = new List<RulesDataSet >();
    #endregion

    #region Constructors
    public RulesDataReference() {

        mRulesDataSets.Add(new RulesDataSet<SampleState>("https://api.myjson.com/bins/51viy?pretty=1"));
    }
    #endregion

}
