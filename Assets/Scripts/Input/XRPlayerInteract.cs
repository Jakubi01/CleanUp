using Item;
using UnityEngine;

namespace Input
{
    /**
     * RightHand -> Item Get
     * if input => action input => held item action
     */
    
    public class XRPlayerInteract : MonoBehaviour
    {
        [SerializeField] private Transform rightHandSocket;
        [SerializeField] private Transform eyeTransform;
        [SerializeField] private LayerMask itemLayerMask;

        private XRPlayerInput _input;
        private XRPlayerAction _action;
        
        private ItemBase _currentWatchingItem;
        private ItemBase _lastWatchingItem;
        private ItemBase _currentHeldItem;
        
        private bool _hasItem;

        private void Awake()
        {
            _input = GetComponent<XRPlayerInput>();
            _action = GetComponent<XRPlayerAction>();
        }
        
        private void Update()
        {
            FocusInteractable();
            TryInteract();
            TryDropItem();
        }

        private void FocusInteractable()
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

        private void TryInteract()
        {
            if (!_input || !_currentWatchingItem) return;
            
            var interactPressed = _input.ConsumeInteract();
            if (!interactPressed) return;

            _currentWatchingItem.Interact(this);
        }

        private void ClearCurrentItem()
        {
            if (!_lastWatchingItem) return;
            
            _lastWatchingItem.HideDescription(); 
            _lastWatchingItem = null;
            _currentWatchingItem = null;
        }

        public bool CanCollect(ItemBase itemBase)
        {
            if(!itemBase) return false;
            return !_hasItem;
        }

        public void CollectItem(ItemBase itemBase)
        {
            if (_hasItem || !itemBase) return;
            
            itemBase.gameObject.transform.SetParent(rightHandSocket);
            itemBase.gameObject.transform.localPosition = Vector3.zero;
            itemBase.gameObject.transform.localRotation = Quaternion.identity;
            
            if (itemBase.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
            if (itemBase.TryGetComponent<Collider>(out var col)) col.enabled = false;
            
            _currentHeldItem = itemBase;
            _hasItem = true;
            _action.SetItem(itemBase);
        }

        private void TryDropItem()
        {
            if (!_input) return;

            var pressed = _input.ConsumeDrop();
            if (!pressed) return;

            DropItem();
        }
        
        private void DropItem()
        {
            if (!_hasItem || !_currentHeldItem) return;
            
            _currentHeldItem.gameObject.transform.SetParent(null);

            if (_currentHeldItem.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = false;
                rb.AddForce(eyeTransform.forward * 2f, ForceMode.Impulse);
            }

            if (_currentHeldItem.TryGetComponent<Collider>(out var col)) col.enabled = true;

            _currentHeldItem = null;
            _hasItem = false;
        }
    }
}