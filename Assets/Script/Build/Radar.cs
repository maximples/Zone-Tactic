using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : Build
{

    [SerializeField] private GameObject radar;
    void Start()
    {
        if (live)
        {
            BuildManager.Instance.AddBuild(this, player);
            if (player == Players.Player1)
            {
                GameObject mask_ = Instantiate(maskFog, transform.position, transform.rotation) as GameObject;
                mask_.transform.parent = transform;
            }
        }
        GetColor();
        CurrentHealth = MaxHealth;
        radar.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
    }
    private void Update()
    {
        if (live && !building && GameStat.activ)
            radar.transform.Rotate(Vector3.up);
    }
}
