/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Controls adjustments to the Player's Home
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : SingletonController<HomeController> {

    DogDatabase dogData;
    DogDescriptor[] dogs;

    int numOfSlots = 10;
    int numOfAvailableSlots = 0;
    int numOfDogsHome = 0;

    public void Init()
    {
        dogData = ((PPGameController) PPGameController.Instance).Data;
        dogs = dogData.Dogs;
    }

    public void addSlot()
    {
        numOfSlots++;
        numOfAvailableSlots++;
    }

    public void removeSlot()
    {
        numOfSlots--;
        if (numOfAvailableSlots > 0)
        {
            numOfAvailableSlots--;
        }
    }

	public void addDog()
    {
        numOfAvailableSlots--;
        numOfDogsHome++;
    }

    public void removeDog()
    {
        numOfAvailableSlots++;
        numOfDogsHome--;
    }

    public DogDescriptor[] Dogs
    {
        get { return dogs; }
    }
}