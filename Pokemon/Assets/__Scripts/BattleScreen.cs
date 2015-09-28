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

	public int closeOnKeyDownX = 99;
	public int switchOnKeyDownX = 99;
	public bool doEnemyTurnNext = false;
	public bool endBattleNext = false;

	public bool lastBattleInSequence;
	public bool isTrainerBattle;

	public Pokemon playerPokemon = null;
	public Pokemon enemyPokemon = null;


	void Awake () {
		S = this;
	}
	
	void Start () {
		closeBattleScreen ();
		closeOnKeyDownX = 99;
		switchOnKeyDownX = 99;

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

	public void refreshBattleInfo(){
		optionSlots [(int)BattleScreenObject.EnemyPokemonName].GetComponent<GUIText> ().text =
			enemyPokemon.pokemonName;
		optionSlots [(int)BattleScreenObject.EnemyPokemonHP].GetComponent<GUIText> ().text =
			enemyPokemon.curHP.ToString() + "/" + enemyPokemon.maxHP.ToString();
		optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text = "";
		optionSlots [(int)BattleScreenObject.PokemonHP].GetComponent<GUIText> ().text =
			playerPokemon.curHP.ToString() + "/" + playerPokemon.maxHP.ToString();
		optionSlots [(int)BattleScreenObject.PokemonNicknameLevelAndStatus].GetComponent<GUIText> ().text =
			playerPokemon.pokemonNickname + " Lv" + playerPokemon.level.ToString();
		optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text = "";

		int numMoves = playerPokemon.moves.Count;
		optionSlots [(int)BattleScreenObject.MoveOption1].GetComponent<GUIText> ().text = playerPokemon.moves[0].moveName;
		if (numMoves > 1) {
			optionSlots [(int)BattleScreenObject.MoveOption2].GetComponent<GUIText> ().text = playerPokemon.moves [1].moveName;
		} else {
			optionSlots [(int)BattleScreenObject.MoveOption2].GetComponent<GUIText> ().text = "-";
			optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text = "-";
			optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text = "-";
		}
		if (numMoves > 2) {
			optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text = playerPokemon.moves [2].moveName;
		} else {
			optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text = "-";
			optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text = "-";
		}
		if (numMoves > 3)
			optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text = playerPokemon.moves[3].moveName;
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

	void doEnemyTurn() {
		// If enemy is a trainer, might also use an item
		// For now just do a random move
		int chosenMoveIndex = Mathf.FloorToInt(enemyPokemon.moves.Count * Random.value);
		Move chosenMove = enemyPokemon.moves[chosenMoveIndex];
		List<string> enemyMoveDialog = chosenMove.doMove (enemyPokemon, playerPokemon, false);

		if (playerPokemon.curHP == 0) {
			enemyMoveDialog.Add(playerPokemon.pokemonNickname + " fainted!\n");
			enemyMoveDialog.Add ("Switch pokemon!");
			switchOnKeyDownX = 1 - enemyMoveDialog.Count;
		}
		Dialog.S.ShowMessage (enemyMoveDialog);

	}

	public void showBattleScreen() {
		// temporary
		isTrainerBattle = true;
		lastBattleInSequence = true;
		enemyPokemon = Player.S.party [3];
		// end temporary

		closeOnKeyDownX = 99;
		switchOnKeyDownX = 99;
		doEnemyTurnNext = false;
		lastBattleInSequence = true;

		if (playerPokemon == null)
			playerPokemon = Player.S.party [0];
		Main.S.paused = true;
		Main.S.battleScreenOpen = true;
		gameObject.SetActive (true);

		string[] intro = {"Wild " + enemyPokemon.pokemonName + " appeared!",
			"Go " + playerPokemon.pokemonNickname + "!!"};
		Dialog.S.ShowMessage (intro);

		optionSlots [(int)BattleScreenObject.EnemyPokemonName].GetComponent<GUIText> ().text = enemyPokemon.pokemonName;
		optionSlots [(int)BattleScreenObject.EnemyPokemonHP].GetComponent<GUIText> ().text =
			enemyPokemon.curHP.ToString() + "/" + enemyPokemon.maxHP.ToString();
		optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text = "";
		optionSlots [(int)BattleScreenObject.PokemonHP].GetComponent<GUIText> ().text =
			playerPokemon.curHP.ToString() + "/" + playerPokemon.maxHP.ToString();
		optionSlots [(int)BattleScreenObject.PokemonNicknameLevelAndStatus].GetComponent<GUIText> ().text =
			playerPokemon.pokemonNickname + " Lv" + playerPokemon.level.ToString();
		optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text = "";
		optionSlots [(int)BattleScreenObject.PokemonHP].GetComponent<GUIText> ().text =
			playerPokemon.curHP.ToString() + "/" + playerPokemon.maxHP.ToString();
		int numMoves = playerPokemon.moves.Count;
		optionSlots [(int)BattleScreenObject.MoveOption1].GetComponent<GUIText> ().text = playerPokemon.moves[0].moveName;
		if (numMoves > 1) {
			optionSlots [(int)BattleScreenObject.MoveOption2].GetComponent<GUIText> ().text = playerPokemon.moves [1].moveName;
		} else {
			optionSlots [(int)BattleScreenObject.MoveOption2].GetComponent<GUIText> ().text = "-";
			optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text = "-";
			optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text = "-";
		}
		if (numMoves > 2) {
			optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text = playerPokemon.moves [2].moveName;
		} else {
			optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text = "-";
			optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text = "-";
		}
		if (numMoves > 3)
			optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text = playerPokemon.moves[3].moveName;
	}

	bool runAwaySuccess(int playerPokemonSpeed, int enemyPokemonSpeed){
		// F = A x 32 / B + 30 x C
		// A is playerPokemonSpeed
		// B is enemyPokemonSpeed divided by 4, mod 256
		// C is the number of times the player has tried to escape including this time
		// Number of attempts is set to 0 if player attacks
		return true;
	}

	void endBattle(){
		// Reset Pokemon's battle stats
		playerPokemon.battleAttack = playerPokemon.attack;
		playerPokemon.battleDefense = playerPokemon.defense;
		playerPokemon.battleSpeed = playerPokemon.speed;
		playerPokemon.battleSpecial = playerPokemon.special;

		List<string> endBattleDialog = new List<string>();
		if (lastBattleInSequence && isTrainerBattle) {
			endBattleDialog.Add("Enemy " + enemyPokemon.pokemonName + " fainted!\n");
			endBattleDialog.Add(playerPokemon.pokemonNickname + " gained " + (enemyPokemon.level * 8).ToString () + " EXP. Points!");
			endBattleDialog.Add(Player.S.name + " got &300 from trainer!");
			print (playerPokemon.pokemonNickname + " gained " + (enemyPokemon.level * 8).ToString () + " EXP. Points");
			playerPokemon.exppoints += enemyPokemon.level * 8;
			print ("Got &300 from trainer.");
			Player.S.money += 300;
			// Push x through dialog to close
			closeOnKeyDownX = 1 - endBattleDialog.Count;
			Dialog.S.ShowMessage(endBattleDialog);
		} else {
			endBattleDialog.Add("Enemy " + enemyPokemon.pokemonName + " fainted!");
			endBattleDialog.Add(playerPokemon.pokemonNickname + " gained " + (enemyPokemon.level * 8).ToString () + " EXP. Points!");
			if (lastBattleInSequence) {
				// Push x through dialog to close
				closeOnKeyDownX = 1 - endBattleDialog.Count;
				Dialog.S.ShowMessage (endBattleDialog);
			} else {
				// Load next enemyPokemon

			}
		}
	}

	void doAttack(int moveNum){
		Move chosenMove = playerPokemon.moves [moveNum];
		List<string> moveDialog = chosenMove.doMove (playerPokemon, enemyPokemon, true);
		if (enemyPokemon.curHP == 0) {
			doEnemyTurnNext = false;
			endBattleNext = true;
		}
		Dialog.S.ShowMessage(moveDialog);
	}

	// Update is called once per frame
	void Update () {
		if (switchOnKeyDownX == 1) {
			switchOnKeyDownX = 99;
			Menu_Pokemon.S.showPokemonMenu();
			doEnemyTurnNext = false;
		} else if (closeOnKeyDownX == 1) {
			closeOnKeyDownX = 99;
			closeBattleScreen ();
		} else if (!Menu.S.secondaryMenuOpen () && !Main.S.inDialog) {
			if (doEnemyTurnNext) {
				doEnemyTurnNext = false;
				doEnemyTurn();
			} else if (endBattleNext){
				endBattleNext = false;
				endBattle ();
			} else if (Input.GetKeyDown (KeyCode.Z)) {
				switch (selectedOption) {
				case (int) BattleScreenObject.MoveOption1:
					doEnemyTurnNext = true;
					doAttack(0);
					break;
				case (int) BattleScreenObject.MoveOption2:
					doEnemyTurnNext = true;
					doAttack(1);
					break;
				case (int) BattleScreenObject.MoveOption3:
					doEnemyTurnNext = true;
					doAttack(2);
					break;
				case (int) BattleScreenObject.MoveOption4:
					doEnemyTurnNext = true;
					doAttack(3);
					break;
				case (int) BattleScreenObject.FightOption:
					optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
						"TYPE/\n" + playerPokemon.moves[0].moveType;
					optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
						playerPokemon.moves[0].curPP.ToString() + "/" + playerPokemon.moves[0].maxPP.ToString();
					selectOption (BattleScreenObject.MoveOption1);
					break;
				case (int) BattleScreenObject.ItemOption:
					Menu_Item.S.showItemMenu ();
					break;
				case (int) BattleScreenObject.PkmnOption:
					Menu_Pokemon.S.showPokemonMenu ();
					break;
				case (int) BattleScreenObject.RunOption:
					if (isTrainerBattle) {
						string[] m = { "There's no running from a trainer battle!" };
						Dialog.S.ShowMessage (m);
					} else {
						if (runAwaySuccess (playerPokemon.speed, enemyPokemon.speed)) {
							string[] m = { "Got away safely!" };
							Dialog.S.ShowMessage (m);
							closeOnKeyDownX = 0;
						} else {
							string[] m = { "Couldn't get away!" };
							Dialog.S.ShowMessage (m);
							doEnemyTurnNext = true;
						}
					}
					break;
				}
			} else if (Input.GetKeyDown (KeyCode.X)) {
				switch (selectedOption) {
				case (int) BattleScreenObject.MoveOption1:
				case (int) BattleScreenObject.MoveOption2:
				case (int) BattleScreenObject.MoveOption3:
				case (int) BattleScreenObject.MoveOption4:
					refreshBattleInfo();
					selectOption (BattleScreenObject.FightOption);
					break;
				}
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				switch (selectedOption) {
				case (int) BattleScreenObject.MoveOption2:
					optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
						"TYPE/\n" + playerPokemon.moves[0].moveType;
					optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
						playerPokemon.moves[0].curPP.ToString() + "/" + playerPokemon.moves[0].maxPP.ToString();
					selectOption (BattleScreenObject.MoveOption1);
					break;
				case (int) BattleScreenObject.MoveOption3:
					optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
						"TYPE/\n" + playerPokemon.moves[1].moveType;
					optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
						playerPokemon.moves[1].curPP.ToString() + "/" + playerPokemon.moves[1].maxPP.ToString();
					selectOption (BattleScreenObject.MoveOption2);
					break;
				case (int) BattleScreenObject.MoveOption4:
					optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
						"TYPE/\n" + playerPokemon.moves[2].moveType;
					optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
						playerPokemon.moves[2].curPP.ToString() + "/" + playerPokemon.moves[2].maxPP.ToString();
					selectOption (BattleScreenObject.MoveOption3);
					break;
				case (int) BattleScreenObject.ItemOption:
					selectOption (BattleScreenObject.FightOption);
					break;
				case (int) BattleScreenObject.RunOption:
					selectOption (BattleScreenObject.PkmnOption);
					break;
				}
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				switch (selectedOption) {
				case (int) BattleScreenObject.MoveOption1:
					if (optionSlots [(int)BattleScreenObject.MoveOption2].GetComponent<GUIText> ().text != "-") {
						optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
							"TYPE/\n" + playerPokemon.moves[1].moveType;
						optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
							playerPokemon.moves[1].curPP.ToString() + "/" + playerPokemon.moves[1].maxPP.ToString();
						selectOption (BattleScreenObject.MoveOption2);
					}
					break;
				case (int) BattleScreenObject.MoveOption2:
					if (optionSlots [(int)BattleScreenObject.MoveOption3].GetComponent<GUIText> ().text != "-") {
						optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
							"TYPE/\n" + playerPokemon.moves[2].moveType;
						optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
							playerPokemon.moves[2].curPP.ToString() + "/" + playerPokemon.moves[2].maxPP.ToString();
						selectOption (BattleScreenObject.MoveOption3);
					}
					break;
				case (int) BattleScreenObject.MoveOption3:
					if (optionSlots [(int)BattleScreenObject.MoveOption4].GetComponent<GUIText> ().text != "-") {
						optionSlots [(int)BattleScreenObject.MoveType].GetComponent<GUIText> ().text =
							"TYPE/\n" + playerPokemon.moves[3].moveType;
						optionSlots [(int)BattleScreenObject.MovePP].GetComponent<GUIText> ().text =
							playerPokemon.moves[3].curPP.ToString() + "/" + playerPokemon.moves[3].maxPP.ToString();
						selectOption (BattleScreenObject.MoveOption4);
					}
					break;
				case (int) BattleScreenObject.FightOption:
					selectOption (BattleScreenObject.ItemOption);
					break;
				case (int) BattleScreenObject.PkmnOption:
					selectOption (BattleScreenObject.RunOption);
					break;
				}
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				switch (selectedOption) {
				case (int) BattleScreenObject.PkmnOption:
					selectOption (BattleScreenObject.FightOption);
					break;
				case (int) BattleScreenObject.RunOption:
					selectOption (BattleScreenObject.ItemOption);
					break;
				}
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				switch (selectedOption) {
				case (int) BattleScreenObject.FightOption:
					selectOption (BattleScreenObject.PkmnOption);
					break;
				case (int) BattleScreenObject.ItemOption:
					selectOption (BattleScreenObject.RunOption);
					break;
				}
			} else {
			}
		}
	}
}

