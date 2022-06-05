using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Unit
{
    [SerializeField] private Build build;
    private float buildRadius = 23;
    void Start()
    {
        idlePosition = transform.position;
        Agent.speed = speed;
        
        isBusy = false;
        cameraMain = GameObject.Find("Main Camera");
        unitManager = cameraMain.GetComponent<UnitManager>();
        CurrentHealth = MaxHealth;
        GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        turrent.GetComponent<MeshRenderer>().material = UnitManager.Instance.GetUnitTexture(player);
        if (player != Players.Player1) { selectionRing.GetComponent<MeshRenderer>().material.color = Color.red; }
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
        if (build.CurrentHealth>= build.MaxHealth)
        {
            build.CurrentHealth = build.MaxHealth;
            build.building = false;
            if (build.nameUnit == "Командный центр")
            {
                MainBase mainBase=build.gameObject.GetComponent<MainBase>();
                mainBase.BaseAdd(mainBase.player);
            }
            build.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            state = UnitState.Idle;
            Agent.stoppingDistance = 0;
        }
        yield return new WaitForSeconds(1);
        isBusy = false;
    }
    public IEnumerator repearStep()
    {
        isBusy = true;
        build.CurrentHealth += build.MaxHealth / build.buildTime;
        if (build.CurrentHealth >= build.MaxHealth)
        {
            build.CurrentHealth = build.MaxHealth;
            build.building = false;
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
        }
    }
}
