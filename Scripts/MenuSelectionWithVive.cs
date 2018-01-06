using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuSelectionWithVive : MonoBehaviour {

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "MenuButton") { // if i just pointed the option, fire the button event once
            Button button = collider.gameObject.GetComponent<Button>();
            button.onClick.Invoke();
        }
    }
}
