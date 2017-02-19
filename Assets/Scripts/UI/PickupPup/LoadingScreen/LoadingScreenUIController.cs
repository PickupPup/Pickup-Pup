using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenUIController : PPUIController
{
    PPScene nextScene;
    float loadingTime;

    protected override void setReferences()
    {
        base.setReferences();
        nextScene = PPScene.Shelter;
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        loadingTime = gameController.Tuning.LoadingTime;
        StartCoroutine(loadSceneAfterDelay(nextScene));
    }

    IEnumerator loadSceneAfterDelay(PPScene scene)
    {
        yield return new WaitForSeconds(loadingTime);
        sceneController.LoadScene(scene);
    }
}
