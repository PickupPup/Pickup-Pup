/*
 * Author: Isaiah Mann
 * Description: Describes the traits of a particular breed
 */

using System;

[Serializable]
public class DogBreed : DogTrait {
	public string Name;
	public string Specialization;
	public CollectibleType ISpecialization {
		get {
			return (CollectibleType) Enum.Parse(typeof(CollectibleType), Specialization); 
		}
	}

	public DogBreed (DogDatabase data) : base(data) {}

	public override string ToString () {
		return string.Format ("[DogBreed:{0} Specialization:{1}]", Name, ISpecialization);
	}
}
