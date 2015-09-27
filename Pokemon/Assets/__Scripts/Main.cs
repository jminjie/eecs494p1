using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	static public Main S;

	public bool inDialog = false;
	public bool paused = false;
	public bool pokemonMenuOpen = false;
	public bool[] pokemonDetailsOpen = {false, false};
	public bool playerMenuOpen = false;
	public bool itemMenuOpen = false;
	public bool selectionBoxOpen = false;
	public bool battleScreenOpen = false;



	// Use this for initialization
	void Awake() {
		Screen.SetResolution(10, 5, true);
		S = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (!inDialog && Input.GetKeyDown (KeyCode.Space)) {
			Menu.S.gameObject.SetActive(true);
			paused = true;
		}
	}
}
