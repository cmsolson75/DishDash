using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class Player : MonoBehaviour, IKitchenObjectParents
{
   public static Player Instance { get; private set; }
   
   [SerializeField] private float playerMovementSpeed = 5f;
   [SerializeField] private GameInputs gameInputs;
   [SerializeField] private LayerMask countersLayerMask;
   // private float _speedBoost = 1f;
   
   private readonly float _rotateSpeed = 15f;
   private Vector3 _lastInteractDir;
   
   [SerializeField] private Transform KitchenObjectHoldPoint;
   private KitchenObject _kitchenObject;
   private void Awake()
   {
      // singleton pattern
      if (Instance != null)
      {
         Debug.Log("More than one player!!!!!!!!");
      }

      Instance = this;
   }
   private void Update()
   {
      HandleInteractions();
      PlayerMovement();
      if (Input.GetKey(KeyCode.LeftShift))
      {
         // for (int i = 0; i < 50; i++)
         // {
         //    _speedBoost = 1f + i;
         // }
         playerMovementSpeed = 15f;
      }
      else
      {
         playerMovementSpeed = 5f;
      }
   }

   // ReSharper disable Unity.PerformanceAnalysis
   private void HandleInteractions()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         //for interactions
         float interactDistance = 2f;
         if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
         {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
               baseCounter.Interact(this);
            }
            // Debug.Log("Testing");
         }
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
         //for interactions
         float interactDistance = 2f;
         if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
         {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
               baseCounter.InteractAlternate(this);
            }
            // Debug.Log("Testing");
         }
      }
      

      Vector2 inputVector = gameInputs.GetMovementVector();
      inputVector = inputVector.normalized;
      Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);

      if (movDir != Vector3.zero)
      {
         _lastInteractDir = movDir;
      }
   }
   private void PlayerMovement()
   {
      Vector2 inputVector = gameInputs.GetMovementVector();
      
      
      // Normalization --> movement
      inputVector = inputVector.normalized;
      inputVector *= playerMovementSpeed;
      
      // Head Direction
      Vector3 movDir = new Vector3(inputVector.x, 0f, inputVector.y);
      float moveDistance = playerMovementSpeed * Time.deltaTime;
      float playerRadius = .5f;
      float playerHeight = 2f;
      
      
      // Raycast for object detection; 
      bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
         playerRadius, movDir, moveDistance);
      
      // Wall Hug Logic
      if (!canMove)
      {
         Vector3 movDirX = new Vector3(movDir.x, 0, 0).normalized;
         canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, movDirX, moveDistance);

         if (canMove)
         {
            // move only on x
            movDir = movDirX;
         }
         else
         {
            // Only Z movment
            Vector3 movDirZ = new Vector3(movDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
               playerRadius, movDirZ, moveDistance);
            if (canMove)
            {
               movDir = movDirZ;
            }
            
         }
      }
      
      // if not in contact with object you can move
      if (canMove)
      {
         transform.position += movDir * Time.deltaTime;
      }
      transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * _rotateSpeed);

   }

   public Transform GetKitchenObjetFollowTransform()
   {
      return KitchenObjectHoldPoint;
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
