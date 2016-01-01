using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.System
{
    public class KeyEvent
    {
        
        public KeyEvent(GameObject input)
        {
            cursor = input;
            original_Speed = 1;
            speed = original_Speed;
        }

        public void KeyUpdate()
        {
            if (Input.GetKey("left"))
                MoveLeft();
            else if (Input.GetKey("right"))
                MoveRight();
        }

        public void MoveLeft()
        {
            cursor.transform.position.Set(cursor.transform.position.x - speed, cursor.transform.position.y, cursor.transform.position.z);
        }

        public void MoveRight()
        {
            cursor.transform.position.Set(cursor.transform.position.x + speed, cursor.transform.position.y, cursor.transform.position.z);
        }

        GameObject cursor;
        float original_Speed;
        float speed;

    }
}
