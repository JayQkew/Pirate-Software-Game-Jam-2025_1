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
   [SerializeField]private CraftingStation _craftingStation;
   [SerializeField] private Weapon _weapon;
   [SerializeField] private Furnace _furnace;
   
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
        HandCorrectLayer = 1 << HandLayerTarget;
    }

    // Update is called once per frame
    void Update()
    { 
        mousePosition = Input.mousePosition;
       inRangeHits = Physics2D.CircleCastAll(transform.position, radius, new Vector2(1, 1), 3f, FloorCorrectLayer);
       Debug.DrawRay(transform.position, radius * new Vector3(1, 1), Color.yellow);
      // Debug.Log(inRangeHits.Length);
    }

    private void LeftClick()
    {
        for (int i = 1; i <= 2; i++)
        {
            if (i == 1)
            {
                int Layer = FloorCorrectLayer;
                mousehitLeft = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Layer);
                if (mousehitLeft.collider != null && inRangeHits.Length > 0)
                {
                    for (int j = 0; j < inRangeHits.Length; j++)
                    {
                        if (inRangeHits[j].collider == mousehitLeft.collider)
                        {
                            FloorItem floorItem = inRangeHits[j].collider.gameObject.GetComponent<FloorItem>();
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
                            return;
                        }
                    }
                }
            }
            else if (i == 2)
            {
                int Layer = HandCorrectLayer;
                mousehitLeft = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Layer);
                if (mousehitLeft.collider != null && mousehitLeft.collider.gameObject.GetComponent<HandID>() != null)
                {
                    HandID handID = mousehitLeft.collider.gameObject.GetComponent<HandID>();
                    if (_craftingStation != null)
                    {
                        _playerInventory.PutInCraftingStation(handID.ID, _craftingStation);
                        return;
                    }
                    
                    else if (_furnace != null)
                    {
                        Debug.Log("Furnace");
                        //Furnace code
                        _playerInventory.PutInCraftingStation(handID.ID, _furnace);
                        return;
                    }
                    
                    else if (_weapon != null)
                    {
                        Debug.Log("Weapon");
                        _playerInventory.PutInCraftingStation(handID.ID, _weapon);
                        //weapon code
                        return;
                    }
                    
                    
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
           _playerInventory.DropItem(handID.ID);
       }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Station")
        {
            _craftingStation = other.gameObject.GetComponent<CraftingStation>();
            _furnace = null;
            _weapon = null;
        }

        if (other.tag == "Furnace")
        {
            _furnace = other.gameObject.GetComponent<Furnace>();
            _craftingStation = null;
            _weapon = null;
        }

        if (other.tag == "Weapon")
        {
            _weapon = other.gameObject.GetComponent<Weapon>();
            _craftingStation = null;
            _furnace = null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Station")
        {
            _craftingStation = other.gameObject.GetComponent<CraftingStation>();
            _furnace = null;
            _weapon = null;
        }

        if (other.tag == "Furnace")
        {
            _furnace = other.gameObject.GetComponent<Furnace>();
            _craftingStation = null;
            _weapon = null;
        }

        if (other.tag == "Weapon")
        {
            _weapon = other.gameObject.GetComponent<Weapon>();
            _craftingStation = null;
            _furnace = null;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Station")
        {
            _craftingStation = null;
            _craftingStation = null;
            _furnace = null;
        }
        if (other.tag == "Furnace")
        {
            _craftingStation = null;
            _craftingStation = null;
            _furnace = null;
        }

        if (other.tag == "Weapon")
        {
            _craftingStation = null;
            _weapon = null;
            _furnace = null;
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
