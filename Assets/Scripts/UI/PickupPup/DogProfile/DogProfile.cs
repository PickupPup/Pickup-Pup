/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Displays a Dog's Profile from its DogDescriptor
 */

using UnityEngine;
using UnityEngine.UI;
using k = PPGlobal;

public class DogProfile : PPUIElement
{
	DogProfileButtonController _buttonController;
	public DogProfileButtonController buttonController
    {
		get 
        {
			if (!_buttonController)
			{
				_buttonController = transform.GetComponent<DogProfileButtonController>();
			}
			return _buttonController;
		}
	}

    protected DogDescriptor dogInfo
    {
        get
        {
            return dog.Info;
        }
    }

    [SerializeField]
    Text nameText;
    [SerializeField]
    Text breedText;
    Text[] descriptionText; 

    [SerializeField]
    Image dogThumbnail;
    [SerializeField]
    Image collarImage;
	[SerializeField]
    UIElement[] descriptionFields; // Normal description must come first (not special)

    [SerializeField]
    protected GameObject iconsObject;
    [SerializeField]
    protected Button collarSlot;

    protected Dog dog;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();

		if (descriptionFields != null) {
			descriptionText = new Text[descriptionFields.Length];
			for (int i = 0; i < descriptionText.Length; i++) {
				descriptionText [i] = descriptionFields [i].GetComponentInChildren<Text> ();
			}
		}
    }

    #endregion

    #region UIElement Overrides

    public override void Hide()
    {
        EventController.Event(k.GetPlayEvent(k.BACK));
        base.Hide();
    }

    #endregion

    #if UNITY_EDITOR

    // Debugging cheat for increasing the affection of a selected dog
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && dogInfo != null)
        {
            dogInfo.IncreaseAffection();
        }
    }

    #endif

    public virtual void SetProfile(Dog dog)
    {
        this.dog = dog;
		buttonController.CalibrateIndex (dog);

        nameText.text = dogInfo.Name;
        breedText.text = dogInfo.Breed.Breed;
		if(descriptionText != null) {
			for(int i = 0; i < descriptionText.Length; i++) {
				if(i < dogInfo.Descriptions.Length) {
					descriptionFields [i].Show ();
					descriptionText [i].text = dogInfo.Descriptions [i];
				} else {
					descriptionFields [i].Hide ();
				}
			}
		}

        dogThumbnail.sprite = dog.Portrait;
        // TODO: Get collar icon
    }

    public void EditName(string newName)
    {
        // TODO: Make DogDescriptor name editable
        nameText.text = dogInfo.Name;
    }

}
