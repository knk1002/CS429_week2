using UnityEngine;
using System;

namespace Assets.Scripts.System
{
	public class BallEvent
	{
		GameObject ball;
		GameBounds gameBounds;

		public float velocityX;
		public float velocityY;
        public int order;
		float radius;
		BallCollisionDetector ballCollisionDetector;
        bool changedState;

		public bool isSinglePlayer;
		public bool outOfBounds;

		public BallEvent (GameObject input, GameBounds gb)
		{
			ball = input;
			gameBounds = gb;
            changedState = true;

			velocityX = 1;
			velocityY = 3;
            order = -1;
			radius = ball.GetComponent<CircleCollider2D> ().radius;
			ballCollisionDetector = ball.GetComponent<BallCollisionDetector> ();

			outOfBounds = false;
		}

		public void update(float deltaTime) {
			
			//Check for GameObject Collisions
			if (ballCollisionDetector.state == State.Cursor) { //Colliding with a Cursor
				//Handle Cursor Collision
				velocityY *= -1;
                /*if(Mainloop.isConnected && order == 1)
                    Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));*/
				//Reset State
				ballCollisionDetector.state = State.None;
			} else if (ballCollisionDetector.state == State.Brick) { // Colliding with a Brick
				
			}

			//Check for out of bounds
			if (ball.transform.position.x - radius < gameBounds.leftBound
			    && velocityX < 0) {
                velocityX *= -1;
                if (Mainloop.isConnected && order == 1)
                    Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));
            } else if (ball.transform.position.x + radius > gameBounds.rightBound
			           && velocityX > 0) {
                velocityX *= -1;
                if (Mainloop.isConnected && order == 1)
                    Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));
            } else if (ball.transform.position.y < gameBounds.lowerBound
			           && velocityY < 0) {
                if (order == 1)
                {
                    outOfBounds = true;
                }

            } else if (ball.transform.position.y > gameBounds.upperBound 
				&& velocityY > 0) {
                if (!isSinglePlayer) {
                    if(order == 1)
                    {
                        outOfBounds = true;
                    }

                } else {
					velocityY *= -1;
				}
			}

			//Move the ball
			ball.transform.Translate(velocityX * deltaTime, velocityY * deltaTime, 0f);
		}

		public void Reset() {
            if(order == 1)
            {
                velocityX = 1;
                velocityY = 3;
            }
            else
            {
                velocityX = -1;
                velocityY = -3;
            }
			
		}

		public void onBrickCollision(Collision2D col) {
			//Handle Brick Collision
			Vector2 side = col.contacts [0].normal;
			float angle = Vector2.Angle (side, Vector2.right);
			Debug.Log (side);
			Debug.Log (angle);
			if (angle >= 30 && angle <= 150) { // front or back
				if (side.y > 0 && velocityY < 0) { //back
					velocityY *= -1;
                    if (Mainloop.isConnected && order == 1)
                        Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));
                } else if (side.y < 0 && velocityY > 0) { //front
					velocityY *= -1;
                    if (Mainloop.isConnected && order == 1)
                        Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));
                }

			} else if (angle > 150 && velocityX > 0) { // left
				velocityX *= -1;
                if (Mainloop.isConnected && order == 1)
                    Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));
            } else if (angle < 30 && velocityX < 0) { //right
				velocityX *= -1;
                if (Mainloop.isConnected && order == 1)
                    Mainloop.ClientConnect.SendBallColide(order, ball.transform.position, new Vector2(velocityX, velocityY));
            } 
			/*
				else {
					Vector2 newVelocity = 
						Vector2.Reflect (new Vector2 (velocityX, velocityY), side.normalized);
					velocityX = newVelocity.x;
					velocityY = newVelocity.y;
				}
				*/
            if(order == 1)
			    col.gameObject.GetComponent<BrickBehavior>().Hit();
		}
	}
}

