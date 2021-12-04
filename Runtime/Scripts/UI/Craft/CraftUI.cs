using System;
using UnityEngine;

namespace ExpressoBits.Inventory.UI
{
    public class CraftUI : MonoBehaviour
    {
        public Crafter Crafter => crafter;
        public bool IsOpen => recipesPanel.gameObject.activeInHierarchy;
        private Crafter crafter;
        private Category category;

        public Action OnOpenCraftUI;
        public Action OnCloseCraftUI;

        [Header("Craft")]
        [SerializeField] private RecipesPanel recipesPanel;
        [SerializeField] private CraftsPanel craftsPanel;

        private void Awake()
        {
            Close();
        }

        public void SetCategory(Category category)
        {
            this.category = category;
        }

        private void ChangedContainer()
        {
            if(IsOpen) recipesPanel.UpdateStats();
        }

        public void SetCrafter(Crafter crafter)
        {
            if(crafter && crafter != null) crafter.Container.OnChanged -= ChangedContainer;
            this.crafter = crafter;
            recipesPanel.SetCrafter(crafter);
            crafter.Container.OnChanged += ChangedContainer;
            craftsPanel.SetCrafter(crafter);
        }

        public void Open()
        {
            recipesPanel.gameObject.SetActive(true);
            OnOpenCraftUI?.Invoke();
        }

        public void Close()
        {
            recipesPanel.gameObject.SetActive(false);
            OnCloseCraftUI?.Invoke();
        }

        public void Clear()
        {
            craftsPanel.Clear();
        }
    }
}

