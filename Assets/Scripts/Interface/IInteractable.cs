using Input;

namespace Interface
{
    public interface IInteractable
    {
        string GetDescription();
        void Interact(XRPlayerInteract player);
        void Action();
        void ShowDescription();
        void HideDescription();
    }
}