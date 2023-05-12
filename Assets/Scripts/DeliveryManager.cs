using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Unity.VisualScripting;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    // private int playerScore = 0;
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    
    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private TextMeshProUGUI playerScoreUI;
    private List<RecipeSO> waitingRecipeSOList;
    
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private int playerScore;
    
    [SerializeField] private TextMeshProUGUI TimerGUI;
    [SerializeField] private TextMeshProUGUI PauseGUI;
    
    private enum State
    {
        Play, 
        Pause
    }
    private State GameState;

    private float _timer;
    public float Timer
    {
        get => _timer;
        set
        {
            _timer = value;
            int minutes = Mathf.FloorToInt(Timer / 60.0f);
            int seconds = Mathf.FloorToInt(Timer % 60.0f);
            TimerGUI.text = $"Time:{minutes:0}:{seconds:00}";
        }
    }

    private void Awake()
    {
        
        Instance = this;
        
        waitingRecipeSOList = new List<RecipeSO>();
        playerScore = 0;
        AddPlayerScore(playerScore);

    }

    private void Start()
    {
        Timer = 120f;
        GameState = State.Play;
    }

    private void Update()
    {
        Time.timeScale = GameState == State.Pause ? 0 : 1;
        
        if (Timer >= 0f)
        {
            Timer -= Time.deltaTime;
        }
        if (Timer <= 0f)
        {
            Timer = 0f;
            Debug.Log("Shift Over");
        }

        // Debug.Log(waitingRecipeSOList.Count);
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                // Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            GameState = GameState == State.Play ? State.Pause : State.Play;
            ToggleGUIVisability(PauseGUI);
        }

    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Debug.Log("3rd Layer");
                    bool ingredientFound = false;
                    // int recepeLength = 0;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Debug.Log("4th Layer");
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Debug.Log("YESSSSSSSS");
                            ingredientFound = true;
                            break;
                        }
                    }
                    
                    if (!ingredientFound)
                    {
                            
                        plateContentsMatchesRecipe = false;
                        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
                    }

                    if (plateContentsMatchesRecipe)
                    {
                        // playerScore += 10; 
                        // Debug.Log(playerScore);
                        Debug.Log("Player delivered the correct recipe");
                        waitingRecipeSOList.RemoveAt(i);
                        int numIngredients = waitingRecipeSO.kitchenObjectSOList.Count;
                        
                        
                        AddPlayerScore(numIngredients);
                        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                        
                        return;
                    }
                }
                // Debug.Log("Incorrect Recipe");
            }
        }
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    // public int AddPlayerScore(int lengthRecipeSo)
    // {
    //     int ingredientScore = 10;
    //     
    //     return lengthRecipeSo * ingredientScore;
    // }
    private void AddPlayerScore(int numIngredient)
    {
        int ingredientValue = 10;
        playerScore += numIngredient * ingredientValue;
        
        playerScoreUI.text = "POINTS: " + playerScore;
        // Debug.Log();
    }
    public void ToggleGUIVisability(TextMeshProUGUI GUI)
    {
        if (GUI.enabled == true)
        {
            GUI.enabled = false;

        }
        else
        {
            GUI.enabled = true;
        }
    }
}

//TODO: Make game better
