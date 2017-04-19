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
        displayAllDogs();
    }

    #endregion

    void displayAllDogs()
    {
        DogDatabase data = DogDatabase.GetInstance;
        int numDogs = data.Dogs.Length;
        viewParent.sizeDelta = Vector2.one + Vector2.right * numDogs * widthPerDisplay;
        foreach(DogDescriptor dog in data.Dogs)
        {
            addDogToView(dog);
        }
    }

    void addDogToView(DogDescriptor dog)
    {
        DogSpriteDisplayTool display = Instantiate(displayPrefab);
        display.Display(dog);
        display.transform.SetParent(viewParent);
        display.transform.localScale = Vector3.one;
    }

}
