/*
 * Author(s): Isaiah Mann
 * Descriptor: A single trait or characteristic of a dog
 */

[System.Serializable]
public abstract class DogTrait : PPData {
	public DogTrait (DogDatabase data) : base(data) {}
}
