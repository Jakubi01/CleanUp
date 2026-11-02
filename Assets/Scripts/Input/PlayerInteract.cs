using Item.Garbage;
using Item.Inventory;
using Item.Tools;
using UnityEngine;

namespace Input
{
    public class XRPlayerInteract : MonoBehaviour
    {
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
        }

        public void EquipTool(ToolBase toolBase)
        {
            
        }

        public bool CanCollect(GarbageBase garbageBase)
        {
            if(!garbageBase) return false;
            
            return _inventory && _inventory.CanCollect(garbageBase);
        }

        public void CollectGarbage(GarbageBase garbageBase)
        {
            if (!_inventory || !garbageBase) return;
            
            _inventory.Collect(garbageBase);
        }
    }
}