/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

public class DogSpriteDisplayTool : MonoBehaviourExtended 
{	
    [SerializeField]
    Text nameDisplay;

    [SerializeField]
    Image dogProfilePortrait;

    [SerializeField]
    Image dogWorldSprite;

    public void Display(DogDescriptor dog)
    {
        this.nameDisplay.text = dog.Name;
        this.dogProfilePortrait.sprite = dog.Portrait;
        this.dogWorldSprite.sprite = dog.WorldSprite;
    }

}
