using UnityEngine;
using System.Collections;

public class ButtonEventManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayButtonClick() {
		Debug.Log ("Loading MainScene");
		Application.LoadLevel ("MainScene");
	}

	public void ExitButtonClick() {
		Debug.Log ("Quitting...");
		Application.Quit ();
	}
}
