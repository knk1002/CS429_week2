using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickBehavior : MonoBehaviour {

	int numHits;
	public int maxHits;

	// Use this for initialization
	void Start () {
		numHits = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (numHits >= maxHits) {
			Die ();
		}
	}

	public void Hit() {
		numHits++;
		if (maxHits - numHits == 1) {
			GetComponent<SpriteRenderer> ().color = Color.red;
		}
		if (maxHits - numHits == 2) {
			GetComponent<SpriteRenderer> ().color = Color.yellow;
		}
		if (maxHits - numHits == 3) {
			GetComponent<SpriteRenderer> ().color = Color.blue;
		}
	}

	public void Die() {
		Destroy (this.gameObject);
	}
}
