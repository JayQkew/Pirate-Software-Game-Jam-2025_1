using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public InputMaster mouseControls;

    [SerializeField] private float radius = 5f;
    [SerializeField] private int layerTarget; //the layer the object is in Unity
    [SerializeField] private int theCorrectLayer;//The layer we change it to
    
   [SerializeField] private Vector2 mousePosition;
   [SerializeField] private RaycastHit2D mousehitLeft;
   [SerializeField] private RaycastHit2D mousehitRight;
   [SerializeField]private RaycastHit2D[] inRangeHits;
   
    private void Awake()
    {
        mouseControls = new InputMaster();

        mouseControls.Player.LeftMB.performed += cntLM => LeftClick();
        mouseControls.Player.RightMB.performed += cntRM => RightClick();
    }

    // Start is called before the first frame update
    void Start()
    {
        theCorrectLayer = 1 << layerTarget;
    }

    // Update is called once per frame
    void Update()
    { mousePosition = Input.mousePosition;
       inRangeHits = Physics2D.CircleCastAll(transform.position, radius, new Vector2(1, 1), 3f, theCorrectLayer);
       Debug.DrawRay(transform.position, radius * new Vector3(1, 1), Color.yellow);
       Debug.Log(inRangeHits.Length);
    }

    private void LeftClick()
    {
        mousehitLeft = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, theCorrectLayer);
        if (mousehitLeft.collider != null && inRangeHits.Length > 0)
        {
            for (int i = 0; i < inRangeHits.Length; i++)
            {
                if (inRangeHits[i].collider == mousehitLeft.collider)
                {
                    Debug.Log(inRangeHits[i].collider.name);
                }
            }
        }
    }

    private void RightClick()
    {
       mousehitRight = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, theCorrectLayer);
       if (mousehitRight.collider != null && inRangeHits.Length > 0)
       {
           for (int i = 0; i < inRangeHits.Length; i++)
           {
               if (inRangeHits[i].collider == mousehitLeft.collider)
               {
                   Debug.Log(inRangeHits[i].collider.name);
               }
           }
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
