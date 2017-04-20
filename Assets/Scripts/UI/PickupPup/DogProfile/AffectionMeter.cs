using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffectionMeter : PPUIElement
{
    [SerializeField]
    Image[] hearts;

    [SerializeField]
    Sprite emptyHeart;
    [SerializeField]
    Sprite halfHeart;
    [SerializeField]
    Sprite fullHeart;

    public void setAffection(float newAffection)
    {
        int halfHeartIntervals = (int)(newAffection * 2);
        
        foreach(Image heart in hearts)
        {
            if(halfHeartIntervals >= 2)
            {
                heart.sprite = fullHeart;
                halfHeartIntervals -= 2;
            }
            else if(halfHeartIntervals == 1)
            {
                heart.sprite = halfHeart;
                halfHeartIntervals -= 1;
            }
            else
            {
                heart.sprite = emptyHeart;
            }
        }

        if(halfHeartIntervals < 0)
        {
            Debug.LogError("Something went wrong affection meter counter (" + halfHeartIntervals + ") should not be less that 0");
        }
    }
}
