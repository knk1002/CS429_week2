using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Stages
{
    public class Stage
    {
        public class BlockInfo
        {
            public enum BlockType
            {
                Normal,
            }

			public BlockType blockType;
            public Vector2 point;
            public Vector2 size;
            public int maxHit;
        }

        public List<BlockInfo> blockInfoList;
    }
}
