using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class BattleScreen : MonoBehaviour {

	public enum BattleScreenObject {
		EnemyPokemonName,
		EnemyPokemonHP,
		MoveType,
		PokemonNicknameLevelAndStatus,
		MovePP,
		PokemonHP,
		MoveOption1,
		FightOption,
		PkmnOption,
		MoveOption2,
		MoveOption3,
		ItemOption,
		RunOption,
		MoveOption4
	}

	public static BattleScreen S;
	public List<GameObject> optionSlots;
	public int selectedOption;

	void Awake () {
		S = this;
	}

	// Use this for initialization
	void Start () {
		closeBattleScreen ();
		
		foreach (Transform child in transform) {
			optionSlots.Add(child.gameObject);
		}
		
		optionSlots = optionSlots.OrderByDescending (m => m.transform.transform.position.y).ToList();
		// EnemyPokemonName
		// EnemyPokemonHP
		// MoveType
		// 3 MoveInfoBackground
		// PokemonNicknameLevelAndStatus
		// MovePP
		// PokemonHP
		// MoveOption1
		// FightOption
		// PkmnOption
		// 10 BattleOptionsBackground
		// MoveOption2
		// MoveOption3
		// ItemOption
		// RunOption
		// MoveOption4

		optionSlots.RemoveAt(10);
		optionSlots.RemoveAt(3);

		selectedOption = (int) BattleScreenObject.FightOption;
		optionSlots [(int) BattleScreenObject.FightOption].GetComponent<GUIText> ().color = Color.red;

	}

	void selectOption(BattleScreenObject selection){
		foreach (GameObject option in optionSlots) {
			option.GetComponent<GUIText>().color = Color.black;
		}
		selectedOption = (int) selection;
		optionSlots [(int) selection].GetComponent<GUIText> ().color = Color.red;
	}

	public void closeBattleScreen() {
		Main.S.battleScreenOpen = false;
		Main.S.paused = false;
		gameObject.SetActive(false);
	}


	public void showBattleScreen() {
		Main.S.paused = true;
		Main.S.battleScreenOpen = true;
		gameObject.SetActive (true);
		

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)){
			switch(selectedOption){
			case (int) BattleScreenObject.MoveOption1:
				print ("Selected first move");
				break;
			case (int) BattleScreenObject.MoveOption2:
				print ("Selected second move");
				break;
			case (int) BattleScreenObject.MoveOption3:
				print ("Selected third move");
				break;
			case (int) BattleScreenObject.MoveOption4:
				print ("Selected fourth move");
				break;
			case (int) BattleScreenObject.FightOption:
				selectOption(BattleScreenObject.MoveOption1);
				break;
			case (int) BattleScreenObject.ItemOption:
				// ItemMenu needs to be opaque
				Menu_Item.S.showItemMenu();
				print ("Selected ITEM");
				break;
			case (int) BattleScreenObject.PkmnOption:
				print ("Selected PKMN");
				break;
			case (int) BattleScreenObject.RunOption:
				print ("Selected RUN");
				break;
			}
		} else if (Input.GetKeyDown (KeyCode.X)){
			switch(selectedOption){
			case (int) BattleScreenObject.MoveOption1:
			case (int) BattleScreenObject.MoveOption2:
			case (int) BattleScreenObject.MoveOption3:
			case (int) BattleScreenObject.MoveOption4:
				selectOption(BattleScreenObject.FightOption);
				break;
			}
		} else if (Input.GetKeyDown (KeyCode.UpArrow)){
			switch(selectedOption){
			case (int) BattleScreenObject.MoveOption2:
				selectOption(BattleScreenObject.MoveOption1);
				break;
			case (int) BattleScreenObject.MoveOption3:
				selectOption(BattleScreenObject.MoveOption2);
				break;
			case (int) BattleScreenObject.MoveOption4:
				selectOption(BattleScreenObject.MoveOption3);
				break;
			case (int) BattleScreenObject.ItemOption:
				selectOption(BattleScreenObject.FightOption);
				break;
			case (int) BattleScreenObject.RunOption:
				selectOption(BattleScreenObject.PkmnOption);
				break;
			}
		} else if (Input.GetKeyDown (KeyCode.DownArrow)){
			switch(selectedOption){
			case (int) BattleScreenObject.MoveOption1:
				if (optionSlots[(int) BattleScreenObject.MoveOption2].GetComponent<GUIText>().text != "-")
				selectOption(BattleScreenObject.MoveOption2);
				break;
			case (int) BattleScreenObject.MoveOption2:
				if (optionSlots[(int) BattleScreenObject.MoveOption3].GetComponent<GUIText>().text != "-")
				selectOption(BattleScreenObject.MoveOption3);
				break;
			case (int) BattleScreenObject.MoveOption3:
				if (optionSlots[(int) BattleScreenObject.MoveOption4].GetComponent<GUIText>().text != "-")
				selectOption(BattleScreenObject.MoveOption4);
				break;
			case (int) BattleScreenObject.FightOption:
				selectOption(BattleScreenObject.ItemOption);
				break;
			case (int) BattleScreenObject.PkmnOption:
				selectOption(BattleScreenObject.RunOption);
				break;
			}
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)){
			switch(selectedOption){
			case (int) BattleScreenObject.PkmnOption:
				selectOption(BattleScreenObject.FightOption);
				break;
			case (int) BattleScreenObject.RunOption:
				selectOption(BattleScreenObject.ItemOption);
				break;
			}
		} else if (Input.GetKeyDown (KeyCode.RightArrow)){
			switch(selectedOption){
			case (int) BattleScreenObject.FightOption:
				selectOption(BattleScreenObject.PkmnOption);
				break;
			case (int) BattleScreenObject.ItemOption:
				selectOption(BattleScreenObject.RunOption);
				break;
			}
		} else {
		}
	}
}

