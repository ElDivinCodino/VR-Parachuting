using UnityEngine;
using System.Collections;

public class Phrasing : MonoBehaviour {

	[HideInInspector] public string[] selectedLanguage;
	[HideInInspector] public AudioClip[] selectedAudio;

	public AudioClip[] englishAudio;
	public AudioClip[] italianAudio;

	public void SetLanguage(string lang) {
		string[] temp = new string[12];

		switch(lang) {
		    case "English":
			    temp [0] = "Your current altitude is: ";
			    temp [1] = " meters.";
			    temp [2] = "Please fasten your seat belt and get ready for taking off.";
			    temp [3] = "Recommended altitude to perform the jump is ";
			    temp [4] = "Remaining ";
			    temp [5] = "Altitude reached: \n Please, get ready for ejection!";
			    temp [6] = "Eject when ready";
			    temp [7] = "Stabilize!";
			    temp [8] = "Open parachute!";
                temp [9] = "Come back above landing area!";
                temp [10] = "You are dead. \n Please open parachute before the security altitude limit! \n Press R to restart or Q to exit.";
                temp [11] = "Congratulations, you performed succesfully the landing procedure!";

                selectedAudio = englishAudio;
			    break;

		    case "Italian":
			    temp [0] = "L'altitudine attuale è: ";
			    temp [1] = " metri.";
			    temp [2] = "Per favore assicurati di aver allacciato la cintura di sicurezza e preparati per il decollo";
			    temp [3] = "L'altitudine consigliata per effettuare il salto è ";
			    temp [4] = "Rimangono ";
			    temp [5] = "Altitudine raggiunta: \n Per favore, preparati per il salto!";
			    temp [6] = "Salta quando vuoi";
			    temp [7] = "Stabilizzati!";
			    temp [8] = "Apri il paracadute!";
                temp [9] = "Torna sopra all'area di atterraggio!";
                temp [10] = "Sei morto. \n Apri il paracadute prima di superare la soglia di sicurezza! \n Premi R per riavviare o Q per uscire.";
                temp [11] = "Congratulazioni, hai completato con successo la procedura di atterraggio!";

                selectedAudio = italianAudio;
			    break;
		}
		selectedLanguage = temp;
	}
}
