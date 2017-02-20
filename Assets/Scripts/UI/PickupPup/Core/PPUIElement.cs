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

    bool hasCanvasRef
    {
        get
        {
            return canvasRef != null;
        }
    }

    UICanvas canvasRef;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        canvasRef = GetComponent<UICanvas>();
    }

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

    #region UIElementOverrides

    public override void Show()
    {
        base.Show();
        if (hasCanvasRef)
        {
            canvasRef.Show();
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (hasCanvasRef)
        {
            canvasRef.Hide();
        }
    }

    #endregion

    protected virtual void enable(bool isEnabled)
    {

    }

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
        Image image, Color imageColor, UIButton button, bool buttonInteractable,
        UIElement uiElement, bool showUIElement)
    {
        if (button)
        {
            button.ToggleInteractable(buttonInteractable);
        }
        setComponents(text, textString, textColor, image, imageColor, uiElement, showUIElement);
    }

}
