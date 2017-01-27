/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Displays the test dogs at home on the Main Screen.
 * Usage: REMOVE THIS CLASS AFTER FIRST PLAYABLE
 */

public class HomeDisplay : PPUIElement
{
    DogSlot[] dogSlots;

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        dogSlots = GetComponentsInChildren<DogSlot>();
    }

    protected override void fetchReferences()
	{
		base.fetchReferences();
        Init();
    }

	#endregion

    // Initializes the Home Display by setting up references and displaying dog thumbnails.
    public void Init()
    {
        displayThumbnails();
    }

    // Displays thumbnails of a sample of dogs currently at home.
    void displayThumbnails()
    {
        int numOfDogs = gameController.DogData.Dogs.Length;

        for(int i = 0; i < dogSlots.Length; i++)
        {
            if(i < numOfDogs)
            {
                // TEMP USE FOR TESTING
                // 12 total stock photos
                //Sprite dogSprite = SpriteUtil.GetDogSprite(Random.Range(1, 12));
				//dogSlots[i].Init(DogDescriptor.Default(), dogSprite);
            }
        }
    }

}
