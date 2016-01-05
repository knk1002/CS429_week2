using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.System {

	public class BrickBehavior : MonoBehaviour {

		GameObject MainLoop;
        public int num;
		int numHits;
		public int maxHits;

		// Use this for initialization
		void Start () {
			MainLoop = GameObject.Find ("Container");
			numHits = 0;
		}
	
		// Update is called once per frame
		void Update () {
			
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
			if (numHits >= maxHits) {
				Die ();
			}
            if(Assets.Scripts.System.Mainloop.isConnected)
            {
                if (Assets.Scripts.System.Mainloop.networkOrder == 1)
                    Assets.Scripts.System.Mainloop.ClientConnect.SendBrickColide(1, Assets.Scripts.System.Mainloop.totalNumBricks - num - 1);
            }
		}

		public void Die() {
			MainLoop.GetComponent<Mainloop> ().numBricks = MainLoop.GetComponent<Mainloop> ().numBricks - 1;
			Destroy (this.gameObject);
		}
	}
}