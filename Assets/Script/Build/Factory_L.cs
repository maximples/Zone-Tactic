using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_L : Build
{
    public Product[] products;
    public Queue<Product> ProductQueue = new Queue<Product>();
    [SerializeField] private GameObject buildPos;
    [SerializeField] private GameObject door;
    private Product p = null;
    private float timer;
    private Unit unit;
    private GameObject newUnit;
    private bool open=false;
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
                UIManager.Instance.buildBar.value = 0;
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
           
            if (select) { UIManager.Instance.buildBar.value = (p.ConstructTime - timer) / p.ConstructTime * 100; }
            timer -= Time.deltaTime;
            if (timer <= 3&&timer>2.2f)
            {
                open = true;
                door.transform.Rotate(Vector3.left * 2);
            }
            if (timer <= 2f)
            {
                newUnit.transform.Translate(Vector3.forward * Time.deltaTime * 10);
            }
            if (timer <= 0)
            {
                open = false;
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
        if (door.transform.eulerAngles.x!=270&&!open) { door.transform.Rotate(Vector3.right * 2);}
    }
    public void BuildProduct(int ID)
    {
        ProductQueue.Enqueue(products[ID]);
        GameStat.player1money -= products[ID].price;
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
