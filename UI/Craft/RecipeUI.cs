using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventories.UI
{
    public class RecipeUI : MonoBehaviour
    {
        private Recipe recipe;
        [SerializeField] private Image productIcon;
        [SerializeField] private Text timeToCraft;
        [SerializeField] private Text productName;
        [SerializeField] private Button craftButton;
        [SerializeField] private List<RequiredItemUI> requiredItemsUI;
        private Crafter crafter;

        private void Awake()
        {
            craftButton.onClick.AddListener(delegate { Craft(); });
        }

        private void OnEnable()
        {
            if(recipe && recipe != null) CheckValidRecipe();
        }

        public void SetRecipe(Crafter crafter,Recipe recipe)
        {
            this.crafter = crafter;
            this.recipe = recipe;
            productIcon.sprite = recipe.Product.Icon;
            TimeSpan time = TimeSpan.FromSeconds(recipe.TimeForCraft);
            string str = time.ToString(@"m\:ss");
            productName.text = recipe.Product.Name;
            timeToCraft.text = str;
            for (int i = 0; i < requiredItemsUI.Count; i++)
            {
                if(recipe.RequiredItems.Count <= i)
                {
                    requiredItemsUI[i].gameObject.SetActive(false);
                }
                else
                {
                    requiredItemsUI[i].gameObject.SetActive(true);
                    requiredItemsUI[i].Set(recipe.RequiredItems[i]);
                }
            }
            CheckValidRecipe();
        }

        public void CheckValidRecipe()
        {
            craftButton.interactable = crafter.CanCraft(recipe);
        }

        private void Craft()
        {
            crafter.CallCraft(recipe);
        }
    }
}

