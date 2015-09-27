using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour {
	public string moveName;
	public string moveType;
	public int maxPP;
	public int curPP;
	public string effectDialog;
	int movePower;
	bool isSpecial;
	public delegate void doEffect(Pokemon attacker, Pokemon defender);

	public Move(string name, string type, int maxpp, int curpp){
		moveName = name;
		moveType = type;
		maxPP = maxpp;
		curPP = curpp;
		effectDialog = "";
		movePower = 0;
		isSpecial = false;
	}

	public Move(string name, string type, string effect, int maxpp, int curpp){
		moveName = name;
		moveType = type;
		maxPP = maxpp;
		curPP = curpp;
		effectDialog = effect;
		movePower = 0;
		isSpecial = false;
	}

	public Move(string name, string type, int power, int maxpp, int curpp){
		moveName = name;
		moveType = type;
		maxPP = maxpp;
		curPP = curpp;
		effectDialog = "";
		movePower = power;
		isSpecial = false;
	}

	float typeMultiplier(string type){
		float multiplier = 1;
		if (moveType == "FIRE") {
			if (type == "WATER" || type == "DRAGON" || type == "ROCK" || type == "FIRE"){
				multiplier /= 2;
			}
			if (type == "GRASS" || type == "BUG" || type == "ICE"){
				multiplier *= 2;
			}
		} else if (moveType == "WATER") {
			if (type == "WATER" || type == "DRAGON" || type == "GRASS"){
				multiplier /= 2;
			}
			if (type == "FIRE" || type == "GROUND" || type == "ROCK"){
				multiplier *= 2;
			}
		} else if (moveType == "GRASS") {
			if (type == "FIRE" || type == "GRASS" || type == "POISON" || type == "FLYING" || type == "BUG" || type == "DRAGON"){
				multiplier /= 2;
			}
			if (type == "WATER" || type == "GROUND" || type == "ROCK"){
				multiplier *= 2;
			}
		} else if (moveType == "BUG") {
			if (type == "GHOST" || type == "FIGHTING" || type == "FLYING" || type == "FIRE"){
				multiplier /= 2;
			}
			if (type == "GRASS" || type == "BUG" || type == "ICE"){
				multiplier *= 2;
			}
		} else if (moveType == "FLYING") {
			if (type == "ELECTRIC" || type == "ROCK"){
				multiplier /= 2;
			}
			if (type == "GRASS" || type == "BUG" || type == "FIGHTING"){
				multiplier *= 2;
			}
		} else if (moveType == "ROCK") {
			if (type == "FIGHTING" || type == "GROUND"){
				multiplier /= 2;
			}
			if (type == "FLYING" || type == "FIRE" || type == "BUG" || type == "ICE"){
				multiplier *= 2;
			}
		} else if (moveType == "GHOST") {
			if (type == "NORMAL" || type == "PSYCHIC"){
				multiplier = 0;
			}
			if (type == "GHOST"){
				multiplier *= 2;
			}
		} else if (moveType == "DRAGON") {
		} else if (moveType == "POISON") {
		} else if (moveType == "GROUND") {
		} else if (moveType == "FIGHTING") {
		} else if (moveType == "PSYCHIC") {
		} else if (moveType == "ICE") {
		} else if (moveType == "ELECTRIC") {
			if (type == "ELECTRIC" || type == "GRASS" || type == "DRAGON"){
				multiplier /= 2;
			}
			if (type == "WATER" || type == "FLYING"){
				multiplier *= 2;
			}
			if (type == "GROUND"){
				multiplier = 0;
			}
		} else if (moveType == "NORMAL") {
			if (type == "ROCK"){
				multiplier /= 2;
			}
			if (type == "GHOST"){
				multiplier = 0;
			}
		}
		return multiplier;
	}

	public int damageCalc(Pokemon attacker, Pokemon defender, List<string> attackMessage){
		float effectiveMovePower = (float)movePower;
		if (Random.value < 0.1) {
			effectiveMovePower *= 1.5f;
			attackMessage.Add ("Critical hit!");
		}
		float typeMultiple = typeMultiplier(defender.type1) * typeMultiplier(defender.type2);
		if (typeMultiple > 1){
			attackMessage.Add("It's super effective!");
		} else if (typeMultiple < 1){
			attackMessage.Add("It's not very effective...");
		}
		effectiveMovePower *= typeMultiple;
		if (!isSpecial) {
			return attacker.attack + Mathf.CeilToInt(effectiveMovePower) - defender.defense;
		} else {
			return attacker.special + Mathf.CeilToInt(effectiveMovePower) - defender.special;
		}
	}

	public List<string> doMove(Pokemon attacker, Pokemon defender, bool playerIsAttacking){
		List<string> dialog = new List<string>();
		if (!playerIsAttacking) {
			dialog.Add("Enemy " + attacker.pokemonName + " used " + moveName + "!");
		} else {
			dialog.Add(attacker.pokemonNickname + " used " + moveName + "!");
		}
		// assume moves either have an effect or do damage
		if (effectDialog != "") {
			dialog.Add(applyMoveEffect (attacker, defender));
		} else {
			defender.curHP -= Mathf.Max(1, damageCalc(attacker, defender, dialog));
			if (defender.curHP < 0)
				defender.curHP = 0;
			BattleScreen.S.refreshBattleInfo();
		}
		return dialog;
	}

	string applyMoveEffect(Pokemon attacker, Pokemon defender){
		string effect;
		effect = defender.pokemonNickname + "'s " + effectDialog;
		if (moveName == "GROWL") {
			if (defender.attack > 2) {
				print (defender.pokemonName + "'s ATTACK fell by 2");
				defender.attack -= 2;
			} else {
				print (defender.pokemonName + "'s ATTACK won't go any lower!");
				effect = defender.pokemonNickname + "'s ATTACK won't go any lower!";
			}
		} else if (moveName == "TAIL WHIP") {
			if (defender.defense > 2){
				print (defender.pokemonName + "'s DEFENSE fell by 2");
				defender.defense -= 2;
			} else {
				print (defender.pokemonName + "'s DEFENSE won't go any lower!");
				effect = defender.pokemonNickname + "'s DEFENSE won't go any lower!";
			}
		}
		return effect;
	}

}
