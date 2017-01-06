/*
 * Author: Isaiah Mann
 * Description: Abstract class for storing data in Pickup Pup
 */

using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public abstract class PPData 
{
	protected const string BLACK_HEX = "#000000";

	const string HEX_HASH_PREFIX = "#";
	const string HEX_NUM_PREFIX = "0x";
	const int CORRECT_HEX_NUM_LENGTH = 6;

	public delegate void DataAction();
	public delegate void DataActionf(float value);

	public delegate void DogAction(Dog dog);
	public delegate void DogActionf(Dog dog, float dogFloat);

	protected DogDatabase data;

	public PPData() 
	{
		this.data = null;
	}

	public PPData(DogDatabase data) 
	{
		Initialize(data);
	}

	public virtual void Initialize(DogDatabase data) 
	{
		this.data = data;
	}

	// Adapated from http://www.bugstacker.com/15/how-to-parse-a-hex-color-string-in-unity-c%23
	protected Color parseHexColor(string hexstring) 
	{
		if(hexstring.StartsWith(HEX_HASH_PREFIX)) 
		{
			hexstring = hexstring.Substring(HEX_HASH_PREFIX.Length);
		}
		else if(hexstring.StartsWith(HEX_NUM_PREFIX)) 
		{
			hexstring = hexstring.Substring(HEX_NUM_PREFIX.Length);
		}

		if(hexstring.Length == CORRECT_HEX_NUM_LENGTH)
		{
			byte r = byte.Parse(hexstring.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hexstring.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hexstring.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, 1);
		}
		else 
		{
			throw new Exception(string.Format("{0} is not a valid color string.", hexstring));
		}
	}
		
	protected string padWithZeroes(int number, int desiredLength) 
	{
		string numberAsString = number.ToString();
		int numberLength = numberAsString.Length;
		if(numberLength < desiredLength) 
		{
			return numberAsString.PadLeft(desiredLength, '0');
		} 
		else 
		{
			return numberAsString;
		}
	}

	protected string formatCost(int cost) 
	{
		// String formatter to concat integer with dollar sign:
		return string.Format("${0}", cost);	
	}

}
