using UnityEngine;

namespace Item.Tools
{
    public enum ToolType { None, Tongs, Broom, Gloves }
    
    public class ToolBase : ItemBase
    {
        public ToolType toolType;
        public Transform handSocketTransform;

        public override void Interact(GameObject player)
        {
            // go.EquipTool(this);
        }
    }
}