using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUIContent mainLogo;
	public GUIStyle logoBox;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		GUI.skin = gameObject.GetComponent<CharacterSelection> ().mainMenuSkin;

		GUI.Box (new Rect (Screen.width / 2 - 256, 10, 512 ,256), mainLogo, logoBox);

		//lance le menu de selection des personnages.
		if (GUI.Button (new Rect(Screen.width / 2 - 90,Screen.height / 2 + 30,180,60),"Play")){
			gameObject.GetComponent<CharacterSelection>().enabled = true;
			enabled = false;
		}
		//bouton pour quitter.
		if (GUI.Button (new Rect (Screen.width / 2 - 60, Screen.height / 2 + 120, 120, 40), "quit")) {
			Application.Quit();
			Debug.Log ("la fonction Quit ne fonctionne pas dans l'editeur");
		}
	}
}
