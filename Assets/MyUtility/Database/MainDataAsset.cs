using System.Collections.Generic;
using UnityEngine;

namespace MyUtility.Database
{
    [CreateAssetMenu(fileName = "New Main Data Asset", menuName = "Naidio/DataAsset/MainDataAsset", order = 0)]
    public class MainDataAsset : ScriptableObject
    {
        // References to the assets
        //[SerializeField] private GameObject emptyInteractor;

        // Getters to the assets
        //public GameObject GetEmptyInteractorPrefab()
        //{
        //    return emptyInteractor;
        //}

        [SerializeField] private List<CookRecipe> recipesList = new List<CookRecipe>();

        public List<CookRecipe> GetRecipesList()
        {
            return recipesList;
        }

        public bool GetRecipe(List<CookIngredient> InIngredients, out CookRecipe OutRecipe)
        {
            // Go to all recipes
            foreach (var recipe in recipesList)
            {
                int correctIngredients = 0;
                // Go to all my ingredients
                foreach (var ingredient in InIngredients)
                {
                    // Go to all ingredients needed
                    foreach (var recipeIngredient in recipe.ingredients)
                    {
                        if (recipeIngredient.type == ingredient.type &&
                            recipeIngredient.amountPercentage <= ingredient.amountPercentage)
                        {
                            correctIngredients++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (correctIngredients == recipe.ingredients.Count)
                {
                    OutRecipe = recipe;
                    return true;
                }
            }

            OutRecipe = null;
            return false;
        }
    }
}