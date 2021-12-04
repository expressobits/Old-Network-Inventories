using UnityEngine;
using UnityEngine.EventSystems;

namespace ExpressoBits.Inventory.UI
{
    public class DropArea : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IDragHandler
    {
        private InventorySystemUI inventorySystemUI;

        private void Awake()
        {
            inventorySystemUI = GetComponentInParent<InventorySystemUI>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // DragSlotUI selectSlot = InventorySystemUI.DragSlotUI;
            // selectSlot.SetDrop(true);
        }

        public void OnDrop(PointerEventData eventData)
        {
            inventorySystemUI.DropSelection();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // SelectSlotUI selectSlot = InventorySystemUI.SelectSlot;
            // if(selectSlot.Item != null && selectSlot.Container != null)
            // {
            //     int index = selectSlot.Index;
            //     selectSlot.Container.TryDropItem(index,selectSlot.Amount);
            // }
            // selectSlot.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DragSlotUI selectSlot = InventorySystemUI.DragSlotUI;
            selectSlot.SetDrop(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DragSlotUI selectSlot = InventorySystemUI.DragSlotUI;
            selectSlot.SetDrop(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // SelectSlotUI selectSlot = InventorySystemUI.SelectSlot;
            // if(selectSlot.Item != null && selectSlot.Container != null)
            // {
            //     int index = selectSlot.Index;
            //     selectSlot.Container.TryDropItem(index,selectSlot.Amount);
            // }
            // selectSlot.Clear();
        }
    }
}

