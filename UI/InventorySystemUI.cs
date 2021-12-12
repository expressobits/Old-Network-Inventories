using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExpressoBits.Inventories.UI
{

    public class InventorySystemUI : MonoBehaviour
    {

        public static InventorySystemUI Instance => instance;
        public static DragSlotUI DragSlotUI => instance.dragSlot;
        private static InventorySystemUI instance; 

        [Header("Special Features")]
        [SerializeField] private DragSlotUI dragSlot;
        [SerializeField] private DropArea dropArea;
        [Header("Inventories")]
        [SerializeField] private ContainerUI playerContainer;
        [SerializeField] private ContainerUI lootContainer;

        private InventoryActionInteractor characterInventory;
        
        public Action OnInventoryOpen;
        public Action OnInventoryClose;
        public Action<SlotUI> OnPointerDown;
        public Action<InventoryActionInteractor> OnSetupContainerInteractor;

        public bool IsOpen => playerContainer.gameObject.activeSelf;

        #region Select Slot
        private bool hasSelected;
        private SlotUI selectedSlot;
        public SlotUI SelectedSlot => selectedSlot;
        public static Action OnClearSelected;
        public static Action<SlotUI> OnSelectedSlot;

        public void Select(SlotUI slotUI)
        {
            if (selectedSlot && selectedSlot != null) selectedSlot.UnSelect();
            selectedSlot = slotUI;
            OnSelectedSlot?.Invoke(slotUI);
            selectedSlot.Select();
            hasSelected = true;
        }

        private void ClearSelected()
        {
            if (selectedSlot && selectedSlot != null) selectedSlot.UnSelect();
            hasSelected = false;
            OnClearSelected?.Invoke();
            DragSlotUI.Clear();
        }

        public void DropSelection()
        {
            DragSlotUI selectSlot = DragSlotUI;
            if(selectSlot.Item != null && selectSlot.Container != null)
            {
                int index = selectSlot.Index;
                characterInventory.RequestDrop(index,selectSlot.Amount,selectSlot.Container);
            }
            selectSlot.Clear();
        }

        public void SetCharacterInventory(InventoryActionInteractor containerInteractor)
        {
            this.characterInventory = containerInteractor;
            playerContainer.SetContainer(containerInteractor.Container);
            containerInteractor.OnOwnerOpenContainer += OpenLootContainer;
            OnSetupContainerInteractor?.Invoke(containerInteractor);
        }

        public void PointerDownSlotUI(SlotUI slotUI,PointerEventData eventData)
        {
            Select(slotUI);
            OnPointerDown?.Invoke(slotUI);
        }
        #endregion

        #region Unity Events
        private void Awake()
        {
            if(instance != null) return;
            instance = this;
            dragSlot.gameObject.SetActive(false);
            dropArea.gameObject.SetActive(false);
            playerContainer.OnPointerDownSlotUI += PointerDownSlotUI;
            lootContainer.OnPointerDownSlotUI += PointerDownSlotUI;
        }

        private void Update()
        {
            if (hasSelected && selectedSlot == null)
            {
                ClearSelected();
            }
        }
        #endregion

        #region Open Close
        public void OpenPlayerInventory()
        {
            playerContainer.gameObject.SetActive(true);
            OnInventoryOpen?.Invoke();
            dropArea.gameObject.SetActive(true);
            ClearSelected();
        }

        public void OpenLootContainer(Container container)
        {
            lootContainer.gameObject.SetActive(true);
            lootContainer.SetContainer(container);
            OpenPlayerInventory();
        }

        public void CloseAllContainers()
        {
            playerContainer.gameObject.SetActive(false);

            // FIXME strong link to container...
            if(lootContainer.gameObject.activeInHierarchy)
            {
                if(lootContainer.Container.TryGetComponent(out StorageObject storageObject))
                {
                    characterInventory.RequestCloseStorage(storageObject);
                }
            }
            lootContainer.gameObject.SetActive(false);
            OnInventoryClose?.Invoke();
            dropArea.gameObject.SetActive(false);
            dragSlot.Clear();
            ClearSelected();
        }

        public static void Open()
        {
            instance.OpenPlayerInventory();
        }

        public static void OpenContainer(Container container)
        {
            instance.OpenLootContainer(container);
        }

        public static void CloseContainers()
        {
            instance.CloseAllContainers();
        }

        internal void Trade(Container container)
        {
            if(DragSlotUI.Container != null && DragSlotUI.Container != container)
            {
                characterInventory.RequestTrade(DragSlotUI.Index,DragSlotUI.Amount,DragSlotUI.Container,container);
            }
            DragSlotUI.Clear();
        }
        #endregion

    }
}