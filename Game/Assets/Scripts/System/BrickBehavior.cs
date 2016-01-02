using UnityEngine;
using System.Collections;

public class BrickBehavior : MonoBehaviour {

	int numHits;
	int maxHits;

	// Use this for initialization
	void Start () {
		numHits = 0;
		maxHits = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (numHits >= maxHits) {
			Die ();
		}
	}

	public void Hit() {
		numHits++;
	}

	public void Die() {
		Destroy (this.gameObject);
	}
}
