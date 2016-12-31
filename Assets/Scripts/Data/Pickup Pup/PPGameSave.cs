/*
 * Author: Isaiah Mann
 * Desc: Serializable data for Pickup Pup
 */

using System.Collections.Generic;

[System.Serializable]
public class PPGameSave : GameSave {
	public List<DogDescriptor> AdoptedDogs;
	public int Money;
	public int Food;

	public PPGameSave (DogDescriptor[] dogs, int money, int food) {
		this.AdoptedDogs = new List<DogDescriptor>(dogs);
		this.Money = money;
		this.Food = food;
	}

	public void Adopt (DogDescriptor dog) {
		AdoptedDogs.Add(dog);
	}
}
