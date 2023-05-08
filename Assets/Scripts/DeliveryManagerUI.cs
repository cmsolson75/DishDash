using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;


    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
        
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        // Working
        // UpdateVisual();
        // foreach (var VARIABLE in COLLECTION)
        // {
        //     
        // }
        Destroy(recipeTemplate);
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        // Working
        UpdateVisual();
    }

    // private void UpdateVisual()
    // {
    //     foreach (Transform child in container)
    //     {
    //         if (child == recipeTemplate) continue;
    //         Destroy(child.gameObject);
    //     }
    //     foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
    //     {
    //         Transform recipeTransform = Instantiate(recipeTemplate, container);
    //         recipeTransform.gameObject.SetActive(true);
    //         recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
    //     }
    // }
    private void UpdateVisual()
    {
        Debug.Log("UpdateVisual called");

        // Delete child objects in reverse order
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            Debug.Log($"Current child: {child.gameObject.name}");

            if (child == recipeTemplate || child.GetComponent<RectTransform>() == recipeTemplate.GetComponent<RectTransform>())
            {
                Debug.Log("Skipping RecipeTemplate");
                continue;
            }

            Debug.Log($"Destroying object: {child.gameObject.name}");
            Destroy(child.gameObject);
        }

        Debug.Log("Objects in container after destruction:");
        foreach (Transform child in container)
        {
            Debug.Log(child.gameObject.name);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
            Debug.Log($"Instantiating new object: {recipeTransform.gameObject.name}");
        }

        Debug.Log("Objects in container after instantiation:");
        foreach (Transform child in container)
        {
            Debug.Log(child.gameObject.name);
        }
    }

}
