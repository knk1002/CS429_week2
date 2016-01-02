using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.System
{
    public class KeyEvent
    {
        
		public KeyEvent(GameObject input, GameBounds gb)
        {
            cursor = input;
			gameBounds = gb;

            original_Speed = 1;
            speed = original_Speed;
        }

        public void KeyUpdate(float deltaTime)
        {
            if (Input.GetKey("left"))
                MoveLeft(deltaTime);
            else if (Input.GetKey("right"))
                MoveRight(deltaTime);
        }

        public void MoveLeft(float deltaTime)
        {
			if (cursor.transform.position.x - 0.4 > gameBounds.leftBound) {
				cursor.transform.Translate (-5 * deltaTime, 0f, 0f);
			}
        }

        public void MoveRight(float deltaTime)
        {
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
