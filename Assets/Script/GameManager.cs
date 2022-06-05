using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    private int waveNamber;
    private float timeStart;
    public Vector3 targetPos;
    public GameObject car;
    public GameObject tankM;
    public GameObject tankL;
    public GameObject tankMissile;
    public GameObject spawnZone1;
    public GameObject spawnZone2;
    public GameObject spawnZone3;
    public GameObject spawnZone4;
    private float spawnRangeX = 10;
    private float startDelay = 15;
    private float spawnInterval = 40f;
    private bool top = true;
    void Start()
    {
        TechnologyOff();
        waveNamber = 1;
        timeStart = startDelay;
        GameStat.player1money = 1200;
        InvokeRepeating("SpawnEnemy", startDelay, spawnInterval);
    }

    void Update()
    {
        waveText.text = "До следующей волны: " + Mathf.Round(timeStart);
        timeStart -= Time.deltaTime;
        if (timeStart <= 0) timeStart = spawnInterval;
    }

    private void SpawnEnemy()
    {
        GameObject new_;
        Unit unit;
        for (int i = 0; i < 2; i++)
        {
            if (top)
            {
                new_ = Instantiate(car, SpawnPos(spawnZone1), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
                new_ = Instantiate(car, SpawnPos(spawnZone2), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
            }
            if (!top)
            {
                new_ = Instantiate(car, SpawnPos(spawnZone3), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
                new_ = Instantiate(car, SpawnPos(spawnZone4), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
            }
        }
        for (int i = 0; i < 1; i++)
        {
            if (top)
            {
                new_ = Instantiate(tankM, SpawnPos(spawnZone1), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
                new_ = Instantiate(tankM, SpawnPos(spawnZone2), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
            }
            if (!top)
            {
                new_ = Instantiate(tankM, SpawnPos(spawnZone3), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
                new_ = Instantiate(tankM, SpawnPos(spawnZone4), car.transform.rotation) as GameObject;
                unit = new_.GetComponent<Unit>();
                unit.player = Players.Player2;
                unit.ComandAttakTerritory(targetPos);
            }
        }
        new_ = Instantiate(tankMissile, SpawnPos(spawnZone1), car.transform.rotation) as GameObject;
        unit = new_.GetComponent<Unit>();
        unit.player = Players.Player2;
        unit.ComandAttakTerritory(targetPos);
        new_ = Instantiate(tankMissile, SpawnPos(spawnZone2), car.transform.rotation) as GameObject;
        unit = new_.GetComponent<Unit>();
        unit.player = Players.Player2;
        unit.ComandAttakTerritory(targetPos);
        if (!top)
        {
            new_ = Instantiate(tankL, SpawnPos(spawnZone2), car.transform.rotation) as GameObject;
            unit = new_.GetComponent<Unit>();
            unit.player = Players.Player2;
            unit.ComandAttakTerritory(targetPos);
        }
        if (top)
        {
            top = false;
        }
        else
        {
            top = true;
        }

    }
    private Vector3 SpawnPos(GameObject spawnZone)
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnZone.transform.position.x - spawnRangeX, spawnZone.transform.position.x + spawnRangeX), 0, spawnZone.transform.position.z);
        return spawnPos;
    }
    private void TechnologyOff()
    {
        GameStat.player1Technology.heavyFactory = false;
        GameStat.player1Technology.heavyTank= false;
        GameStat.player1Technology.fireTank= false;
        GameStat.player1Technology.towerMulti = false;
        GameStat.player1Technology.RSZO = false;
    }
}
