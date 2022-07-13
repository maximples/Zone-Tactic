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
            if (playerControl == Players.Player1 || playerControl == Players.Player3)
            {
                Unit unit = other.GetComponent<Unit>();
                if (unit.player == Players.Player2 & unit.live)
                {
                    unit.GetDamag(damag);
                }
            }
            if (playerControl == Players.Player2)
            {
                Unit unit = other.GetComponent<Unit>();
                if (unit.player != Players.Player2 & unit.live)
                {
                    unit.GetDamag(damag);
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
                    build.GetDamag(damag);
                }
            }
            if (playerControl == Players.Player2)
            {
                if (build.player != Players.Player2 & build.live)
                {
                    build.GetDamag(damag);
                }
            }
        }
    }
}
