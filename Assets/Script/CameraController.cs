using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    private float horizontalInput;
    private float verticalInput;
    private float zoomInput;
    private float speed = 65;
    private float rotetSpeed = 55;
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
        focus.transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        focus.transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        transform.Translate(Vector3.forward * Time.deltaTime * speed*20 * zoomInput);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
