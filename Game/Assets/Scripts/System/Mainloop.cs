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

            //Instantiate Game Logic
            gameBounds = new GameBounds(-4f, 4f, 2.4f, -2.4f);
            KeyboardInput = new KeyEvent(myCursor, gameBounds);
            BallLogic = new BallEvent(Ball, gameBounds);

            //Load Level
            stageParser = StageParser.Instance;
            nowStage = stageParser.getStage(1);
            LoadLevel();
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

            foreach (Assets.Scripts.Stages.Stage.BlockInfo b in nowStage.blockInfoList)
            {
                GameObject Temp = (GameObject)Instantiate(brick, new Vector3(b.point.x, b.point.y, 0), Quaternion.identity);

                switch (b.maxHit)
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
        }
    }
}
