using Input;
using Item.Tools;
using UnityEngine;

namespace Item.Garbage
{
    public class GarbageBase : ItemBase
    {
        public GarbageData data;               // 프리팹마다 에셋 하나 지정
        public ToolType requiredToolType = ToolType.None;
        public int scoreValue = 10;
        
        public override void Action()
        {
            
        }
    }
}