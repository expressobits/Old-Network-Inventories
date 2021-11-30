using System;
using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class CategoryButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image background;
        [SerializeField] private Button button;
        private Category category;
        private Action<Category> OnSetCategory;

        private void Awake()
        {
            button.onClick.AddListener(delegate{ CallUpdateCategory(); });
        }

        private void CallUpdateCategory()
        {
            OnSetCategory?.Invoke(category);
        }

        public void SetCategory(Category category,Action<Category> onSetCategory)
        {
            this.category = category;
            icon.sprite = category.Icon;
            background.color = category.Color;
            OnSetCategory = onSetCategory;
        }
    }
}

