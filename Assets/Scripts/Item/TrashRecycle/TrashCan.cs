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
        public GarbageType garbageType;

        private GarbageBase _garbage;

        private void Awake()
        {
            if (descriptor)
            {
                descriptor.Init(GetDescription());
            }
        }
        
        public string GetDescription()
        {
            return trashCanName.ToUpper();
        }

        public void Interact(XRPlayerInteract player)
        {
            if (!player || !player.CurrentInteractable.TryGetComponent(out GarbageBase garbage)) return;

            _garbage = garbage;
        }

        public void Action()
        {
            if (!_garbage) return;

            _garbage.Action();
            GameManager.Instance.AddGameScore(_garbage.scoreValue);
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
    }
}