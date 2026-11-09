using Item;
using UnityEngine;

namespace Input
{
    public class XRPlayerAction : MonoBehaviour
    {
        private XRPlayerInput _input;
            
        private ItemBase _currentHeldItem;

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
            if (!_input || !_currentHeldItem) return;

            var pressed = _input.ConsumeAction();
            if (!pressed) return;

            _currentHeldItem.Action();
        }
        
        public void SetItem(ItemBase itemBase)
        {
            if (!itemBase) return;
            
            _currentHeldItem = itemBase;
        }
    }
}