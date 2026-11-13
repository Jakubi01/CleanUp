using System.Globalization;
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
            if (descriptor != null)
            {
                descriptor.Init(GetDescription());
            }

            if (GameManager.Instance != null)
            {
                _gameManager = GameManager.Instance;
            }
        }
        
        public string GetDescription()
        {
            return trashCanName.ToUpper(CultureInfo.InvariantCulture);
        }

        public void Interact(XRPlayerInteract player)
        {
            if (player?.CurrentInteractable is null ||
                !player.CurrentInteractable.TryGetComponent(out GarbageBase garbage)) return;
            
            CompareType(garbage);
        }

        private void CompareType(GarbageBase garbage)
        {
            if (garbage is null) return;

            var typeMatch = garbageType == garbage.type;

            _gameManager ??= GameManager.Instance;
            
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
            descriptor?.Show();
        }

        public void HideDescription()
        {
            descriptor?.Hide();
        }
        
        public void Action() { }
    }
}
