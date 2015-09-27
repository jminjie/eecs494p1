using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum menuItem{
	pokedex,
	pokemon,
	item,
	player,
	save,
	option,
	exit
}

public class Menu : MonoBehaviour {

	public static Menu S;

	public int activeItem;
	public List<GameObject> menuItems;

	void Awake () {
		S = this;
	}

	// Use this for initialization
	void Start () {
		bool first = true;
		activeItem = 0;

		foreach (Transform child in transform) {
			menuItems.Add(child.gameObject);
		}

		menuItems = menuItems.OrderByDescending (m => m.transform.transform.position.y).ToList();

		foreach (GameObject go in menuItems) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(first) itemText.color = Color.red;
			first = false;
		}

		gameObject.SetActive (false);

	}

	public bool secondaryMenuOpen() {
		return Main.S.playerMenuOpen || Main.S.itemMenuOpen
			|| Main.S.selectionBoxOpen || Main.S.pokemonMenuOpen
				|| Main.S.pokemonDetailsOpen[0] || Main.S.pokemonDetailsOpen[1];
	}

	// Update is called once per frame
	void Update () {
		if (!secondaryMenuOpen () && !Main.S.battleScreenOpen) {
			if (Input.GetKeyDown (KeyCode.X)) {
				gameObject.SetActive (false);
				Main.S.paused = false;
			} else if (Main.S.paused) {
				if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Z)) {
					switch (activeItem) {
					case (int) menuItem.pokedex:
						string[] oak = {"OAK: Sorry " + Player.S.name + ", I'm still working on the POKEDEX!"};
						Dialog.S.ShowMessage(oak);
						break;
					case (int) menuItem.pokemon:
						Menu_Pokemon.S.showPokemonMenu();
						break;
					case (int) menuItem.item:
						Menu_Item.S.showItemMenu();
						break;
					case (int) menuItem.player:
						Menu_Player.S.showPlayerMenu ();
						break;
					case (int) menuItem.save:
						string[] saver = {"OAK: You don't really need to save your game!"};
						Dialog.S.ShowMessage(saver);
						break;
					case (int) menuItem.option:
						BattleScreen.S.showBattleScreen();
						break;
					case (int) menuItem.exit:
						gameObject.SetActive (false);
						Main.S.paused = false;
						break;
					}
				}
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MoveDownMenu ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MoveUpMenu ();
			}
		}
	}

	public void MoveDownMenu(){
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.black;
		if (activeItem < menuItems.Count - 1) {
			++activeItem;
		} else {
			activeItem = 0;
		}
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.red;
	}

	public void MoveUpMenu() {
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.black;
		if (activeItem != 0) {
			--activeItem;
		} else {
			activeItem = menuItems.Count - 1;
		}
		menuItems [activeItem].GetComponent<GUIText> ().color = Color.red;
	}

}
