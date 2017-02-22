/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a one-time popup prompt for a particular scene
 */

using UnityEngine;

public class PopupPrompt : PPUIElement
{
    [SerializeField]
    PromptID currentID;
    LanguageDatabase languages;

    #region MonoBehaviourExtended Overrides

    protected override void fetchReferences()
    {
        base.fetchReferences();
        languages = PPGameController.GetInstance.Languages;
    }

    #endregion

    public void Set(PromptID id)
    {
        currentID = id;
        if (currentID != PromptID.None)
        {
            if(languages == null)
            {
                checkReferences();
            }
            SetText(languages.GetTerm(currentID.ToString()));
        }
    }

}

public enum PromptID
{
    None,
    ShelterPrompt,
    ScoutingPrompt,
	FirstLivingRoomPrompt,
	ShopPrompt,
}
