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
            enum BlockType
            {
                Normal,
            }

            public Vector2 Point
            {
                get
                {
                    return point;
                }
            }

            Vector2 point;
            Vector2 size;
        }

        List<BlockInfo> blockInfoList;
        public List<BlockInfo> BlockInfoList
        {
            get
            {
                return blockInfoList;
            }
        }
        
    }
}
