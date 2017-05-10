/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using System.Collections.Generic;
using k = PPGlobal;

public class MixpanelAnalyticsInterchange : IAnalyticsInterchange
{
	#region IAnalyticsInterchange Interface

	void IAnalyticsInterchange.SendEvent(AnalyticsEvent targetEvent)
	{
		Mixpanel.SendEvent(targetEvent.ID, targetEvent.Properties);
	}

    void IAnalyticsInterchange.UpdateProperties(PPGameSave gameSave)
    {
        Dictionary<string, object> properties = Mixpanel.SuperProperties;
        properties[CurrencyType.Coins.ToString()] = gameSave.Currencies.Coins.Amount;
        properties[CurrencyType.DogFood.ToString()] = gameSave.Currencies.DogFood.Amount;
        properties[k.TIME_PLAYED] = gameSave.TimePlayed.ToReadableString();
        properties[k.SESSION_COUNT] = gameSave.GameSessionCount;
        properties[k.ADOPTED_DOGS_COUNT] = gameSave.AdoptedDogs.Count;
    }

	#endregion

}
