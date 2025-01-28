using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Transform startPosition; 
    public Transform endPosition;   
    public float startZoom;     
    public float endZoom;     
    public float transitionSpeed = 5f; 
    public KeyCode zoomInKey = KeyCode.I;  
    public KeyCode zoomOutKey = KeyCode.O;

    public float whenExteriorDisappear;
    public GameObject tankExterior;

    private Camera cam;
    private bool isMoving = false; 
    //private Vector3 targetPosition;
    private float targetZoom;

    private float currentZoomValue;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKey(zoomInKey))
        {
            // Start moving towards the start position and max zoom
            isMoving = true;
            //targetPosition = startPosition.position;
            currentZoomValue -= Time.deltaTime * transitionSpeed;
            //targetZoom = maxZoom;
        }
        else if (Input.GetKey(zoomOutKey))
        {
            // Start moving towards the end position and min zoom
            isMoving = true;
            //targetPosition = endPosition.position;
            currentZoomValue += Time.deltaTime * transitionSpeed;
            //targetZoom = minZoom;
        }
        else
        {
            // Stop movement when no keys are pressed
            isMoving = false;
        }

        if (isMoving)
        {
            MoveAndZoom();
        }
    }

    private void MoveAndZoom()
    {
        if (currentZoomValue > 1) currentZoomValue = 1;
        if (currentZoomValue < 0) currentZoomValue = 0;
        
        // Interpolate the camera's position using Lerp
        transform.position = Vector3.Lerp(
            startPosition.position, 
            endPosition.position, 
            currentZoomValue
        );

        // Interpolate the zoom using Lerp
        cam.orthographicSize = Mathf.Lerp(
            startZoom, 
            endZoom, 
            currentZoomValue
        );
        
        tankExterior.SetActive(currentZoomValue > whenExteriorDisappear);
    }
}
