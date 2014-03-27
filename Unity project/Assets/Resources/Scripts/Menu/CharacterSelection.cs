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

	private string[] _blocTooltips = {"Earth", "Ice","Metal", "Plant", "Rock"};
	private string[] _sourceTooltips = {"Electricity", "Lava", "Sand", "Water", "Wind"};
	public Texture[] textureBloc;
	public Texture[] textureSource;
	private GUIContent[] _iconBloc = new GUIContent[5];
	private GUIContent[] _iconSource = new GUIContent[5];

	public BlocElement[] monsterPicture;
	public BlocElement[] totemPicture;
	public GUISkin mainMenuSkin;
	public GUIStyle characterBox;
	public GUIStyle arrowRight;
	public GUIStyle arrowLeft;
	public Texture deco01;

	private int[] _monsterX = new int[] {0, 0, 0};
	private int[] _monsterY = new int[] {0, 0, 0};
	private int[] _totemX = new int[] {0, 0, 0};
	private int[] _totemY = new int[] {0, 0, 0};

	void Awake () {

		for (int i = 0; i < 5; ++i) {
			_iconBloc[i] = new GUIContent("", textureBloc[i], _blocTooltips[i]); 
		}
		for (int i = 0; i < 5; ++i) {
			_iconSource[i] = new GUIContent("", textureSource[i], _sourceTooltips[i]); 
		}
		enabled = false;


	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		GUI.skin = mainMenuSkin;

		//lance le jeu.
		if (GUI.Button (new Rect(_sw / 2 - 40, _sh - 120, 80, 60),"GO!")){

			//transfer les infos vers un script qui garde l'info pendant le changement de level.
			UnitSelected.monsterBloc = _monsterX;
			UnitSelected.monsterSource = _monsterY;
			UnitSelected.totemBloc = _totemX;
			UnitSelected.totemSource = _totemY;

			//change le level
			Application.LoadLevel("MainScene");
		}
		//retourne a l'écran titre.
		if (GUI.Button (new Rect(_sw / 2 - 40, _sh - 50, 80, 40),"Back")){
			gameObject.GetComponent<MainMenu>().enabled = true;
			enabled = false;
		}

		/******************************
		 * Debut du choix des monstres*
		 * ****************************/

		//Choix du monstre 1
		GUI.BeginGroup (new Rect (10, 10, 300, _sh));

		GUI.Box (new Rect(0,0,300,_sh - 20),"",characterBox);
		GUI.DrawTexture (new Rect (10, 35, 280, 230), deco01);
		GUI.DrawTexture (new Rect (10, 185, 280, 230), deco01);

		// choix de l'element pour les blocs
		GUI.Box (new Rect (60, 20, 40, 40), _iconBloc[_monsterX[0]]);
		GUI.Label (new Rect(60, 60, 40, 20), GUI.tooltip);
		GUI.tooltip = null;

		if(GUI.Button (new Rect(15,20,40,40),"",arrowLeft)){
			_monsterX[0] -= 1;
			if (_monsterX[0] <= -1){
				_monsterX[0] = 4;
			}
		}
		if(GUI.Button (new Rect(105,20,40,40),"",arrowRight)){
			_monsterX[0] += 1;
			if (_monsterX[0] >= 5){
				_monsterX[0] = 0;
			}
		}

		//choix de l'element pour les sources
		GUI.Box (new Rect (60, 80, 40, 40), _iconSource[_monsterY[0]]);
		GUI.Label (new Rect(60, 120, 60, 20), GUI.tooltip);
		GUI.tooltip = null;

		if(GUI.Button (new Rect(15,80,40,40),"",arrowLeft)){
			_monsterY[0] -= 1;
			if (_monsterY[0] <= -1){
				_monsterY[0] = 4;
			}
		}
		if(GUI.Button (new Rect(105,80,40,40),"",arrowRight)){
			_monsterY[0] += 1;
			if (_monsterY[0] >= 5){
				_monsterY[0] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (170, 20, 110, 110), monsterPicture [_monsterX[0]].sourceElement [_monsterY[0]]);


		GUI.EndGroup();



		//Choix du monstre 2
		GUI.BeginGroup (new Rect (10, _sh / 3 + 10, 300, _sh / 3 - 10));
		
		//GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (60, 20, 40, 40), _iconBloc[_monsterX[1]]);
		GUI.Label (new Rect(60, 60, 40, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(15,20,40,40),"",arrowLeft)){
			_monsterX[1] -= 1;
			if (_monsterX[1] <= -1){
				_monsterX[1] = 4;
			}
		}
		if(GUI.Button (new Rect(105,20,40,40),"",arrowRight)){
			_monsterX[1] += 1;
			if (_monsterX[1] >= 5){
				_monsterX[1] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (60, 80, 40, 40), _iconSource[_monsterY[1]]);
		GUI.Label (new Rect(60, 120, 60, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(15,80,40,40),"",arrowLeft)){
			_monsterY[1] -= 1;
			if (_monsterY[1] <= -1){
				_monsterY[1] = 4;
			}
		}
		if(GUI.Button (new Rect(105,80,40,40),"",arrowRight)){
			_monsterY[1] += 1;
			if (_monsterY[1] >= 5){
				_monsterY[1] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (170, 15, 110, 110), monsterPicture [_monsterX[1]].sourceElement [_monsterY[1]]);
		
		GUI.EndGroup();



		//Choix du monstre 3
		GUI.BeginGroup (new Rect (10, _sh * 2 / 3 + 10, 300, _sh / 3 - 10));
		
		//GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (60, 20, 40, 40), _iconBloc[_monsterX[2]]);
		GUI.Label (new Rect(60, 60, 40, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(15,20,40,40),"",arrowLeft)){
			_monsterX[2] -= 1;
			if (_monsterX[2] <= -1){
				_monsterX[2] = 4;
			}
		}
		if(GUI.Button (new Rect(105,20,40,40),"",arrowRight)){
			_monsterX[2] += 1;
			if (_monsterX[2] >= 5){
				_monsterX[2] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (60, 80, 40, 40), _iconSource[_monsterY[2]]);
		GUI.Label (new Rect(60, 120, 60, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(15,80,40,40),"",arrowLeft)){
			_monsterY[2] -= 1;
			if (_monsterY[2] <= -1){
				_monsterY[2] = 4;
			}
		}
		if(GUI.Button (new Rect(105,80,40,40),"",arrowRight)){
			_monsterY[2] += 1;
			if (_monsterY[2] >= 5){
				_monsterY[2] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (170, 10, 110, 110), monsterPicture [_monsterX[2]].sourceElement [_monsterY[2]]);
		
		GUI.EndGroup();

		/******************************************************
		 * Fin du choix des monstres, debut du choix des totems*
		 * ****************************************************/

		//Choix du Totem 1
		GUI.BeginGroup (new Rect (_sw - 310, 10, 300, _sh));
		
		GUI.Box (new Rect(0,0,300,_sh - 20),"", characterBox);
		GUI.DrawTexture (new Rect (10, 35, 280, 230), deco01);
		GUI.DrawTexture (new Rect (10, 185, 280, 230), deco01);
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (200, 20, 40, 40), _iconBloc[_totemX[0]]);
		GUI.Label (new Rect(200, 60, 40, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(155,20,40,40),"",arrowLeft)){
			_totemX[0] -= 1;
			if (_totemX[0] <= -1){
				_totemX[0] = 4;
			}
		}
		if(GUI.Button (new Rect(245,20,40,40),"",arrowRight)){
			_totemX[0] += 1;
			if (_totemX[0] >= 5){
				_totemX[0] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (200, 80, 40, 40), _iconSource[_totemY[0]]);
		GUI.Label (new Rect(200, 120, 60, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(155,80,40,40),"",arrowLeft)){
			_totemY[0] -= 1;
			if (_totemY[0] <= -1){
				_totemY[0] = 4;
			}
		}
		if(GUI.Button (new Rect(245,80,40,40),"",arrowRight)){
			_totemY[0] += 1;
			if (_totemY[0] >= 5){
				_totemY[0] = 0;
			}
		}
		//Affiche le totem choisi.
		GUI.Box (new Rect (20, 20, 110, 110), totemPicture [_totemX[0]].sourceElement [_totemY[0]]);
		
		GUI.EndGroup();



		//Choix du Totem 2
		GUI.BeginGroup (new Rect (_sw - 310, _sh / 3 + 10, 300, _sh / 3 - 10));
		
		//GUI.Box (new Rect(0,0,300,_sh),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (200, 20, 40, 40), _iconBloc[_totemX[1]]);
		GUI.Label (new Rect(200, 60, 40, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(155,20,40,40),"",arrowLeft)){
			_totemX[1] -= 1;
			if (_totemX[1] <= -1){
				_totemX[1] = 4;
			}
		}
		if(GUI.Button (new Rect(245,20,40,40),"",arrowRight)){
			_totemX[1] += 1;
			if (_totemX[1] >= 5){
				_totemX[1] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (200, 80, 40, 40), _iconSource[_totemY[1]]);
		GUI.Label (new Rect(200, 120, 60, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(155,80,40,40),"",arrowLeft)){
			_totemY[1] -= 1;
			if (_totemY[1] <= -1){
				_totemY[1] = 4;
			}
		}
		if(GUI.Button (new Rect(245,80,40,40),"",arrowRight)){
			_totemY[1] += 1;
			if (_totemY[1] >= 5){
				_totemY[1] = 0;
			}
		}
		//Affiche le totem choisi.
		GUI.Box (new Rect (20, 15, 110, 110), totemPicture [_totemX[1]].sourceElement [_totemY[1]]);
		
		GUI.EndGroup();



		//Choix du Totem 3
		GUI.BeginGroup (new Rect (_sw - 310, _sh * 2 / 3 + 10, 300, _sh / 3 - 10));
		
		//GUI.Box (new Rect(0,0,300,_sh / 3),"");
		
		// choix de l'element pour les blocs
		GUI.Box (new Rect (200, 20, 40, 40), _iconBloc[_totemX[2]]);
		GUI.Label (new Rect(200, 60, 40, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(155,20,40,40),"",arrowLeft)){
			_totemX[2] -= 1;
			if (_totemX[2] <= -1){
				_totemX[2] = 4;
			}
		}
		if(GUI.Button (new Rect(245,20,40,40),"",arrowRight)){
			_totemX[2] += 1;
			if (_totemX[2] >= 5){
				_totemX[2] = 0;
			}
		}
		
		//choix de l'element pour les sources
		GUI.Box (new Rect (200, 80, 40, 40), _iconSource[_totemY[2]]);
		GUI.Label (new Rect(200, 120, 60, 20), GUI.tooltip);
		GUI.tooltip = null;
		
		if(GUI.Button (new Rect(155,80,40,40),"",arrowLeft)){
			_totemY[2] -= 1;
			if (_totemY[2] <= -1){
				_totemY[2] = 4;
			}
		}
		if(GUI.Button (new Rect(245,80,40,40),"",arrowRight)){
			_totemY[2] += 1;
			if (_totemY[2] >= 5){
				_totemY[2] = 0;
			}
		}
		//Affiche le monstre choisi.
		GUI.Box (new Rect (20, 10, 110, 110), totemPicture [_totemX[2]].sourceElement [_totemY[2]]);
		
		GUI.EndGroup();
	}
}
