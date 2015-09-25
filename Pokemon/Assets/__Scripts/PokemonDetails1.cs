using UnityEngine;
using System.Collections;

public class PokemonDetails1 : MonoBehaviour {

	public static PokemonDetails1 S;

	// Use this for initialization
	void Awake () {
		S = this;
	}

	void Start(){
		close ();
	}

	public void show(){
		Pokemon curPokemon = Player.S.party [Menu_Pokemon.S.selectedPokemon];
		
		GameObject pokemonNicknameAndLevel = transform.Find("PokemonNicknameAndLevel").gameObject;
		GUIText pokemonNicknameAndLevelText = pokemonNicknameAndLevel.GetComponent<GUIText>();
		pokemonNicknameAndLevelText.text = curPokemon.pokemonNickname + " Lv" + curPokemon.level.ToString();
		
		GameObject pokemonHP = transform.Find("PokemonHP").gameObject;
		GUIText pokemonHPText = pokemonHP.GetComponent<GUIText>();
		pokemonHPText.text = "HP " + curPokemon.curHP.ToString () + "/" + curPokemon.maxHP.ToString ();
		
		GameObject pokemonStatus = transform.Find("PokemonStatus").gameObject;
		GUIText pokemonStatusText = pokemonStatus.GetComponent<GUIText>();
		pokemonStatusText.text = "STATUS/" + curPokemon.status;
		
		GameObject pokemonAttackValue = transform.Find("PokemonAttackValue").gameObject;
		GUIText pokemonAttackValueText = pokemonAttackValue.GetComponent<GUIText>();
		pokemonAttackValueText.text = curPokemon.attack.ToString ();
		
		GameObject pokemonDefenseValue = transform.Find("PokemonDefenseValue").gameObject;
		GUIText pokemonDefenseValueText = pokemonDefenseValue.GetComponent<GUIText>();
		pokemonDefenseValueText.text = curPokemon.defense.ToString ();
		
		GameObject pokemonSpeedValue = transform.Find("PokemonSpeedValue").gameObject;
		GUIText pokemonSpeedValueText = pokemonSpeedValue.GetComponent<GUIText>();
		pokemonSpeedValueText.text = curPokemon.speed.ToString ();
		
		GameObject pokemonSpecialValue = transform.Find("PokemonSpecialValue").gameObject;
		GUIText pokemonSpecialValueText = pokemonSpecialValue.GetComponent<GUIText>();
		pokemonSpecialValueText.text = curPokemon.special.ToString ();
		
		GameObject pokemonType1 = transform.Find("PokemonType1").gameObject;
		GUIText pokemonType1Text = pokemonType1.GetComponent<GUIText>();
		pokemonType1Text.text = "TYPE/\n" + curPokemon.type1;
		
		if (curPokemon.type2 != "") {
			GameObject pokemonType2 = transform.Find ("PokemonType2").gameObject;
			GUIText pokemonType2Text = pokemonType2.GetComponent<GUIText> ();
			pokemonType2Text.text = "TYPE2/\n" + curPokemon.type2;
		} else {
			GameObject pokemonType2 = transform.Find ("PokemonType2").gameObject;
			GUIText pokemonType2Text = pokemonType2.GetComponent<GUIText> ();
			pokemonType2Text.text = "";
		}
		
		GameObject pokemonIdno = transform.Find("PokemonIdno").gameObject;
		GUIText pokemonIdnoText = pokemonIdno.GetComponent<GUIText>();
		pokemonIdnoText.text = "IdNo/\n " + curPokemon.idno.ToString();
		
		GameObject pokemonOT = transform.Find("PokemonOT").gameObject;
		GUIText pokemonOTText = pokemonOT.GetComponent<GUIText>();
		pokemonOTText.text = "OT/\n" + curPokemon.ot;

		Main.S.pokemonDetailsOpen [0] = true;
		gameObject.SetActive (true);
	}

	public void close(){
		Main.S.pokemonDetailsOpen [0] = false;
		gameObject.SetActive (false);
	}
	void Update(){
		if (!Main.S.selectionBoxOpen && !Main.S.inDialog) {
			if (Input.GetKeyDown (KeyCode.Z)){
				PokemonDetails2.S.show();
				close();
			}
		}
	}
}
