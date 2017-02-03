/*
 * Author: Isaiah Mann
 * Description: Describes the traits of a particular breed
 */

using System;
using UnityEngine;

[Serializable]
public class DogBreed : DogTrait 
{
	#region Static Accessors

	public static DogBreed Default 
	{
		get 
		{
			DogBreed breed = new DogBreed(DogDatabase.GetInstance);
			breed.breed = string.Empty;
			return breed;
		}
	}

	#endregion

	#region Instance Accessors

	public string Breed 
	{
		get 
		{
			return this.breed;
		}
	}
		
	public string CostToAdoptStr 
	{ 
		get 
		{
			return PPData.FormatCost(costToAdopt);
		}
	}

	public int CostToAdopt 
	{
		get 
		{
			return costToAdopt;
		}
	}

    public string Size
    {
        get
        {
            return size;
        }
    }

	public CurrencyType ISpecialization 
	{
		get 
		{
			return (CurrencyType) Enum.Parse(typeof(CurrencyType), specialization); 
		}
	}

	public float TimeToReturn
	{
		get
		{
			return timeToReturn;
		}
	}

	#endregion

	[SerializeField]
	string breed;
	[SerializeField]
	string specialization;
	[SerializeField]
	float timeToReturn;
	[SerializeField]
	int costToAdopt;
    [SerializeField]
    string size;

	public DogBreed(DogDatabase data) : base(data)
	{
		// NOTHING
	}

	#region System.Object Overrides

	public override string ToString() 
	{
		return string.Format("[DogBreed:{0} Specialization:{1}]", breed, ISpecialization);
	}

	public override bool Equals(object obj) 
	{
		if(obj is DogBreed) 
		{
			return (obj as DogBreed).breed.Equals(breed);
		} 
		else
		{
			return base.Equals(obj);
		}
	}

	public override int GetHashCode() 
	{
		return breed.GetHashCode();
	}

	#endregion

}
