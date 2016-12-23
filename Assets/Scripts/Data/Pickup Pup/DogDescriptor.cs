/*
 * Author(s): Isaiah Mann 
 * Description: Stores the data about a dog
 */

using UnityEngine;

[System.Serializable]
public class DogDescriptor : PPDescriptor {
	const float DEFAULT_TIME_TO_COLLECT = 10f;

	public string Name;
	public int Age;
	public string Color;
	public float TimeToCollect = DEFAULT_TIME_TO_COLLECT;
	public bool IsOutCollecting = false;
	public float RemainingTimeCollecting = 0;
	public string Breed;

	public DogBreed IBreed {
		get {
			return data.GetBreed(Breed);
		}
	}
	public Color IColor {
		get {
			return parseHexColor(Color);
		}
	}

	public DogDescriptor (DogDatabase data) : base(data) {}
}
