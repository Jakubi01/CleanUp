using Item.Tools;
using UnityEngine;

namespace Item.Garbage
{
    public enum GarbageType { General, Recycle, Hazardous }
    
    public class GarbageBase : ItemBase
    {
        public GarbageType garbageType;
        public ToolType requiredToolType = ToolType.None;
        public int scoreValue = 10;

        public override void Interact(GameObject player)
        {
            //if (!player.CanCollect(this)) return;
            
            //player.CollectGarbage(this);
            Destroy(gameObject);
        }
    }
}