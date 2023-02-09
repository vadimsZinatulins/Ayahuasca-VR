using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using MyUtility.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boiler : MonoBehaviour
{
    [Header("Location")]
    [SerializeField] private Transform spawnLocation;
    
    [Header("Water")]
    [SerializeField] private MeshRenderer waterRenderer;
    [SerializeField] private Color defaultColor;
    [SerializeField] private ParticleSystem waterBoilingPS;
    [SerializeField] private List<AudioClip> waterSplashVFX = new List<AudioClip>();
    private Renderer waterBoilingPSRenderer;

    [Header("UI")] 
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private TextMeshProUGUI ingredientsText;
    [SerializeField] private Slider progressBar;
    
    [Header("Cooking")]
    [SerializeField] private float CookingTime = 5f;
    private Dictionary<IngredientType, BoilerIngredient> CurrentIngredients = new Dictionary<IngredientType, BoilerIngredient>();
    private bool recipeExists = false;
    private CookRecipe recipe;
    private float currentCookingTimer = 0;

    private void Awake()
    {
        if (waterBoilingPS)
        {
            waterBoilingPSRenderer = waterBoilingPS.GetComponent<Renderer>();
        }
        waterRenderer.material.color = defaultColor;
        waterBoilingPS.GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
        
        progressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (recipeExists)
        {
            currentCookingTimer += Time.deltaTime;
            progressBar.value = currentCookingTimer / CookingTime;
            if (currentCookingTimer >= CookingTime)
            {
                Cook();
                currentCookingTimer = 0;
            }
        }
    }

    private void LateUpdate()
    {
        if (BS_Player.Instance != null)
        {
            Vector3 direction = (BS_Player.Instance.playerBodyPosition.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z);

            canvasTransform.transform.eulerAngles = new Vector3(0, angle * Mathf.Rad2Deg - 90, 0);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Coockable>(out Coockable coockable))
        {
            BoilerIngredient boilerIngredient = new BoilerIngredient(coockable.GetIngredientType(), coockable.GetCookColor(), coockable.GetCutPercentage());
            
            if (CurrentIngredients.ContainsKey(boilerIngredient.ingredient.type))
            {
                var currentIngredient = CurrentIngredients[boilerIngredient.ingredient.type];

                currentIngredient.ingredient.amountPercentage += boilerIngredient.ingredient.amountPercentage;
                currentIngredient.color = Color.Lerp(currentIngredient.color, boilerIngredient.color, 0.5f);

                CurrentIngredients[boilerIngredient.ingredient.type] = currentIngredient;
            }
            else
            {
                CurrentIngredients.Add(coockable.GetIngredientType(), boilerIngredient);
            }

            // Spawn an audio clip for the water splash
            if (waterSplashVFX.Count > 0 && N_SoundManager.instance != null)
            {
                AudioClip clip = waterSplashVFX[Random.Range(0, waterSplashVFX.Count)];
                AudioSource source = N_SoundManager.instance.GetAudioSource(clip, clip.length, true);
                source.transform.position = transform.position;
                source.Play();
            }

            UpdateBoiler();
            Destroy(coockable.gameObject);
        }
        else
        {
            Debug.LogWarning("Ingredient not of type coockable");
            other.transform.position = spawnLocation.position;
        }
    }

    private void UpdateBoiler()
    {
        Color color = Color.white;
        string currentIngredientsText = "<b>Ingredients</b> \n";
        float totalPercentage = 0.1f;

        foreach (var ingredients in CurrentIngredients)
        {
            currentIngredientsText += $"{ingredients.Value.ingredient.type:g}:{ingredients.Value.ingredient.amountPercentage} \n";
            color = Color.Lerp(color, ingredients.Value.color, ingredients.Value.ingredient.amountPercentage / totalPercentage);
            totalPercentage += ingredients.Value.ingredient.amountPercentage;
        }

        waterRenderer.material.color = new Color(color.r,color.g, color.b, waterRenderer.material.color.a);
        waterBoilingPSRenderer.material.SetColor("_Color",color);
        ingredientsText.text = currentIngredientsText;

        if (CheckForRecipe())
        {
            recipeExists = true;
            progressBar.gameObject.SetActive(true);
        }
        else
        {
            recipeExists = false;
            progressBar.gameObject.SetActive(false);
        }
    }

    private bool CheckForRecipe()
    {
        MainDataAsset DataAsset = N_FunctionLibrary.GetMainDataAsset();
        if (DataAsset != null)
        {
            List<CookIngredient> cookIngredients = new List<CookIngredient>();
            foreach (var currentIngredient in CurrentIngredients)
            {
                cookIngredients.Add(currentIngredient.Value.ingredient);
            }

            CookRecipe r;
            if (DataAsset.GetRecipe(cookIngredients, out r))
            {
                recipe = r;
                currentCookingTimer = 0;
                return true;
            }
        }

        return false;
    }

    public void Cook()
    {
        foreach(var objectsProduced in recipe.objectsProduced)
        {
            Instantiate(objectsProduced, spawnLocation.position, spawnLocation.rotation);
        }
        
        recipeExists = false;
        progressBar.gameObject.SetActive(false);
        progressBar.value = 0;
        CurrentIngredients = new Dictionary<IngredientType, BoilerIngredient>();
        waterRenderer.material.color = defaultColor;
        waterBoilingPSRenderer.material.SetColor("_Color", defaultColor);
    }
}

public struct BoilerIngredient
{
    public Color color;
    public CookIngredient ingredient;

    public BoilerIngredient(IngredientType InType, Color InColor, float InAmount)
    {
        ingredient.type = InType;
        color = InColor;
        ingredient.amountPercentage = InAmount;
    }
}