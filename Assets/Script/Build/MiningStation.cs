using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningStation : Build
{
    public GameObject resursPoint;

    [SerializeField] private GameObject Tover;
    private float incom=10;
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
        CurrentHealth = MaxHealth;
        GetColor();
        Tover.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
    }
    private void Update()
    {
        if (live && !building&&GameStat.activ)
        {
            Tover.transform.Rotate(Vector3.up * 0.5f);
            if (player == Players.Player1)
            {
                GameStat.player1money = GameStat.player1money + incom * Time.deltaTime;
            }
        }
    }
    public override void GetDamag(int damag)
    {
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth <= 0)
        {
            resursPoint.GetComponent<BoxCollider>().enabled = true;
            BuildManager.Instance.RemoveUnit(this, player);
            if (this == UIManager.Instance.build)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            Instantiate(destroyBuild, transform.position, transform.rotation);
            GameStat.player1Incom -= incom;
            Destroy(gameObject);
        }
    }
    public override void OnSetTarget(TargetPoint target, int num)
    {

    }
}
