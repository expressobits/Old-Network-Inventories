using Unity.Netcode;
using UnityEngine;
using ExpressoBits.Interactions;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// An object that represents the item in the world
    /// </summary>
    [RequireComponent(typeof(ItemObject))]
    public class PickableItemObject : InteractableAction
    {
        public bool IsInvalid => invalidItem;
        public ItemObject ItemObject => itemObject;

        private ItemObject itemObject;
        private bool invalidItem;

        private void Awake()
        {
            itemObject = GetComponent<ItemObject>();
        }

        private void SetInvalid()
        {
            invalidItem = true;
        }

        public void Pick()
        {
            SetInvalid();
            NetworkObject.Despawn();
        }

        public override void Action(Interactor interactor)
        {
            if(IsInvalid) return;
            Pick();
        }

        public override string PreviewMessage => "to get " + ItemObject.Item.name;
    }
}
