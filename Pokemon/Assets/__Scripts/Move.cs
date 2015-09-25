using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public string moveName;
	public int maxPP;
	public int curPP;

	public Move(string name, int maxpp, int curpp){
		moveName = name;
		maxPP = maxpp;
		curPP = curpp;
	}

}
