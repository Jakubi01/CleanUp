using Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Item
{
    public class ItemBase : MonoBehaviour, IInteractable
    {
        [Header("Item Info")]
        public string itemName;
        public string description;

        public string GetDescription()
        {
            return $"[{itemName}] {description}";
        }

        public virtual void Interact(GameObject player)
        {
            
        }
    }
}