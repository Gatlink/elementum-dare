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
	private int[] _monsterX = new int[] {0, 0, 0};
	private int[] _monsterY = new int[] {0, 0, 0};
	private int[] _totemX = new int[] {0, 0, 0};
	private int[] _totemY = new int[] {0, 0, 0};
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

			//transfer les infos vers un script qui garde l'info pendant le changement de level.
			UnitSelected.monsterBloc = _monsterX;
			UnitSelected.monsterSource = _monsterY;
			UnitSelected.totemBloc = _totemX;
			UnitSelected.totemSource = _totemY;

			//change le level
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
		GUI.Box (new Rect (50, 20, 40, 40), iconBloc[_monsterX[0]]);

		if(GUI.Button (new Rect(10,20,30,40),"<")){
			_monsterX[0] -= 1;
			if (_monsterX[0] <= -1){
				_monsterX[0] = 4;
			}
		}
		if(GUI.Button (new Rect(100,20,30,40),">")){
			_monsterX[0] += 1;
			if (_monsterX[0] >= 5){
				_monsterX[0] = 0;
			}
		}

		//choix de l'element pour les sources
		GUI.Box (new Rect (50, 100, 40, 40), iconSource[_monsterY[0]]);

		if(GUI.Button (new Rect(10,100,30,40),"<")){
			_monsterY[0] -= 1;
			if (_monsterY[0] <= -1){
				_monsterY[0] = 4;
			}
		}
		if(GUI.Button (new Rect(100,100,30,40),">")){
			_monsterY[0] += 1;
			if (_monsterY[0] >= 5){
				_monsterY[0] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (160, 10, 130, 130), monsterPicture [_monsterX[0]].sourceElement [_monsterY[0]]);

		GUI.EndGroup();



		//Choix du monstre 2
		GUI.BeginGroup (new Rect (10, _sh / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (50, 20, 40, 40), iconBloc[_monsterX[1]]);
		
		if(GUI.Button (new Rect(10,20,30,40),"<")){
			_monsterX[1] -= 1;
			if (_monsterX[1] <= -1){
				_monsterX[1] = 4;
			}
		}
		if(GUI.Button (new Rect(100,20,30,40),">")){
			_monsterX[1] += 1;
			if (_monsterX[1] >= 5){
				_monsterX[1] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (50, 100, 40, 40), iconSource[_monsterY[1]]);
		
		if(GUI.Button (new Rect(10,100,30,40),"<")){
			_monsterY[1] -= 1;
			if (_monsterY[1] <= -1){
				_monsterY[1] = 4;
			}
		}
		if(GUI.Button (new Rect(100,100,30,40),">")){
			_monsterY[1] += 1;
			if (_monsterY[1] >= 5){
				_monsterY[1] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (160, 10, 130, 130), monsterPicture [_monsterX[1]].sourceElement [_monsterY[1]]);
		
		GUI.EndGroup();



		//Choix du monstre 3
		GUI.BeginGroup (new Rect (10, _sh * 2 / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (50, 20, 40, 40), iconBloc[_monsterX[2]]);
		
		if(GUI.Button (new Rect(10,20,30,40),"<")){
			_monsterX[2] -= 1;
			if (_monsterX[2] <= -1){
				_monsterX[2] = 4;
			}
		}
		if(GUI.Button (new Rect(100,20,30,40),">")){
			_monsterX[2] += 1;
			if (_monsterX[2] >= 5){
				_monsterX[2] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (50, 100, 40, 40), iconSource[_monsterY[2]]);
		
		if(GUI.Button (new Rect(10,100,30,40),"<")){
			_monsterY[2] -= 1;
			if (_monsterY[2] <= -1){
				_monsterY[2] = 4;
			}
		}
		if(GUI.Button (new Rect(100,100,30,40),">")){
			_monsterY[2] += 1;
			if (_monsterY[2] >= 5){
				_monsterY[2] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (160, 10, 130, 130), monsterPicture [_monsterX[2]].sourceElement [_monsterY[2]]);
		
		GUI.EndGroup();

		/******************************************************
		 * Fin du choix des monstres, debut du choix des totems*
		 * ****************************************************/

		//Choix du Totem 1
		GUI.BeginGroup (new Rect (_sw - 310, 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (210, 20, 40, 40), iconBloc[_totemX[0]]);
		
		if(GUI.Button (new Rect(170,20,30,40),"<")){
			_totemX[0] -= 1;
			if (_totemX[0] <= -1){
				_totemX[0] = 4;
			}
		}
		if(GUI.Button (new Rect(260,20,30,40),">")){
			_totemX[0] += 1;
			if (_totemX[0] >= 5){
				_totemX[0] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (210, 100, 40, 40), iconSource[_totemY[0]]);
		
		if(GUI.Button (new Rect(170,100,30,40),"<")){
			_totemY[0] -= 1;
			if (_totemY[0] <= -1){
				_totemY[0] = 4;
			}
		}
		if(GUI.Button (new Rect(260,100,30,40),">")){
			_totemY[0] += 1;
			if (_totemY[0] >= 5){
				_totemY[0] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (10, 10, 130, 130), totemPicture [_totemX[0]].sourceElement [_totemY[0]]);
		
		GUI.EndGroup();



		//Choix du Totem 2
		GUI.BeginGroup (new Rect (_sw - 310, _sh / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (210, 20, 40, 40), iconBloc[_totemX[1]]);
		
		if(GUI.Button (new Rect(170,20,30,40),"<")){
			_totemX[1] -= 1;
			if (_totemX[1] <= -1){
				_totemX[1] = 4;
			}
		}
		if(GUI.Button (new Rect(260,20,30,40),">")){
			_totemX[1] += 1;
			if (_totemX[1] >= 5){
				_totemX[1] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (210, 100, 40, 40), iconSource[_totemY[1]]);
		
		if(GUI.Button (new Rect(170,100,30,40),"<")){
			_totemY[1] -= 1;
			if (_totemY[1] <= -1){
				_totemY[1] = 4;
			}
		}
		if(GUI.Button (new Rect(260,100,30,40),">")){
			_totemY[1] += 1;
			if (_totemY[1] >= 5){
				_totemY[1] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (10, 10, 130, 130), totemPicture [_totemX[1]].sourceElement [_totemY[1]]);
		
		GUI.EndGroup();



		//Choix du Totem 3
		GUI.BeginGroup (new Rect (_sw - 310, _sh * 2 / 3 + 10, 300, _sh / 3 - 10));
		
		GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (210, 20, 40, 40), iconBloc[_totemX[2]]);
		
		if(GUI.Button (new Rect(170,20,30,40),"<")){
			_totemX[2] -= 1;
			if (_totemX[2] <= -1){
				_totemX[2] = 4;
			}
		}
		if(GUI.Button (new Rect(260,20,30,40),">")){
			_totemX[2] += 1;
			if (_totemX[2] >= 5){
				_totemX[2] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (210, 100, 40, 40), iconSource[_totemY[2]]);
		
		if(GUI.Button (new Rect(170,100,30,40),"<")){
			_totemY[2] -= 1;
			if (_totemY[2] <= -1){
				_totemY[2] = 4;
			}
		}
		if(GUI.Button (new Rect(260,100,30,40),">")){
			_totemY[2] += 1;
			if (_totemY[2] >= 5){
				_totemY[2] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (10, 10, 130, 130), totemPicture [_totemX[2]].sourceElement [_totemY[2]]);
		
		GUI.EndGroup();
	}
}
