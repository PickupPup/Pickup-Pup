/*
 * Author(s): Isaiah Mann
 * Description: Generic class for unit testing
 * Usage: [no notes]
 */

using UnityEngine;

public abstract class MonoTest : MonoBehaviourExtended 
{
    const string PASS = "TEST PASSED";
    const string FAIL = "TEST FAILED";

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        processTest();
    }

    #endregion

    protected virtual void processTest()
    {   
        string testFeedback;
        bool success = RunTest(out testFeedback);
        if(success)
        {
            Debug.LogFormat("{0}\n{1}", PASS, testFeedback);
        }
        else
        {
            Debug.LogErrorFormat("{0}\n{1}", FAIL, testFeedback);
        }
    }

    public abstract bool RunTest(out string feedback);

}
