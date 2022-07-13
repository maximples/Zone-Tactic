using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlLevel1 : MonoBehaviour
{
    public static ControlLevel1 Instance;
    public GameObject messengPanel;
    public GameObject tank;
    public GameObject laboratori;
    public GameObject target1;
    public GameObject target2;
    public GameObject position_;
    public GameObject startPanel;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI messageText;
    public AI enemyControl;
    public AIAllies alliesControl;
    public bool activ = false;
    private RectTransform rectTransform;
    private Rect rect;
    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform=messengPanel.GetComponent<RectTransform> ();
        Instance = this;
        GameStat.activ = false;
    }
    private void Update()
    {
        if (laboratori==null)
        { GameManager.Instance.Win("���������"); }
        if (target1 == null&&target2==null)
        { GameManager.Instance.Win("������"); }
    }
    // Update is called once per frame
    public void EventControl(int pointID)
    {
        if (pointID == 0)
        {
            messengPanel.SetActive(true);
            rectTransform.sizeDelta = new Vector2(350, 200);
            messageText.text = "��������:\n �� ���������� ������� �������";
            StopAllCoroutines();
            StartCoroutine(timeMesseng(5));
        }
        if (pointID == 1)
        {
            messengPanel.SetActive(true);
            rectTransform.sizeDelta = new Vector2(350, 200);
            messageText.text = "�������:\n ������� ������� �� ��������";
            StopAllCoroutines();
            StartCoroutine(timeMesseng(8));
        }
        if (pointID == 2)
        {
            messengPanel.SetActive(true);
            rectTransform.sizeDelta = new Vector2(350, 200);
            messageText.text = "�������:\n �� ����� ��������� ���� �����";
            StopAllCoroutines();
            StartCoroutine(timeMesseng(7));
        }
        if (pointID == 3)
        {
            messengPanel.SetActive(true);
            rectTransform.sizeDelta = new Vector2(350, 400);
            messageText.text = "����: \n����� ����� ���������� ��������� ����. �������� ��� ����� ���������, ��������� ����� ����� �� �����������, ��������� ����� �� ��������.";
            StopAllCoroutines();
            StartCoroutine(timeMesseng(25));
            enemyControl.start = true;
            alliesControl.start = true;
        }
        if (pointID == 4)
        {
            messengPanel.SetActive(true);
            rectTransform.sizeDelta = new Vector2(350, 400);
            messageText.text = "����: \n�� ���������� ���� ��������. ��������� ������� � ���. ���������� �������� ��������� ���� ����������.";
            questText.text = "�������� �����������. ��������� ���� �����";
            StopAllCoroutines();
            activ=true;
            alliesControl.ActivPlayer();
            StartCoroutine(timeMesseng(25));
            StartCoroutine(NewMesseng());
        }
    }
    IEnumerator timeMesseng (float time)
    {
        yield return new WaitForSeconds(time);
        messengPanel.SetActive(false);
    }
    IEnumerator NewMesseng()
    {
        yield return new WaitForSeconds(30);
        messengPanel.SetActive(true);
        rectTransform.sizeDelta = new Vector2(350, 350);
        messageText.text = "�������: \n�������� �� ���������� ������������ ������������������ �������. ��������� ���� ����� � ����� ����� ������.";
        StopAllCoroutines();
        StartCoroutine(timeMesseng(20));
        StartCoroutine(GetTank());
    }
    IEnumerator GetTank()
    {
        yield return new WaitForSeconds(200);
        messengPanel.SetActive(true);
        rectTransform.sizeDelta = new Vector2(350, 200);
        messageText.text = "�������: \n������� ������. ������� �������� ����� � ��������.";
        StartCoroutine(timeMesseng(10));
        Instantiate(tank, position_.transform.position, tank.transform.rotation);
        StartCoroutine(GetTank());
    }
    public void StartGame()
    {
        Destroy(startPanel);
        GameStat.activ = true;
    }
}
