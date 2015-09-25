﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Menu_Pokemon : MonoBehaviour {
	public static Menu_Pokemon S;
	
	public List<GameObject> pokemonSlots;
	public int selectedPokemon;
	int maxPokemonNameChar = 10;

	
	void Awake() {
		S = this;
	}

	void Start () {
		bool justOpened = true;
		selectedPokemon = 0;
		closePokemonMenu ();
		
		foreach (Transform child in transform) {
			pokemonSlots.Add(child.gameObject);
		}
		
		pokemonSlots = pokemonSlots.OrderByDescending (m => m.transform.transform.position.y).ToList();
		
		foreach (GameObject go in pokemonSlots) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(justOpened) itemText.color = Color.red;
			justOpened = false;
		}
	}

	public void closePokemonMenu() {
		Main.S.pokemonMenuOpen = false;
		gameObject.SetActive(false);
	}

	public void showPokemonMenu() {
		Main.S.pokemonMenuOpen = true;
		gameObject.SetActive (true);
		// Hide details cards
		PokemonDetails1.S.close();
		PokemonDetails2.S.close();
		
		// Clear the pokemon menu
		for (int j = 0; j < 7; ++j) {
			GameObject itemSlot = transform.Find ("PokemonSlot" + (j + 1).ToString ()).gameObject;
			GUIText itemSlotText = itemSlot.GetComponent<GUIText> ();
			itemSlotText.text = "";
		}
		
		int i;
		for (i = 0; i < 6 && i < Player.S.party.Count; ++i) {
			GameObject pokemonSlot = transform.Find("PokemonSlot" + (i+1).ToString()).gameObject;
			GUIText pokemonSlotText = pokemonSlot.GetComponent<GUIText>();
			pokemonSlotText.text = Player.S.party[i].pokemonNickname;
		}
		GameObject cancelSlot = transform.Find("PokemonSlot" + (i+1).ToString()).gameObject;
		GUIText cancelSlotText = cancelSlot.GetComponent<GUIText>();
		cancelSlotText.text = "CANCEL";
	}
	
	public void MoveDownPokemonMenu(){
		pokemonSlots[selectedPokemon].GetComponent<GUIText> ().color = Color.black;
		// Not count - 1 because we should be able to select the CANCEL button
		if (selectedPokemon < Player.S.party.Count) {
			++selectedPokemon;
		} else {
			// Don't wrap around
		}
		pokemonSlots [selectedPokemon].GetComponent<GUIText> ().color = Color.red;
	}
	
	public void MoveUpPokemonMenu() {
		pokemonSlots[selectedPokemon].GetComponent<GUIText> ().color = Color.black;
		if (selectedPokemon != 0) {
			--selectedPokemon;
		} else {
			// Don't wrap around
		}
		pokemonSlots [selectedPokemon].GetComponent<GUIText> ().color = Color.red;
	}

	public void switchPokemon(){
		if (Player.S.party.Count < 2) {
			print ("Nothing to switch.");
			return;
		} else {
			print ("Do some switching");
		}
	}


	// Update is called once per frame
	void Update () {
		if (!Main.S.selectionBoxOpen && !Main.S.inDialog) {
			if (Main.S.pokemonDetailsOpen[0]) {
				if (Input.GetKeyDown (KeyCode.Z)){
					PokemonDetails1.S.close();
					PokemonDetails2.S.show();
				}
			} else if (Main.S.pokemonDetailsOpen[1]) {
				if (Input.GetKeyDown (KeyCode.Z)){
					PokemonDetails2.S.close();
					showPokemonMenu();
				}
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MoveDownPokemonMenu ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MoveUpPokemonMenu ();
			} else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Z)) {
				if (pokemonSlots [selectedPokemon].GetComponent<GUIText> ().text == "CANCEL") {
					closePokemonMenu ();
				} else {
					// Open up STATS/SWITCH menu
					SelectionBox.S.setOptions("STATS", "SWITCH");
					SelectionBox.S.openSelectionBox ();
				}
			}
		}
	}
}