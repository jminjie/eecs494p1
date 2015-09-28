using UnityEngine;
using System.Collections;

public class PokemonDetails2 : MonoBehaviour {

	public static PokemonDetails2 S;
	
	// Use this for initialization
	void Awake () {
		S = this;
	}

	void Start(){
		close ();
	}

	public void show(){
		Pokemon curPokemon = Player.S.party [Menu_Pokemon.S.selectedPokemon];
		
		GameObject pokemonName = transform.Find("PokemonName").gameObject;
		GUIText pokemonNameText = pokemonName.GetComponent<GUIText>();
		pokemonNameText.text = curPokemon.pokemonName;
		
		GameObject pokemonNo = transform.Find("PokemonNo").gameObject;
		GUIText pokemonNoText = pokemonNo.GetComponent<GUIText>();
		pokemonNoText.text = "No." + curPokemon.no.ToString("000");
		
		GameObject pokemonExp = transform.Find("PokemonExp").gameObject;
		GUIText pokemonExpText = pokemonExp.GetComponent<GUIText>();
		pokemonExpText.text = "EXP POINTS/" + curPokemon.exppoints.ToString();
		
		GameObject pokemonLevelUp = transform.Find("PokemonLevelUp").gameObject;
		GUIText pokemonLevelUpText = pokemonLevelUp.GetComponent<GUIText>();
		pokemonLevelUpText.text = "LEVEL UP/" + (curPokemon.expToLevel(curPokemon.level) - curPokemon.exppoints).ToString();
		
		for (int i = 0; i < 4 && i < curPokemon.moves.Count; i++){
			GameObject pokemonMove = transform.Find("PokemonMove" + (i+1).ToString()).gameObject;
			GUIText pokemonMoveText = pokemonMove.GetComponent<GUIText>();
			pokemonMoveText.text = curPokemon.moves[i].moveName;
			GameObject pokemonMovePP = transform.Find("PokemonMovePP" + (i+1).ToString()).gameObject;
			GUIText pokemonMovePPText = pokemonMovePP.GetComponent<GUIText>();
			pokemonMovePPText.text = "pp " + curPokemon.moves[i].curPP.ToString()
				+ "/" + curPokemon.moves[i].maxPP.ToString();
		}

		for (int i = curPokemon.moves.Count; i < 4; i++) {
			GameObject pokemonMove = transform.Find("PokemonMove" + (i+1).ToString()).gameObject;
			GUIText pokemonMoveText = pokemonMove.GetComponent<GUIText>();
			pokemonMoveText.text = "-";
			GameObject pokemonMovePP = transform.Find("PokemonMovePP" + (i+1).ToString()).gameObject;
			GUIText pokemonMovePPText = pokemonMovePP.GetComponent<GUIText>();
			pokemonMovePPText.text = "--";
		}
		Main.S.pokemonDetailsOpen [1] = true;
		gameObject.SetActive (true);
	}
	
	public void close(){
		Main.S.pokemonDetailsOpen [1] = false;
		gameObject.SetActive (false);
	}

	void Update(){
		if (!Main.S.selectionBoxOpen && !Main.S.inDialog) {
			if (Input.GetKeyDown (KeyCode.Z)){
				Menu_Pokemon.S.showPokemonMenu();
				close();
			}
		}
	}

}
