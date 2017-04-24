/*
 * Author(s): Isaiah Mann
 * Description: [to be added]
 * Usage: [no notes]
 */

using k = PPGlobal;

[System.Serializable]
public class DogVoucherData : SpecialGiftData
{
	#region Instance Accessors

	#region CurrencyData Overrides

	public override UnityEngine.Sprite Icon {
		get 
		{
			if(hasDogToRedeem)
			{
				return dogToRedeem.Portrait;
			}
			else
			{
				return base.Icon;
			}
		}
	}

	#endregion

	#endregion
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

	LanguageDatabase languages
	{
		get
		{
			return LanguageDatabase.GetInstance;
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

	#region Object Overrides

	public override string ToString ()
	{
		if(hasDogToRedeem)
		{
			return string.Format(
				languages.GetTerm(k.DOG_VOUCHER_MESSAGE),
				this.finderDog.Name,	
				this.dogToRedeem.Name
			);
		}
		else
		{
			return string.Empty;
		}
	}

	#endregion

}
