using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.System
{
    public class Mainloop : MonoBehaviour
    {
        private GameObject myCursor;
        private KeyEvent KeyboardInput; 

        void Start()
        {
            KeyboardInput = new KeyEvent(myCursor);
        }

        void Update()
        {
            KeyboardInput.KeyUpdate();
        }
    }
}
