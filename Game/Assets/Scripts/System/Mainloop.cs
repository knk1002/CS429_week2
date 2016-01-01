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
        public GameObject myCursor;
        public GameObject ConnectButton;

        private KeyEvent KeyboardInput;

        NetworkClient ClientConnect;

        bool isConnected;

        void Start()
        {
            KeyboardInput = new KeyEvent(myCursor);
        }

        public void ConnectButtonClick()
        {
            ClientConnect = new NetworkClient();
            isConnected = ClientConnect.init();

            if (isConnected)
            {
                StartCoroutine(Listen());
            }
        }

        IEnumerator Listen()
        {
            while (true)
            {
                NetworkMessage message = null;
            }

            yield return null;

        }

        void Update()
        {
            KeyboardInput.KeyUpdate(Time.deltaTime);
        }
    }
}
