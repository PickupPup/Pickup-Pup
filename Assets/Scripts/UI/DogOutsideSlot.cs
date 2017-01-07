/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog that's outside (has name and timer).
 */

using UnityEngine;
using UnityEngine.UI;

public class DogOutsideSlot : DogSlot
{
    Text nameText;
    Text timerText;

    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);

        Text[] text = GetComponentsInChildren<Text>();
        nameText = text[0];
        timerText = text[1];

        nameText.text = dog.Name;
    }

    #endregion

}
