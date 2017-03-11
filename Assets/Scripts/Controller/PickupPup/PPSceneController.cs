/*
 * Author: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles scene loading and management
 */

using UnityEngine;
using UnityEngine.SceneManagement;

using k = PPGlobal;

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
    bool readyToSwitchScenes
    {
        get
        {
            return sceneLoadingBlockers == NONE_VALUE;        
        }
    }
        
    // Whether blocks should be cleared ever time the controller loads a new scene
    [SerializeField]
    bool shouldZeroOutSceneLoadingBlockersOnLoadScene = true;

    // Set this to invalid until the script is fully loaded
    int sceneLoadingBlockers = INVALID_VALUE;

	#region MonoBehaviourExtended

	protected override void fetchReferences()
	{
		base.fetchReferences();
        // Now that refs are fetched, scenes can be loaded (need to be able to save before loading)
        zeroOutSceneLoadingBlockers();
	}

	#endregion

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

    public void LoadScene(PPScene scene, bool refreshSystems = false) 
	{
		dataController.SaveGame();
		SceneManager.LoadScene((int) scene);
        if(shouldPlayChangeSceneSFX(scene))
        {
            EventController.Event(k.GetPlayEvent(k.CHANGE_SCENE));
        }
        if(shouldZeroOutSceneLoadingBlockersOnLoadScene)
        {
            zeroOutSceneLoadingBlockers();
        }
        if(refreshSystems)
        {
            gameController.HandleSystemReset(caller:this);
        }
	}

    public bool RequestLoadScene(PPScene scene, bool refreshSystems)
    {
        if(canLoadScene(scene)) 
        {
            LoadScene(scene, refreshSystems);
            return true;
        }
        else 
        {
            return false;
        }
    }
        
    public bool RequestReloadCurrentScene(bool refreshSystems = false)
    {
        return RequestLoadScene(CurrentScene, refreshSystems);
    }

    public void BlockFromLoadingScenes()
    {
        sceneLoadingBlockers++;
    }

    public void UnblockFromLoadingScenes()
    {
        sceneLoadingBlockers--;
    }
        
    public bool IsWorldScene(PPScene scene)
    {
        return scene == PPScene.LivingRoom || scene == PPScene.Yard;
    }

	public void LoadSceneAsync(PPScene scene)
	{
		SceneManager.LoadSceneAsync((int) scene);
	}

    // Currently does not care about which scene, but may need more advanced logic in future
    bool canLoadScene(PPScene scene)
    {
        return readyToSwitchScenes;
    }

    void zeroOutSceneLoadingBlockers()
    {
        sceneLoadingBlockers = NONE_VALUE;
    }

    bool shouldPlayChangeSceneSFX(PPScene newScene)
    {
        return IsWorldScene(newScene);
    }

}

public enum PPScene 
{
    Loading,
    Shelter,
    Shop,
    LivingRoom,
    Yard,

}
