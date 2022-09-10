using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    GameObject s;                   //���� ���� �� ��Ÿ�� ��������
    GameObject hs;                  //���� ���� �� ��Ÿ�� ���� �ְ� ����
    public GameObject sco;          //���â���� ��Ÿ�� ���� ����
    public GameObject hsc;          //���â���� ��Ÿ�� �ְ� ����
    GameObject d;                   //�ý��� ���� ������Ʈ
    public GameObject sc;           //�Ͻ� ���� �� ������ ���� ����
    public int score;               //���� ���� ����
    private int highscore = 0;      //�ְ� ���� ����
    string KeyName = "HighScore";   //�ְ������� �޸𸮿� �����ϴ� Ű����
    int lasthighscore = 0;          //���� �ְ�����
    // Start is called before the first frame update
    void Awake()
    {
        //PlayerPrefs.DeleteAll();  //�޸𸮿� ����� �ְ����� ����
        s = GameObject.Find("Score");
        hs = GameObject.Find("HighScore");
        d = GameObject.Find("Director");
        score = 0;                  //���� ���� �ʱ�ȭ
        highscore = PlayerPrefs.GetInt(KeyName,0);  //�޸𸮿� ����� �ְ� ���� �ҷ�����
        hs.GetComponent<Text>().text = $"{highscore.ToString("0")}";    //�ְ����� ���
        
    }

    // Update is called once per frame
    void Update()
    {
        s.GetComponent<Text>().text = score.ToString("0");          //�������� ǥ��(������)
        sco.GetComponent<Text>().text = score.ToString("0");        //�������� ǥ��(���â)
        
        if (score>highscore)           //������ ����Ǿ��� �� ������ �ְ��������� ���� ���
        {
            hs.GetComponent<Text>().text = score.ToString("0");
            if (d.GetComponent<Dir>().IsEnd)
            {
                PlayerPrefs.SetInt(KeyName, score);
                highscore = PlayerPrefs.GetInt(KeyName, 0);//�ְ����� ����(�޸�)
                hsc.GetComponent<Text>().text = $"{highscore.ToString("0")}";
            }
        }
        else
        {
            hs.GetComponent<Text>().text = $"{highscore.ToString("0")}";
        }
        hsc.GetComponent<Text>().text = $"{highscore.ToString("0")}";
        if(GameObject.Find("Director").GetComponent<Dir>().IsPause)
        {
            sc.GetComponent<Text>().text = score.ToString("0");
        }
        
    }
}
