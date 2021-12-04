using System;
using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class ItemInfoUI : MonoBehaviour
    {

        [SerializeField] private Text itemName;
        [SerializeField] private Text itemDescription;
        [SerializeField] private Text itemWeight;
        [SerializeField] private ContainerUI inventoryUI;
        [SerializeField] private InventorySystemUI inventorySystemUI;

        private void Awake()
        {
            InventorySystemUI.OnSelectedSlot += SelectedSlot;
            InventorySystemUI.OnClearSelected += Hide;
            inventorySystemUI.OnInventoryClose += Hide;
            Hide();
        }

        private void OnEnable()
        {
            
        }

        private void SelectedSlot(SlotUI slotUI)
        {
            Show(slotUI.Slot,slotUI.Item);
        }

        public void Show(Slot slot,Item item)
        {
            itemName.text = item.name;
            itemDescription.text = item.Description;
            float totalWeight = item.Weight * slot.amount;
            itemWeight.text = totalWeight.ToString();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

