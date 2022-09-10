using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dir : MonoBehaviour
{
    GameObject f;           //���� �ٷ�� ������Ʈ
    public GameObject p;    //�Ͻ����� �˾�
    public GameObject g;    //���� ���� �˾�
    public GameObject m;    //�غ� ȭ�鿡���� UI
    public GameObject u;    //���� ȭ�鿡���� UI
    public bool IsPause;    //�Ͻ� ���� Ȯ��
    public bool restart;    //���� �ٽ� ���� Ȯ��
    public bool IsEnd;      //������ �������� Ȯ��
    AudioSource click;      //��ư ȿ����
    AudioSource bgm;        //�����
    // Start is called before the first frame update
    private void Awake()
    {
        Screen.SetResolution(720, 800, false);
    }
    void Start()
    {
        restart = false;    //����� x
        IsEnd = false;      //������ ����
        IsPause = false;    //�Ͻ����� x
        f = GameObject.Find("Field");
        click = GameObject.Find("Click").GetComponent<AudioSource>();
        bgm = GameObject.Find("Bgm").GetComponent<AudioSource>();
        bgm.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (g.activeSelf == true)
        {
            bgm.volume -= Time.deltaTime * 0.2f;
        }
        else if (IsPause == false)
        {
            bgm.volume += Time.deltaTime * 0.1f;
        }

    }
    public void GamePause()
    {
        if (IsPause == false)       //�Ͻ����� ���°� �ƴ� ��
        {
            Time.timeScale = 0.0f;  //�ð� ����
            IsPause = true;         //�Ͻ� ����
            p.SetActive(true);      //�Ͻ� ���� �˾� on
            bgm.Pause();            //����� �Ͻ� ����
        }
    }
    public void Continue()
    {
        click.Play();
        if (IsPause == true)        //�Ͻ����� ������ ��
        {
            Time.timeScale = 1.0f;  //�ð� ���
            IsPause = false;        //�Ͻ� ���� ����
            p.SetActive(false);     //�Ͻ� ���� �˾� off
            bgm.Play();
        }
    }
    public void ReStart()
    {
        click.Play();
        bgm.Stop();
        restart = true;             //����� on
        Time.timeScale = 1.0f;      //�ð� ���
        f.GetComponent<Field>().ResetBlock();       //�� ����
        if (p.activeSelf == true && IsPause == true)  //�Ͻ����� �����̰� �˾��� ���� ���
        {
            IsPause = false;                        //�Ͻ� ���� ����
            p.SetActive(false);                     //�˾� ����
        }
        if (g.activeSelf == true && IsEnd == true)      //���� ���� �����̰� �˾��� ���� ���
        {
            IsEnd = false;                          //���� ���� ����
            g.SetActive(false);                     //���� ���� �˾� ����
        }
        //GameObject.Find("CreateShape").GetComponent<CreateShape>().Createshape();   //���� �� ����
        u.GetComponent<Score>().score = 0;          //���� �ʱ�ȭ
        bgm.Play();
        bgm.volume = 0;
        GameObject.Find("CreateShape").GetComponent<CreateShape>().next.Clear();
        GameObject.Find("CreateShape").GetComponent<CreateShape>().change = true;


    }
    public void Gameover()
    {
        if (IsEnd == false)                         //���� ���� ���°� �ƴ� ��
        {
            f.GetComponent<Field>().ResetBlock();   //�� ����
            IsEnd = true;                           //���� ���� 
            g.SetActive(true);                      //���� ���� �˾� on
        }
    }
    public void GameStart()
    {
        Time.timeScale = 1.0f;                      //�ð� ���
        u.SetActive(true);                          //���� ui on
        m.SetActive(false);                         //�غ� ui off
        f.GetComponent<Field>().gameover = false;   //���� ���� ���� ����
        ReStart();                                  //�����
        click.Play();                               //ȿ���� ���
        bgm.Play();                                 //����� ���
        bgm.volume = 0;
        GameObject.Find("CreateShape").GetComponent<CreateShape>().next.Clear();
        Destroy(GameObject.Find("CreateShape").GetComponent<CreateShape>().saveblock);
        GameObject.Find("CreateShape").GetComponent<CreateShape>().blocksave = false;
        GameObject.Find("CreateShape").GetComponent<CreateShape>().change = true;
    }
    public void GameExit()
    {
        f.GetComponent<Field>().gameover = true;    //���� ����(���� ����)on
        Time.timeScale = 0.0f;                      //�ð� ����
        u.SetActive(false);                         //���� ui off
        m.SetActive(true);                          //���� ui on
        f.GetComponent<Field>().ResetBlock();       //�� ����

        bgm.Stop();

        click.Play();
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
