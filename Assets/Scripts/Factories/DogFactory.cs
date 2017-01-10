/*
 * Author(s): Isaiah Mann
 * Description: Produces dog instances
 */

using UnityEngine;

public class DogFactory : ObjectFactory<Dog>
{
	const float STANDARD_TIME_STEP_SEC = 1f;

	bool hideGameObjects = true;

	public DogFactory(bool hideGameObjects)
	{
		this.hideGameObjects = hideGameObjects;
	}

	public override Dog Create(params object[] args)
	{
		DogDescriptor info = args[0] as DogDescriptor;
		// <onoBehaviours must be assigned to GameObjects
		GameObject dogObject = new GameObject();
		if(this.hideGameObjects)
		{
			dogObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		}
		Dog dog = dogObject.AddComponent<Dog>();
		dog.Set(info);
		PPTimer scoutingTimer = new PPTimer(dog.TotalTimeToReturn, STANDARD_TIME_STEP_SEC);
		dog.GiveTimer(scoutingTimer);
		return dog;
	}	

	public override Dog[] CreateGroup(params object[] args)
	{
		Dog[] dogs = new Dog[args.Length];
		for(int i = 0; i < dogs.Length; i++)
		{
			dogs[i] = Create(args[i]);
		}
		return dogs;
	}

}
