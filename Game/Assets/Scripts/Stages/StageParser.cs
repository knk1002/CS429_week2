using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Stages
{
    public class StageParser
    {
        private static StageParser instance;

        private StageParser()
        {
            serializer = new JsonSerializer();
        }

        public Stage getStage(int num)
        {
            
            string path = "Assets/Resources/Stages/" + num.ToString() + ".json";

            Stage stage;

            using (StreamReader file = File.OpenText(path))
            {
                stage = (Stage)serializer.Deserialize(file, typeof(Stage));
            }

			UnityEngine.Debug.Log (stage.blockInfoList.Count);

            return stage;
        }

        public static StageParser Instance
        {
            get
            {
                if (instance == null)
                    instance = new StageParser();

                return instance;
            }
        }

        JsonSerializer serializer;
    }
}
