using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.System
{
    public class KeyEvent : MonoBehaviour
    {
        bool moveNow;
        
		public KeyEvent(GameObject input, GameBounds gb)
        {
            moveNow = false;
            cursor = input;
			gameBounds = gb;

            original_Speed = 1;
            speed = original_Speed;
        }

        public void KeyUpdate(float deltaTime)
        {
			Touch touch;
			if (Input.touchCount > 0) {
				touch = Input.touches [0];
				if (touch.position.x <= Screen.width / 3)
					MoveLeft (deltaTime);
				else if (touch.position.x >= 2 * Screen.width / 3)
					MoveRight (deltaTime);
			} else {
                if (Input.GetKey("left"))
                    MoveLeft(deltaTime);
                else if (Input.GetKey("right"))
                    MoveRight(deltaTime);
                else
                {
                    if (moveNow)
                    {
                        Mainloop.ClientConnect.SendMoveEnd(Mainloop.networkOrder, cursor.transform.position);
                        moveNow = false;
                    }
                }
			}
        }

        public void MoveLeft(float deltaTime)
        {
            if(!moveNow && Mainloop.isConnected)
            {
                Mainloop.ClientConnect.SendMoveStart("left", cursor.transform.position, Mainloop.networkOrder);
                moveNow = true;
            }
			if (cursor.transform.position.x - 0.4 > gameBounds.leftBound) {
				cursor.transform.Translate (-5 * deltaTime, 0f, 0f);
			}
        }

        public void MoveRight(float deltaTime)
        {
            if (!moveNow && Mainloop.isConnected)
            {
                Mainloop.ClientConnect.SendMoveStart("right", cursor.transform.position, Mainloop.networkOrder);
                moveNow = true;
            }
            if (cursor.transform.position.x + 0.4 < gameBounds.rightBound) {
				cursor.transform.Translate (5 * deltaTime, 0f, 0f);
			}
        }

        GameObject cursor;
		GameBounds gameBounds;
        float original_Speed;
        float speed;

    }
}
