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
	public bool IsOutCollecting = false;
	public float RemainingTimeCollecting = 0;
	public string Breed;
    public int SpriteID;
	public int CostToAdopt {
		get {
			return IBreed.CostToAdopt + ModCost;
		}
	}
	public string CostToAdoptStr {
		get {
			if (ModCost == 0) {
				return IBreed.CostToAdoptStr;
			} else {
				return formatCost(CostToAdopt);
			}
		}
	}
	public int ModCost;

	DogBreed _iBreed;
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

	public static DogDescriptor Default () {
		DogDescriptor descriptor = new DogDescriptor(DogDatabase.Instance);
		descriptor.Name = string.Empty;
		descriptor.Age = 0;
		descriptor.Breed = string.Empty;
		descriptor.Color = BLACK_HEX;
        descriptor.SpriteID = 0;
		descriptor.IsOutCollecting = false;
		return descriptor;
	}
}