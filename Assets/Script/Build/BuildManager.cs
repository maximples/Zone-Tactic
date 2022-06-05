using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class BuildManager : MonoBehaviour {

    public static BuildManager Instance;
    public BuildTemplate[] templates;
    public Build SelectBuild;
    private List<Build> BuildsPlayer1;
    private List<Build> BuildsPlayer2;
    private Camera mainCamera;
    private GameObject ghost;
    private BuildGhost ghostComponent;
    private int layerMask = 1 << 3;
    private Builder builder;
    private float rotetSpeed = 35;
    private int Id;
    private void Awake()
    {
        Instance = this;
        BuildsPlayer1 = new List<Build>();
        BuildsPlayer2 = new List<Build>();
        mainCamera = Camera.main;
        
    }
    private void FixedUpdate()
    {
        if (ghost != null)
        {
            if (Input.GetKey(KeyCode.R))
            {
                ghost.transform.Rotate(Vector3.up * Time.deltaTime * rotetSpeed);
            }
        }
    }
    void Update()
    {
        if (GameStat.actionState == ActionState.Build && ghost != null)
        {
            ghost.transform.position = CastFromCursor();// new Vector3(0, ghost.transform.localScale.y / 2, 0);
            if(!ghostComponent.IsClose && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                GameStat.player1money -= templates[Id].price;
                GameStat.actionState = ActionState.Free;
                ghostComponent = ghost.GetComponent<BuildGhost>();
                ghost.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                builder.Building(ghost);
                ghostComponent.goodPos.SetActive(false);
                MiningStation miningStation;
                miningStation=ghost.gameObject.GetComponent<MiningStation>();
                if (miningStation != null)
                {
                    miningStation.resursPoint.GetComponent<BoxCollider>().enabled = false;
                    miningStation.transform.position = miningStation.resursPoint.transform.position;
                }
                Build build =ghost.gameObject.GetComponent<Build>();
                build.building = true;
                build.live = true;
                build.CurrentHealth = 0;
                build.transform.localScale = new Vector3(1.5f, 0.001f, 1.5f);
                AddBuild(build, Players.Player1);
                Destroy(ghostComponent);
                ghost = null;
            }
            if (Input.GetMouseButtonDown(1) )
            {
                GameStat.actionState = ActionState.Free;
                Destroy(ghost);
                ghost = null;
            }
        }
  
    }
    public void AddBuild(Build build, Players controller)
    {
        if (controller == Players.Player1) { BuildsPlayer1.Add(build); }
        if (controller == Players.Player2) { BuildsPlayer2.Add(build); }
    }

    public void RemoveUnit(Build build, Players controller)
    {
        if (controller == Players.Player1) { BuildsPlayer1.Remove(build); }
        if (controller == Players.Player2) { BuildsPlayer2.Remove(build); }

    }

    public Build[] GetAllBuilds(Players controller, Force getForce)
    {
        if (getForce == Force.Allies)
        {
            if (controller == Players.Player1)
            {
                return BuildsPlayer1.ToArray();
            }
            if (controller == Players.Player2)
            {
                return BuildsPlayer2.ToArray();
            }
        }
        if (getForce == Force.Enemies)
        {
            if (controller == Players.Player1)
            {
                return BuildsPlayer2.ToArray();
            }
            if (controller == Players.Player2)
            {
                return BuildsPlayer1.ToArray();
            }
        }
        return BuildsPlayer1.ToArray();
    }

    public void buildBuilding(int ID,Builder newBuilder)
    {
        Id = ID;
        if (GameStat.actionState == ActionState.Free)
        {
            GameStat.actionState = ActionState.Build;
            ghost = Instantiate(templates[ID].BuildPrefab, CastFromCursor(), Quaternion.identity) as GameObject;
            ghostComponent = ghost.GetComponent<BuildGhost>();
            Build build = ghost.gameObject.GetComponent<Build>();
            build.building = true;
            build.live = false;
            builder = newBuilder;
        }
    }

    Vector3 CastFromCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit,Mathf.Infinity, layerMask);
        if (hit.transform.tag == "Ground")
        {
            return hit.point;
        }
        return ghost.transform.position;
    }
}
