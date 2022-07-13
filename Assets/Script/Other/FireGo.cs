using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGo : MonoBehaviour
{
    [HideInInspector] public int damag=1;
    [HideInInspector] public float speed=10;
    [HideInInspector] public Players playerControl;
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
            if (playerControl == Players.Player1 || playerControl == Players.Player3)
            {
                if (unit.player == Players.Player2 & unit.live)
                {
                    Instantiate(explosion, transform.position, explosion.transform.rotation);
                    unit.GetDamag(damag);
                    Destroy(gameObject);
                }
            }
            if (playerControl == Players.Player2)
            {
                if (unit.player == Players.Player1 & unit.live|| unit.player == Players.Player3 & unit.live)
                {
                    Instantiate(explosion, transform.position, explosion.transform.rotation);
                    unit.GetDamag(damag);
                    Destroy(gameObject);
                }
            }
        }
        if (other.gameObject.transform.tag == "Build")
        {
            Build build = other.GetComponent<Build>();
            if (playerControl == Players.Player1 || playerControl == Players.Player3)
            {
                if (build.player == Players.Player2 & build.live)
                {
                    Instantiate(explosion, transform.position, explosion.transform.rotation);
                    build.GetDamag(damag);
                    Destroy(gameObject);
                }
            }
            if (playerControl == Players.Player2)
            {
                if (build.player == Players.Player1 & build.live || build.player == Players.Player3 & build.live)
                {
                    Instantiate(explosion, transform.position, explosion.transform.rotation);
                    build.GetDamag(damag);
                    Destroy(gameObject);
                }
            }
        }
        if (other.gameObject.transform.tag == "Ground")
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
        }


    }
}
