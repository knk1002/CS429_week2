using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
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
            Stage stage = null;
            TextAsset targetFile = (TextAsset)Resources.Load(num.ToString());

            stage = JsonConvert.DeserializeObject<Stage>(targetFile.text);

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
