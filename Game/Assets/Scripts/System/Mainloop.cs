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
		public GameObject StartButton;
		public GameObject Ball;
		public GameObject ClearedSprite;
		public GameObject GameOverSprite;

		public GameObject brick;

		private GameBounds gameBounds;
        private KeyEvent KeyboardInput;
		private BallEvent BallLogic;
		private GameState gameState;

        private StageParser stageParser;
        private Stage nowStage;

        NetworkClient ClientConnect;

        bool isConnected;
		int life;
		public int numBricks;
		bool isSinglePlayer;

        void Start()
        {
            networkOrder = -1;

			//Instantiate Game Logic
			gameBounds = new GameBounds(-3.2f, 3.2f, 2.4f, -2.4f);
			KeyboardInput = new KeyEvent(myCursor, gameBounds);
			BallLogic = new BallEvent(Ball, gameBounds);
			life = 3;

			//Position the Objects
			Reset();

            //Load Level
			gameState = GameState.Start;

            stageParser = StageParser.Instance;
        }

        public void ConnectButtonClick()
        {
            ClientConnect = new NetworkClient();
            isConnected = ClientConnect.init();
			isSinglePlayer = false;
			BallLogic.isSinglePlayer = isSinglePlayer;
			ClearBricks ();

            if (isConnected)
            {
                ConnectButton.SetActive(false);
                StartCoroutine(Listen());
                Debug.Log("Waiting for opponent");
            }

        }

        public void StartButtonClick()
        {
			isSinglePlayer = true;
			BallLogic.isSinglePlayer = isSinglePlayer;
			life = 3;
			Reset ();
			ClearBricks ();

			nowStage = stageParser.getStage(1);
            LoadLevel();
			ConnectButton.SetActive(false);
			StartButton.SetActive (false);
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
			//Game Loop
			if (gameState == GameState.Start) {

			} else if (gameState == GameState.Playing) {
				KeyboardInput.KeyUpdate (Time.deltaTime);
				BallLogic.update (Time.deltaTime);
				if (BallLogic.outOfBounds == true) {
					Debug.Log ("Out of Bounds!");
					life--;
					Reset ();
					BallLogic.outOfBounds = false;
					if (life <= 0) {
						gameState = GameState.GameOver;
					} else {
						gameState = GameState.Playing;
					}
				}
				if (numBricks <= 0) {
					gameState = GameState.Cleared;
				}
			} else if (gameState == GameState.GameOver) {
				Instantiate(GameOverSprite, Vector3.zero, Quaternion.identity);
				Debug.Log("Game Over!");
				ConnectButton.SetActive (true);
				StartButton.SetActive (true);
				gameState = GameState.Stopped;
			} else if (gameState == GameState.Paused) {
				//Pause();
				gameState = GameState.Playing;
			} else if (gameState == GameState.Cleared) {
				Instantiate(ClearedSprite, Vector3.zero, Quaternion.identity);
				/*
				LoadLevel ();
				Reset ();
				gameState = GameState.Playing;
				*/
				Debug.Log("Cleared!");
				ConnectButton.SetActive (true);
				StartButton.SetActive (true);
				gameState = GameState.Stopped;
			}

			//KeyPress for Escape
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.LoadLevel ("MainMenu");
			}
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
				numBricks++;
            }
            if(isConnected)
            {
                ClientConnect.SendLoad(networkOrder);
            }
        }

		void Reset() {
			myCursor.transform.position = new Vector3 (0f, -2.2f, 0f);
			if (isSinglePlayer) {
				opCursor.transform.position = new Vector3 (0f, 4f, 0f);
			} else {
				opCursor.transform.position = new Vector3 (0f, 2.2f, 0f);
			}
			Ball.transform.position = new Vector3 (0f, -1.8f, 0f);
			BallLogic.Reset ();
		}

		void ClearBricks() {
			GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
			foreach (GameObject brick in bricks) {
				Destroy (brick);
			}
		}
    }
}
