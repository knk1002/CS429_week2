using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assets.Scripts.System
{
    public class Mainloop : MonoBehaviour
    {
        public GameObject myCursor;
		//public GameObject opCursor;
        public GameObject ConnectButton;
		public GameObject Ball;

		private GameBounds gameBounds;
        private KeyEvent KeyboardInput;
		private BallEvent BallLogic;

        NetworkClient ClientConnect;

        bool isConnected;

        void Start()
        {
			gameBounds = new GameBounds (-4, 4, 3, -3);
			KeyboardInput = new KeyEvent(myCursor, gameBounds);
			BallLogic = new BallEvent (Ball, gameBounds);
        }

        public void ConnectButtonClick()
        {
            ClientConnect = new NetworkClient();
            isConnected = ClientConnect.init();

            if (isConnected)
            {
                ConnectButton.SetActive(false);
                StartCoroutine(Listen());
                Debug.Log("Waiting for opponent");
            }
        }

        IEnumerator Listen()
        {
            while (true)
            {
                NetworkMessage message = null;
                JsonSerializer serializer = new JsonSerializer();

                if (ClientConnect.netStream.DataAvailable)
                {
                    message = serializer.Deserialize<NetworkMessage>(new JsonTextReader(ClientConnect.streamReader));
                    Debug.Log("Got a message... " + message.ToString());
                }

                yield return null;
            }
        }

        void FixedUpdate()
        {
            KeyboardInput.KeyUpdate(Time.deltaTime);
			BallLogic.update (Time.deltaTime);
        }
    }
}
