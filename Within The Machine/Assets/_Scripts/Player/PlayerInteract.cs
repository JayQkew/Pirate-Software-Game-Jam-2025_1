using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInteract : MonoBehaviour
{
    public InputMaster mouseControls;

    [SerializeField] private float radius = 5f;
    [SerializeField] private int FloorlayerTarget; //the layer the object is in Unity
    [SerializeField] private int FloorCorrectLayer;//The layer we change it to
    
    [SerializeField]private int HandLayerTarget;
    [SerializeField]private int HandCorrectLayer;
    
   [SerializeField] private Vector2 mousePosition;
   [SerializeField] private RaycastHit2D mousehitLeft;
   [SerializeField] private RaycastHit2D mousehitRight;
   [SerializeField]private RaycastHit2D[] inRangeHits;
   
   [SerializeField]private PlayerInventory _playerInventory;
   
    private void Awake()
    {
        mouseControls = new InputMaster();

        mouseControls.Player.LeftMB.performed += cntLM => LeftClick();
        mouseControls.Player.RightMB.performed += cntRM => RightClick();
    }

    // Start is called before the first frame update
    void Start()
    {
        FloorCorrectLayer = 1 << FloorlayerTarget;
        HandLayerTarget = 2 << FloorlayerTarget;
    }

    // Update is called once per frame
    void Update()
    { mousePosition = Input.mousePosition;
       inRangeHits = Physics2D.CircleCastAll(transform.position, radius, new Vector2(1, 1), 3f, FloorCorrectLayer);
       Debug.DrawRay(transform.position, radius * new Vector3(1, 1), Color.yellow);
      // Debug.Log(inRangeHits.Length);
    }

    private void LeftClick()
    {
        mousehitLeft = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, FloorCorrectLayer);
        if (mousehitLeft.collider != null && inRangeHits.Length > 0)
        {
            for (int i = 0; i < inRangeHits.Length; i++)
            {
                if (inRangeHits[i].collider == mousehitLeft.collider)
                {
                   FloorItem floorItem = inRangeHits[i].collider.gameObject.GetComponent<FloorItem>();
                   if (floorItem.whereTag == FloorItem.where.floor)
                   {
                       
                       if (floorItem != null)
                       {
                           if (_playerInventory.PickupItem(floorItem.itemSlot))
                           {
                               Debug.Log("Pick up item");
                               Destroy(floorItem.gameObject);
                           }
                        
                       }
                   }
                        
                    Debug.Log(inRangeHits[i].collider.name);
                }
            }
        }
    }

    private void RightClick()
    {
       mousehitRight = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, HandCorrectLayer);
       if (mousehitRight.collider != null && mousehitRight.collider.gameObject.GetComponent<HandID>() != null)
       {
           HandID handID = mousehitRight.collider.gameObject.GetComponent<HandID>();
           Debug.Log(handID.ID);
       }
    }

    private void OnEnable()
    {
        mouseControls.Enable();
    }

    private void OnDisable()
    {
        mouseControls.Disable();
    }
}
