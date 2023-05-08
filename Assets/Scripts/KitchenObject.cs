using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    // private ClearCounter _kitchenObjectParent;
    private IKitchenObjectParents _kitchenObjectParents;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParents kitchenObjectParents)
    {
        if (this._kitchenObjectParents != null)
        {
            this._kitchenObjectParents.ClearKitchenObject();
        }

        this._kitchenObjectParents = kitchenObjectParents;

        if (kitchenObjectParents.HasKitchenObject())
        {
            Debug.Log("IParent Already Has a KitchenObject!");
        }
        kitchenObjectParents.SetKitchenObject(this);
        transform.parent = kitchenObjectParents.GetKitchenObjetFollowTransform();
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public IKitchenObjectParents GetKitchenObjectParent()
    {
        return _kitchenObjectParents;
    }

    public void DestroySelf()
    {
        _kitchenObjectParents.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO,
        IKitchenObjectParents kitchenObjectParents)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParents);

        return kitchenObject;

    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
