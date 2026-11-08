using System;
using Item;
using Item.Garbage;
using Item.Tools;
using UnityEngine;

namespace Input
{
    /**
     * RightHand -> Garbage
     * LeftHand  -> Tool
     */
    
    public class XRPlayerInteract : MonoBehaviour
    {
        [SerializeField] private Transform rightHandSocket;
        [SerializeField] private Transform leftHandSocket;
        [SerializeField] private Transform eyeTransform;
        [SerializeField] private LayerMask itemLayerMask;

        private ItemBase _currentWatchingItem;
        private ItemBase _lastWatchingItem;
        
        private GarbageBase _currentHeldGarbage;
        private ToolBase _currentHeldTool;
        
        private bool _hasRightHand;
        private bool _hasLeftHand;

        private void Update()
        {
            if (!eyeTransform) return;
            
            if (!Physics.Raycast(eyeTransform.position, eyeTransform.forward, out var hit, 5f, itemLayerMask))
            {
                ClearCurrentItem();
                return;
            }

            if (!hit.collider.gameObject.TryGetComponent<ItemBase>(out var itemBase))
            {
                ClearCurrentItem();
                return;
            }

            if (_lastWatchingItem == itemBase) return;

            ClearCurrentItem();
            _currentWatchingItem = itemBase;
            _lastWatchingItem = itemBase;

            if (_currentWatchingItem)
            {
                _currentWatchingItem.ShowDescription();
            }
        }

        private void ClearCurrentItem()
        {
            if (!_lastWatchingItem) return;
            
            _lastWatchingItem.HideDescription(); 
            _lastWatchingItem = null;
            _currentWatchingItem = null;
        }

        public void EquipTool(ToolBase toolBase)
        {
            if (!toolBase || _hasLeftHand) return;
            
            toolBase.gameObject.transform.SetParent(leftHandSocket);
            toolBase.gameObject.transform.localPosition = Vector3.zero;
            toolBase.gameObject.transform.localRotation = Quaternion.identity;
            _currentHeldTool = toolBase;
            _hasLeftHand = true;
        }

        public bool CanCollect(GarbageBase garbageBase)
        {
            if(!garbageBase) return false;
            return !_hasRightHand;
        }

        public void CollectGarbage(GarbageBase garbageBase)
        {
            if (_hasRightHand || !garbageBase) return;
            
            garbageBase.gameObject.transform.SetParent(rightHandSocket);
            garbageBase.gameObject.transform.localPosition = Vector3.zero;
            garbageBase.gameObject.transform.localRotation = Quaternion.identity;
            _currentHeldGarbage = garbageBase;
            _hasRightHand = true;
        }
    }
}