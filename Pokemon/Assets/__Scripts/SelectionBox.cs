using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SelectionBox : MonoBehaviour {

	public static SelectionBox S;

	public int selection;
	public List<GameObject> choices;
	
	void Awake () {
		S = this;
	}

	// Use this for initialization
	void Start () {
		selection = 0;
		closeSelectionBox();
		
		foreach (Transform child in transform) {
			choices.Add(child.gameObject);
		}
		
		choices = choices.OrderByDescending (m => m.transform.transform.position.y).ToList();

		GUIText firstChoiceText = choices [0].GetComponent<GUIText> ();
		firstChoiceText.color = Color.red;
	}

	public void setOptions(string first, string second){
		GameObject selection1 = transform.Find ("Selection1").gameObject;
		GUIText selection1Text = selection1.GetComponent<GUIText> ();
		selection1Text.text = first;

		GameObject selection2 = transform.Find ("Selection2").gameObject;
		GUIText selection2Text = selection2.GetComponent<GUIText> ();
		selection2Text.text = second;
	}

	public void closeSelectionBox(){
		gameObject.SetActive(false);
		Main.S.selectionBoxOpen = false;
	}

	public void openSelectionBox(){
		gameObject.SetActive (true);
		Main.S.selectionBoxOpen = true;
	}

	public void MoveDownSelectionBox(){
		choices[selection].GetComponent<GUIText> ().color = Color.black;
		if (selection < SelectionBox.S.choices.Count - 1) {
			++selection;
		} else {
			// Don't loop around for selection box
		}
		choices [selection].GetComponent<GUIText> ().color = Color.red;
	}
	
	public void MoveUpSelectionBox() {
		choices[selection].GetComponent<GUIText> ().color = Color.black;
		if (selection != 0) {
			--selection;
		} else {
			// Don't loop around for selection box
		}
		choices[selection].GetComponent<GUIText> ().color = Color.red;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.X)) {
			closeSelectionBox ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			MoveDownSelectionBox ();
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			MoveUpSelectionBox ();
		} else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Z)) {
			// Somehow need to communicate what was selected to whatever opened SelectionBox
			if (choices[selection].GetComponent<GUIText> ().text == "USE") {
				if (Main.S.battleScreenOpen) {
					if (Player.S.itemPack[Menu_Item.S.selectedItem].isUseableInBattle) {
						Menu_Item.S.useSelectedItem();
						Menu_Item.S.closeItemMenu();
						BattleScreen.S.doEnemyTurnNext = true;
					} else {
						string[] oak = {"OAK: This isn't the time for that!"};
						Dialog.S.ShowMessage (oak);
					}
					closeSelectionBox ();
				} else { 
					if (Player.S.itemPack[Menu_Item.S.selectedItem].isUseableInWorld) {
						Menu_Item.S.useSelectedItem();
					} else {
						string[] oak = {"OAK: This isn't the time for that!"};
						Dialog.S.ShowMessage (oak);
					}
					closeSelectionBox ();
				}
			} else if (choices[selection].GetComponent<GUIText> ().text == "TOSS") {
				if (Player.S.itemPack[Menu_Item.S.selectedItem].isTossable) {
					Menu_Item.S.tossSelectedItem();
				} else {
					Dialog.S.gameObject.SetActive (true);
					Color maxAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
					maxAlpha.a = 255;
					GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = maxAlpha;
					string[] oak = {"OAK: This item is important!"};
					Dialog.S.ShowMessage (oak);
				}
				closeSelectionBox ();
			} else if (choices[selection].GetComponent<GUIText> ().text == "STATS") {
				PokemonDetails1.S.show();
				Menu_Pokemon.S.closePokemonMenu();
				closeSelectionBox ();
			} else if (choices[selection].GetComponent<GUIText> ().text == "SWITCH") {
				if (Main.S.battleScreenOpen){
					if (Player.S.party[Menu_Pokemon.S.selectedPokemon].curHP == 0){
						string[] m = {Player.S.party[Menu_Pokemon.S.selectedPokemon].pokemonNickname + " has no HP!"};
						Dialog.S.ShowMessage(m);
						closeSelectionBox();
					} else {
						BattleScreen.S.playerPokemon = Player.S.party[Menu_Pokemon.S.selectedPokemon];
						BattleScreen.S.refreshBattleInfo();
						closeSelectionBox();
						Menu_Pokemon.S.closePokemonMenu();
					}
				} else {
					Menu_Pokemon.S.switchingPokemon = Menu_Pokemon.S.selectedPokemon;
					Menu_Pokemon.S.switching = true;
					closeSelectionBox ();
				}
			}
		}
	}
}
