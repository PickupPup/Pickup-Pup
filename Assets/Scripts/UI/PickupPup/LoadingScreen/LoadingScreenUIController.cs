/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann 
 * Description: Used to cover up loaded with a screen
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LoadingScreenUIController : PPUIController
{
	const PPScene STARTING_SCENE = PPScene.Home;

	[SerializeField]
	bool loadNextSceneOnInit;

	PPScene nextScene = STARTING_SCENE;

	public void SetNextScene(PPScene scene)
	{
		this.nextScene = scene;
	}

	public void LoadNextScene()
	{
		sceneController.LoadSceneAsync(nextScene);
	}


	#region MonoBehaviourExtended Overrides 

    protected override void fetchReferences()
    {
        base.fetchReferences();
		if(loadNextSceneOnInit)
		{
			LoadNextScene();
		}
    }

	#endregion

}
