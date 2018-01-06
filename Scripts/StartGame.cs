using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour {

	public Slider loadingSlider;
	public GameObject loadingScreen;
	public int sceneToLoad;

	private AsyncOperation async;

	public void StartingGame() {
		loadingScreen.SetActive (true);
		StartCoroutine (LoadSceneWithSlider (sceneToLoad));
	}

	IEnumerator LoadSceneWithSlider(int i) {
		async = SceneManager.LoadSceneAsync (i);

		while (!async.isDone) {
			loadingSlider.value = async.progress;
			yield return null;
		}
	}
}
