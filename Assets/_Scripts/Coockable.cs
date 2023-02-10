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

        public void SetIngredientType(IngredientType InType)
        {
            ingredientType = InType;
        }

        public Color GetCookColor()
        {
            return cookColor;
        }
        
        public void SetCookColor(Color InColor)
        {
            cookColor = InColor;
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