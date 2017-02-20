/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

public class DogWorld : PPUIController 
{	
    DogHotSpot[] dogs;

    protected override void setReferences()
    {
        base.setReferences();
        dogs = GetComponentsInChildren<DogHotSpot>();
    }

}
