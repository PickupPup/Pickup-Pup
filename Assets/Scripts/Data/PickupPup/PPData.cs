/*
 * Author: Isaiah Mann
 * Description: Abstract class for storing data in Pickup Pup
 */

using System;
using System.Globalization;
using UnityEngine;
using k = PPGlobal;

[Serializable]
public abstract class PPData 
{
	protected const string BLACK_HEX = k.BLACK_HEX;
	protected const string DOG_GIFT_REPORT_FORMAT = k.DOG_GIFT_REPORT_FORMAT;
	protected const string GENERIC_GIFT_REPORT_FORMAT = k.GENERIC_GIFT_REPORT_FORMAT;
	protected const int NONE_INT = k.NONE_VALUE;
	protected const int NOT_FOUND_INT = k.INVALID_VALUE;
	protected const int DEFAULT_COINS = k.DEFAULT_COINS;
	protected const int DEFAULT_DOG_FOOD = k.DEFAULT_DOG_FOOD;
	protected const int DEFAULT_HOME_SLOTS = k.DEFAULT_HOME_SLOTS;
	protected const int DEFAULT_CURRENCY_AMOUNT = k.DEFAULT_CURRENCY_AMOUNT;
	protected const float DEFAULT_TIME_TO_COLLECT = k.DEFAULT_TIME_TO_COLLECT;
	protected const float DEFAULT_DISCOUNT = k.DEFAULT_DISCOUNT_DECIMAL;

	const string HEX_HASH_PREFIX = k.HEX_HASH_PREFIX;
	const string HEX_NUM_PREFIX = k.HEX_NUM_PREFIX;
	const int CORRECT_HEX_NUM_LENGTH = k.CORRECT_HEX_NUM_LENGTH;

	public delegate void DataAction();
	public delegate void DataActionf(float value);

	public delegate void DogAction(Dog dog);
	public delegate void DogActionf(Dog dog, float dogFloat);
	public delegate void DogActionStr(string eventName, Dog dog);
	public delegate void PPDogAction(PPEvent ppEvent, Dog Dog);

    public delegate void NamedCurrencyAction(string actionName, CurrencyData currency);
    public delegate void CurrencyAction(CurrencyData currency);

	protected DogDatabase database
    {
        get
        {
            if(_database == null)
            {
                _database = DogDatabase.GetInstance;
            }
            return _database;
        }
        set
        {
            _database = value;
        }
    }

    // Need to prevent serializing this to avoid a serialization loop:
    [NonSerialized]
    DogDatabase _database;

	public PPData() 
	{
		this.database = null;
	}

	public PPData(DogDatabase data) 
	{
		Initialize(data);
	}

	public virtual void Initialize(DogDatabase data) 
	{
		this.database = data;
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

	public static string FormatCost(int cost) 
	{
		// String formatter to concat integer with dollar sign:
		return string.Format("${0}", cost);	
	}

	protected string formatTime(float time)
	{
		int hours = (int) time / 3600;
		int minutes = ((int) time / 60) % 60;
		int seconds = (int) time % 60;
		return string.Format("{0}:{1}:{2}", 
			padWithZeroes(hours, 2), 
			padWithZeroes(minutes, 2),
			padWithZeroes(seconds, 2)
		);

	}

    protected Color getColor(int[] rgba)
    {
        return getColor(convertColorValues(rgba));
    }

    protected Color getColor(float[] rgba)
    {
        return new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
    }

    protected float[] convertColorValues(int[] rgba)
    {
        float[] convertedValues = new float[rgba.Length];
        for(int i = 0; i < rgba.Length; i++)
        {
            convertedValues[i] = rgba[i] / 255f;
        }
        return convertedValues;
    }

}
