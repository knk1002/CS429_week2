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
        public int networkOrder;
        public GameObject myCursor;
		public GameObject opCursor;
        public GameObject ConnectButton;
		public GameObject Ball;

		public GameObject brick;
		public GameObject brick2;

		private GameBounds gameBounds;
        private KeyEvent KeyboardInput;
		private BallEvent BallLogic;
		private GameState gameState;

        private StageParser stageParser;
        private Stage nowStage;

        NetworkClient ClientConnect;

        bool isConnected;

        void Start()
        {
            networkOrder = -1;

			//Position the Objects
			myCursor.transform.position.Set (0f, -2.2f, 0f);
			opCursor.transform.position.Set (0f, 2.2f, 0f);
			Ball.transform.position.Set (0f, -1.8f, 0f);

            //Instantiate Game Logic
            gameBounds = new GameBounds(-4f, 4f, 2.4f, -2.4f);
            KeyboardInput = new KeyEvent(myCursor, gameBounds);
            BallLogic = new BallEvent(Ball, gameBounds);

            //Load Level
			gameState = GameState.Start;

            stageParser = StageParser.Instance;
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

        public void StartButtonClick()
        {
            nowStage = stageParser.getStage(1);
            LoadLevel();
            gameState = GameState.Playing;
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

                if (message != null)
                {
                    switch (message.Type)
                    {
                        case NetworkMessage.MessageType.PlayerOrder:
                            networkOrder = Convert.ToInt32(message.Arguments[0]);
                            break;
                        case NetworkMessage.MessageType.Connect:
                            nowStage = stageParser.getStage(1);
                            LoadLevel();
                            break;
                        case NetworkMessage.MessageType.Start:
                            gameState = GameState.Playing;
                            break;
                        default:
                            break;

                    }
                }

                yield return null;
            }
        }

        void FixedUpdate()
        {
			if (gameState == GameState.Start) {

			} else if (gameState == GameState.Playing) {
				KeyboardInput.KeyUpdate (Time.deltaTime);
				BallLogic.update (Time.deltaTime);
			} else if (gameState == GameState.GameOver) {

			} else if (gameState == GameState.Paused) {

			}
        }

        void Update()
        {
            FixedUpdate();
        }

		void LoadLevel() {
            for(int i = 0; i < nowStage.blockInfoList.Count; i++)
            {
                GameObject Temp = (GameObject)Instantiate(brick, new Vector3(nowStage.blockInfoList[i].point.x, nowStage.blockInfoList[i].point.y, 0), Quaternion.identity);

                switch (nowStage.blockInfoList[i].maxHit)
                {
                    case 1:
                        Temp.GetComponent<SpriteRenderer>().color = Color.red;
                        Temp.GetComponent<BrickBehavior>().maxHits = 1;
                        break;
                    case 2:
                        Temp.GetComponent<SpriteRenderer>().color = Color.yellow;
                        Temp.GetComponent<BrickBehavior>().maxHits = 2;
                        break;
                    case 3:
                        Temp.GetComponent<SpriteRenderer>().color = Color.blue;
                        Temp.GetComponent<BrickBehavior>().maxHits = 3;
                        break;
                    default:
                        break;

                }
            }
            if(isConnected)
            {
                ClientConnect.SendLoad(networkOrder);
            }
        }
    }
}
