using UnityEngine;
using System;

namespace Assets.Scripts.System
{
	public class BallEvent
	{
		GameObject ball;
		GameBounds gameBounds;

		float velocityX;
		float velocityY;
		float radius;
		BallCollisionDetector ballCollisionDetector;

		public BallEvent (GameObject input, GameBounds gb)
		{
			ball = input;
			gameBounds = gb;

			velocityX = 2;
			velocityY = 4;
			radius = ball.GetComponent<CircleCollider2D> ().radius;
			ballCollisionDetector = ball.GetComponent<BallCollisionDetector> ();
		}

		public void update(float deltaTime) {
			//Check for GameObject Collisions
			if (ballCollisionDetector.state == State.Cursor) { //Colliding with a Cursor
				velocityY *= -1;
			} else if (ballCollisionDetector.state == State.Brick) { // Colliding with a Brick
				
			}
			//Check for out of bounds
			if (ball.transform.position.x - radius < gameBounds.leftBound
			    || ball.transform.position.x + radius > gameBounds.rightBound) {
				velocityX *= -1;
			} else if (ball.transform.position.y - radius < gameBounds.lowerBound
			           || ball.transform.position.y + radius > gameBounds.upperBound) {
				velocityY *= -1;
			}
			//Move the ball
			ball.transform.Translate(velocityX * deltaTime, velocityY * deltaTime, 0f);
		}

	}
}

