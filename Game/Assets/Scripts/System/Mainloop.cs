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
        public static int networkOrder;
        public GameObject myCursor;
		public GameObject opCursor;
        public GameObject ConnectButton;
		public GameObject StartButton;
		public GameObject Ball;
		public GameObject ClearedSprite;
		public GameObject GameOverSprite;

		public GameObject brick;

        bool opCursorMove;
        string opCursorDir;
        Vector3 opCursorPos;

		private GameBounds gameBounds;
		public KeyEvent KeyboardInput;
		public BallEvent BallLogic;
		private GameState gameState;

        private StageParser stageParser;
        private Stage nowStage;

        public static NetworkClient ClientConnect;
        List<NetworkMessage> messageQueue;

        public static bool isConnected;
		int life;
		public int numBricks;
        public static int totalNumBricks;
        List<GameObject> brickList;
		bool isSinglePlayer;

        void Awake()
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
                StartButton.SetActive(false);
                messageQueue = new List<NetworkMessage>();
                opCursorMove = false;
                opCursorDir = "";
                StartCoroutine(Listen());
                StartCoroutine(executeMessage());
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
                    messageQueue.Add(message);
                    Debug.Log("Got a message... " + message.ToString());
                }              

                yield return null;
            }
        }

        IEnumerator executeMessage()
        {
            while(true)
            {
                if (messageQueue.Count > 0)
                {
                    NetworkMessage message = messageQueue[0];
                    switch (message.Type)
                    {
                        case NetworkMessage.MessageType.PlayerOrder:
                            networkOrder = Convert.ToInt32(message.Arguments[0]);
                            BallLogic.order = networkOrder;
                            if (networkOrder == 2)
                            {
                                Ball.transform.position = new Vector3(0, 1.8f, 0);
                                BallLogic.velocityX *= -1;
                                BallLogic.velocityY *= -1;
                            }
                            break;
                        case NetworkMessage.MessageType.Connect:
                            nowStage = stageParser.getStage(1);
                            LoadLevel();
                            break;
                        case NetworkMessage.MessageType.Start:
                            gameState = GameState.Playing;
                            break;
                        case NetworkMessage.MessageType.MoveStart:
                            opCursorMove = true;
                            opCursorDir = (string)message.Arguments[0];
                            opCursor.transform.position = new Vector3(-Convert.ToInt32(message.Arguments[1]), -Convert.ToInt32(message.Arguments[2]), Convert.ToInt32(message.Arguments[3]));
                            break;
                        case NetworkMessage.MessageType.MoveEnd:
                            opCursorMove = false;
                            opCursorDir = "";
                            opCursor.transform.position = new Vector3(-Convert.ToInt32(message.Arguments[0]), -Convert.ToInt32(message.Arguments[1]), Convert.ToInt32(message.Arguments[2]));
                            break;
                        case NetworkMessage.MessageType.BallColide:
                            Ball.transform.position = new Vector3(-Convert.ToInt32(message.Arguments[0]), -Convert.ToInt32(message.Arguments[1]), Convert.ToInt32(message.Arguments[2]));
                            BallLogic.velocityX = -Convert.ToInt32(message.Arguments[3]);
                            BallLogic.velocityY = -Convert.ToInt32(message.Arguments[4]);
                            break;
                        case NetworkMessage.MessageType.Die:
                            life--;
                            Reset();
                            BallLogic.outOfBounds = false;
                            if (life <= 0)
                            {
                                gameState = GameState.GameOver;
                            }
                            else {
                                gameState = GameState.Playing;
                            }
                            break;
                        case NetworkMessage.MessageType.BrickColide:
                            if(brickList[Convert.ToInt32(message.Arguments[0])] != null)
                                brickList[Convert.ToInt32(message.Arguments[0])].GetComponent<BrickBehavior>().Hit();
                            break;
                        default:
                            break;
                    }

                    messageQueue.RemoveAt(0);
                }
                yield return null;
            }
        }

        void FixedUpdate()
        {
			//Game Loop
			if (gameState == GameState.Start) {

			} else if (gameState == GameState.Playing) {
                if(isConnected)
                {
                    if(opCursorMove)
                    {
                        if(opCursorDir == "left")
                        {
                            if (opCursor.transform.position.x + 0.4 < gameBounds.rightBound)
                            {
                                opCursor.transform.Translate(5 * Time.deltaTime, 0f, 0f);
                            }
                        }
                        else if(opCursorDir == "right")
                        {
                            if (opCursor.transform.position.x - 0.4 > gameBounds.leftBound)
                            {
                                opCursor.transform.Translate(-5 * Time.deltaTime, 0f, 0f);
                            }
                        }
                    }
                }
				KeyboardInput.KeyUpdate (Time.deltaTime);
				BallLogic.update (Time.deltaTime);
				if (BallLogic.outOfBounds == true) {
                    if(networkOrder == 1)
                    {
                        life--;
                        Reset();
                        BallLogic.outOfBounds = false;
                        if (life <= 0)
                        {
                            gameState = GameState.GameOver;
                        }
                        else {
                            gameState = GameState.Playing;
                        }
                        ClientConnect.SendDie(networkOrder);
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
            totalNumBricks = nowStage.blockInfoList.Count;
            brickList = new List<GameObject>();
            for(int i = 0; i < nowStage.blockInfoList.Count; i++)
            {
                GameObject Temp = (GameObject)Instantiate(brick, new Vector3(nowStage.blockInfoList[i].point.x, nowStage.blockInfoList[i].point.y, 0), Quaternion.identity);
                Temp.GetComponent<BrickBehavior>().num = i;

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
                brickList.Add(Temp);
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
            if(networkOrder == 2)
			    Ball.transform.position = new Vector3 (0f, 1.8f, 0f);
            else
                Ball.transform.position = new Vector3(0f, -1.8f, 0f);
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
