using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.SO
{
    [CreateAssetMenu(fileName = "New Cook Recipe", menuName = "Ayahuasca/Cook/Recipe", order = 0)]
    public class CookRecipes : ScriptableObject
    {
        public List<CookIngredient> ingredients = new List<CookIngredient>();
        // Can be a normal item and some vfx (smoke and other stuff)
        public List<GameObject> objectsProduced = new List<GameObject>();
    }

    public struct CookIngredient
    {
        public IngredientType type;
        public float amountPercentage;
    }
}
