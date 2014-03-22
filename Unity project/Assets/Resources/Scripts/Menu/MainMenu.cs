using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUIContent mainLogo;
	public Rect rectLogo = new Rect (Screen.width / 8,Screen.height / 8,Screen.width * 3 / 4 ,Screen.height * 3 / 8);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		GUI.Box (rectLogo,mainLogo);

		//lance le menu de selection des personnages.
		if (GUI.Button (new Rect(Screen.width / 2 - 120,Screen.height / 2 + 30,240,60),"Play")){
			gameObject.GetComponent<CharacterSelection>().enabled = true;
			enabled = false;
		}
		//bouton pour quitter.
		if (GUI.Button (new Rect (Screen.width / 2 - 80, Screen.height / 2 + 120, 160, 40), "quit")) {
			Application.Quit();
			Debug.Log ("la fonction Quit ne fonctionne pas dans l'editeur");
		}
	}
}
