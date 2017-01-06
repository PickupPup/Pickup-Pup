/*
 * Author(s): Isaiah Mann 
 * Description: Helper class to assist with strings from text files
 */

using UnityEngine;

public static class TextAssetUtil 
{
	static char CorrectQuoatationMark 
	{
		get
		{
			return '"';
		}
	}
		
	public static string FileText(string path) 
	{
		return convertQuotationMarks(Resources.Load<TextAsset>(path).text);
	}

	// JSON only accepts the standard quotation marks
	static string convertQuotationMarks(string original) 
	{
		char[] characters = original.ToCharArray();
		for(int i = 0; i < characters.Length; i++) 
		{
			if(isIncorrectQuotationMark(characters[i])) 
			{
				characters[i] = CorrectQuoatationMark;
			}
		}
		return new string(characters);
	}

	static bool isIncorrectQuotationMark(char character) 
	{
		return (int) character == 8220 || (int) character == 8221;
	}

}
