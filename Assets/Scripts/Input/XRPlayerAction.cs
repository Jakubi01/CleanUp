using Interface;
using Item.Garbage;
using UnityEngine;

namespace Input
{
    public class XRPlayerAction : MonoBehaviour
    {
        private XRPlayerInput _input;
        private IInteractable _interactable;

        private void Awake()
        {
            _input = GetComponent<XRPlayerInput>();
        }

        private void Update()
        {
            TryItemAction();
        }

        private void TryItemAction()
        {
            if (!_input || _interactable == null) return;

            if (_interactable is GarbageBase _) return;

            var pressed = _input.ConsumeAction();
            if (!pressed) return;
            
            _interactable.Action();
        }
        
        public void SetItem(IInteractable interactable)
        {
            _interactable = interactable;
        }

        public void ClearItem(IInteractable interactable)
        {
            if (ReferenceEquals(_interactable, interactable))
                _interactable = null;
        }
    }
}
