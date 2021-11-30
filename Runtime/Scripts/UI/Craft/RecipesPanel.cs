using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class RecipesPanel : MonoBehaviour
    {
        [SerializeField] private RecipeUI recipeUIPrefab;
        [SerializeField] private CategoryButton categoryButtonPrefab;
        [SerializeField] private RectTransform recipesContent;
        [SerializeField] private RectTransform buttonsPanel;
        private Category category;
        private Crafter crafter;
        private List<RecipeUI> recipeUIs = new List<RecipeUI>();
        private List<CategoryButton> categoryButtons = new List<CategoryButton>();
        private List<Recipe> recipes = new List<Recipe>();


        public void SetCrafter(Crafter crafter)
        {
            this.crafter = crafter;
            List<Category> categories = GetCategories(crafter.Recipes);
            UpdateButtons(categories);
            SetCategory(categories[0]);
        }

        public void UpdateStats()
        {
            UpdateRecipes(recipes);
        }

        #region Private Methods
        private List<Category> GetCategories(List<Recipe> recipes)
        {
            List<Category> categories = new List<Category>();
            for (int i = 0; i < recipes.Count; i++)
            {
                Category category = recipes[i].Product.Category;
                if (!categories.Contains(category))
                {
                    categories.Add(category);
                }
            }
            return categories;
        }

        private void SetCategory(Category category)
        {
            this.category = category;
            recipes.Clear();
            foreach (var recipe in crafter.Recipes)
            {
                if (recipe.Product.Category == category) recipes.Add(recipe);
            }
            UpdateRecipes(recipes);
        }

        private void UpdateButtons(List<Category> categories)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                if (categoryButtons.Count <= i)
                {
                    CategoryButton categoryButton = Instantiate(categoryButtonPrefab, buttonsPanel);
                    categoryButtons.Add(categoryButton);
                }
                else
                {
                    categoryButtons[i].gameObject.SetActive(true);
                }
                categoryButtons[i].SetCategory(categories[i], SetCategory);
            }
            for (int i = categories.Count; i < categoryButtons.Count; i++)
            {
                categoryButtons[i].gameObject.SetActive(false);
            }
        }

        private void UpdateRecipes(List<Recipe> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                if (recipeUIs.Count <= i)
                {
                    RecipeUI recipeUI = Instantiate(recipeUIPrefab, recipesContent);
                    recipeUIs.Add(recipeUI);
                }
                else
                {
                    recipeUIs[i].gameObject.SetActive(true);
                }
                recipeUIs[i].SetRecipe(crafter, recipes[i]);
            }
            for (int i = recipes.Count; i < recipeUIs.Count; i++)
            {
                recipeUIs[i].gameObject.SetActive(false);
            }
        }
        #endregion
    }
}

