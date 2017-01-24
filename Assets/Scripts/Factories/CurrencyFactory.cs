/*
 * Author(s): Isaiah Mann
 * Description: Produces currencies from numerical / JSON args
 * Usage: [no notes]
 */

using System;
using System.Reflection;

public class CurrencyFactory : ObjectFactory<CurrencyData>
{
    // Expects: (string type, int amount, float percent (if discout))
    public override CurrencyData Create (params object[] args)
    {
        CurrencyType type = Enum.Parse(typeof(CurrencyType), args[0].ToString());
        int amount = (int) args[1];
        if(type)
        {
            return new 
        }
        else
        {
            return new CurrencyData(type, amount);
        }
    }

    public CardMechanic GetMechanic (string json) {
        JSONNode node = JSON.Parse(json);
        string id = node[ID];
        string variantType =  node[VARIANT];
        MechanicType type = (MechanicType) Enum.Parse(typeof(MechanicType), node[TYPE]);
        int delay = node[DELAY].AsInt;
        int duration = node[DURATION].AsInt;
        int power = node[POWER].AsInt;
        string[] delegates = JSONToStringArray(node[DELEGATES].AsArray);
        MechanicStats stats = new MechanicStats(id, type, delegates);
        Type classType = Type.GetType(getClassName(variantType));
        ConstructorInfo constructor = classType.GetConstructor(new Type[]{typeof(MechanicStats)});
        return constructor.Invoke(new object[]{stats}) as CardMechanic;
    }


    public override CurrencyData[] CreateGroup (params object[] args)
    {
        throw new System.NotImplementedException ();
    }

}
