using Assets.Scripts.Manager;
using Input;
using Interface;
using Item.Garbage;
using Item.InventorySystem;
using UnityEngine;
using UserInterface;

namespace Item.TrashRecycle
{
    public class TrashCan : MonoBehaviour, IInteractable
    {
        [Header("TrashCan Settings")] 
        [SerializeField] private ItemDescriptor descriptor;
        [SerializeField] private string trashCanName;
        [SerializeField] private GarbageType garbageType;

        private GameManager _gameManager;

        private void Awake()
        {
            if (descriptor)
            {
                descriptor.Init(GetDescription());
            }

            if (GameManager.Instance)
            {
                _gameManager = GameManager.Instance;
            }
        }
        
        public string GetDescription()
        {
            return trashCanName.ToUpper();
        }

        public void Interact(XRPlayerInteract player)
        {
            if (!player || !player.CurrentInteractable ||
                !player.CurrentInteractable.TryGetComponent(out GarbageBase garbage)) return;
            
            CompareType(garbage);
        }

        private void CompareType(GarbageBase garbage)
        {
            if (!garbage) return;

            var typeMatch = garbageType == garbage.type;

            if (!_gameManager)
                _gameManager = GameManager.Instance;
            
            if (typeMatch)
            {
                _gameManager.AddGameScore(garbage.scoreValue);
            }
            else
            {
                _gameManager.RemoveGameScore(garbage.scoreValue);
            }
            
            _gameManager.ShowTypeMatchResult(typeMatch);
            garbage.Action();
        }

        public void ShowDescription()
        {
            if (!descriptor) return;
            descriptor.Show();
        }

        public void HideDescription()
        {
            if(!descriptor) return;
            descriptor.Hide();
        }
        
        public void Action() { }
    }
}
