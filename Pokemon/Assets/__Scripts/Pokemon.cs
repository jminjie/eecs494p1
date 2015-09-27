using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pokemon : MonoBehaviour {

	public string pokemonName;
	public string pokemonNickname;
	public int maxHP;
	public int curHP;
	public int no; // pokemon number
	public int level;
	public string status;
	public int attack;
	public int defense;
	public int speed;
	public int special;
	public string type1;
	public string type2;
	public int idno;
	public string ot; // original trainer
	public int exppoints;
	public List<Move> moves;

	public Pokemon (string name){
		pokemonName = name;
		pokemonNickname = name;
	}

	public int expToLevel(int level){
		// For now assume every level takes 100 exp to pass
		return 100;
	}

}
