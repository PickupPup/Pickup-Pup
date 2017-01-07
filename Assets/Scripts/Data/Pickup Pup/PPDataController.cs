/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles save for Pickup Pup
 */

using System.Collections.Generic;

public class PPDataController : DataController 
{
	#region Static Accessors

	// Casts the singleton from the superclass:
	public static PPDataController GetInstance
	{
		get 
		{
			return Instance as PPDataController;
		}
	}

	#endregion

	#region Instance Accessors

	public List<DogDescriptor> AdoptedDogs 
	{
		get 
		{
			return currentGame.AdoptedDogs;
		}
	}

	public int DogCount 
	{
		get 
		{
			return AdoptedDogs.Count;
		}
	}
		
	public Currency Coins 
	{
		get 
		{
			return currentGame.Coins;
		}
	}

	public Currency DogFood 
	{
		get 
		{
			return currentGame.Food;
		}
	}

    public Currency VacantHomeSlots
    {
        get
        {
            return currentGame.VacantHomeSlots;
        }
    }

	#endregion

	PPGameSave currentGame;
	MonoActionInt onCoinsChange;
	MonoActionInt onFoodChange;
    MonoActionInt onVacantHomeSlotsChange;

	public bool SaveGame()
	{
		Buffer(getCurrentGame());
		return Save();
	}

	public void SubscribeToCoinsChange(MonoActionInt coinsAction) 
	{
		onCoinsChange += coinsAction;
	}
		
	public void UnsubscribeFromCoinsChange(MonoActionInt coinsAction)
	{
		onCoinsChange -= coinsAction;
	}

	public void SubscribeToFoodChange(MonoActionInt foodAction) 
	{
		onFoodChange += foodAction;
	}

	public void UnsubscribeToFoodChange(MonoActionInt foodAction)
	{
		onFoodChange -= foodAction;
	}

    public void SubscribeToVacantHomeSlotsChange(MonoActionInt VacantHomeSlotsAction)
    {
        onVacantHomeSlotsChange += VacantHomeSlotsAction;
    }

    public void UnsubscribeToVacantHomeSlotsChange(MonoActionInt VacantHomeSlotsAction)
    {
        onVacantHomeSlotsChange -= VacantHomeSlotsAction;
    }
		
	public PPGameSave LoadGame()
	{
		currentGame = Load() as PPGameSave;
		return currentGame;
	}
		
	#region DataController Overrides

	protected override SerializableData getDefaultFile() 
	{
		return new PPGameSave(new DogDescriptor[0], Currency.Defaults);
	}		
		
	public override void Reset() 
	{
		base.Reset();
		LoadGame();
		callOnCoinsChange(currentGame.Coins.Amount);
		callOnFoodChange(currentGame.Food.Amount);
        callOnVacantHomeSlotsChange(currentGame.VacantHomeSlots.Amount);
	}

	#endregion

	protected void callOnCoinsChange(int coins) 
	{ 
		if(onCoinsChange != null)
		{
			onCoinsChange(coins);
		}
	}

	protected void callOnFoodChange(int food)
	{
		if(onFoodChange != null) 
		{
			onFoodChange(food);
		}
	}

    protected void callOnVacantHomeSlotsChange(int VacantHomeSlots)
    {
        if(onVacantHomeSlotsChange != null)
        {
            onVacantHomeSlotsChange(VacantHomeSlots);
        }
    }

    protected PPGameSave getCurrentGame() 
	{
		return currentGame;
	}
		
	public bool HasCurrency(CurrencyType type) 
	{
		return currentGame.HasCurrency(type);
	}

	public void ChangeCoins(int deltaCoins) 
	{
		this.currentGame.ChangeCoins(deltaCoins);
		callOnCoinsChange(Coins.Amount);
		SaveGame();
	}

	public void ChangeFood(int deltaFood) 
	{
		this.currentGame.ChangeFood(deltaFood);
		callOnFoodChange(DogFood.Amount);
		SaveGame();
	}

    public void ChangeVacantHomeSlots(int deltaVacantHomeSlots)
    {
        this.currentGame.ChangeVacantHomeSlots(deltaVacantHomeSlots);
        callOnVacantHomeSlotsChange(VacantHomeSlots.Amount);
        SaveGame();
    }

	public void Adopt(DogDescriptor dog) 
	{
		this.currentGame.Adopt(dog);
		SaveGame();
	}

}
