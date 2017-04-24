/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

[System.Serializable]
public class DogVoucherData : SpecialGiftData
{
	bool hasDogToRedeem
	{
		get
		{
			return dogToRedeem != null;
		}
	}

	DogDatabase dogDatabase
	{
		get
		{
			return DogDatabase.GetInstance;
		}
	}

	DogDescriptor dogToRedeem;

	public DogVoucherData() : 
	base(CurrencyType.DogVoucher)
	{
		SetRandomDog();
	}

	public DogVoucherData(DogDescriptor dog) : 
	base(CurrencyType.DogVoucher)
	{
		SetDog(dog);
	}

	public void SetRandomDog()
	{
		SetDog(dogDatabase.RandomDog(mustBeUnadopted:true));
	}

	public void SetDog(DogDescriptor dog)
	{
		this.dogToRedeem = dog;
	}

	#region CurrencyData Overrides

	public override void Give()
	{
		if(hasDogToRedeem)
		{
			dataController.Adopt(dogToRedeem);
			dogToRedeem = null;
		}
	}

	#endregion

}
