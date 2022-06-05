using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ObjectSelect : MonoBehaviour {

    [SerializeField] private UnitManager unitManager;
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Texture2D SelectTexture;
    public GameObject TargetPoint;
    [SerializeField] private GameObject managerUI;
    private List<ISelect> selectObjects = new List<ISelect>();
    private Vector3 ?firstPoint = null;
    private Camera mainCamera;
    private bool selectEnemy = false;
    private void Start()
    {
        mainCamera = Camera.main;
        if (unitManager == null)
        {
            unitManager = GetComponent<UnitManager>();
        }
        if(buildManager == null)
        {
            buildManager = GetComponent<BuildManager>();
        }
        if (uiManager == null)
        {
            uiManager = managerUI.GetComponent<UIManager>();
        }
    }

    private void Update()
    {
        if(GameStat.actionState == ActionState.Free)
        {

            if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
            {
                uiManager.OnDeselectUnit();
                selectEnemy = false;
                RaycastHit hit = CastFromCursor();
                if(unitManager.SelectUnit != null)
                {
                    unitManager.SelectUnit.OnDeselect();
                    unitManager.SelectUnit = null;
                }
                if (buildManager.SelectBuild != null)
                {
                    buildManager.SelectBuild.OnDeselect();
                    buildManager.SelectBuild = null;
                }
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Ground" )
                    {
                        
                        firstPoint = Input.mousePosition;
                        if (unitManager.SelectUnit!=null)
                        { 
                            unitManager.SelectUnit.OnDeselect();
                            unitManager.SelectUnit = null;
                         }
                    }
                    if (hit.transform.tag == "Unit")
                    {
                        unitManager.SelectUnit = hit.transform.GetComponent<Unit>();
                        uiManager.OnSelectUnit(hit.collider.gameObject);
                        if (unitManager.SelectUnit.player == Players.Player1)
                        {
                             firstPoint = Input.mousePosition;
                        }
                        if (unitManager.SelectUnit.player == Players.Player2)
                        {
                            selectEnemy = true;
                            unitManager.SelectUnit.OnSelect(1);
                        }                          
                    }
                    if (hit.transform.tag == "Build")
                    {
                        buildManager.SelectBuild = hit.transform.GetComponent<Build>();
                        uiManager.OnSelectBuild(buildManager.SelectBuild);
                        selectObjects.ForEach(x => x.OnDeselect());
                        selectObjects.Clear();
                        selectObjects.Add(buildManager.SelectBuild);
                        hit.transform.SendMessage("OnSelect", (object)1);
                    }
                }
            }
            if (selectObjects.Count > 0 && Input.GetMouseButtonDown(1)&& !EventSystem.current.IsPointerOverGameObject()&&!selectEnemy)
            {
                RaycastHit hit = CastFromCursor();
                if (hit.transform.tag == "Ground")
                {
                    GameObject target = Instantiate(TargetPoint, hit.point, Quaternion.identity) as GameObject;
                    TargetPoint targetPoint = target.GetComponent<TargetPoint>();
                    if (buildManager.SelectBuild!= null)
                    {
                        buildManager.SelectBuild.OnSetTarget(targetPoint,1) ;
                    }
                    else
                    {
                        //selectObjects.ForEach(x => x.OnSetTarget(targetPoint));
                        int i = 1;
                        foreach (Unit unit in selectObjects)
                        {
                            if (unit.live)
                            {
                                unit.OnSetTarget(targetPoint, i);
                                i++;
                            }
                        }

                    }
                }
                if (hit.transform.tag == "Unit")
                {
                    Unit tagretUnit = hit.transform.GetComponent<Unit>();
                    foreach (Unit unit in selectObjects)
                    {
                        if (unit.live)
                        {
                            unit.CommandUnit(tagretUnit);
                        }
                    }
                }
                if (hit.transform.tag == "Build")
                {
                    Build tagretBuild = hit.transform.GetComponent<Build>();
                    foreach (Unit unit in selectObjects)
                    {
                        if (unit.live)
                        {
                            unit.CommandBuild(tagretBuild);
                        }
                    }
                }
            }
            if (firstPoint != null && Input.GetMouseButtonUp(0) )
            {
                selectObjects.ForEach(x => x.OnDeselect());
                selectObjects.Clear();
                if (unitManager.SelectUnit != null)
                {
                    selectObjects.Add(unitManager.SelectUnit);
                    unitManager.SelectUnit.SendMessage("OnSelect", (object)1);
                }
                Check((Vector3)firstPoint, Input.mousePosition);
                firstPoint = null;
            }
        }

        if(firstPoint != null && GameStat.actionState == ActionState.Build)
        {
            firstPoint = null; 
        }
    }

    private RaycastHit CastFromCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit; 
    }
    

    private void OnGUI()
    {
        if(firstPoint != null && SelectTexture != null)
        {
            GUI.DrawTexture(GetRectFromPoints((Vector3)firstPoint, Input.mousePosition), SelectTexture);
        }
    }

    private Rect GetRectFromPoints(Vector3 one, Vector3 two)
    {
        float width = two.x - one.x;
        float height = (Screen.height - two.y) - (Screen.height - one.y); 
        return new Rect(one.x, Screen.height - one.y, width, height);
    }

    private void Check(Vector3 firstPoint, Vector3 endPoint)
    {
        if (endPoint.x < firstPoint.x)
        {
            var x1 = firstPoint.x;
            var x2 = endPoint.x;
            firstPoint.x = x2;
            endPoint.x = x1;
        }

        if (endPoint.y > firstPoint.y)
        {
            var y1 = firstPoint.y;
            var y2 = endPoint.y;
            firstPoint.y = y2;
            endPoint.y = y1;
        }

        Rect rect = GetRectFromPoints((Vector3)firstPoint, endPoint);
        foreach(Unit unit in unitManager.GetAllUnits(Players.Player1,Force.Allies))
        {
            Vector3[] points = { unit.GetComponent<Collider>().bounds.max, unit.GetComponent<Collider>().bounds.min, unit.transform.position };
            foreach(Vector3 point in points)
            {
                Vector3 screenPoint = mainCamera.WorldToScreenPoint(point);
                screenPoint = new Vector3(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
                if(rect.Contains(screenPoint))
                {        
                    selectObjects.Add(unit);
                    if (selectObjects.Count == 1)
                    {
                        uiManager.OnSelectUnit(unit.gameObject);
                    }
                    unit.SendMessage("OnSelect", selectObjects.Count);
                    break;
                }
            }
        }
        foreach (Unit unit in unitManager.GetAllAirUnits(Players.Player1, Force.Allies))
        {
            Vector3[] points = { unit.GetComponent<Collider>().bounds.max, unit.GetComponent<Collider>().bounds.min, unit.transform.position };
            foreach (Vector3 point in points)
            {
                Vector3 screenPoint = mainCamera.WorldToScreenPoint(point);
                screenPoint = new Vector3(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
                if (rect.Contains(screenPoint))
                {
                    selectObjects.Add(unit);
                    if (selectObjects.Count == 1)
                    {
                        uiManager.OnSelectUnit(unit.gameObject);
                    }
                    unit.SendMessage("OnSelect", selectObjects.Count);
                    break;
                }
            }
        }
    }
}
