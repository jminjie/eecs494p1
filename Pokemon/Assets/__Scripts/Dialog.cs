using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
	
	public static Dialog S;
	public int speechNum = 0;
	public int speechLength = 0;
	public string[] copySpeech;

	void Awake(){
		S = this;
	}
	
	// Use this for initialization
	void Start () {
		HideDialogBox ();
	}
	
	public void ShowMessage (string[] message){
		Player.S.GetComponent<BoxCollider> ().enabled = false;
		gameObject.SetActive (true);
		Main.S.inDialog = true;
		Color fullAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		fullAlpha.a = 255;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = fullAlpha;

		copySpeech = message;
		speechLength = message.Length;
		GameObject dialogBox = transform.Find("Text").gameObject;
		Text goText = dialogBox.GetComponent<Text>();
		goText.text = message[speechNum];
	}

	public void ShowMessage (List<string> message){
		Player.S.GetComponent<BoxCollider> ().enabled = false;
		gameObject.SetActive (true);
		Main.S.inDialog = true;
		Color fullAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		fullAlpha.a = 255;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = fullAlpha;
		speechLength = message.Count;
		copySpeech = new string[speechLength];
		for (int i = 0; i < message.Count && i < copySpeech.Length; ++i) {
			copySpeech[i] = message[i];
		}
		GameObject dialogBox = transform.Find("Text").gameObject;
		Text goText = dialogBox.GetComponent<Text>();
		goText.text = message[speechNum];
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Main.S.inDialog && (Input.GetKeyDown (KeyCode.X))) {
			// Internal logic so battle screen knows when to close
			if (Main.S.battleScreenOpen && BattleScreen.S.closeOnKeyDownX != 99){
				BattleScreen.S.closeOnKeyDownX++;
			} else if (Main.S.battleScreenOpen && BattleScreen.S.switchOnKeyDownX != 99){
				BattleScreen.S.switchOnKeyDownX++;
			}

			//
			if (speechNum == (speechLength - 1)) {
				HideDialogBox ();
				speechNum = 0;
				Player.S.GetComponent<BoxCollider> ().enabled = true;
			} else {
				++speechNum;
				ShowMessage (copySpeech);
			}
		}
	}
	
	void HideDialogBox(){
		Color noAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		noAlpha.a = 0;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = noAlpha;
		gameObject.SetActive (false);
		Main.S.inDialog = false;
	}
}
