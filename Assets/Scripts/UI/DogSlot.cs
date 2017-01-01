/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Displays a Dog's thumbnail (applies to both Outside and Home slots).
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogSlot : MonoBehaviourExtended {

    Image backgroundImage;
    Image dogImage;

    Text nameText;
    Text timerText;

    /// <summary>
    /// Initializes this Dog Slot by setting component references and displaying its sprites.
    /// </summary>
	public void Init(Sprite dogSprite, Sprite backgroundSprite = null) {
        Image[] images = GetComponentsInChildren<Image>();
        backgroundImage = images[0];
        dogImage = images[1];

        // Home slots don't have text components
        Text[] text = GetComponentsInChildren<Text>();
        if (text.Length == 2)
        {
            nameText = text[0];
            timerText = text[1];
        }

        setSlot(dogSprite, backgroundSprite);
    }
	
    /// <summary>
    /// Sets the dog and background sprites of this Dog Slot.
    /// </summary>
	void setSlot(Sprite dogSprite, Sprite backgroundSprite = null)
    {
        dogImage.sprite = dogSprite;
        backgroundImage.sprite = backgroundSprite;
    }
}