using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    [SerializeField] private AudioClip ItemPickUp;
    // [SerializeField] private Transform counterTopPoint;
    // [SerializeField] private ClearCounter secondClearCounter;
    // private KitchenObject _kitchenObject;
    
    
    public override void Interact(Player player)
    {
        // Debug.Log("Interact");
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            AudioManager.Instance.PlaySound(ItemPickUp, 0.5f);
            // Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            // kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        }
    }
}
