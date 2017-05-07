/*
 * Author(s): Isaiah Mann
 * Description: A tool for displaying all of the dogs sprites (portraits and in world)
 * Usage: [no notes]
 */

using UnityEngine;

public class DogSpriteTool : MonoBehaviourExtended 
{	
    [SerializeField]
    DogSpriteDisplayTool displayPrefab;
	
    [SerializeField]
    RectTransform viewParent;

    [SerializeField]
    float widthPerDisplay = 400;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        DogSpriteDisplayTool[] dogDisplays = displayAllDogs();
        string portraitReport = dogPortaitSpriteReport(dogDisplays);
        if(!string.IsNullOrEmpty(portraitReport))
        {
            Debug.LogError(portraitReport);
        }
        string worldReport = dogWorldSpriteReport(dogDisplays);
        if(!string.IsNullOrEmpty(worldReport))
        {
            Debug.LogError(worldReport);
        }
    }

    #endregion

    DogSpriteDisplayTool[] displayAllDogs()
    {
        DogDatabase data = DogDatabase.GetInstance;
        int numDogs = data.Dogs.Length;
        DogDescriptor[] dogs = data.Dogs;
        DogSpriteDisplayTool[] displays = new DogSpriteDisplayTool[numDogs];
        viewParent.sizeDelta = Vector2.one + Vector2.right * numDogs * widthPerDisplay;
        for(int i = 0; i < numDogs; i++)
        {
            displays[i] = addDogToView(dogs[i]);
        }
        return displays;
    }

    DogSpriteDisplayTool addDogToView(DogDescriptor dog)
    {
        DogSpriteDisplayTool display = Instantiate(displayPrefab);
        display.Display(dog);
        display.transform.SetParent(viewParent);
        display.transform.localScale = Vector3.one;
        return display;
    }

    string dogPortaitSpriteReport(DogSpriteDisplayTool[] displays)
    {
        int count = 0;
        string report = "BROKEN PORTAIT SPIRTES:\n";
        foreach(DogSpriteDisplayTool display in displays)
        {
            if(!display.CheckPortrait())
            {
                report += string.Format("{0}\n", display.Dog.Name);
                count++;
            }
        }
        if(count > 0)
        {
            return report;
        }
        else
        {
            return string.Empty;
        }
    }

    string dogWorldSpriteReport(DogSpriteDisplayTool[] displays)
    {
        int count = 0;
        string report = "BROKEN WORLD SPRITES:\n";
        foreach(DogSpriteDisplayTool display in displays)
        {
            if(!display.CheckWorld())
            {
                report += string.Format("{0}\n", display.Dog.Name);
                count++;
            }
        }
        if(count > 0)
        {
            return report;
        }
        else
        {
            return string.Empty;
        }
    }

}
