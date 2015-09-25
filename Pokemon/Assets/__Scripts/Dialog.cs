using UnityEngine;
using System.Collections;
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
		Main.S.inDialog = true;
		//you have to disable the raycast while in dialog
		Player.S.GetComponent<BoxCollider> ().enabled = false;
		copySpeech = message;
		speechLength = message.Length;
		GameObject dialogBox = transform.Find("Text").gameObject;
		Text goText = dialogBox.GetComponent<Text>();
		goText.text = message[speechNum];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (speechNum == (speechLength - 1) && Main.S.inDialog && (Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.Z)) ) {
			HideDialogBox();
			speechNum = 0;		
			//enabling raycast again
			Player.S.GetComponent<BoxCollider> ().enabled = true;
			
		}
		
		if (speechNum != speechLength && Main.S.inDialog && (Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.Z))) {
			++speechNum;
			ShowMessage(copySpeech);
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
