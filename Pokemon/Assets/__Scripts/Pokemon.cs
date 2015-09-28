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
	public int battleAttack;
	public int battleDefense;
	public int battleSpeed;
	public int battleSpecial;

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

	public Pokemon(string name, string nickname, int hp, int n, int lv, int att, int def, int spee, int spec, string t1, string t2){
		pokemonName = name;
		pokemonNickname = nickname;
		maxHP = hp;
		curHP = hp;
		no = n;
		level = lv;
		attack = att;
		defense = def;
		speed = spee;
		special = spec;
		type1 = t1;
		type2 = t2;
		idno = Mathf.FloorToInt(Random.value * 100000);
		ot = Player.S.name;
		exppoints = 0;
	}

	public int expToLevel(int level){
		// For now assume every level takes 100 exp to pass
		return level*5;
	}

}
