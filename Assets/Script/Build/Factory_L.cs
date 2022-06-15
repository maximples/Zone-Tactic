using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_L : Build
{
    public Product[] products;
    public Queue<Product> ProductQueue = new Queue<Product>();
    [SerializeField] private GameObject buildPos;
    [SerializeField] private GameObject mesh1;
    [SerializeField] private GameObject mesh2;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject door1;
    private Product p = null;
    private float timer;
    private Unit unit;
    private GameObject newUnit;
    private bool open=false;
    public bool work = false;
    private bool obstacleYes = false;
    private UnityEngine.AI.NavMeshObstacle obstacle;
    void Start()
    {
        open = false;
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
        mesh1.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        mesh2.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        door.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        door1.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
    }

    public void Update()
    {
        if (GameStat.activ)
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
                work = true;
                p = ProductQueue.Dequeue();
                if (select && player == Players.Player1)
                {
                    UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
                }
                timer = p.ConstructTime;
                newUnit = Instantiate(p.Prefab, buildPos.transform.position, buildPos.transform.rotation) as GameObject;
                unit = newUnit.GetComponent<Unit>();
                unit.player = player;
                if (player == Players.Player2) { unit.GetColor(); }
                newUnit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                newUnit.GetComponent<Unit>().enabled = false;
                newUnit.GetComponent<BoxCollider>().enabled = false;
            }
            if (p != null)
            {

                if (select && player == Players.Player1)
                {
                    UIManager.Instance.buildBar.value = (p.ConstructTime - timer) / p.ConstructTime * 100;
                    UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
                }
                timer -= Time.deltaTime;
                if (timer <= 3 && timer > 2.2f)
                {
                    open = true;
                    door.transform.Rotate(Vector3.left * 1.5f);
                }
                if (timer <= 2 && timer > 0)
                {
                    if (!obstacleYes)
                    {
                        obstacle = newUnit.AddComponent<UnityEngine.AI.NavMeshObstacle>();
                        obstacleYes = true;
                    }
                    newUnit.transform.Translate(Vector3.forward * Time.deltaTime * 10);
                }
                if (timer <= 0)
                {
                    obstacleYes = false;
                    Destroy(obstacle);
                    open = false;
                    newUnit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                    newUnit.GetComponent<Unit>().enabled = true;
                    newUnit.GetComponent<BoxCollider>().enabled = true;
                    if (Target != null)
                    {

                        unit.OnSetTarget(Target, 1);
                    }
                    work = false;
                    p = null;
                }
            }
            if (door.transform.eulerAngles.x < 269 || door.transform.eulerAngles.x > 271 && !open && !building) { door.transform.Rotate(Vector3.right * 1.5f); }
        }
    }
    public void BuildProduct(int ID)
    {
        ProductQueue.Enqueue(products[ID]);
        if (player == Players.Player1)
        {
            GameStat.player1money -= products[ID].price;
            UIManager.Instance.BuildBarText.text = "В очереди " + ProductQueue.Count;
        }
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
