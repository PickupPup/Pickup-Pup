/*
 * Author(s): Isaiah Mann
 * Description: Displays a dog's souvenir
 * Usage: [no notes]
 */

using UnityEngine;

public class SouvenirDisplay : CurrencyDisplay
{
    #region Instance Accessors 

    public bool HasSouvenir
    {
        get
        {
            return this.souvenir != null;
        }
    }

    #endregion

    [SerializeField]
    PPUIElement nameDisplay;
    [SerializeField]
    PPUIElement descriptionDisplay;

    [SerializeField]
    PPUIButton[] hideButtons;

    SouvenirData souvenir;

    public void Init(SouvenirData souvenir)
    {
        this.souvenir = souvenir;
        display(souvenir);
        initHideButtons();
    }

    void initHideButtons()
    {
        foreach(PPUIButton button in this.hideButtons)
        {
            button.TryUnsubscribeAll();
            button.SubscribeToClick(Hide);
        }
    }

    public override void Hide ()
    {
        Destroy();
    }

    void display(SouvenirData souvenir)
    {
        nameDisplay.SetText(souvenir.DisplayName);
        descriptionDisplay.SetText(souvenir.Description);
        setIcon(souvenir);
    }

}
