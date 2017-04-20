using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffectionMeter : PPUIElement
{
    [SerializeField]
    Image[] hearts;

    [SerializeField]
    Sprite[] heartIncrements;

    public void setAffection(float newAffection)
    {
        float affectionIncrease = PPGameController.GetInstance.Tuning.AffectionIncrease;

        int heartIntervals = (int)(newAffection / affectionIncrease);

        foreach(Image heart in hearts)
        {
            int currentHeartToUse = Mathf.Min(heartIntervals, heartIncrements.Length-1);

            heart.sprite = heartIncrements[currentHeartToUse];
            heartIntervals -= currentHeartToUse;
        }

        if(heartIntervals < 0)
        {
            Debug.LogError("Something went wrong affection meter counter (" + heartIntervals + ") should not be less that 0");
        }
    }
}
