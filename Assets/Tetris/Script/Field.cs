using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static int w = 10;           //�� �ʺ�
    public static int h = 16;           //�� ����
    GameObject[] block;                 //���� ���� �ҷ��� �迭
    public GameObject s;                //������ ������ ������Ʈ
    GameObject d;                       //�ý��� ���� ������Ʈ
    List<GameObject> line = new List<GameObject>();     //������ �� ã���� Ȯ���� ����Ʈ
    List<GameObject> lineblock = new List<GameObject>();//������ �� ��
    List<int> deleteH = new List<int>();                //������ �� ����
    List<GameObject> upline = new List<GameObject>();   //���� ���� �� ����� ����Ʈ
    bool IsPause;                                       //�Ͻ����� Ȯ��
    public bool gameover;                               //���� ���� Ȯ��
    int count;                                          //�� ���� �� �����ӿ� ���
    bool alpha;                                         //�� ���� �� �����ӿ� ���
    AudioSource delete;                                 //�� ���� ȿ����
    // Start is called before the first frame update
    void Start()
    {
        d = GameObject.Find("Director");
        IsPause = false;                                //�Ͻ����� x
        gameover = true;                                //���ӿ���(�غ�ȭ��) Ȯ��
        alpha = true;
        count = 0;
        delete = GameObject.Find("Delete").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool BlockMove(GameObject b)         //�� �̵� �� ��ġ���� Ȯ��
    {
        for (int i = 0; i < 4; i++)             //���� ���� �����ϴ� ���� �� �� ��ŭ �ݺ�(�߽��� ����)
        {
            block = GameObject.FindGameObjectsWithTag("Block");             //�ʿ� �ִ� ��� ���� �� ����
            if (Mathf.Round(b.transform.GetChild(i).position.x) < 0)        //�� �ʺ� ����� ���ϰ� �۵�
            {
                b.transform.position += new Vector3(1, 0, 0);

            }
            else if (Mathf.Round(b.transform.GetChild(i).position.x) > 9)   //�� �ʺ� ����� ���ϰ� �۵�
            {
                b.transform.position -= new Vector3(1, 0, 0);
            }
            for (int x = 0; x < block.Length; x++)                          
            {
                if (Mathf.Round(block[x].transform.position.y) == Mathf.Round(b.transform.GetChild(i).position.y)       //���� y���� ������ Ȯ��
                    && Mathf.Round(block[x].transform.position.x) == Mathf.Round(b.transform.GetChild(i).position.x))   //���� x���� ������ Ȯ��
                {
                    if (block[x].transform.parent != b.transform)           //���� ������ Ȯ��
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public bool UnderBlock(GameObject b)        //�Ʒ� �� Ȯ��
    {
        for (int i = 0; i < 4; i++)
        {
            block = GameObject.FindGameObjectsWithTag("Block");
            if (Mathf.Round(b.transform.GetChild(i).position.y) < 1)    //�� ���̸� ����� ���ϰ� �ۿ�
            {
                b.transform.position += new Vector3(0, 1, 0);
                return false;
            }
            if (Mathf.Round(b.transform.GetChild(i).position.y) > 16)   //�� ���̸� ����� ���ϰ� �ۿ�
            {
                b.transform.position -= new Vector3(0, 1, 0);
                return false;
            }
            for (int x = 0; x < block.Length; x++)
            {
                if (Mathf.Round(block[x].transform.position.y) == Mathf.Round(b.transform.GetChild(i).position.y)       //���� y���� ������ Ȯ��
                    && Mathf.Round(block[x].transform.position.x) == Mathf.Round(b.transform.GetChild(i).position.x))   //���� x���� ������ Ȯ��
                {
                    if (block[x].transform.parent != b.transform)       //���� ������ Ȯ��
                    {
                        b.transform.position += new Vector3(0, 1, 0);
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public void CheckLine()                 //�� ���� ���� ä�������� Ȯ�� �� ����
    {
        deleteH.Clear();
        lineblock.Clear();
        for (int i = h+1; i > 0; i--)        //���� ��ŭ �ݺ�
        {
            block = GameObject.FindGameObjectsWithTag("Block"); //�ʿ� �ִ� ��� ��
            line.Clear();                   //����Ʈ �ʱ�ȭ
            
            for (int x = 0; x < block.Length; x++)  
            {
                if ((int)Mathf.Round(block[x].transform.position.x) > -1 && (int)Mathf.Round(block[x].transform.position.x) < 10)
                {
                    if ((int)Mathf.Round(block[x].transform.position.y) == i)   //������ ���̿� �ִ� �� �� Ȯ��
                    {
                        line.Add(block[x]);
                    }
                }
            }
            if (line.Count >= 10)       //�� �� >=�ʺ� ==> ����� ��
            {
                deleteH.Add(i);
                for (int y = 0; y < line.Count; y++)    //������ �� �� ����
                {
                    lineblock.Add(line[y]);
                }
                s.GetComponent<Score>().score += 10;    //���� �� ���� ȹ��
                //LineDown(i);                            //������ ���� ���� ��� ���� ������ �Լ�
            }
        }
        if (lineblock.Count >= 10)
        {
            deleteEffect();
        }
        Invoke("CreateBlock", 1.0f);
    }
    void deleteEffect()
    {
        if (count < 4)
        {
            if (alpha)
            {
                for (int i = 0; i < lineblock.Count; i++)
                {
                    Color c = lineblock[i].GetComponent<Renderer>().material.color;
                    c.a = 0f;
                    lineblock[i].GetComponent<Renderer>().material.color = c;    
                }
                Invoke("deleteEffect", 0.2f);
                alpha = false;
                count++;
            }
            else
            {
                for (int i = 0; i < lineblock.Count; i++)
                {
                    Color c = lineblock[i].GetComponent<Renderer>().material.color;
                    c.a = 1.0f;
                    lineblock[i].GetComponent<Renderer>().material.color = c;
                }
                Invoke("deleteEffect", 0.2f);
                alpha = true;
                count++;
            }
        }
        else
        {
            alpha = true;
            count = 0;
            deleteLine();
        }
    }
    void CreateBlock()
    {
        GameObject.Find("CreateShape").GetComponent<CreateShape>().Createshape();   //�� ����
    }
    void deleteLine()   //�� ���� �Լ�
    {
        for(int i = 0;i<lineblock.Count;i++)
        {
            Destroy(lineblock[i]);
        }
        for(int i = 0;i<deleteH.Count;i++)
        {
            LineDown(deleteH[i]);
        }
        lineblock.Clear();
        deleteH.Clear();
        delete.Play();
    }

    void LineDown(int h)        //������ ���� ���� ��� ���� ������ �Լ�
    {
        block = GameObject.FindGameObjectsWithTag("Block");
        upline.Clear();         //����Ʈ Ŭ����
        for (int x = 0; x < block.Length; x++)
        {
            if ((int)Mathf.Round(block[x].transform.position.x) > -1 && (int)Mathf.Round(block[x].transform.position.x) < 10)
            {
                if (Mathf.Round(block[x].transform.position.y) > h) //������ ���� ���� ��� �� ����
                {
                    upline.Add(block[x]);
                }
            }
        }
        for (int y = 0; y < upline.Count; y++)                  //�� ������
        {
            upline[y].transform.position -= new Vector3(0, 1, 0);
        }
        //upline.Clear();         
    }

    public bool CheckCreate(GameObject b)                       //���� �����Ǿ��� �� ��ġ�� �� Ȯ��
    {        
        for (int i = 0; i < 4; i++)
        {
            block = GameObject.FindGameObjectsWithTag("Block");

            for (int x = 0; x < block.Length; x++)
            {
                if (Mathf.Round(block[x].transform.position.y) == Mathf.Round(b.transform.GetChild(i).position.y)       //���� y���� ������ Ȯ��
                    && Mathf.Round(block[x].transform.position.x) == Mathf.Round(b.transform.GetChild(i).position.x))   //���� x���� ������ Ȯ��
                {
                    if (block[x].transform.parent != b.transform)   //���� ������ Ȯ��
                    {
                        Destroy(b);                                 //������ ���� �� ����
                        gameover = true;                            //���� ���� Ȯ��
                        d.GetComponent<Dir>().Gameover();           //���� ���� �Լ� ����
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void ResetBlock()                    //�� ����
    {
        Destroy(GameObject.Find("CreateShape").GetComponent<CreateShape>().t);  
        
        block = GameObject.FindGameObjectsWithTag("Block");
        for (int i=0;i<block.Length;i++)        //��� �� ����
        {
            Destroy(block[i]);
        }
        gameover = false;                       //���� ���� ����
        
    }
}
