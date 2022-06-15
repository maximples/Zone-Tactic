using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGo : MonoBehaviour
{
    [HideInInspector] public int damag = 1;
    [HideInInspector] public float speed = 10;
    [HideInInspector] public float distant =30;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Players playerControl;
    [HideInInspector] public Vector3 targetPos;
    private Vector3 offset;
    [SerializeField] private GameObject explosion;
    void Start()
    {
        startPos = transform.position;
        offset = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Destroy(gameObject, 4);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Vector3.SqrMagnitude(targetPos - transform.position)< Vector3.SqrMagnitude(targetPos - startPos)/3)
        {
            transform.LookAt(targetPos + offset);
        }
    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Unit")
        {
            Unit unit = other.GetComponent<Unit>();
            if (unit.player != playerControl & unit.live)
            {
                Instantiate(explosion, transform.position, explosion.transform.rotation);
                Destroy(gameObject);
            }
        }
        if (other.gameObject.transform.tag == "Build")
        {
            Build build = other.GetComponent<Build>();
            if (build.player != playerControl & build.live)
            {
                Instantiate(explosion, transform.position, explosion.transform.rotation);
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