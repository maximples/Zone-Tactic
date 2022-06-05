using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGhostTower : BuildGhost
{
    MiningStation miningStation;

    void Start()
    {
        miningStation = GetComponent<MiningStation>();
       IsClose = true;
       badPos.SetActive(true);

    }
    public void Update()
    {
        if ((timer -= Time.deltaTime) <= 0 && !IsClose)
        {
            IsClose = true;
            badPos.SetActive(true);
            goodPos.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.tag == "Resource Point")
        {
            timer = 0.05F;
            IsClose = false;
            goodPos.SetActive(true);
            badPos.SetActive(false);
            miningStation.resursPoint = other.gameObject;
        }

    }
}