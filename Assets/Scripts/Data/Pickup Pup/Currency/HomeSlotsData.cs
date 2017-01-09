/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's home slots currency.
 */

[System.Serializable]
public class HomeSlotsData : CurrencyData
{
    HomeSlot[] slots;

    public HomeSlotsData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.HomeSlots;
        amount = initialAmount;
        slots = new HomeSlot[amount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new HomeSlot(HomeSlotStatus.Vacant);
        }
    }

    // Only the vacant slots are exchanged as currency
    // Ex: you need a vacant slot to adopt a dog
    public override bool CanAfford(int cost)
    {
        return HasVacantSlot;
    }
  
    public int VacantSlots
    {
        get
        {
            int vacantSlots = 0;
            foreach(HomeSlot slot in slots)
            {
                if (slot.Status == HomeSlotStatus.Vacant)
                {
                    vacantSlots++;
                }
            }
            return vacantSlots;
        }
    }

    public bool HasVacantSlot
    {
        get
        {
            foreach (HomeSlot slot in slots)
            {
                if (slot.Status == HomeSlotStatus.Vacant)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class HomeSlot
    {
        HomeSlotStatus status;

        public HomeSlot(HomeSlotStatus status)
        {
            this.status = status;
        }

        public HomeSlotStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

    }

}

public enum HomeSlotStatus
{
    Occupied,
    Vacant

}
