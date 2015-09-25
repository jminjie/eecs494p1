using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_Player : MonoBehaviour {
	
	public static Menu_Player S;
	
	void Awake () {
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		closePlayerMenu ();
	}
	
	public void showPlayerMenu() {
		Main.S.playerMenuOpen = true;
		gameObject.SetActive (true);
		
		GameObject name = transform.Find("Name").gameObject;
		GUIText nameText = name.GetComponent<GUIText>();
		nameText.text = "NAME/" + Player.S.name;
		
		GameObject money = transform.Find("Money").gameObject;
		GUIText monText = money.GetComponent<GUIText>();
		monText.text = "MONEY/" + Player.S.money.ToString();
		
		GameObject time = transform.Find("Time").gameObject;
		GUIText timeText = time.GetComponent<GUIText>();
		// This only works up to one hour
		timeText.text = "TIME/0:" + (Mathf.Floor(Time.time/60)).ToString("00");
	}
	
	public void closePlayerMenu(){
		Main.S.playerMenuOpen = false;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Main.S.playerMenuOpen && Input.GetKey (KeyCode.X)) {
			closePlayerMenu();
		}
	}
}
