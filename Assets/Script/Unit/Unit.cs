using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour, ISelect {

    public enum UnitState
    {
        Idle,
        AgresivStand,
        MoveTarget,
        MoveUnit,
        MoveEnemy,
        MoveFrend,
        Building,
        Repear,
        AttakTerritory
    }
    public float attackRadius = 20;
    public float agroRadius = 60;
    public int damag = 3;
    public float speed = 10;
    public Sprite image;
    public string nameUnit;
    public int MaxHealth=100;
    public int CurrentHealth;
    public TargetPoint Target = null;
    public Players player;
    public UnitState state;
    public bool live = true;
    [SerializeField] protected GameObject selectionRing;
    [SerializeField] protected GameObject comandTarget;
    [SerializeField] protected GameObject turrent;
    [SerializeField] protected GameObject destroyEffect;
    [SerializeField] protected GameObject HpBar;
    protected GameObject cameraMain;
    protected GameObject enemyTarget;
    protected UnitManager unitManager;
    protected float speedFire = 40;
    protected float randTime;
    protected bool isBusy = false;
    [SerializeField]  protected Vector3 targetPosition;
    [SerializeField] protected GameObject gun;
    protected Vector3 idlePosition;
    protected UnityEngine.AI.NavMeshAgent _agent;
    protected int mask;
    public UnityEngine.AI.NavMeshAgent Agent
    {
        get
        {
            if(_agent == null)
            {
                _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            }
            return _agent;
        }
        set
        {
            _agent = value;
        }
    }
        public void OnSelect(int num)
    {
        selectionRing.SetActive(true);
        HpBar.SetActive(true);
    }

    public void OnDeselect()
    {
        if (live)
        {
            selectionRing.SetActive(false);
            HpBar.SetActive(false);
        }
    }

    public void OnSetTarget(TargetPoint target, int num)
    {
        if (live)
        {
            Agent.isStopped = false;
            float xOffset = 0;
            float zOffset = 0;
            if (num == 2) { xOffset = 5; zOffset = 0; }
            if (num == 3) { xOffset = -5; zOffset = 0; }
            if (num == 4) { xOffset = 0; zOffset = -7; }
            if (num == 5) { xOffset = 5; zOffset = -7; }
            if (num == 6) { xOffset = -5; zOffset = -7; }
            if (num == 7) { xOffset = 0; zOffset = 7; }
            if (num == 8) { xOffset = 5; zOffset = 7; }
            if (num == 9) { xOffset = -5; zOffset = 7; }
            if (num == 10) { xOffset = 10; zOffset = 0; }
            if (num == 11) { xOffset = -10; zOffset = 0; }
            if (num == 12) { xOffset = 10; zOffset = -7; }
            if (num == 13) { xOffset = -10; zOffset = -7; }
            if (num == 14) { xOffset = 10; zOffset = 7; }
            if (num == 15) { xOffset = 10; zOffset = 7; }
            if (Target != null)
            {
                Target.RemoveLink();
            }
            state = UnitState.MoveTarget;
            Target = target;
            Target.AddLink();
            Vector3 offset = new Vector3(xOffset, 0, zOffset);
            targetPosition = Target.transform.position + offset;
        }
    }
    public IEnumerator FindEnemy(float time,float radius)
    {
        isBusy = true;
        foreach (Unit unit in unitManager.GetAllUnits(player,Force.Enemies))
        {
            if (unit.live)
            {
                if (Vector3.SqrMagnitude(unit.gameObject.transform.position - transform.position) < radius * radius)
                {
                    enemyTarget = unit.gameObject;
                    unit.AgroFindEnemy();
                    break;
                }
            }
        }
        if (enemyTarget == null)
        {
            foreach (Build build in BuildManager.Instance.GetAllBuilds(player, Force.Enemies))
            {
                if (build.live)
                {
                    if (Vector3.SqrMagnitude(build.gameObject.transform.position - transform.position) < radius * radius)
                    {
                        enemyTarget = build.gameObject;
                        break;
                    }
                }
            }
        }
        yield return new WaitForSeconds(time + randTime);
        isBusy = false;
    }
    public void AgroFindEnemy()
    {
        foreach (Unit unit in unitManager.GetAllUnits(player, Force.Allies))
        {
            if (unit.live)
            {
                if (unit.state == UnitState.Idle) { unit.state = UnitState.AgresivStand; }
            }
        }
        foreach (Unit unit in unitManager.GetAllAirUnits(player, Force.Allies))
        {
            if (unit.live)
            {
                if (unit.state == UnitState.Idle) { unit.state = UnitState.AgresivStand; }
            }
        }


    }
    public void AgroEnemy()
    {
        foreach (Unit unit in unitManager.GetAllUnits(player, Force.Enemies))
        {
            if (unit.live)
            {
                if (unit.state == UnitState.Idle) { unit.state = UnitState.AgresivStand; }
            }
        }
        foreach (Unit unit in unitManager.GetAllAirUnits(player, Force.Enemies))
        {
            if (unit.live)
            {
                if (unit.state == UnitState.Idle) { unit.state = UnitState.AgresivStand; }
            }
        }
    }
    public virtual void GetDamag(int damag)
    {
        if (state == UnitState.Idle) { AgroFindEnemy(); }
        CurrentHealth = CurrentHealth - damag;
        if (CurrentHealth<=0)
        {
            unitManager.RemoveUnit(this, player);
            if(this== UIManager.Instance.unit)
            { UIManager.Instance.OnDeselectUnit(); }
            live = false;
            if (state == UnitState.MoveTarget)
            {
                _agent.ResetPath();
            }
            Instantiate(destroyEffect, turrent.transform.position, turrent.transform.rotation);
            Destroy (gameObject);
        }
    }
    public void CommandUnit(Unit commandUnit)
    {
        if (live)
        {
            StartCoroutine(SelectRingFlip(commandUnit));
            if (commandUnit.player != Players.Player1)
            {
                Agent.isStopped = false;
                comandTarget = commandUnit.gameObject;
                state = UnitState.MoveEnemy;
                AgroEnemy();
            }
            if (commandUnit.player == Players.Player1)
            {
                Agent.isStopped = false;
                comandTarget = commandUnit.gameObject;
                state = UnitState.MoveFrend;
            }
        }
    }
    public virtual void CommandBuild(Build commandBuild)
    {
        if (live)
        {
            StartCoroutine(commandBuild.SelectRingFlip(commandBuild));
            if (commandBuild.player != Players.Player1)
            {
                Agent.isStopped = false;
                comandTarget = commandBuild.gameObject;
                state = UnitState.MoveEnemy;
                AgroEnemy();
            }
            if (commandBuild.player == Players.Player1)
            {
                Agent.isStopped = false;
                comandTarget = commandBuild.gameObject;
                state = UnitState.MoveFrend;
            }
        }
    }
    public IEnumerator SelectRingFlip(Unit unit)
    {
        if (unit.live)
        {
            unit.selectionRing.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        if (unit.live) 
        { 
            unit.selectionRing.SetActive(false);
        }
    }
    public void ComandAttakTerritory(Vector3 comandPos)
    {
        Agent.isStopped = false;
        targetPosition =comandPos;
        state = UnitState.AttakTerritory;
    }
}
