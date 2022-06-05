using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGo : MonoBehaviour
{
    public int damag=1;
    public float speed=10;
    public Players playerControl;
    [SerializeField] private GameObject explosion;
    void Start()
    {
        Destroy(gameObject,3);
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
            if (unit.player != playerControl&unit.live)
            {
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                unit.GetDamag(damag);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.transform.tag == "Build")
        {
            Build build = other.GetComponent<Build>();
            if (build.player != playerControl & build.live)
            {
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                build.GetDamag(damag);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.transform.tag == "Ground")
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
        }


    }
}
