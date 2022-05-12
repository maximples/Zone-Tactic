using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour {

    public List<BuildTemplate> Templates;

    public Build SelectBuild;

    private GameObject ghost;
    private BuildGhost ghostComponent;

    void OnGUI()
    {
        GUILayout.BeginVertical();
        foreach(BuildTemplate template in Templates)
        {
            if(GUILayout.Button(template.BuildIco, GUILayout.Width(50), GUILayout.Height(50)))
            {
                GameStat.actionState = ActionState.Build;
                ghost = Instantiate(template.BuildPrefab, CastFromCursor(), Quaternion.identity) as GameObject;
                ghostComponent = ghost.GetComponent<BuildGhost>();
            }
        }
        GUILayout.EndVertical();

        if(SelectBuild != null)
        {
            GUILayout.BeginArea(new Rect(0, Screen.height - 50, Screen.width, 50));
            GUILayout.BeginHorizontal();
            {
                foreach(Product p in SelectBuild.Products)
                {
                    if (GUILayout.Button(p.Name))
                    {
                        SelectBuild.ProductQueue.Enqueue(p);
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }

    void Update()
    {
        if (GameStat.actionState == ActionState.Build && ghost != null)
        {
            ghost.transform.position = CastFromCursor() + new Vector3(0, ghost.transform.localScale.y / 2, 0);
            if(!ghostComponent.IsClose && Input.GetMouseButtonDown(0))
            {
                GameStat.actionState = ActionState.Free;
                ghost.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
                ghost.GetComponent<Renderer>().material.color = Color.white;
                Destroy(ghostComponent);
                ghost = null;
            }
        }
    }

    Vector3 CastFromCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
        {
            return hit.point;
        }
        return new Vector3(0, 0, 0);
    }
}
