using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventory.UI
{
    public class RecipesPanel : MonoBehaviour
    {
        [SerializeField] private RecipeUI recipeUIPrefab;
        [SerializeField] private GameObject content;

        private List<RecipeUI> recipeUIs = new List<RecipeUI>();

        public void UpdateRecipes(Crafter crafter,List<Recipe> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                if(recipeUIs.Count <= i)
                {
                    recipeUIs.Add(Instantiate(recipeUIPrefab,content.transform));
                }
                else
                {
                    recipeUIs[i].gameObject.SetActive(true);
                }
                recipeUIs[i].SetRecipe(crafter,recipes[i]);
            }
            for (int i = recipes.Count; i < recipeUIs.Count; i++)
            {
                recipeUIs[i].gameObject.SetActive(false);
            }
        }
    }
}

