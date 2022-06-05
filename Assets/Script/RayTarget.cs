using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public bool GoodTarget(int mask)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
        if (hit.transform != null)
        {
            if (hit.transform.tag == "Ground")
            {
                return false;
            }
            else
                return true;
        }
        else
        {
            return false;
        }
    }
}
