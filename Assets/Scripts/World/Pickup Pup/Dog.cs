/*
 * Author: Isaiah Mann
 * Description: Controls a dog in the game world
 */

public class Dog : MobileObjectBehaviour {
	DogDescriptor descriptor;
	public string Name {
		get {
			return descriptor.Name;
		}
	}

	public void Set (DogDescriptor descriptor) {
		this.descriptor = descriptor;
	}
}
