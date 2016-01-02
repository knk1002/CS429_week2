using UnityEngine;
using System.Collections;

namespace Assets.Scripts.System {

	public class BallCollisionDetector : MonoBehaviour {

		public State state;

		// Use this for initialization
		void Start () {
			state = State.None;
		}
	
		// Update is called once per frame
		void Update () {
			
		}

		void OnTriggerEnter2D (Collider2D col) {
			Debug.Log ("Called");
			if (col.gameObject.tag == "Cursor") {
				state = State.Cursor;
				//Do Something
				Debug.Log("Collision with Cursor!");
			} else if (col.gameObject.tag == "Brick") {
				state = State.Brick;
				//Do Something
				Debug.Log("Collision with Brick!");
			}
		}

		void OnTriggerExit2D (Collider2D col) {
			state = State.None;
			Debug.Log("Out of Collision");
		}
	}
}