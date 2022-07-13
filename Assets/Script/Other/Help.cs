using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    private Unit unit;
    private SphereCollider sphereCollider;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        unit=GetComponent<Unit>();
        sphereCollider= gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 50;
        sphereCollider.isTrigger = true;
        unit.live = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Unit")
        {
            Unit unitNew = other.GetComponent<Unit>();
            if (unitNew != null)
            {
                if (unitNew.player == Players.Player1&&unitNew.live)
                {
                    unit.enabled = true;
                    unit.live = true;
                    Destroy(sphereCollider);
                    Destroy(rb);
                    Destroy(this);
                }
            }
        }
    }

}
