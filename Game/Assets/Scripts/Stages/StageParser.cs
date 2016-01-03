﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

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
            Stage stage;
            string path = "Assets/Resources/Stages/" + num.ToString() + ".json";
            
            Debug.Assert(File.Exists(path));
            using (StreamReader file = File.OpenText(path))
            {
                stage = (Stage)serializer.Deserialize(file, typeof(Stage));
            }

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