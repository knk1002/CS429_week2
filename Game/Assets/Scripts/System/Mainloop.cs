using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Assets.Scripts.Stages;

namespace Assets.Scripts.System
{
    public class Mainloop : MonoBehaviour
    {
        public GameObject myCursor;
		public GameObject opCursor;
        public GameObject ConnectButton;
		public GameObject Ball;

		public GameObject brick;
		public GameObject brick2;

		private GameBounds gameBounds;
        private KeyEvent KeyboardInput;
		private BallEvent BallLogic;

        private StageParser stageParser;
        private Stage nowStage;

        NetworkClient ClientConnect;

        bool isConnected;

        void Start()
        {
			//Position the Objects
			myCursor.transform.position.Set (0f, -2.2f, 0f);
			opCursor.transform.position.Set (0f, 2.2f, 0f);
			Ball.transform.position.Set (0f, -1.8f, 0f);

			//Load Level
			LoadLevel();

			//Instantiate Game Logic
			gameBounds = new GameBounds (-4f, 4f, 2.4f, -2.4f);
			KeyboardInput = new KeyEvent(myCursor, gameBounds);
			BallLogic = new BallEvent (Ball, gameBounds);
            stageParser = StageParser.Instance;
            nowStage = stageParser.getStage(1);
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

		void LoadLevel() {
			for (int x = 0; x < 12; x++) {
				for (int y = 0; y < 4; y++) {
					float xpos = (float)x / 2 - 3f;
					float ypos = (float)y / 4 - 0.5f;
					if (y % 2 == 0) {
						Instantiate (brick, new Vector3 (xpos, ypos, 0), Quaternion.identity);
					} else {
						Instantiate (brick2, new Vector3 (xpos, ypos, 0), Quaternion.identity);
					}
				}
			}
		}
    }
}
