using UnityEngine;
using System.Collections;

public enum TileType{
	/// <summary>
	/// These tiles cannot be moved into and cannot be interacted with
	/// </summary>
	immovable = 'I',
	/// <summary>
	/// A door or entrance to a cave
	/// </summary>
	door = 'D',
	/// <summary>
	/// A sign that can be read
	/// </summary>
	sign = 'S',
	/// <summary>
	/// A water tile
	/// </summary>
	water = 'W',
	/// <summary>
	/// Long grass that can trigger battles
	/// </summary>
	longGrass = 'G',
	/// <summary>
	/// Tile contains a ledge that can be jumped over
	/// </summary>
	ledge = 'L',
	/// <summary>
	/// Tile can be moved through freely
	/// </summary>
	open = 'O',
	/// <summary>
	/// Tile contains tree that can be cut
	/// </summary>
	tree = 'T',

	pickup = 'P'
}

public class Tile : MonoBehaviour {
    static Sprite[]         spriteArray;

    public Texture2D        spriteTexture;
	public int				x, y;
	public int				tileNum;
	private BoxCollider		bc;
    private Material        mat;

    private SpriteRenderer  sprend;

	void Awake() {
        if (spriteArray == null) {
            spriteArray = Resources.LoadAll<Sprite>(spriteTexture.name);
        }

		bc = GetComponent<BoxCollider>();

        sprend = GetComponent<SpriteRenderer>();
	}

	public void SetTile(int eX, int eY, int eTileNum = -1) {
		if (x == eX && y == eY) return; // Don't move this if you don't have to. - JB

		x = eX;
		y = eY;
		transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3")+"x"+y.ToString("D3");

		tileNum = eTileNum;
		if (tileNum == -1 && ShowMapOnCamera.S != null) {
			tileNum = ShowMapOnCamera.MAP[x,y];
			if (tileNum == 0) {
				ShowMapOnCamera.PushTile(this);
			}
		}

        sprend.sprite = spriteArray[tileNum];

		if (ShowMapOnCamera.S != null) SetCollider();

        gameObject.SetActive(true);
		if (ShowMapOnCamera.S != null) {
			if (ShowMapOnCamera.MAP_TILES[x,y] != null) {
				if (ShowMapOnCamera.MAP_TILES[x,y] != this) {
					ShowMapOnCamera.PushTile( ShowMapOnCamera.MAP_TILES[x,y] );
				}
			} else {
				ShowMapOnCamera.MAP_TILES[x,y] = this;
			}
		}
	}


	// Arrange the collider for this tile
	void SetCollider() {
        
        // Collider info from collisionData
        bc.enabled = true;
        TileType type = (TileType)ShowMapOnCamera.S.collisionS[tileNum];

		bc.center = Vector3.zero;
		bc.size = Vector3.one;

        switch (type) {
		case TileType.pickup: 
			//bc.gameObject.tag = "item";
			bc.gameObject.layer = LayerMask.NameToLayer("Pickup");
			bc.isTrigger = true;
			break;
        case TileType.immovable: 
			//bc.gameObject.tag = "Immovable";
			bc.gameObject.layer = LayerMask.NameToLayer("Immovable");
			bc.isTrigger = true;
			break;
        case TileType.door: 
			//bc.gameObject.tag = "Door";
			bc.gameObject.layer = LayerMask.NameToLayer("Default");
            break;
        case TileType.sign:
			//bc.gameObject.tag = "Sign";
			bc.gameObject.layer = LayerMask.NameToLayer("Immovable");
            break;
        case TileType.water: 
			//bc.gameObject.tag = "WaterTile";
			bc.gameObject.layer = LayerMask.NameToLayer("WaterTile");
			break;
        case TileType.longGrass: 
			//bc.gameObject.tag = "Grass";
			bc.gameObject.layer = LayerMask.NameToLayer("Grass");
			break;
		case TileType.ledge: 
			//bc.gameObject.tag = "Ledge";
			bc.gameObject.layer = LayerMask.NameToLayer("Ledge");
			bc.isTrigger = true;
			break;
		case TileType.open: 
			//bc.gameObject.tag = "Open";
			bc.gameObject.layer = LayerMask.NameToLayer("Open");
			break;
		case TileType.tree: 
			//bc.gameObject.tag = "Tree";
			bc.gameObject.layer = LayerMask.NameToLayer("Immovable");
			break;
        default: // Anything else: _, |, etc.
            bc.enabled = false;
			bc.gameObject.layer = LayerMask.NameToLayer("Default");
            break;
        }

	}

}