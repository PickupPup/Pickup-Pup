/*
 * Author(s): Isaiah Mann
 * Description: Controls the display of a group of dogs in an organized UI
 * Usage: Should be attached to the parent element in the UI that holds the child elements
 */

public class DogBrowser : PPUIElement 
{	
	DogSlot[] elements;

	protected override void setReferences()
	{
		base.setReferences();
		elements = GetComponentsInChildren<DogSlot>();
	}

	public void Set(Dog[] dogs) 
	{
		for(int i = 0; i < elements.Length; i++)
		{
			elements[i].Init(dogs[i]);
		}
	}

}
