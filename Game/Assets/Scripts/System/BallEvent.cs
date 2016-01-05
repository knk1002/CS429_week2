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

		public bool outOfBounds;

		public BallEvent (GameObject input, GameBounds gb)
		{
			ball = input;
			gameBounds = gb;

			velocityX = 1;
			velocityY = 3;
			radius = ball.GetComponent<CircleCollider2D> ().radius;
			ballCollisionDetector = ball.GetComponent<BallCollisionDetector> ();

			outOfBounds = false;
		}

		public void update(float deltaTime) {
			
			//Check for GameObject Collisions
			if (ballCollisionDetector.state == State.Cursor) { //Colliding with a Cursor
				//Handle Cursor Collision
				velocityY *= -1;
				//Reset State
				ballCollisionDetector.state = State.None;
			} else if (ballCollisionDetector.state == State.Brick) { // Colliding with a Brick
				//Handle Brick Collision
				GameObject brick = ballCollisionDetector.collider.gameObject;
				Vector2 side = ballCollisionDetector.collider.contacts [0].normal;
				float angle = Vector2.Angle (side, Vector2.right);
				//Debug.Log (angle);
				if (angle >= 45 && angle <= 135) { // front or back
					velocityY *= -1;
				} else if (angle > 135) { // left
					velocityX *= -1;
				} else if (angle < 45) { //right
					velocityX *= -1;
				} 
				/*
				else {
					Vector2 newVelocity = 
						Vector2.Reflect (new Vector2 (velocityX, velocityY), side.normalized);
					velocityX = newVelocity.x;
					velocityY = newVelocity.y;
				}
				*/
				brick.GetComponent<BrickBehavior>().Hit();
				//Reset State
				ballCollisionDetector.state = State.None;
			}

			//Check for out of bounds
			if (ball.transform.position.x - radius < gameBounds.leftBound
			    && velocityX < 0) {
				velocityX *= -1;
			} else if (ball.transform.position.x + radius > gameBounds.rightBound
			           && velocityX > 0) {
				velocityX *= -1;
			} else if (ball.transform.position.y < gameBounds.lowerBound
			           && velocityY < 0) {
				outOfBounds = true;
			} else if (ball.transform.position.y > gameBounds.upperBound 
				&& velocityY > 0) {
				outOfBounds = true;
			}

			//Move the ball
			ball.transform.Translate(velocityX * deltaTime, velocityY * deltaTime, 0f);
		}

	}
}

