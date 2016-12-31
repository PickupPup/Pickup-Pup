/*
 * Author: Isaiah Mann
 * Desc: Handles save for Pickup Pup
 */

using System.Collections.Generic;

public class PPDataController : DataController {
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

	public int Money {
		get {
			return currenGame.Money;
		}
	}

	public int Food {
		get {
			return currenGame.Food;
		}
	}

	PPGameSave currenGame;
	MonoActionInt onMoneyChange;
	MonoActionInt onFoodChange;

	public bool SaveGame () {
		Buffer(getCurrentGame());
		return Save();
	}

	public void SubscribeToMoneyChange (MonoActionInt moneyAction) {
		onMoneyChange += moneyAction;
	}
		
	public void UnsubscribeFromMoneyChange (MonoActionInt moneyAction) {
		onMoneyChange -= moneyAction;
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

	protected void callOnMoneyChange (int money) { 
		if (onMoneyChange != null) {
			onMoneyChange(money);
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
		
	public void ChangeMoney (int deltaMoney) {
		this.currenGame.Money += deltaMoney;
		callOnMoneyChange(Money);
		SaveGame();
	}

	public void ChangeFood (int deltaFood) {
		this.currenGame.Food += deltaFood;
		callOnFoodChange(Food);
		SaveGame();
	}

	public void Adopt (DogDescriptor dog) {
		this.currenGame.Adopt(dog);
		SaveGame();
	}

	protected override SerializableData getDefaultFile () {
		return new PPGameSave(new DogDescriptor[0], money:0, food:0);
	}		
}
