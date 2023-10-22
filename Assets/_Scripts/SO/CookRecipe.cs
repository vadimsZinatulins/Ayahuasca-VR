using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

    [CreateAssetMenu(fileName = "New Recipe", menuName = "Ayahusca/Recipes", order = 0)]
    public class CookRecipe : ScriptableObject
    {
        public List<CookIngredient> ingredients;
        // Can be a normal item and some vfx (smoke and other stuff)
        public List<GameObject> objectsProduced;
    }

    [Serializable]
    public struct CookIngredient
    {
        public IngredientType type;
        public float amountPercentage;

        public CookIngredient(BoilerIngredient InBoilerIngredient)
        {
            type = InBoilerIngredient.ingredient.type;
            amountPercentage = InBoilerIngredient.ingredient.amountPercentage;
        }
    }
