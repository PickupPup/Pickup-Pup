/*
 * Author(s): Grace Barrett-Snyder, Isaiah Mann
 * Description: Displays a Dog's thumbnail (applies to both Outside and Home slots).
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogSlot : MonoBehaviourExtended 
{
	[SerializeField]
	bool setBackground = true;
	[SerializeField]
	Text price;

	DogDescriptor dog;
    Image backgroundImage;
    Image dogImage;

    Text nameText;
    Text timerText;

    /// <summary>
    /// Initializes this Dog Slot by setting component references and displaying its sprites.
    /// </summary>
	public void Init(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null) {
		this.dog = dog;
		Image[] images = GetComponentsInChildren<Image>();
		if(images.Length == 2 && setBackground) 
		{
			backgroundImage = images[0];
	        dogImage = images[1];
		} 
		else if(images.Length == 1 || !setBackground) 
		{
			dogImage = images[0];
		}

        // Home slots don't have text components
        Text[] text = GetComponentsInChildren<Text>();
        if(text.Length == 2)
        {
            nameText = text[0];
            timerText = text[1];
		} 
		else if(text.Length == 1) 
		{
			nameText = text[0];
		}

		setSlot(this.dog, dogSprite, backgroundSprite);
    }
	
    /// <summary>
    /// Sets the dog and background sprites of this Dog Slot.
    /// </summary>
	void setSlot(DogDescriptor dog, Sprite dogSprite, Sprite backgroundSprite = null)
    {
		if (nameText) {
			nameText.text = dog.Name;
		}
        dogImage.sprite = dogSprite;
		if (backgroundImage) {
        	backgroundImage.sprite = backgroundSprite;
		}
		if (price) 
		{
			price.text = dog.CostToAdoptStr;
		}
    }
}