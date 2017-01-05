/*
 * Author(s): Grace Barrett-Snyder 
 * Description: Utility class to load and access sprites from Resources.
 */

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class SpriteUtil
{
    const string FILE_PATH = "Sprites/"; // Path within Resources Folder
    const string BASE_FILENAME = "sample-dog-"; // Filename format before the ID

    static Dictionary<int, Sprite> dogSprites = new Dictionary<int, Sprite>();

    /// <summary>
    /// Gets the Dog Sprite associated with the specified ID.
    /// </summary>
    public static Sprite GetDogSprite(int spriteID)
    {
        // If the sprite ID isn't listed in the dog sprite dictionary
        // (meaning it hasn't been loaded before)
        if (! dogSprites.ContainsKey(spriteID))
        {
            // Load the sprite from file
            // Filename doesn't include extension 
            Sprite dogSprite = Resources.Load<Sprite>(FILE_PATH + BASE_FILENAME + spriteID);
            dogSprites.Add(spriteID, dogSprite); // Add it to sprite dictionary
        }

        return dogSprites[spriteID];
    }
}
