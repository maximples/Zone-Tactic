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
        Tover.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
    }
    private void Update()
    {
        if (live && !building)
        {
            Tover.transform.Rotate(Vector3.up * 0.5f);
            GameStat.player1money = GameStat.player1money + incom * Time.deltaTime;
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
            Destroy(gameObject);
        }
    }
}
