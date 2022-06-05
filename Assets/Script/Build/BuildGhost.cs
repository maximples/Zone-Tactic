using UnityEngine;
using System.Collections;

public class BuildGhost : MonoBehaviour {

    public bool IsClose;
    public GameObject goodPos;
    [SerializeField] protected GameObject badPos;
    protected float timer;
    void Start()
    {
        IsClose = true;
    }
    void Update()
    {
       if ((timer -= Time.deltaTime) <= 0 && IsClose)
        {
            IsClose = false;
            badPos.SetActive(false);
            goodPos.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        timer = 0.05F;
        IsClose = true;
        goodPos.SetActive(false);
        badPos.SetActive(true);

    }
}
