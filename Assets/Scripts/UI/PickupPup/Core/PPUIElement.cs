/*
 * Author(s): Isaiah Mann
 * Description: UI Elements for Pickup Pup
 */

using k = PPGlobal;
using UnityEngine;
using UnityEngine.UI;

public class PPUIElement : UIElement 
{
    protected const string FIND_GIFT = k.FIND_GIFT;
    protected const string REDEEM_GIFT = k.REDEEM_GIFT;
    protected const string TAP_TO_REDEEM = k.TAP_TO_REDEEM;

    protected PPGameController gameController;
    protected PPSceneController sceneController;
    protected LanguageDatabase languageDatabase;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        gameController = PPGameController.GetInstance;
        sceneController = PPSceneController.Instance;
        languageDatabase = LanguageDatabase.Instance;
    }

    protected virtual bool requestReloadScene()
    {
        return sceneController.RequestReloadCurrentScene();
    }

    #endregion

    protected void setComponents(Text text, string textString, Color textColor,
        Image image, Color imageColor, UIElement uiElement, bool showUIElement)
    {
        if (text)
        {
            text.text = textString;
            text.color = textColor;
        }
        if (image)
        {
            image.color = imageColor;
        }
        if (uiElement)
        {
            if (showUIElement)
            {
                uiElement.Show();
            }
            else
            {
                uiElement.Hide();
            }
        }
    }

    protected void setComponents(Text text, string textString, Color textColor,
        Image image, Color imageColor, Button button, bool buttonInteractable,
        UIElement uiElement, bool showUIElement)
    {
        if (button)
        {
            button.interactable = buttonInteractable;
        }
        setComponents(text, textString, textColor, image, imageColor, uiElement, showUIElement);
    }

}
