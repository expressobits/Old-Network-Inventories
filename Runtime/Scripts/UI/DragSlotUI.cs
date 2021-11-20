using System;
using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class DragSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text itemAmountText;
        [SerializeField] private GameObject selectedLayer;
        [SerializeField] private Image dropIcon;
        [SerializeField] private Image transferIcon;

        public int Index => index;
        public Item Item => item;
        public Container Container => container;
        public byte Amount => amount;

        private Item item;
        private int index;
        private Container container;
        private byte amount;

        private void OnEnable()
        {
            SetDrop(false);
            SetTransfer(false);
        }

        public void SetInfos(Item item,int index, Container container, byte amount)
        {
            this.item = item;
            this.amount = amount;
            this.index = index;
            this.container = container;
            UpdateSlotData(item, amount);
            // SetDrop(false);
            // SetTransfer(false);
        }

        public void SetDrop(bool flagDrop)
        {
            dropIcon.gameObject.SetActive(flagDrop);
        }

        public void SetTransfer(bool flagDrop)
        {
            transferIcon.gameObject.SetActive(flagDrop);
        }

        private void UpdateSlotData(Item item, byte amount)
        {
            if (item != null)
            {
                gameObject.SetActive(true);
                iconImage.sprite = item.Icon;
                itemNameText.text = item.name;
                itemAmountText.text = amount.ToString();
            }
            else
            {
                Clear();
            }

        }

        public void Clear()
        {
            gameObject.SetActive(false);
            itemNameText.text = "Null";
            amount = 0;
            index = -1;
            container = null;
            item = null;
            SetDrop(false);
            SetTransfer(false);
        }

        // public virtual void UpdateSlot()
        // {

        //     // if (slot.itemId == 0)
        //     // {
        //     //     image.enabled = false;
        //     //     amountText.text = "";
        //     //     itemNameText.text = "";
        //     // }
        //     // else
        //     // {
        //     //     Item item = ItemDatabase.Instance.GetItem(slot.itemId);
        //     //     image.enabled = true;
        //     //     image.sprite = item.icon;
        //     //     amountText.text = slot.amount > 1 ? slot.amount + "" : "";
        //     //     itemNameText.text = item.itemName;
        //     // }
        //     if (slot.amount > 0)
        //     {
        //         gameObject.SetActive(true);
        //         //GetComponent<MouseTrack>().UpdateMousePosition();
        //     }
        //     else
        //     {
        //         gameObject.SetActive(false);
        //     }
        // }



    }
}

