using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

	public float time;
	// Use this for initialization
	void Start () {
		time = 3f;
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0) {
			Destroy (this.gameObject);
		}
	}
}
