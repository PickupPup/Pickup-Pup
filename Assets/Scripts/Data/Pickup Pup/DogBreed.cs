/*
 * Author: Isaiah Mann
 * Description: Describes the traits of a particular breed
 */

using System;

[Serializable]
public class DogBreed : DogTrait 
{

	#region Instance Accessors

	public string CostToAdoptStr { 
		get 
		{
			return formatCost(CostToAdopt);
		}
	}
	public CurrencyType ISpecialization 
	{
		get 
		{
			return (CurrencyType) Enum.Parse(typeof(CurrencyType), Specialization); 
		}
	}

	#endregion

	public string Breed;
	public string Specialization;
	public float TimeToReturn;
	public int CostToAdopt;

	public DogBreed(DogDatabase data) : base(data)
	{
		// NOTHING
	}

	#region System.Object Overrides

	public override string ToString() 
	{
		return string.Format ("[DogBreed:{0} Specialization:{1}]", Breed, ISpecialization);
	}

	public override bool Equals(object obj) 
	{
		if(obj is DogBreed) 
		{
			return (obj as DogBreed).Breed.Equals(Breed);
		} 
		else
		{
			return base.Equals(obj);
		}
	}

	public override int GetHashCode() 
	{
		return Breed.GetHashCode();
	}

	#endregion

}
