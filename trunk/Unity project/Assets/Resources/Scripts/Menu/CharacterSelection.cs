using UnityEngine;
using System.Collections;

[System.Serializable]
public class BlocElement {
	public string name;
	public GUIContent[] sourceElement;
}

public class CharacterSelection : MonoBehaviour {

	private float _sw = Screen.width;
	private float _sh = Screen.height;
	public GUIContent [] iconBloc;
	public GUIContent [] iconSource;
	private int _monsterX1 = 0, _monsterY1 = 0, _monsterX2 = 0, _monsterY2 = 0, _monsterX3 = 0, _monsterY3 = 0;
	private int _totemX1 = 0, _totemY1 = 0, _totemX2 = 0, _totemY2 = 0, _totemX3 = 0, _totemY3 = 0;
	public BlocElement[] monsterPicture;
	public BlocElement[] totemPicture;


	void Awake () {
		
		enabled = false;

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		//lance le jeu.
		if (GUI.Button (new Rect(_sw / 2 - 40, _sh - 110, 80, 60),"GO!")){
			Application.LoadLevel("MainScene");
		}
		//retourne a l'écran titre.
		if (GUI.Button (new Rect(_sw / 2 - 30, _sh - 30, 60, 20),"Back")){
			gameObject.GetComponent<MainMenu>().enabled = true;
			enabled = false;
		}

		/******************************
		 * Debut du choix des monstres*
		 * ****************************/

		//Choix du monstre 1
		GUI.BeginGroup (new Rect (10, 10, 300, _sh / 3 - 10));

		GUI.Box (new Rect(0,0,300,_sh / 3),"");

		// choix de l'element pour les blocs
		GUI.Box (new Rect (50, 20, 40, 40), iconBloc[_monsterX1]);

		if(GUI.Button (new Rect(10,20,30,40),"<")){
			_monsterX1 -= 1;
			if (_monsterX1 <= -1){
				_monsterX1 = 4;
			}
		}
		if(GUI.Button (new Rect(100,20,30,40),">")){
			_monsterX1 += 1;
			if (_monsterX1 >= 5){
				_monsterX1 = 0;
			}
		}

		//choix de l'element pour les sources
		GUI.Box (new Rect (50, 100, 40, 40), iconSource[_monsterY1]);

		if(GUI.Button (new Rect(10,100,30,40),"<")){
			_monsterY1 -= 1;
			if (_monsterY1 <= -1){
				_monsterY1 = 4;
			}
		}
		if(GUI.Button (new Rect(100,100,30,40),">")){
			_monsterY1 += 1;
			if (_monsterY1 >= 5){
				_monsterY1 = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (160, 10, 130, 130), monsterPicture [_monsterX1].sourceElement [_monsterY1]);

		GUI.EndGroup();



		//Choix du monstre 2
		GUI.BeginGroup (new Rect (10, _sh / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (50, 20, 40, 40), iconBloc[_monsterX2]);
		
		if(GUI.Button (new Rect(10,20,30,40),"<")){
			_monsterX2 -= 1;
			if (_monsterX2 <= -1){
				_monsterX2 = 4;
			}
		}
		if(GUI.Button (new Rect(100,20,30,40),">")){
			_monsterX2 += 1;
			if (_monsterX2 >= 5){
				_monsterX2 = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (50, 100, 40, 40), iconSource[_monsterY2]);
		
		if(GUI.Button (new Rect(10,100,30,40),"<")){
			_monsterY2 -= 1;
			if (_monsterY2 <= -1){
				_monsterY2 = 4;
			}
		}
		if(GUI.Button (new Rect(100,100,30,40),">")){
			_monsterY2 += 1;
			if (_monsterY2 >= 5){
				_monsterY2 = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (160, 10, 130, 130), monsterPicture [_monsterX2].sourceElement [_monsterY2]);
		
		GUI.EndGroup();



		//Choix du monstre 3
		GUI.BeginGroup (new Rect (10, _sh * 2 / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (50, 20, 40, 40), iconBloc[_monsterX3]);
		
		if(GUI.Button (new Rect(10,20,30,40),"<")){
			_monsterX3 -= 1;
			if (_monsterX3 <= -1){
				_monsterX3 = 4;
			}
		}
		if(GUI.Button (new Rect(100,20,30,40),">")){
			_monsterX3 += 1;
			if (_monsterX3 >= 5){
				_monsterX3 = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (50, 100, 40, 40), iconSource[_monsterY3]);
		
		if(GUI.Button (new Rect(10,100,30,40),"<")){
			_monsterY3 -= 1;
			if (_monsterY3 <= -1){
				_monsterY3 = 4;
			}
		}
		if(GUI.Button (new Rect(100,100,30,40),">")){
			_monsterY3 += 1;
			if (_monsterY3 >= 5){
				_monsterY3 = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (160, 10, 130, 130), monsterPicture [_monsterX3].sourceElement [_monsterY3]);
		
		GUI.EndGroup();

		/******************************************************
		 * Fin du choix des monstres, debut du choix des totems*
		 * ****************************************************/

		//Choix du Totem 1
		GUI.BeginGroup (new Rect (_sw - 310, 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (210, 20, 40, 40), iconBloc[_totemX1]);
		
		if(GUI.Button (new Rect(170,20,30,40),"<")){
			_totemX1 -= 1;
			if (_totemX1 <= -1){
				_totemX1 = 4;
			}
		}
		if(GUI.Button (new Rect(260,20,30,40),">")){
			_totemX1 += 1;
			if (_totemX1 >= 5){
				_totemX1 = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (210, 100, 40, 40), iconSource[_totemY1]);
		
		if(GUI.Button (new Rect(170,100,30,40),"<")){
			_totemY1 -= 1;
			if (_totemY1 <= -1){
				_totemY1 = 4;
			}
		}
		if(GUI.Button (new Rect(260,100,30,40),">")){
			_totemY1 += 1;
			if (_totemY1 >= 5){
				_totemY1 = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (10, 10, 130, 130), totemPicture [_totemX1].sourceElement [_totemY1]);
		
		GUI.EndGroup();



		//Choix du Totem 2
		GUI.BeginGroup (new Rect (_sw - 310, _sh / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (210, 20, 40, 40), iconBloc[_totemX2]);
		
		if(GUI.Button (new Rect(170,20,30,40),"<")){
			_totemX2 -= 1;
			if (_totemX2 <= -1){
				_totemX2 = 4;
			}
		}
		if(GUI.Button (new Rect(260,20,30,40),">")){
			_totemX2 += 1;
			if (_totemX2 >= 5){
				_totemX2 = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (210, 100, 40, 40), iconSource[_totemY2]);
		
		if(GUI.Button (new Rect(170,100,30,40),"<")){
			_totemY2 -= 1;
			if (_totemY2 <= -1){
				_totemY2 = 4;
			}
		}
		if(GUI.Button (new Rect(260,100,30,40),">")){
			_totemY2 += 1;
			if (_totemY2 >= 5){
				_totemY2 = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (10, 10, 130, 130), totemPicture [_totemX2].sourceElement [_totemY2]);
		
		GUI.EndGroup();



		//Choix du Totem 3
		GUI.BeginGroup (new Rect (_sw - 310, _sh * 2 / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (210, 20, 40, 40), iconBloc[_totemX3]);
		
		if(GUI.Button (new Rect(170,20,30,40),"<")){
			_totemX3 -= 1;
			if (_totemX3 <= -1){
				_totemX3 = 4;
			}
		}
		if(GUI.Button (new Rect(260,20,30,40),">")){
			_totemX3 += 1;
			if (_totemX3 >= 5){
				_totemX3 = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (210, 100, 40, 40), iconSource[_totemY3]);
		
		if(GUI.Button (new Rect(170,100,30,40),"<")){
			_totemY3 -= 1;
			if (_totemY3 <= -1){
				_totemY3 = 4;
			}
		}
		if(GUI.Button (new Rect(260,100,30,40),">")){
			_totemY3 += 1;
			if (_totemY3 >= 5){
				_totemY3 = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (10, 10, 130, 130), totemPicture [_totemX3].sourceElement [_totemY3]);
		
		GUI.EndGroup();
	}
}
