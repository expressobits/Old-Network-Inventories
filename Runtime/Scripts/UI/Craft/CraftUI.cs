using System;
using UnityEngine;

namespace ExpressoBits.Inventory.UI
{
    public class CraftUI : MonoBehaviour
    {
        public Crafter Crafter => crafter;
        public bool IsOpen => recipesPanel.gameObject.activeInHierarchy;
        private Crafter crafter;

        public Action OnOpenCraftUI;
        public Action OnCloseCraftUI;

        [Header("Craft")]
        [SerializeField] private RecipesPanel recipesPanel;

        private void Awake()
        {
            Close();
            
        }

        private void ChangedContainer()
        {
            if(IsOpen) recipesPanel.UpdateRecipes(crafter,crafter.Recipes);
        }

        public void SetCrafter(Crafter crafter)
        {
            if(crafter && crafter != null) crafter.Container.OnChanged -= ChangedContainer;
            this.crafter = crafter;
            recipesPanel.UpdateRecipes(crafter,crafter.Recipes);
            crafter.Container.OnChanged += ChangedContainer;
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
    }
}

