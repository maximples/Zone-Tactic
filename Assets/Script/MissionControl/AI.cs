  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public enum EnemyState
    {
        Free,
        Attack
    }
    public Players player;
    public float time = 85;
    public EnemyState enemyState=EnemyState.Free;
    public GameObject mainBase;
    public GameObject target = null;
    private Fabrica_M fabricaM;
    private Factory_L factoryL;
    public float enemyMoney=10000;
    public float enemyForce =1;
    public float enemyArmy = 0;
    public float enemyStrong = 2;
    private Vector3 basePos;
    public Vector3 targetPos;
    private List<Unit> UnitsAttack;
    private List<Unit> UnitsDefens;
    public bool start = false;
    // Start is called before the first frame update
    void Awake()
    {

        UnitsAttack = new List<Unit>();
    }
    void Start()
    {
       enemyStrong = SaveData.Instance.difficultyLevel;
        start = false;
        //StartCoroutine(Timer());
        basePos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStat.activ && start)
        {
            if (target == null) { FindTarget(); }
            InitiEnemyForce();
            enemyMoney = enemyMoney + 50 * Time.deltaTime;
            switch (enemyState)
            {
                case EnemyState.Free:
                    {
                        if (enemyArmy >= enemyForce)
                        {
                            enemyState = EnemyState.Attack;
                            AttackEnemy();
                        }
                    }
                    break;
                case EnemyState.Attack:
                    {
                        if (InitiAttackGroup() <= enemyForce / 3)
                        {
                            enemyState = EnemyState.Free;
                            FullBack();
                            enemyForce += enemyStrong;
                            if (enemyForce > 30) { enemyForce = 35; }
                            if (enemyForce > 30&&enemyStrong==2) { enemyForce = 20; }
                            if (enemyForce > 30 && enemyStrong == 1) { enemyForce = 15; }
                        }
                    }
                    break;
            }
            if (enemyArmy < enemyForce)
            {
                BuildingUnit();
            }
        }
    }
    private void BuildingUnit()
    {

        int iRand = 0;
        foreach (Build build in BuildManager.Instance.GetAllBuilds(player, Force.Allies))
        {
            iRand = Random.Range(1, 10);
            if (build.live)
            {
                if (build.tipBuild == TipBuild.FabricaM)
                {
                    fabricaM = build.gameObject.GetComponent<Fabrica_M>();

                    if (enemyForce < 3 && !fabricaM.work)
                    { 
                        if (iRand <= 7 && !fabricaM.work && enemyForce <= 2)
                        {
                            if (enemyMoney >= fabricaM.products[0].price && !fabricaM.work)
                            {
                                enemyMoney -= fabricaM.products[0].price;
                                fabricaM.BuildProduct(0);
                                fabricaM.work = true;
                            }
                        }
                    if (iRand >7  && !fabricaM.work && enemyForce <= 2)
                    {
                        if (enemyMoney >= fabricaM.products[1].price && !fabricaM.work)
                        {
                            enemyMoney -= fabricaM.products[1].price;
                            fabricaM.BuildProduct(1);
                            fabricaM.work = true;
                        }
                    }
                }
                        
                    
                    if (enemyForce >= 3&&!fabricaM.work)
                    {
                        if (iRand <= 2 && !fabricaM.work&& enemyForce >= 3)
                        {
                            if (enemyMoney >= fabricaM.products[0].price && !fabricaM.work)
                            {
                                enemyMoney -= fabricaM.products[0].price;
                                fabricaM.BuildProduct(0);
                                fabricaM.work = true;
                            }
                        }
                        if (iRand <= 8 && iRand > 2&& !fabricaM.work && enemyForce >= 3)
                        {
                            if (enemyMoney >= fabricaM.products[1].price && !fabricaM.work)
                            {
                                enemyMoney -= fabricaM.products[1].price;
                                fabricaM.BuildProduct(1);
                                fabricaM.work = true;
                            }
                        }
                        if (iRand > 8 && !fabricaM.work && enemyForce >= 3)
                        {
                            if (enemyMoney >= fabricaM.products[2].price && !fabricaM.work)
                            {
                                enemyMoney -= fabricaM.products[2].price;
                                fabricaM.BuildProduct(2);
                                fabricaM.work = true;
                            }
                        }
                    }
                }
                if (build.tipBuild == TipBuild.FabricaL)
                {
                    factoryL = build.gameObject.GetComponent<Factory_L>();
                    if (enemyForce >= 10 && !factoryL.work)
                    {
                        if (iRand <= 6 && !factoryL.work && enemyForce >= 10)
                        {
                            if (enemyMoney >= factoryL.products[0].price && !factoryL.work)
                            {
                                enemyMoney -= factoryL.products[0].price;
                                factoryL.BuildProduct(0);
                                factoryL.work = true;
                            }
                        }
                        if (iRand > 6 && !factoryL.work && enemyForce >= 10)
                        {
                            if (enemyMoney >= factoryL.products[1].price && !factoryL.work)
                            {
                                enemyMoney -= factoryL.products[1].price;
                                factoryL.BuildProduct(1);
                                factoryL.work = true;
                            }
                        }
                    }
                }

            }
        }
    }
    public void InitiEnemyForce()
    {
        int i=0;
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle)
            {
                i++;
            }
        }
        enemyArmy = i;
    }
    public void AttackEnemy()
    {
        float speed = 15;
        int i=0;
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle&&i<enemyForce)
            {
                if (unit.tipUnit == TipUnit.TankF&&speed>=9) { speed = 9; }
                if (unit.tipUnit == TipUnit.TankM && speed >= 8) { speed = 8; }
                if (unit.tipUnit == TipUnit.TankL && speed >= 7) { speed = 7; }
                if (unit.tipUnit == TipUnit.TankR && speed >= 6) { speed = 6; }
                i++;
            }
        }
        i = 0;
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle && i < enemyForce)
            {
                UnitsAttack.Add(unit);
                i++;
                unit.Agent.speed = speed;
                unit.ComandAttakTerritory(targetPos);
            }
        }
    }
    public void AttackEnemyGroup()
    {
        foreach (Unit unit in UnitsAttack)
        {
            if (unit.live)
            {
                unit.ComandAttakTerritory(targetPos);
            }
        }
    }
    public void FindTarget()
    {
        foreach (Build build in BuildManager.Instance.GetAllBuilds(player, Force.Enemies))
        {
            if (build.live)
            {
                target=build.gameObject;
                targetPos = new Vector3 (target.gameObject.transform.position.x,0, target.gameObject.transform.position.z);
                break;
            }
        }
        if (target == null)
        {
            foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Enemies))
            {
                if (unit.live)
                {
                    target = unit.gameObject;
                    targetPos = new Vector3(target.gameObject.transform.position.x, 0, target.gameObject.transform.position.z);
                    break;
                }
            }
        }
        if (enemyState == EnemyState.Attack) { AttackEnemyGroup(); }
    }
    public int InitiAttackGroup()
    {
        int i = 0;
        foreach (Unit unit in UnitsAttack)
        {
            if (unit.live)
            {
                i++;
            }
        }
        return i;
    }
    public void FullBack()
    {
        int i = 0;
        foreach (Unit unit in UnitsAttack)
        {
            if (unit.live)
            {
                unit.OnSetTargetPos(basePos, i);
                i++;
            }

        }
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        start = true;
    }
}
