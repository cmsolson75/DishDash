using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParents
{
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject _kitchenObject;
    //Make this the base of Interact:
    public virtual void Interact(Player player)
    {
        Debug.Log("Base Class");
    }
    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("Base Class");
    }
    
    public Transform GetKitchenObjetFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
