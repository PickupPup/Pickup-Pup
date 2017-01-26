﻿/*
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
		
	PPDataController dataController;

	#region MonoBehaviourExtended

	protected override void fetchReferences()
	{
		base.fetchReferences();
		dataController = PPDataController.GetInstance;
	}

	#endregion

	public void LoadMainMenu() 
	{
		LoadScene(PPScene.MainMenu);
	}

    public void LoadShelter()
    {
        LoadScene(PPScene.Shelter);
    }

    public void LoadShop()
    {
        LoadScene(PPScene.Shop);
    }

    public void LoadLivingRoom()
    {
        LoadScene(PPScene.LivingRoom);
    }

    public void LoadYard()
    {
        LoadScene(PPScene.Yard);
    }

    public void LoadScene(PPScene scene) 
	{
		dataController.SaveGame();
		SceneManager.LoadScene((int) scene);
	}

}

public enum PPScene 
{
	MainMenu,
    Shelter,
    Shop,
    LivingRoom,
    Yard,

}