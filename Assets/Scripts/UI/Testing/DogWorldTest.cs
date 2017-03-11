/*
 * Author(s): Isaiah Mann
 * Description: Tests showing dogs w/ the dog world feature
 * Usage: [no notes]
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogWorldTest : MonoTest 
{
    public override bool RunTest(out string feedback)
    {
        DogDatabase data = DogDatabase.GetInstance;
        for(int i = 0; i < 3; i++)
        {
            dataController.Adopt(data.Dogs[i]);
        }
        feedback = "Good job, Joel";
        return true;
    }

}
