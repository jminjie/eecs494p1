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
		
		Pokemon charles = new Pokemon ("CHARMANDER", "CHARLES", 39, 4, 7, 11, 9, 10, 8, "FIRE", "");
		charles.moves = new List<Move>();
		charles.moves.Add (Move.getMove("SCRATCH"));
		charles.moves.Add (Move.getMove("GROWL"));
		charles.moves.Add (Move.getMove("EMBER"));
		party.Add (charles);

		Pokemon york = new Pokemon ("RATTATA", "YORK", 25, 19, 3, 9, 8, 8, 5, "NORMAL", "");
		york.moves = new List<Move>();
		york.moves.Add (Move.getMove("TACKLE"));
		york.moves.Add (Move.getMove("TAIL WHIP"));
		party.Add (york);

		Pokemon james = new Pokemon ("BULBASAUR", "JAMES", 45, 1, 7, 9, 9, 9, 12, "GRASS", "POISON");
		james.moves = new List<Move>();
		james.moves.Add (Move.getMove("TACKLE"));
		james.moves.Add (Move.getMove("GROWL"));
		james.moves.Add (Move.getMove("VINE WHIP"));
		party.Add (james);

		Pokemon joe = new Pokemon ("SQUIRTLE", "JOE", 44, 4, 7, 9, 10, 8, 10, "WATER", "");
		joe.moves = new List<Move>();
		joe.moves.Add (Move.getMove("TACKLE"));
		joe.moves.Add (Move.getMove("TAIL WHIP"));
		joe.moves.Add (Move.getMove("WATER GUN"));
		party.Add (joe);
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
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				moveVec = Vector3.left;
				direction = Direction.left;
				sprend.sprite = leftSprite;
				moving = true;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				moveVec = Vector3.up;
				direction = Direction.up;
				sprend.sprite = upSprite;
				moving = true;
			} else if (Input.GetKey (KeyCode.DownArrow)) {
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
		} else {
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
