using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Build
{

    [SerializeField] private GameObject Radar;
    void Start()
    {
        if (live)
        {
            BuildManager.Instance.AddBuild(this, player);
            BaseAdd(player);
        }
        CurrentHealth = MaxHealth;
        if (player == Players.Player1)
        {
            gameObject.layer = 6;
        }
        if (player == Players.Player2)
        {
            gameObject.layer = 7;
        }
        mesh.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        Radar.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
    }
    private void Update()
    {
        if(live&&!building)
        Radar.transform.Rotate(Vector3.up);
    }
    public void BaseAdd(Players controller)
    {
        if (live && !building)
        {
            if (controller == Players.Player1)
            {
                GameStat.baseNumberPlaer1++;
            }
            if (controller == Players.Player2)
            {
                GameStat.baseNumberPlaer2++;
            }
        }
    }
    public override void GetDamag(int damag)
    {
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth <= 0)
        {
            BuildManager.Instance.RemoveUnit(this, player);
            if (this == UIManager.Instance.build)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            if (player == Players.Player1)
            {
                GameStat.baseNumberPlaer1--;
            }
            if (player == Players.Player2)
            {
                GameStat.baseNumberPlaer2--;
            }
            Instantiate(destroyBuild, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
