/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's home slots currency.
 */

[System.Serializable]
public class HomeSlotsData : CurrencyData
{
    HomeSlot[] slots;

    #region Instance Accessors

    public int VacantSlots
    {
        get
        {
            return countSlotsWithStatus(HomeSlotStatus.Vacant);
        }
    }

    public bool HasVacantSlot
    {
        get
        {
            return hasSlotWithStatus(HomeSlotStatus.Vacant);
        }
    }

    public int OccupiedSlots
    {
        get
        {
            return countSlotsWithStatus(HomeSlotStatus.Occupied);
        }
    }

    public bool HasOccupiedSlot
    {
        get
        {
            return hasSlotWithStatus(HomeSlotStatus.Occupied);
        }
    }

    #endregion

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

    #region CurrencyData Overrides

    // Only the vacant slots are exchanged as currency
    // Ex: you need a vacant slot to adopt a dog
    public override bool CanAfford(int cost)
    {
        return HasVacantSlot;
    }

    public override void IncreaseBy(int deltaAmount)
    {
        int slotsToChange = UnityEngine.Mathf.Abs(deltaAmount);
        if (deltaAmount > 0)
        {
            // Change occupied slot(s) to vacant
            changeSlots(HomeSlotStatus.Vacant, slotsToChange);
        }
        else if (deltaAmount < 0)
        {
            // Change vacant slot(s) to occupied
            changeSlots(HomeSlotStatus.Occupied, slotsToChange);
        }
    }

    #endregion

    void changeSlots(HomeSlotStatus desiredStatus, int numOfSlotsToChange)
    {
        int slotsChanged = 0;
        foreach(HomeSlot slot in slots)
        {
            if (slot.Status != desiredStatus)
            {
                slot.Status = desiredStatus;
                slotsChanged++;
                if (slotsChanged == numOfSlotsToChange)
                {
                    return;
                }
            }
        }
    }

    bool hasSlotWithStatus(HomeSlotStatus status)
    {
        foreach (HomeSlot slot in slots)
        {
            if (slot.Status == status)
            {
                return true;
            }
        }
        return false;
    }

    int countSlotsWithStatus(HomeSlotStatus status)
    {
        int slotsWithStatus = 0;
        foreach(HomeSlot slot in slots)
        {
            if(slot.Status == status)
            {
                slotsWithStatus++;
            }
        }
        return slotsWithStatus;
    }

}

public enum HomeSlotStatus
{
    Occupied,
    Vacant

}

[System.Serializable]
public class HomeSlot
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