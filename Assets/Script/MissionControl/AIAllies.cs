using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAllies : MonoBehaviour
{
    public enum EnemyState
    {
        Free,
        Attack
    }
    static public AIAllies instance;
    public Players player;
    public float time = 85;
    private EnemyState enemyState = EnemyState.Free;
    public GameObject mainBase;
    public GameObject target = null;
    private Fabrica_M fabricaM;
    private Factory_L factoryL;
    public float enemyMoney = 1000;
    public float enemyForce = 6;
    public float enemyArmy = 0;
    private Vector3 basePos;
    private Vector3 targetPos;
    private List<Unit> UnitsAttack;
    private List<Unit> UnitsDefens;
    public bool start = false;
    public bool defens = false;
    public int builderInt = 0;
    private bool comandDefens = false;
    void Awake()
    {
        instance = this;
        UnitsAttack = new List<Unit>();
    }
    void Start()
    {
        start = false;
        basePos = transform.position;

    }

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
                        if (enemyArmy >= enemyForce*2)
                        {
                            enemyState = EnemyState.Attack;
                            AttackEnemy();
                        }
                        if(!comandDefens)
                        { ComandUnit(transform.position); }
                    }
                    break;
                case EnemyState.Attack:
                    {
                        if (InitiAttackGroup() <= enemyForce / 2)
                        {
                            enemyState = EnemyState.Free;
                            FullBack();
                        }
                    }
                    break;
            }
            if (enemyArmy < enemyForce*2)
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
                    if (iRand <= 2 && !fabricaM.work)
                    {
                        if (builderInt < 1)
                        {
                            if (enemyMoney >= fabricaM.products[3].price && !fabricaM.work)
                            {
                                fabricaM.BuildProduct(3);
                                fabricaM.work = true;
                            }
                        }
                    }
                        if (iRand <= 6 && !fabricaM.work )
                        {
                            if (enemyMoney >= fabricaM.products[0].price && !fabricaM.work)
                            {
                                enemyMoney -= fabricaM.products[0].price;
                                fabricaM.BuildProduct(0);
                                fabricaM.work = true;
                            }
                        }
                        if (iRand > 6 && !fabricaM.work)
                        {
                            if (enemyMoney >= fabricaM.products[1].price && !fabricaM.work)
                            {
                                enemyMoney -= fabricaM.products[1].price;
                                fabricaM.BuildProduct(1);
                                fabricaM.work = true;
                            }
                        }
                }

            }
        }
    }
    public void InitiEnemyForce()
    {
        int i = 0;
        builderInt = 0;
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle)
            {
                i++;
                if (unit.tipUnit==TipUnit.Builder)
                {
                    i--;
                    builderInt++;
                }    
            }
        }
        enemyArmy = i;
    }
    public void AttackEnemy()
    {
        float speed = 15;
        int i = 0;
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle && i < enemyForce)
            {
                if (unit.tipUnit == TipUnit.TankF && speed >= 9) { speed = 9; }
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
                if (unit.tipUnit != TipUnit.Builder)
                {
                    UnitsAttack.Add(unit);
                    i++;
                    unit.Agent.speed = speed;
                    unit.ComandAttakTerritory(targetPos);
                }
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
                target = build.gameObject;
                targetPos = target.gameObject.transform.position;
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
                    targetPos = target.gameObject.transform.position;
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
    public void Defens(Vector3 position)
    {
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle)
            {
                unit.ComandAttakTerritory(position);
                StartCoroutine(unit.IdleState());
            }

        }
        defens = true;
        StopAllCoroutines();
        StartCoroutine(Defens());
    }
    public IEnumerator Defens()
    {
        yield return new WaitForSeconds(10);
        defens=false;
        comandDefens = false;
    }
    public IEnumerator ComandDefens()
    {
        yield return new WaitForSeconds(15);
        comandDefens = false;
    }
    public void ComandUnit(Vector3 position)
    {
        comandDefens = true;
        int i = 0;
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live && unit.state == Unit.UnitState.Idle)
            {
                unit.OnSetTargetPos(position,i);
                i++;
                StartCoroutine(unit.IdleState());
            }

        }
        StartCoroutine(ComandDefens());

    }
    public void ActivPlayer()
    {
        foreach (Unit unit in UnitManager.Instance.GetAllUnits(player, Force.Allies))
        {
            if (unit.live)
            {
                GameObject mask_ = Instantiate(unit.maskFog, unit.transform.position, unit.transform.rotation) as GameObject;
                mask_.transform.parent = unit.transform;
            }

        }
        foreach (Build build in BuildManager.Instance.GetAllBuilds(player, Force.Allies))
        {
          if(build.live)
            {
                GameObject mask_ = Instantiate(build.maskFog, build.transform.position, build.transform.rotation) as GameObject;
                mask_.transform.parent = build.transform;
            }
        }
    }
}


