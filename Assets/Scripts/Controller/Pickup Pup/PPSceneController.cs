/*
 * Author: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles scene loading and management
 */

using UnityEngine.SceneManagement;

public class PPSceneController : SingletonController<PPSceneController> 
{
	#region Instance Accessors

	public PPScene CurrentScene 
	{
		get 
		{
			return (PPScene) SceneManager.GetActiveScene().buildIndex;
		}
	}

	#endregion
		
	public void LoadStart() 
	{
		LoadScene(PPScene.Start);
	}

	public void LoadHome() 
	{
		LoadScene(PPScene.Home);
	}

    public void LoadShelter()
    {
        LoadScene(PPScene.Shelter);
    }

    public void LoadShop()
    {
        LoadScene(PPScene.Shop);
    }

    public void LoadScene(PPScene scene) 
	{
		SceneManager.LoadScene((int) scene);
	}

}

public enum PPScene 
{
	Start,
	Home,
    Shelter,
    Shop,
}
