using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	static public CameraFollow S;

	void Awake(){
		S = this;
	}

	void Update () {
		Vector3 destination = Player.S.transform.position;

		//Keep the same z value for the camera
		destination.z = Camera.main.transform.position.z;

		//Offset to ensure that you don't end up with half tiles on the edge of the screen
		destination.x += 0.5f;
		transform.position = destination;
	}
}
