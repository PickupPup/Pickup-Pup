/*
 * Author(s): Isaiah Mann
 * Description: Dogs will travel / be displayed at this spot on the UI
 * Usage: [no notes]
 */

using System.Linq;
using System.Collections.Generic;

public class DogHotSpot : PPUIElement
{	
    static HashSet<DogHotSpot> activeHotSpots = new HashSet<DogHotSpot>();

    public static DogHotSpot[] GetActiveHotSpots()
    {
        return activeHotSpots.ToArray();
    }

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        activeHotSpots.Add(this);
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        activeHotSpots.Remove(this);
    }

    #endregion

    public void PlaceDog(Dog dog)
    {
        // TODO: Implement
    }

}
