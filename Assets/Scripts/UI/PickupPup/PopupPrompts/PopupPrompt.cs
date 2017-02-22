/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a one-time popup prompt for a particular scene
 */

using UnityEngine;

public class PopupPrompt : PPUIElement
{
    [SerializeField]
    PromptID currentID;

    public void Set(PromptID id)
    {
        currentID = id;
        if (currentID != PromptID.None)
        {
			SetText(languageDatabase.GetTerm(currentID.ToString()));
        }
    }

}

public enum PromptID
{
    None,
    ShelterPrompt,
    ScoutingPrompt,
    ShopPrompt,
    FirstLivingRoomPrompt,
}
