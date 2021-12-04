using Unity.Netcode;
using UnityEngine;
using ExpressoBits.Interactions;

namespace ExpressoBits.Inventory
{
    public class ItemObject : NetworkBehaviour, IItemObject, IInteractable, IPreviewInteract
    {
        public Item Item => item;

        [SerializeField] private Item item;
        public Transform Transform => transform;

        private bool invalidItem;

        public bool IsInvalid => invalidItem;

        public void SetInvalid()
        {
            invalidItem = true;
        }

        public string PreviewMessage()
        {
            return "to get "+ item.name;
        }

        public void Interact(Interactor interactor)
        {
            return;
        }
    }   
}
