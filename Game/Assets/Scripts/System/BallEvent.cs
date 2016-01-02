using UnityEngine;
using System;

namespace Assets.Scripts.System
{
	public class BallEvent
	{
		GameObject ball;
		float velocityX;
		float velocityY;
		BallCollisionDetector ballCollisionDetector;

		public BallEvent (GameObject input)
		{
			ball = input;
			velocityX = 2;
			velocityY = 4;
			ballCollisionDetector = ball.GetComponent<BallCollisionDetector> ();
		}

		public void update(float deltaTime) {
			if (ballCollisionDetector.state == State.Cursor) { //Colliding with a Cursor
				velocityY *= -1;
			} else if (ballCollisionDetector.state == State.Brick) { // Colliding with a Brick
				
			}
			ball.transform.Translate(velocityX * deltaTime, velocityY * deltaTime, 0f);
		}

	}
}

