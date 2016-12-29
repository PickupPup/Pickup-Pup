/*
 * Author: Isaiah Mann
 * Description: Controls a dog in the game world
 */

using UnityEngine;

public class Dog : MobileObjectBehaviour {

	[SerializeField]
	// Tracks how long the dog will be away from the house
	protected PPTimer awayFromHomeTimer;
	DogDescriptor descriptor;

	public string Name {
		get {
			return descriptor.Name;
		}
	}

	public void Set (DogDescriptor descriptor) {
		this.descriptor = descriptor;
	}

	protected override void SetReferences () {
		base.SetReferences ();
		awayFromHomeTimer.Init();
	}
}
