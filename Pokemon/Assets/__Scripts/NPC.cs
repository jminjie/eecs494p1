using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public string[] speech;
	public Sprite upSprite;
	public Sprite downSprite;
	public Sprite leftSprite;
	public Sprite rightSprite;
	

	public bool moving = false;
	public Vector3 targetPos;
	public Direction direction;
	public Vector3 moveVec = Vector3.zero;
	public float moveSpeed = 3;
	bool justMovedDown = true;


	public SpriteRenderer sprend;

	// Use this for initialization
	void Start () {
		sprend = gameObject.GetComponent<SpriteRenderer> ();
	}

	public void PlayDialog(){
		print (speech);
		Dialog.S.gameObject.SetActive (true);
		Color maxAlpha = GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color;
		maxAlpha.a = 255;
		GameObject.Find ("DialogBackground").GetComponent<GUITexture> ().color = maxAlpha;
		Dialog.S.ShowMessage (speech);
	}

	public void moveUp(){
		moveVec = Vector3.up;
		direction = Direction.up;
		moving = true;
		sprend.sprite = upSprite;
	}

	public void moveDown() {
		moveVec = Vector3.down;
		direction = Direction.down;
		moving = true;
		sprend.sprite = downSprite;
	}

	public Vector3 pos {
		get { return transform.position;}
		set { transform.position = value;}
	}

	public void FacePlayer(Direction playerDir){
		switch(playerDir){
		case Direction.down:
			sprend.sprite = upSprite;
			break;
		case Direction.up:
			sprend.sprite = downSprite;
			break;
		case Direction.left:
			sprend.sprite = rightSprite;
			break;
		case Direction.right:
			sprend.sprite = leftSprite;
			break;
		}
	}
	void FixedUpdate(){
		if (!Main.S.inDialog && !Main.S.paused) {
			if (!moving) {
				if (Random.value < 0.01) {
					if (justMovedDown){
						moveUp ();
						justMovedDown = false;
					} else {
						moveDown ();
						justMovedDown = true;
					}
				}
				targetPos = pos + moveVec;
			} else {
				if ((targetPos - pos).magnitude < moveSpeed * Time.fixedDeltaTime) {
					pos = targetPos;
					moving = false;
				} else {
					pos += (targetPos - pos).normalized * moveSpeed * Time.fixedDeltaTime;
				}
			}
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
}
