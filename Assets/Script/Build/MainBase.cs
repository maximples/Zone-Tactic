using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Build
{

    [SerializeField] private GameObject radar;
    void Start()
    {
        if (live)
        {
            BuildManager.Instance.AddBuild(this, player);
            BaseAdd(player);
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
        if(live&&!building&&GameStat.activ)
        radar.transform.Rotate(Vector3.up);
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
    public override void OnSetTarget(TargetPoint target, int num)
    {

    }
}
