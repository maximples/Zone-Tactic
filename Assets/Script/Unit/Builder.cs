using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Unit
{  
    private Build build;
    [SerializeField] private ParticleSystem repireEffect;
    private GameObject repirePos;
    private GameObject repireTarget;
    private float buildRadius = 23;
    void Awake()
    {
        GetObject();
    }
    void Start()
    {
        repirePos = turrent.transform.Find("Pos").gameObject; 
        idlePosition = transform.position;
        Agent.speed = speed;
        if (player == Players.Player1)
        {
            GameObject mask_ = Instantiate(maskFog, transform.position, transform.rotation) as GameObject;
            mask_.transform.parent = transform;
        }
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        GetColor();
        if (player != Players.Player1) 
        {
            Projector myProjector = selectionRing.GetComponent<Projector>();
            myProjector.material = GameManager.Instance.player2Material;
        }
        if (player == Players.Player1)
        {
            gameObject.layer = 6;
        }
        if (player == Players.Player2)
        {
            gameObject.layer = 7;
        }
        UnitManager.Instance.AddUnit(this, player);
    }

    void Update()
    {
        if (live && GameStat.activ)
        {
            switch (state)
            {

                case UnitState.Idle:
                    {
                        if (turrent.transform.localEulerAngles.y > 2)
                        {

                            if (turrent.transform.localEulerAngles.y > 180)
                            {
                                turrent.transform.Rotate(Vector3.up);
                            }
                            else
                            {
                                turrent.transform.Rotate(Vector3.down);
                            }
                        }
                        if (Target != null)
                        {
                            Target.RemoveLink();
                            Target = null;
                        }
                        if(!isBusy)
                        {StartCoroutine(FindRepire(3, agroRadius)); }
                    }
                    break;
                case UnitState.MoveTarget:
                    {
                     
                        Agent.SetDestination(targetPosition);
                        if (Vector3.Distance(transform.position, targetPosition) <= 3)
                        {
                            Agent.isStopped = true;
                            idlePosition = transform.position;
                            state = UnitState.Idle;
                        }

                    }
                    break;
                case UnitState.MoveEnemy:
                    {
                        if (comandTarget != null)
                        {
                            if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) > attackRadius * attackRadius)
                            {
                                Agent.SetDestination(comandTarget.transform.position);
                            }
                            if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) < attackRadius * attackRadius)
                            {
                                enemyTarget = comandTarget;
                                Agent.isStopped = true;
                            }
 
                        }
                        if (comandTarget == null)
                        {
                            _agent.ResetPath();
                            idlePosition = transform.position;
                            state = UnitState.Idle;
                        }
                    }
                    break;
                case UnitState.MoveFrend:
                    {
                        if (comandTarget != null)
                        {
                            Agent.SetDestination(comandTarget.transform.position);
                        }
                        else

                        if (comandTarget == null)
                        {
                            Agent.isStopped = true;
                            idlePosition = transform.position;
                            state = UnitState.Idle;
                        }

                    }
                    break;
                case UnitState.Building:
                    {
                        if (comandTarget != null)
                        {
                            Agent.SetDestination(comandTarget.transform.position);
                            if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) < buildRadius* buildRadius)
                            {
                                if (build.building&& !isBusy )
                                {
                                    turrent.transform.LookAt(comandTarget.transform.position);
                                    StartCoroutine(buildingStep());
                                }
                            }
                        }
                        else

                        if (comandTarget == null)
                        {
                            Agent.isStopped = true;
                            idlePosition = transform.position;
                            state = UnitState.Idle;
                            Agent.stoppingDistance = 0;
                        }
                        if (build.CurrentHealth >= build.MaxHealth)
                        {
                            state = UnitState.Idle;
                            Agent.ResetPath();
                            Agent.stoppingDistance = 0;
                        }
                    }
                    break;
                case UnitState.Repear:
                    {
                        if (comandTarget != null)
                        {
                            Agent.SetDestination(comandTarget.transform.position);
                            if (Vector3.SqrMagnitude(comandTarget.transform.position - transform.position) < buildRadius * buildRadius)
                            {
                                if (!isBusy&& build.CurrentHealth < build.MaxHealth)
                                {
                                    turrent.transform.LookAt(comandTarget.transform.position);
                                    StartCoroutine(repearStep());
                                }
                            }
                        }
                        else

                        if (comandTarget == null)
                        {
                            Agent.isStopped = true;
                            idlePosition = transform.position;
                            state = UnitState.Idle;
                            Agent.stoppingDistance = 0;
                        }
                        if (build.CurrentHealth >= build.MaxHealth)
                        {
                            state = UnitState.Idle;
                            Agent.stoppingDistance = 0;
                        }

                    }
                    break;
                case UnitState.AgresivStand:
                    {
                        state = UnitState.Idle;
                    }
                    break;
            }
        }

    }
   public void Building(GameObject building)
    {
        Agent.isStopped = false;
        comandTarget = building;
        build = building.GetComponent<Build>();
        state = UnitState.Building;
        Agent.stoppingDistance = 14;
    }
    public IEnumerator buildingStep()
    {
        isBusy = true;
        build.CurrentHealth += build.MaxHealth/ build.buildTime;
        build.transfom = build.transfom + 1.5f / build.buildTime;
        build.transform.localScale = new Vector3(1.5f, build.transfom, 1.5f);
        Instantiate(repireEffect, repirePos.transform.position, build.transform.rotation);
        if (build.CurrentHealth >= build.MaxHealth)
        {
            build.CurrentHealth = build.MaxHealth;
            build.building = false;
            if (player == Players.Player1)
            {
                GameObject mask_ = Instantiate(build.maskFog, build.transform.position, build.transform.rotation) as GameObject;
                mask_.transform.parent = build.transform;
            }
            if (build.tipBuild == TipBuild.MainBase)
            {
                MainBase mainBase = build.gameObject.GetComponent<MainBase>();
                mainBase.BaseAdd(mainBase.player);
            }
            if (build.tipBuild == TipBuild.RepireTover)
            {

                BoxCollider box = build.GetComponent<BoxCollider>();
                box.size = new Vector3(3, 5, 3);
            }

            build.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            state = UnitState.Idle;
            Agent.stoppingDistance = 0;
            Agent.ResetPath();
        }
        yield return new WaitForSeconds(1);
        isBusy = false;
    }
    public IEnumerator repearStep()
    {
        isBusy = true;
        build.CurrentHealth = Mathf.Round(build.CurrentHealth+build.MaxHealth / build.buildTime);
        Instantiate(repireEffect, repirePos.transform.position, build.transform.rotation);
        if (build.CurrentHealth >= build.MaxHealth)
        {
            build.CurrentHealth = build.MaxHealth;
            state = UnitState.Idle;
            Agent.stoppingDistance = 0;
        }
        yield return new WaitForSeconds(1);
        isBusy = false;
    }
    public override void CommandBuild(Build commandBuild)
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
                if (commandBuild.building)
                {
                    build=commandBuild; 
                    comandTarget = commandBuild.gameObject;
                    Agent.isStopped = false;
                    Agent.stoppingDistance = 14;
                    state = UnitState.Building;
                }
                else
                {
                    if (commandBuild.CurrentHealth< commandBuild.MaxHealth)
                    {
                        build = commandBuild;
                        comandTarget = commandBuild.gameObject;
                        Agent.isStopped = false;
                        state = UnitState.Repear;
                        Agent.stoppingDistance = 14;
                    }
                    else 
                    {
                        Agent.isStopped = false;
                        comandTarget = commandBuild.gameObject;
                        state = UnitState.MoveFrend;
                    }
                }
            }
            if (Target != null)
            {
                Target.RemoveLink();
                Target = null;
            }
        }
    }
    public IEnumerator FindRepire(float time, float radius)
    {

        isBusy = true;
        foreach (Build build_ in BuildManager.Instance.GetAllBuilds(player, Force.Allies))
        {
            if (build_.live&&build_.CurrentHealth<build_.MaxHealth&&!build_.building)
            {
                if (Vector3.SqrMagnitude(build_.gameObject.transform.position - transform.position) < radius * radius)
                {
                    comandTarget = build_.gameObject;
                    build = build_;
                    state = UnitState.Repear;
                    Agent.isStopped = false;
                    Agent.SetDestination(comandTarget.transform.position);
                    break;
                }
            }
        }
        yield return new WaitForSeconds(time + randTime);
        isBusy = false;
    }
}
