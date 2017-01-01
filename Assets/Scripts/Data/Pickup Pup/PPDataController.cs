/*
 * Author: Isaiah Mann
 * Desc: Handles save for Pickup Pup
 */

using System.Collections.Generic;

public class PPDataController : DataController {
	// Casts the singleton from the superclass:
	public static PPDataController GetInstance {
		get {
			return Instance as PPDataController;
		}
	}

	public List<DogDescriptor> AdoptedDogs {
		get {
			return currenGame.AdoptedDogs;
		}
	}

	public int DogCount {
		get {
			return AdoptedDogs.Count;
		}
	}
		
	public Currency Coins {
		get {
			return currenGame.Coins;
		}
	}

	public Currency DogFood {
		get {
			return currenGame.Food;
		}
	}

	PPGameSave currenGame;
	MonoActionInt onCoinsChange;
	MonoActionInt onFoodChange;

	public bool SaveGame () {
		Buffer(getCurrentGame());
		return Save();
	}

	public void SubscribeToCoinsChange (MonoActionInt coinsAction) {
		onCoinsChange += coinsAction;
	}
		
	public void UnsubscribeFromCoinsChange (MonoActionInt coinsAction) {
		onCoinsChange -= coinsAction;
	}

	public void SubscribeToFoodChange (MonoActionInt foodAction) {
		onFoodChange += foodAction;
	}

	public void UnsubscribeToFoodChange (MonoActionInt foodAction) {
		onFoodChange -= foodAction;
	}
		
	public PPGameSave LoadGame () {
		currenGame = Load() as PPGameSave;
		return currenGame;
	}

	public override void Reset () {
		base.Reset ();
		LoadGame();
		callOnCoinsChange(currenGame.Coins.Amount);
		callOnFoodChange(currenGame.Food.Amount);
	}

	protected void callOnCoinsChange (int coins) { 
		if (onCoinsChange != null) {
			onCoinsChange(coins);
		}
	}

	protected void callOnFoodChange (int food) {
		if (onFoodChange != null) {
			onFoodChange(food);
		}
	}

	protected PPGameSave getCurrentGame () {
		return currenGame;
	}
		
	public bool HasCurrency (CurrencyType type) {
		return currenGame.HasCurrency(type);
	}

	public void ChangeCoins (int deltaCoins) {
		this.currenGame.ChangeCoins(deltaCoins);
		callOnCoinsChange(Coins.Amount);
		SaveGame();
	}

	public void ChangeFood (int deltaFood) {
		this.currenGame.ChangeFood(deltaFood);
		callOnFoodChange(DogFood.Amount);
		SaveGame();
	}

	public void Adopt (DogDescriptor dog) {
		this.currenGame.Adopt(dog);
		SaveGame();
	}

	protected override SerializableData getDefaultFile () {
		return new PPGameSave(new DogDescriptor[0], Currency.Defaults);
	}		
}
