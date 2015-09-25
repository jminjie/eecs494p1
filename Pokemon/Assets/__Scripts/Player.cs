using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction{
	down,
	left,
	up,
	right
}

public class Player : MonoBehaviour {
	
	public static Player S;
	public float moveSpeed;
	public float jumpSpeed;
	public int tileSize;
	
	public Sprite upSprite;
	public Sprite downSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;
	
	public SpriteRenderer sprend;
	
	public bool _________________;
	
	public RaycastHit hitInfo;
	
	public bool moving = false;
	public Vector3 targetPos;
	public Direction direction;
	public Vector3 moveVec;
	public Vector3 doorCurLoc;

	public List<Item> itemPack;
	public List<Pokemon> party;
	public int money;
	public string name;

	
	void Awake(){
		S = this;
	}

		void Start() {
			sprend = gameObject.GetComponent<SpriteRenderer> ();
			Item pokeball = new Item();
			pokeball.itemName = "POKEBALL";
			pokeball.itemQuantity = 1;
			pokeball.isUseableInBattle = true;
			pokeball.isUseableInWorld = false;
			pokeball.isTossable = true;
			itemPack.Add (pokeball);
			
			Item potion = new Item();
			potion.itemName = "POTION";
			potion.itemQuantity = 99;
			potion.isUseableInBattle = true;
			potion.isUseableInWorld = true;
			potion.isTossable = true;
			itemPack.Add (potion);
			
			Item superPotion = new Item();
			superPotion.itemName = "SUPER POTION";
			superPotion.itemQuantity = 3;
			superPotion.isUseableInBattle = true;
			superPotion.isUseableInWorld = true;
			superPotion.isTossable = true;
			itemPack.Add (superPotion);
			
			Item parcel = new Item();
			parcel.itemName = "OAK'S PARCEL";
			parcel.itemQuantity = 1;
			parcel.isUseableInBattle = false;
			parcel.isUseableInWorld = false;
			parcel.isTossable = false;
			itemPack.Add (parcel);
			
			Pokemon charles = new Pokemon();
			charles.pokemonName = "CHARMANDER";
			charles.pokemonNickname = "CHARLES";
			charles.maxHP = 30;
			charles.curHP = 30;
			charles.no = 4; // Format as 004
			charles.level = 5;
			charles.status = "OK";
			charles.attack = 11;
			charles.defense = 9;
			charles.speed = 10;
			charles.special = 8;
			charles.type1 = "FIRE";
			charles.type2 = "";
			charles.idno = 67475;
			charles.ot = "RED"; // original trainer
			charles.exppoints = 0;
			charles.moves = new List<Move>();
			charles.moves.Add (new Move ("SCRATCH", 35, 35));
			charles.moves.Add (new Move ("GROWL", 40, 40));
			party.Add (charles);
			
			Pokemon york = new Pokemon();
			york.pokemonName = "RATTATA";
			york.pokemonNickname = "YORK";
			york.maxHP = 25;
			york.curHP = 25;
			york.no = 19; // Format as 004
			york.level = 3;
			york.status = "OK";
			york.attack = 9;
			york.defense = 8;
			york.speed = 8;
			york.special = 5;
			york.type1 = "NORMAL";
			york.type2 = "";
			york.idno = 67342;
			york.ot = "RED"; // original trainer
			york.exppoints = 0;
			york.moves = new List<Move>();
			york.moves.Add (new Move ("SCRATCH", 35, 35));
			york.moves.Add (new Move ("TAIL WHIP", 40, 40));
			party.Add (york);
		}
		
		new public Rigidbody rigidbody{
			get {return gameObject.GetComponent<Rigidbody>();}
		}
		
		public Vector3 pos{
			get { return transform.position;}
			set { transform.position = value;}
		}
		
		void FixedUpdate(){
			
			if(gameObject.GetComponent<BoxCollider>().enabled == false && 
			   (transform.position.x == doorCurLoc.x + 1 || transform.position.x == doorCurLoc.x - 1 ||
			 transform.position.y == doorCurLoc.y + 1 || transform.position.y == doorCurLoc.y - 1)){
				gameObject.GetComponent<BoxCollider>().enabled = true;
			}
			
			if (!moving && !Main.S.inDialog && !Main.S.paused) {
				
				
				if(Input.GetKeyDown(KeyCode.Z)){
					CheckForAction();
				}
				
				if (Input.GetKey (KeyCode.RightArrow)) {
					moveVec = Vector3.right;
					direction = Direction.right;
					sprend.sprite = rightSprite;
					moving = true;
				}
				
				else if (Input.GetKey (KeyCode.LeftArrow)) {
					moveVec = Vector3.left;
					direction = Direction.left;
					sprend.sprite = leftSprite;
					moving = true;
				}
				
				else if (Input.GetKey (KeyCode.UpArrow)) {
					moveVec = Vector3.up;
					direction = Direction.up;
					sprend.sprite = upSprite;
					moving = true;
				}
				
				else if (Input.GetKey (KeyCode.DownArrow)) {
					moveVec = Vector3.down;
					direction = Direction.down;
					sprend.sprite = downSprite;
					moving = true; 
				} else {
					moveVec = Vector3.zero;
					moving = false;
				}
				
				if(Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] {"Immovable", "NPC", "Pickup", "Ledge","WaterTile"}) ) && 
				   gameObject.GetComponent<BoxCollider>().enabled == false){
					//just incase box collider is disabled when we need it to not be disabled
					gameObject.GetComponent<BoxCollider>().enabled = true;
				}
				
				if(Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] {"Immovable", "NPC", "Pickup", "Ledge","WaterTile"}) )){
					
					moveVec = Vector3.zero;
					moving = false;
				}
				
				if(Physics.Raycast(GetRay(), out hitInfo, 1f, GetLayerMask(new string[] {"Ledge"})) && direction == Direction.down && Input.GetKey(KeyCode.DownArrow)){
					//check if the ledge underneath is a "Ledge" layer, if so and the player presses down, shift the player down
					Vector3 underLedge = transform.position;
					underLedge.y = underLedge.y - 2f;
					moveVec = Vector3.down;
					moving = true;
					transform.position = Vector3.Lerp(transform.position, underLedge, Time.deltaTime * jumpSpeed);
				}
				
				targetPos = pos + moveVec;
				
			} 
			
			else{
				if((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime){
					pos = targetPos;
					moving = false;
					
				} else{
					pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
				}
				
				
			}
			
		}
		
		public void CheckForAction(){
			if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"NPC"}))) {
				print("hitINFO: " + hitInfo + "           ");
				NPC npc = hitInfo.collider.gameObject.GetComponent<NPC>();
				npc.FacePlayer(direction);
				npc.PlayDialog();
			}
			
			if (Physics.Raycast (GetRay (), out hitInfo, 1f, GetLayerMask (new string[] {"Pickup"}))) {
				//picking up item
				print("Pickup Item " + hitInfo + "           ");
				Item item = hitInfo.collider.gameObject.GetComponent<Item>();
				item.PlayDialog();
				item.enabled = false;
			}
			
		}
		
		Ray GetRay(){
			switch (direction){
			case Direction.down:
				return new Ray(pos, Vector3.down);
			case Direction.left:
				return new Ray(pos, Vector3.left);
			case Direction.right:
				return new Ray(pos, Vector3.right);
			case Direction.up:
				return new Ray(pos, Vector3.up);
			default:
				return new Ray();
				
			}		
		}
		
		int GetLayerMask(string[] layerNames){
			int layerMask = 0;
			
			foreach(string layer in layerNames){
				layerMask = layerMask | (1 << LayerMask.NameToLayer(layer));
			}
			
			return layerMask;
		}
		
		public void MoveThroughDoor(Vector3 doorLoc){
			if (doorLoc.z <= 0)
				doorLoc.z = transform.position.z;
			moving = false;
			moveVec = Vector3.zero;
			transform.position = doorLoc;
			doorCurLoc = transform.position;
			
		}
	}
