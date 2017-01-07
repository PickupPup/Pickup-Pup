/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a DogSlot for a Dog currently at home (no text).
 */

using UnityEngine;

public class DogHomeSlot : DogSlot
{
    #region DogSlot Overrides

    public override void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
        base.Init(dog, dogSprite, backgroundSprite);
    }

    #endregion

}
