using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boiler : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private MeshRenderer waterRenderer;
    [SerializeField] private TextMeshPro ingredientsText;
    [SerializeField] private ParticleSystem waterBoilingPS;
    [SerializeField] private List<AudioClip> waterSplashVFX = new List<AudioClip>();
    private Dictionary<IngredientType, BoilerIngredient> CurrentIngredients = new Dictionary<IngredientType, BoilerIngredient>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Coockable>(out Coockable coockable))
        {
            BoilerIngredient boilerIngredient = new BoilerIngredient(coockable.GetIngredientType(), coockable.GetCookColor(), coockable.GetPercentage());
            
            if (CurrentIngredients.ContainsKey(boilerIngredient.type))
            {
                var currentIngredient = CurrentIngredients[boilerIngredient.type];

                currentIngredient.amountPercentage += boilerIngredient.amountPercentage;
                currentIngredient.color = Color.Lerp(currentIngredient.color, boilerIngredient.color, 0.5f);

                CurrentIngredients[boilerIngredient.type] = currentIngredient;
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
            currentIngredientsText += $"{ingredients.Value.type:g}:{ingredients.Value.amountPercentage} \n";
            color = Color.Lerp(color, ingredients.Value.color, ingredients.Value.amountPercentage / totalPercentage);
            totalPercentage += ingredients.Value.amountPercentage;
        }

        waterRenderer.material.color = new Color(color.r,color.g, color.b, waterRenderer.material.color.a);
        waterBoilingPS.GetComponent<Renderer>().material.SetColor("_Color",color);
        ingredientsText.text = currentIngredientsText;
    }
}

public struct BoilerIngredient
{
    public IngredientType type;
    public Color color;
    public float amountPercentage;

    public BoilerIngredient(IngredientType InType, Color InColor, float InAmount)
    {
        type = InType;
        color = InColor;
        amountPercentage = InAmount;
    }
}