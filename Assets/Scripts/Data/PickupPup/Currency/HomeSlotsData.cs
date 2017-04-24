/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's home slots currency.
 */

[System.Serializable]
public class HomeSlotsData : CurrencyData
{
    int vacantSlots;

    #region Instance Accessors

    public int VacantSlots
    {
        get
        {
            return vacantSlots;
        }
    }

    public bool HasVacantSlot
    {
        get
        {
            return vacantSlots > 0;
        }
    }

    public int OccupiedSlots
    {
        get
        {
            return amount - vacantSlots;
        }
    }

    public bool HasOccupiedSlot
    {
        get
        {
            return OccupiedSlots > 0;
        }
    }

    #endregion

    public HomeSlotsData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.HomeSlots;
        amount = initialAmount;
        vacantSlots = amount;
    }

    #region CurrencyData Overrides

    // Only the vacant slots are exchanged as currency
    // Ex: you need a vacant slot to adopt a dog
    public override bool CanAfford(int cost)
    {
        return vacantSlots >= cost;
    }

    public override void ChangeBy(int deltaAmount)
    {
        vacantSlots += deltaAmount;
    }

	public override void Give()
	{
		throw new System.NotImplementedException();
	}

    #endregion

}
