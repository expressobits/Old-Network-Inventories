using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class ContainerUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] private SlotUI slotUIPrefab;
        [SerializeField] private Transform slotContentParent;
        [SerializeField] private Text weightText;
        private Container container;
        private List<SlotUI> slotUIs = new List<SlotUI>();
        
        public Container Container => container;

        public Action<SlotUI,PointerEventData> OnPointerDownSlotUI;

        public void SetContainer(Container container)
        {
            if (this.container) container.Slots.OnListChanged -= ListChanged;
            this.container = container;
            container.Slots.OnListChanged += ListChanged;
            Setup(container.Slots);
        }

        private void Setup(NetworkList<Slot> slots)
        {
            Clear();
            for (int i = 0; i < slots.Count; i++)
            {
                AddSlot(i, slots[i]);
            }
            weightText.text = Container.Weight.ToString("0.0");
        }

        private void Clear()
        {
            foreach (Transform i in slotContentParent.transform)
            {
                Destroy(i.gameObject);
            }
            slotUIs.Clear();
        }

        private void ListChanged(NetworkListEvent<Slot> changeEvent)
        {
            weightText.text = Container.Weight.ToString("0.0");
            switch (changeEvent.Type)
            {
                case NetworkListEvent<Slot>.EventType.Add:
                    AddSlot(changeEvent.Index, changeEvent.Value);
                    break;
                case NetworkListEvent<Slot>.EventType.Insert:
                    break;
                case NetworkListEvent<Slot>.EventType.Remove:
                    RemoveSlot(changeEvent.Index, changeEvent.Value);
                    break;
                case NetworkListEvent<Slot>.EventType.RemoveAt:
                    RemoveSlot(changeEvent.Index, changeEvent.Value);
                    break;
                case NetworkListEvent<Slot>.EventType.Value:
                    UpdateSlot(changeEvent.Index, changeEvent.Value);
                    break;
                case NetworkListEvent<Slot>.EventType.Clear:
                    //UpdateSlot(changeEvent.Index, changeEvent.Value);
                    break;
                case NetworkListEvent<Slot>.EventType.Full:
                    //UpdateSlot(changeEvent.Index, changeEvent.Value);
                    break;
            }
        }

        private void UpdateSlot(int index, Slot slot)
        {
            if (index >= slotUIs.Count) return;
            SlotUI slotUI = slotUIs[index];
            Item item = container.Items.GetItem(slot.itemId);
            slotUI.SetSlot(item, slot,this);

            //if (selectedSlot == slotUI) OnSelectedSlot?.Invoke(slotUI);
        }

        private void RemoveSlot(int index, Slot slot)
        {
            if (index >= slotUIs.Count) return;
            SlotUI slotUI = slotUIs[index];
            Destroy(slotUI.gameObject);
            slotUIs.RemoveAt(index);
        }

        private void AddSlot(int index, Slot slot)
        {
            // TODO change code to slot intern
            Item item = container.Items.GetItem(slot.itemId);
            SlotUI slotUI = Instantiate(slotUIPrefab, slotContentParent);
            slotUI.OnPointerDownEvent += PointerDownSlotUI;
            slotUI.OnDragEvent += DragSlotUI;
            //slotUI.OnDropEvent += DropSlotUI;
            slotUI.SetSlot(item, slot,this);
            slotUIs.Add(slotUI);
        }

        #region Selection
        

        private void PointerDownSlotUI(SlotUI slotUI, PointerEventData eventData)
        {
            OnPointerDownSlotUI?.Invoke(slotUI,eventData);
        }

        private void DragSlotUI(SlotUI slotUI, PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                DragSlotUI selectSlotUI = InventorySystemUI.DragSlotUI;
                int index = container.Slots.IndexOf(slotUI.Slot);
                selectSlotUI.SetInfos(slotUI.Item, index, Container, slotUI.Slot.Amount);
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                DragSlotUI selectSlotUI = InventorySystemUI.DragSlotUI;
                int index = container.Slots.IndexOf(slotUI.Slot);
                selectSlotUI.SetInfos(slotUI.Item, index, Container, 1);
            }
            else
            {
                DragSlotUI selectSlotUI = InventorySystemUI.DragSlotUI;
                int index = container.Slots.IndexOf(slotUI.Slot);
                selectSlotUI.SetInfos(slotUI.Item, index, Container, (byte)Mathf.Ceil(slotUI.Slot.Amount/2f));
            }

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DragSlotUI selectSlotUI = InventorySystemUI.DragSlotUI;
            selectSlotUI.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DragSlotUI selectSlot = InventorySystemUI.DragSlotUI;
            selectSlot.SetTransfer(selectSlot.Container != this.Container);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DragSlotUI selectSlot = InventorySystemUI.DragSlotUI;
            selectSlot.SetTransfer(false);
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventorySystemUI.Instance.Trade(Container);
        }
        #endregion
    }
}

