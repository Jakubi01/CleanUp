using Interface;
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
        
        private IInteractable  _currentWatchingItem;
        private IInteractable  _lastWatchingItem;

        private bool _hasItem;

        public ItemBase CurrentInteractable { get; private set; }

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

            if (!hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                ClearCurrentItem();
                return;
            }

            if (_lastWatchingItem == interactable) return;
    
            _input?.ConsumeInteract();

            ClearCurrentItem();
            _currentWatchingItem = interactable;
            _lastWatchingItem = interactable;

            _currentWatchingItem.ShowDescription();
        }

        private void TryInteract()
        {
            if (!_input || _currentWatchingItem == null) return;
    
            var interactPressed = _input.ConsumeInteract();
            if (!interactPressed) return;

            _currentWatchingItem.Interact(this);
        }

        private void ClearCurrentItem()
        {
            switch (_lastWatchingItem)
            {
                case null:
                    return;
                case Object obj when obj:
                case not null:
                    _lastWatchingItem.HideDescription();
                    break;
            }

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
            
            CurrentInteractable = itemBase;
            CurrentInteractable.IsHeld = true;
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
            if (!_hasItem || !CurrentInteractable) return;
            
            CurrentInteractable.gameObject.transform.SetParent(null);

            if (CurrentInteractable.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = false;
                rb.AddForce(eyeTransform.forward * 2f, ForceMode.Impulse);
            }

            if (CurrentInteractable.TryGetComponent<Collider>(out var col)) col.enabled = true;

            CurrentInteractable.IsHeld = false;
            CurrentInteractable = null;
            _hasItem = false;
        }
    }
}
