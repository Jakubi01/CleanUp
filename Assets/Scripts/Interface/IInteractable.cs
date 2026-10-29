using UnityEngine;

namespace Interface
{
    public interface IInteractable
    {
        string GetDescription();
        void Interact(GameObject player);
    }
}