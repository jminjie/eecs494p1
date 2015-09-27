using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Menu_Item : MonoBehaviour {

	public static Menu_Item S;

	public List<GameObject> itemSlots;
	public int selectedItem;
	int numSlots = 9;
	int maxItemNameChar = 12;
	public bool switching = false;

	void Awake() {
		S = this;
	}

	// Use this for initialization
	void Start () {
		bool justOpened = true;
		selectedItem = 0;
		closeItemMenu ();

		foreach (Transform child in transform) {
			itemSlots.Add(child.gameObject);
		}
		
		itemSlots = itemSlots.OrderByDescending (m => m.transform.transform.position.y).ToList();
		
		foreach (GameObject go in itemSlots) {
			GUIText itemText = go.GetComponent<GUIText>();
			if(justOpened) itemText.color = Color.red;
			justOpened = false;
		}
	}

	public void closeItemMenu() {
		Main.S.itemMenuOpen = false;
		gameObject.SetActive(false);
		//Color noAlpha = GameObject.Find ("FullscreenBackground").GetComponent<GUITexture> ().color;
		//noAlpha.a = 0;
		//GameObject.Find ("FullscreenBackground").GetComponent<GUITexture> ().color = noAlpha;
	}

	public void showItemMenu() {
		//Color allAlpha = GameObject.Find ("FullscreenBackground").GetComponent<GUITexture> ().color;
		//allAlpha.a = 255;
		//GameObject.Find ("FullscreenBackground").GetComponent<GUITexture> ().color = allAlpha;
		Main.S.itemMenuOpen = true;
		gameObject.SetActive (true);

		// Clear the item menu
		for (int j = 0; j < numSlots; ++j) {
			GameObject itemSlot = transform.Find ("ItemSlot" + (j + 1).ToString ()).gameObject;
			GUIText itemSlotText = itemSlot.GetComponent<GUIText> ();
			itemSlotText.text = "";
		}

		int i;
		for (i = 0; i < numSlots - 1 && i < Player.S.itemPack.Count; ++i) {
			GameObject itemSlot = transform.Find("ItemSlot" + (i+1).ToString()).gameObject;
			GUIText itemSlotText = itemSlot.GetComponent<GUIText>();
			Item curItem = Player.S.itemPack[i];
			itemSlotText.text = appendAmount(curItem.itemName.ToUpper(), curItem.itemQuantity);
		}
		GameObject cancelSlot = transform.Find("ItemSlot" + (i+1).ToString()).gameObject;
		GUIText cancelSlotText = cancelSlot.GetComponent<GUIText>();
		cancelSlotText.text = "CANCEL";
	}

	string appendAmount(string name, int quantity) {
		// To show amount, items with quantity > 1 must have name.Length <= 9
		if (quantity == 1) {
			return name;
		} else {
			if (name.Length > maxItemNameChar){
				name = name.Substring(0, maxItemNameChar).TrimEnd();
			}
			while (name.Length < maxItemNameChar + 1){
				name += " ";
			}
			name += "x" + quantity.ToString();
			return name;
		}
	}

	public void MoveDownItemMenu(){
		itemSlots[selectedItem].GetComponent<GUIText> ().color = Color.black;
		// Not count - 1 because we should be able to select the CANCEL button
		if (selectedItem < Player.S.itemPack.Count) {
			++selectedItem;
		} else {
			// Need to scroll down through items but for now, a max of 6 items
		}
		itemSlots [selectedItem].GetComponent<GUIText> ().color = Color.red;
	}
	
	public void MoveUpItemMenu() {
		itemSlots[selectedItem].GetComponent<GUIText> ().color = Color.black;
		if (selectedItem != 0) {
			--selectedItem;
		} else {
			// Need to scroll up through items but for now, a max of 6 items
		}
		itemSlots [selectedItem].GetComponent<GUIText> ().color = Color.red;
	}

	public void useSelectedItem(){

		Player.S.itemPack[selectedItem].itemQuantity--;
		string[] m = {Player.S.name + " used " + Player.S.itemPack [selectedItem].itemName + "!"};
		Dialog.S.ShowMessage (m);
		if (Player.S.itemPack[selectedItem].itemQuantity == 0) {
			Player.S.itemPack.RemoveAt(selectedItem);
		}
		// Refresh the item display if its open
		if (Main.S.itemMenuOpen) {
			Menu_Item.S.closeItemMenu();
			Menu_Item.S.showItemMenu();
		}

	}

	public void tossSelectedItem(){
		print ("Tossed " + Player.S.itemPack [selectedItem].itemName);
		Player.S.itemPack.RemoveAt(selectedItem);
		// Refresh the item display if its open
		if (Main.S.itemMenuOpen) {
			Menu_Item.S.closeItemMenu();
			Menu_Item.S.showItemMenu();
		}
	}

	// Update is called once per frame
	void Update () {
		if (!Main.S.selectionBoxOpen && !Main.S.inDialog) {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MoveDownItemMenu ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MoveUpItemMenu ();
			} else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Z)) {
				if (itemSlots [selectedItem].GetComponent<GUIText> ().text == "CANCEL") {
					closeItemMenu ();
				} else {
					// Open up USE/TOSS option box
					SelectionBox.S.setOptions("USE", "TOSS");
					SelectionBox.S.openSelectionBox ();
				}
			}
		}
	}
}
