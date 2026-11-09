using Interface;
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

            var pressed = _input.ConsumeAction();
            if (!pressed) return;

            _interactable.Action();
        }
        
        public void SetItem(IInteractable interactable)
        {
            if (interactable == null) return;
            
            _interactable = interactable;
        }
    }
}