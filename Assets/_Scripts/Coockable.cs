using UnityEngine;

namespace DefaultNamespace
{
    public class Coockable : Cuttable
    {
        [SerializeField] IngredientType ingredientType;
        [SerializeField] Color cookColor;

        public IngredientType GetIngredientType()
        {
            return ingredientType;
        }

        public Color GetCookColor()
        {
            return cookColor;
        }
    }

    public enum IngredientType
    {
        Chacrona,
        Jagube,
        Cidreira,
        Camomila
    }
}