using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class SlotUI : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler, IDragHandler//, IDropHandler
    {
        [Header("UI Components")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text itemAmountText;
        [SerializeField] private GameObject selectedLayer;
        [SerializeField] private Image background;

        [Header("Audios")]
        [SerializeField] private AudioClip enterAudioClip;
        [SerializeField] private AudioClip clickAudioClip;

        public Action<SlotUI,PointerEventData> OnPointerDownEvent;
        public Action<SlotUI,PointerEventData> OnDragEvent;
        public Action<SlotUI,PointerEventData> OnDropEvent;
        public Slot Slot => slot;
        //public int Index => index;

        private Item item;
        //private int index;
        private Slot slot;
        private ContainerUI containerUI;
        private AudioSource audioSource;

        public Item Item => item;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            UnSelect();
        }

        public void SetSlot(Item item,Slot slot,ContainerUI containerUI)
        {
            this.item = item;
            this.slot = slot;
            this.containerUI = containerUI;
            background.color = item.Category.Color;
            //this.index = index;
            UpdateSlotData(item,slot);
        }

        private void UpdateSlotData(Item item,Slot slot)
        {
            if(item != null)
            {
                iconImage.sprite = item.Icon;
                itemNameText.text = item.name;
                itemAmountText.text = slot.amount.ToString();
            }
            else
            {
                itemNameText.text = "Null";
            }
            
        }

        public void Select()
        {
            selectedLayer.SetActive(true);
        }

        public void UnSelect()
        {
            selectedLayer.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(this,eventData);
            audioSource.PlayOneShot(clickAudioClip);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this,eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            audioSource.PlayOneShot(enterAudioClip);
        }

        // public void OnDrop(PointerEventData eventData)
        // {
        //     OnDropEvent?.Invoke(this,eventData);
        // }
    }
}

