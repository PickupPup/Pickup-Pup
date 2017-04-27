/*
 * Author(s): Grace Barrett-Snyder
 * Description: Controls a dog's name tag above its dog world slot.
 */

using UnityEngine.UI;

public class NameTag : PPUIElement
{
    Dog dog;

    UIButton uiButton;
    Text nameText;

    PPData.DogAction onNameTagClick;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        uiButton = gameObject.AddComponent<UIButton>();
        uiButton.SubscribeToClick(ExecuteClick);
        UISFXHandler sfxScript = GetComponent<UISFXHandler>();
        sfxScript.DisableSounds();
        nameText = GetComponentInChildren<Text>();
    }

    #endregion

    public void Init(DogSlot dogSlot, Dog dog)
    {
        this.dog = dog;
        nameText.text = dog.Info.Name;
        dogSlot.SubscribeToClickWhenOccupied(show);       
    }

    public void ExecuteClick()
    {
        callOnClick(this.dog);
    }

    public void SubscribeToClick(PPData.DogAction clickAction)
    {
        onNameTagClick += clickAction;
    }

    public void UnsubscribeFromClick(PPData.DogAction clickAction)
    {
        onNameTagClick -= clickAction;
    }

    protected virtual void callOnClick(Dog dog)
    {
        if (onNameTagClick != null)
        {
            onNameTagClick(dog);
        }
    }

    public void TryDeselect()
    {
        Hide();
    }

    void show(Dog dog)
    {
        base.Show();
    }

}
