using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    
    [SerializeField] private FryingRecepeSO[] _fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOArray;
    [SerializeField] private AudioClip Sizzle;
    private float _fryingTimer;
    private float _burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private State state;
    
    private FryingRecepeSO _fryingRecepeSO;
    // private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;
                    if (_fryingTimer > _fryingRecepeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecepeSO.output, this);
                        state = State.Fried;
                        _burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }

                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;
                    if (_burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                    }

                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    /*Frying Progress*/
                    _fryingRecepeSO = GetFryingRecepeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    AudioManager.Instance.PlaySound(Sizzle, 1f);
                    _fryingTimer = 0f;
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        state = State.Idle;
                    }
                    
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecepeSO fryingRecepeSO = GetFryingRecepeSOWithInput(inputKitchenObjectSO);
        return fryingRecepeSO != null;
    }

    private KitchenObjectSO GetOutputForIntput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecepeSO fryingRecepeSO = GetFryingRecepeSOWithInput(inputKitchenObjectSO);
        if (fryingRecepeSO != null)
        {
            return fryingRecepeSO.output;
        }
        else
        {
            return null;
        }
        
    }

    private FryingRecepeSO GetFryingRecepeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecepeSO fryingRecepeSO in _fryingRecipeSOArray)
        {
            if (fryingRecepeSO.input == inputKitchenObjectSO)
            {
                return fryingRecepeSO;
            }
        }

        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

        return null;
    }
}
