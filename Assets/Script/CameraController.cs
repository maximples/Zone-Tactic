using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private GameObject miniMapMarcer;
    private float marcerZoomX=100;
    private float marcerZoomZ=100;
    private float horizontalInput;
    private float verticalInput;
    private float zoomInput;
    private float zoomMin = 1.1f;
    private float zoomMax = 5;
    public float zoom = 2;
    private float speed = 85;
    private float rotetSpeed = 65;
    private Camera cameraMain;
    private Vector3 scaleChange;
    public Canvas canvas;
    private void Start()
    {
        cameraMain = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        zoomInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(KeyCode.Q))
        {
            focus.transform.Rotate(Vector3.up * Time.deltaTime * rotetSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            focus.transform.Rotate(Vector3.down * Time.deltaTime * rotetSpeed);
        }
        focus.transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput* zoom);
        focus.transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput* zoom);
        if (Input.mousePosition.x <= 10) { focus.transform.Translate(-Vector3.right * Time.deltaTime * speed * zoom*0.7f); }
        if (Input.mousePosition.y <= 10) { focus.transform.Translate(-Vector3.forward * Time.deltaTime * speed * zoom * 0.7f); }
        if (Input.mousePosition.y >= cameraMain.pixelHeight - 10) { focus.transform.Translate(Vector3.forward * Time.deltaTime * speed * zoom * 0.7f); }
        if (Input.mousePosition.x >= cameraMain.pixelWidth - 10) { focus.transform.Translate(Vector3.right * Time.deltaTime * speed * zoom * 0.7f); }
        if (focus.transform.position.x <= 25) { focus.transform.position = new Vector3(25.1f, focus.transform.position.y, focus.transform.position.z); }
        if (focus.transform.position.x >= 600) { focus.transform.position = new Vector3(599.9f, focus.transform.position.y, focus.transform.position.z); }
        if (focus.transform.position.z >= 600) { focus.transform.position = new Vector3(focus.transform.position.x, focus.transform.position.y, 599.9f); }
        if (focus.transform.position.z <= 25) { focus.transform.position = new Vector3(focus.transform.position.x, focus.transform.position.y, 60.1f); }
        if (zoom < zoomMin && zoomInput>0)
        { zoomInput = 0; }
        if (zoom > zoomMax && zoomInput < 0)
        { { zoomInput = 0; } }
        transform.Translate(Vector3.forward * Time.deltaTime * speed * 20 * zoomInput);
        marcerZoomX += zoomInput * 10;
        marcerZoomZ += zoomInput * 10;
        zoom -= zoomInput;
        scaleChange = new Vector3(zoomInput * 30, 0, zoomInput * 30);
        miniMapMarcer.transform.localScale -= scaleChange;
    }
    public void ClicMiniMap()
    {
        float scalekoof = canvas.scaleFactor;
        float distantMiniMapX = 80;
        float distantMiniMapY = 80;
        float MiniMapSize = 200;
        distantMiniMapX *= scalekoof;
        distantMiniMapY *= scalekoof;
        MiniMapSize *= scalekoof;
        focus.transform.position=new Vector3((Screen.width - Input.mousePosition.x  - distantMiniMapX) / MiniMapSize*650, focus.transform.position.y, (Screen.height - Input.mousePosition.y - distantMiniMapY) / MiniMapSize * 650);
    }
}
