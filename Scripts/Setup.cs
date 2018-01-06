using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Setup : MonoBehaviour {

	[HideInInspector] public string language;
	[HideInInspector] public string device;
	public Text languageText;
	public Text deviceText;
	public StartGame startGame;

	private int langCounter = 0;
	private int devCounter = 0;

	void Awake() {
		language = "English";
		device = "HTCVive";
		DontDestroyOnLoad (gameObject);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            Application.Quit();
        } else if (Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }

    public void SetupLanguage(int i) {

		langCounter += i;

		int effective = Mathf.Abs(langCounter % 2);

		switch(effective) {
		case 0:
			language = "English";
			languageText.text = "English";
			break;
		case 1:
			language = "Italian";
			languageText.text = "Italiano";
			break;
		default:
			language = "English";
			languageText.text = "English";
			break;
		}
	}

	public void SetupDevice(int d) {
		devCounter += d;

		int effective = Mathf.Abs(devCounter % 2);

		switch(effective) {
		case 0:
			device = "HTCVive";
			deviceText.text = "HTC Vive";
			startGame.sceneToLoad = 1;
			break;
		case 1:
			device = "Oculus";
			deviceText.text = "Oculus Rift";
			startGame.sceneToLoad = 2;
			break;
		default:
			device = "HTCVive";
			deviceText.text = "HTC Vive";
			startGame.sceneToLoad = 1;
			break;
		}
	}
}
