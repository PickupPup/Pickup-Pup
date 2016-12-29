/*
 * Author: Isaiah
 * Description: Handles scene loading and management
 */

using UnityEngine.SceneManagement;

public class PPSceneController : SingletonController<PPSceneController> {
	public PPScene CurrentScene {
		get {
			return (PPScene) SceneManager.GetActiveScene().buildIndex;
		}
	}
		
	public void LoadStart () {
		LoadScene(PPScene.Start);
	}

	public void LoadHome () {
		LoadScene(PPScene.Home);
	}
		
	public void LoadScene (PPScene scene) {
		SceneManager.LoadScene((int) scene);
	}
}

public enum PPScene {
	Start,
	Home,
}
