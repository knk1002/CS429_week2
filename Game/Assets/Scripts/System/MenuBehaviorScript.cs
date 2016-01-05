using UnityEngine;
using System.Collections;

public class MenuBehaviorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//KeyPress for Escape
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Debug.Log ("Quitting...");
			Application.Quit();
		}
	}
}
