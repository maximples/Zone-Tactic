using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSelect : MonoBehaviour {

    public UnitManager unitManager;
    public BuildManager buildManager;
    public Texture2D SelectTexture;
    public GameObject TargetPoint;

    private List<ISelect> selectObjects = new List<ISelect>();
    private Vector3 ?firstPoint = null;

    private void Start()
    {
        if(unitManager == null)
        {
            unitManager = GetComponent<UnitManager>();
        }
        if(buildManager == null)
        {
            buildManager = GetComponent<BuildManager>();
        }
    }

    private void Update()
    {
        if(GameStat.actionState == ActionState.Free)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = CastFromCursor();
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Ground" || hit.transform.tag == "Unit")
                    {
                        firstPoint = Input.mousePosition;
                    }
                    if (hit.transform.tag == "Build")
                    {
                        buildManager.SelectBuild = hit.transform.GetComponent<Build>();
                        selectObjects.ForEach(x => x.OnDeselect());
                        selectObjects.Clear();
                        selectObjects.Add(buildManager.SelectBuild);
                        hit.transform.SendMessage("OnSelect", (object)1);
                    }
                }
            }
            if (selectObjects.Count > 0 && Input.GetMouseButtonDown(1))
            {
                RaycastHit hit = CastFromCursor();
                if (hit.transform.tag == "Ground")
                {
                    GameObject target = Instantiate(TargetPoint, hit.point, Quaternion.identity) as GameObject;
                    TargetPoint targetPoint = target.GetComponent<TargetPoint>();
                    if (targetPoint == null)
                    {
                        Debug.Log("<color=red>TargetPoint Prefab is not valid, please add TargetPoint component</color>");
                    }
                    else
                    {
                        selectObjects.ForEach(x => x.OnSetTarget(targetPoint));
                    }
                }
            }
            if (firstPoint != null && Input.GetMouseButtonUp(0))
            {
                selectObjects.ForEach(x => x.OnDeselect());
                selectObjects.Clear();
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        foreach(Unit unit in unitManager.GetAllUnits())
        {
            Vector3[] points = { unit.GetComponent<Collider>().bounds.max, unit.GetComponent<Collider>().bounds.min, unit.transform.position };
            foreach(Vector3 point in points)
            {
                Vector3 screenPoint = Camera.main.WorldToScreenPoint(point);
                screenPoint = new Vector3(screenPoint.x, Screen.height - screenPoint.y, screenPoint.z);
                if(rect.Contains(screenPoint))
                {
                    selectObjects.Add(unit);
                    unit.SendMessage("OnSelect", selectObjects.Count);
                    break;
                }
            }
        }
    }
}
