using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogOutsideSlot : DogSlot
{
    Text nameText;
    Text timerText;

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);

        Text[] text = GetComponentsInChildren<Text>();
        nameText = text[0];
        timerText = text[1];

        nameText.text = dog.Name;
    }

}
