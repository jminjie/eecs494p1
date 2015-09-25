using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public string itemName;
	public int itemQuantity;
	public bool isUseableInBattle;
	public bool isUseableInWorld;
	public bool isTossable;

	public string[] speech;

	public void PlayDialog(){
		print (speech[0]);
		Dialog.S.gameObject.SetActive (true);
		Color noAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		noAlpha.a = 255;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = noAlpha;
		Dialog.S.ShowMessage (speech);
		gameObject.SetActive (false);
	}	

	public void AddToMenu(){
		//add item to menu

	}
}
