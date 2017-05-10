/*
 * Author(s): Isaiah Mann
 * Description: Describes an analytics event related to currency
 * Usage: [no notes]
 */

using k = PPGlobal;

public class CurrencyAnalyticsEvent : PPAnalyticsEvent
{
    public const string SHOP_PURCHASE = "Shop Purchase";

    public CurrencyAnalyticsEvent(string id, CurrencyData currency) :
    base(id, getPropertyKeys(), getPropertyValues(currency))
    {
        // NOTHING
    }

    static string[] getPropertyKeys()
    {
        return new string[]{k.TYPE, k.AMOUNT};
    }

    static object[] getPropertyValues(CurrencyData currency)
    {
        return new object[]{currency.Type.ToString(), currency.Amount};
    }

}
