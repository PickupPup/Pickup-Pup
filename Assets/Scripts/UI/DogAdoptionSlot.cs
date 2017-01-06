using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogAdoptionSlot : DogSlot
{
    Text priceOrAdoptionStatus;

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);

        priceOrAdoptionStatus = GetComponentInChildren<Text>();
        priceOrAdoptionStatus.text = dog.CostToAdoptStr;
    }

    public void Adopt()
    {
        priceOrAdoptionStatus.text = "Adopted";
    }

}
