using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventories.UI
{
    public class SelectSlotActionsUI : MonoBehaviour
    {
        [SerializeField] private ContainerUI inventoryUI;
        [SerializeField] private Button dropButton;

        private void Start()
        {
            dropButton.interactable = false;
            InventorySystemUI.OnSelectedSlot += SelectSlot;
            dropButton.onClick.AddListener(delegate{ DropSelected(); });
        }

        private void SelectSlot(SlotUI slotUI)
        {
            if(slotUI)
            {
                dropButton.interactable = true;
            }
        }

        private void DropSelected()
        {
            //InventorySystemUI.Instance.DropSelected();
        }
    }
}

