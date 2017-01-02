/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Displays the Player's dogs at home on the Main Screen.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDisplay : MonoBehaviourExtended {

    const int NUM_OF_SHOWN_SLOTS = 10; // # of slots shown on main screen

    HomeController homeController;
    DogSlot[] dogSlots;
    DogDescriptor[] dogs; // Contains dogs both inside and outside

    void Start()
    {
        Init();
    }

    /// <summary>
    /// Initializes the Home Display by setting up references and displaying dog thumbnails.
    /// </summary>
    public void Init()
    {
        homeController = HomeController.Instance;
        dogs = homeController.Dogs;
        dogSlots = GetComponentsInChildren<DogSlot>();

        displayThumbnails();
    }

    /// <summary>
    /// Displays thumbnails of a sample of dogs currently at home.
    /// </summary>
    void displayThumbnails()
    {
        //int numOfDogs = dogs.Length;
        int numOfDogs = 12;

        for(int i = 0; i < NUM_OF_SHOWN_SLOTS; i++)
        {
            if (i < numOfDogs)
            {
                // TEMP USE FOR TESTING
                // 12 total stock photos
                Sprite dogSprite = SpriteUtil.getDogSprite(Random.Range(1, 12));

                // TODO: Add condition to check if the dog is outside
                // If so, it shouldn't appear at home

                //Sprite dogSprite = SpriteUtil.getDogSprite(dogs[i].SpriteID);
				dogSlots[i].Init(DogDescriptor.Default(), dogSprite);
            }
        }
    }
}