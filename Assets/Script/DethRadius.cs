using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DethRadius : MonoBehaviour
{
    private int damag = 30;
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Unit")
        {
            Unit unit = other.GetComponent<Unit>();
            unit.GetDamag(damag);
            
        }
        if (other.gameObject.transform.tag == "Build")
        {
            Build build = other.GetComponent<Build>();
            build.GetDamag(damag);
        }
    }
}