/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Controls adjustments to the Player's Home
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : SingletonController<HomeController> {

    DogDatabase dogData;
    List<DogDescriptor> dogs;

    int numOfSlots = 10;
    int numOfAvailableSlots = 0;
    int numOfDogsHome = 0;

    public void Init()
    {
        dogData = ((PPGameController) PPGameController.Instance).Data;
        dogs = new List<DogDescriptor>(dogData.Dogs);
    }

    public void AddSlot()
    {
        numOfSlots++;
    }

    public void RemoveSlot()
    {
        numOfSlots--;
    }

	public void AddDog(DogDescriptor dog)
    {
        dogs.Add(dog);
        numOfDogsHome++;
    }

    public void RemoveDog(DogDescriptor dog)
    {
        dogs.Remove(dog);
        numOfDogsHome--;
    }

    public List<DogDescriptor> Dogs
    {
        get { return dogs; }
    }

    public int DogCount
    {
        get { return dogs.Count; }
    }

    public int AvailableSlots
    {
        get { return numOfSlots - dogs.Count; }
    }
}