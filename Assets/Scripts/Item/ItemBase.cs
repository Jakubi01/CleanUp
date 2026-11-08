using System;
using Input;
using Interface;
using UnityEngine;
using UserInterface;

namespace Item
{
    public class ItemBase : MonoBehaviour, IInteractable
    {
        [Header("Item Info")]
        public string itemName;
        public string description;
        
        [Header("Item Description UI")]
        [SerializeField] private ItemDescriptor itemDescriptor;

        private void Awake()
        {
            if (itemDescriptor)
            {
                itemDescriptor.Init(GetDescription());
            }
        }
        
        public string GetDescription()
        {
            return $"[{itemName}] {description}";
        }

        public virtual void Interact(XRPlayerInteract player)
        {
            if (!player.CanCollect(this)) return;
            
            player.CollectItem(this);
        }

        public void ShowDescription()
        {
            if (!itemDescriptor) return;
            
            itemDescriptor.Show();
        }

        public void HideDescription()
        {
            if (!itemDescriptor) return;
            
            itemDescriptor.Hide();
        }
    }
}