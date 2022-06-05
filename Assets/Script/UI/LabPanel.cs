using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabPanel : MonoBehaviour
{
    public Laboratory laboratory;

    public void BuilUnitBotton_0_0()
    {

      *//  Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        if (GameStat.player1money >= fabrica.products[0].price)
        {
            GameStat.player1money -= fabrica.products[0].price;
            fabrica.BuildProduct(0);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(UIManager.Instance.Message("Недостаточно денег"));
        }
    }
    public void EnterBotton_0_0()
    {
        Fabrica_M fabrica = build.gameObject.GetComponent<Fabrica_M>();
        nameUnitPrefab.text = fabrica.products[0].Name;
        costUnit.text = "Цена:  " + fabrica.products[0].price + " Время: " + fabrica.products[0].ConstructTime;
        description.text = fabrica.products[0].description;
    }
    public void ExiteBotton()
    {
        nameUnitPrefab.text = "";
        costUnit.text = "";
        description.text = "";
    }
}
