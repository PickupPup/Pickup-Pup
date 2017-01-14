/*
 * Author(s): Isaiah Mann
 * Description: Assists in loading and saving persistent files
 */

using UnityEngine;
using System.Collections;
using System.IO;

public static class FileUtil 
{
	public static string CreatePath(params string[] directories)
	{
		if(directories.Length >= 2) 
		{
			string path = Path.Combine(directories[0], directories[1]);
			for(int i = 2; i < directories.Length; i++) 
			{
				path = Path.Combine(path, directories[i]);
			}
			return path;
		} 
		else if(directories.Length == 1) 
		{
			return Path.Combine(directories[0], string.Empty);
		} 
		else 
		{
			return Path.Combine(string.Empty, string.Empty);
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
				characters[i] = correctQuoatationMark();
			}
		}

		return new string(characters);

	}

	static bool isIncorrectQuotationMark(char character) 
	{
		return (int) character == 8220 || (int) character == 8221;
	}

	static char correctQuoatationMark() 
	{
		return '"';
	}

	public static void WriteStringToPath(string text, string path) 
	{
		File.WriteAllText(path, text);
	}

	public static void AppendStringToPath(string text, string path) 
	{
		File.AppendAllText(path, text);
	}

	public static bool FileExistsAtPath(string path) 
	{
		return File.Exists(path);
	}

}
