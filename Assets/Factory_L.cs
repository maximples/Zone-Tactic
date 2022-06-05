using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_L : Build
{
    public Product[] products;
    public Queue<Product> ProductQueue = new Queue<Product>();
    [SerializeField] private GameObject buildPos;
    private Product p = null;
    private float timer;
    private Unit unit;
    private GameObject newUnit;
    private bool left = true;
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
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
    }

    public void Update()
    {
        if (p == null)
        {
            if (select)
            {
                UIManager.Instance.BuildBar.value = 0;
                UIManager.Instance.BuildBarText.text = "";
            }
        }
        if (p == null && ProductQueue.Count > 0)
        {
            p = ProductQueue.Dequeue();
            UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
            timer = p.ConstructTime;
            newUnit = Instantiate(p.Prefab, buildPos.transform.position, buildPos.transform.rotation) as GameObject;
            unit = newUnit.GetComponent<Unit>();
            unit.player = player;
            newUnit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            newUnit.GetComponent<Unit>().enabled = false;
            newUnit.GetComponent<BoxCollider>().enabled = false;
        }
        if (p != null)
        {
           
            if (select) { UIManager.Instance.BuildBar.value = (p.ConstructTime - timer) / p.ConstructTime * 100; }
            timer -= Time.deltaTime;
            if (timer <= 1)
            {
                newUnit.transform.Translate(Vector3.forward * Time.deltaTime * 8);
            }
            if (timer <= 0)
            {
                newUnit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                newUnit.GetComponent<Unit>().enabled = true;
                newUnit.GetComponent<BoxCollider>().enabled = true;
                if (Target != null)
                {

                    unit.OnSetTarget(Target, 1);
                }
                p = null;
            }
        }
    }
    public void BuildProduct(int ID)
    {
        ProductQueue.Enqueue(products[ID]);
        UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
    }
    public override void GetDamag(int damag)
    {
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth <= 0)
        {
            if (newUnit != null) { Destroy(newUnit); }
            BuildManager.Instance.RemoveUnit(this, player);
            if (this == UIManager.Instance.build)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            Instantiate(destroyBuild, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
