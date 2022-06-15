using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamag : MonoBehaviour
{
    [HideInInspector] public int damag = 1;
    [HideInInspector] public float speed = 10;
    [HideInInspector] public Players playerControl;
    void Start()
    {
        Destroy(gameObject, 1);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Unit")
        {
            Unit unit = other.GetComponent<Unit>();
            if (unit.player != playerControl & unit.live)
            {
                unit.GetDamag(damag);
            }
        }
        if (other.gameObject.transform.tag == "Build")
        {
            Build build = other.GetComponent<Build>();
            if (build.player != playerControl & build.live)
            {
                build.GetDamag(damag);
            }
        }
    }
}
