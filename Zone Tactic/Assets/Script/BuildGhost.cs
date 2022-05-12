using UnityEngine;
using System.Collections;

public class BuildGhost : MonoBehaviour {

    public bool IsClose;
    private float timer;

    public void Update()
    {
        if ((timer -= Time.deltaTime) <= 0 && IsClose)
        {
            IsClose = false;
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public void OnCollisionStay(Collision col)
    {
        timer = 0.2F;
        IsClose = true;
        GetComponent<Renderer>().material.color = Color.red;
    }
}
