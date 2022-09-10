using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public float fallspeed;             //�������� �ӵ�(�� �� �̵��ϴµ� �ɸ��� �ð�)
    float movespeed;                    //�¿� �̵� �Ÿ�
    public bool target;                        //���۰����� �� Ȯ��
    GameObject field;                   //���� �����ϴ� ������Ʈ
    GameObject dir;                     //�ý��� ���� ������Ʈ
    public bool save;
    // Start is called before the first frame update
    void Start()
    {
        save = false;
        dir = GameObject.Find("Director");
        field = GameObject.Find("Field");
        target = true;                  //�����Ǹ� ���۰����� ������ ����
        fallspeed = 1;                  
        movespeed = 1;
        Invoke("FallShape", fallspeed); //���� �������� �Լ�
    }

    // Update is called once per frame
    void Update()
    {
        if (!field.GetComponent<Field>().gameover)      //���ӿ����� �ƴ� �� �۵�
        {
            if (target && !dir.GetComponent<Dir>().IsPause)     //�����̴� ����̰� �Ͻ������� �ƴ� �� �۵�
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))        
                {
                    this.transform.position += new Vector3(-movespeed, 0, 0);
                    if (!field.GetComponent<Field>().BlockMove(this.gameObject))    //�̵��� ���⿡ �ٸ� ���� �ִ��� && ���� �ʺ� ������� Ȯ��
                        this.transform.position += new Vector3(+movespeed, 0, 0);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    this.transform.position += new Vector3(movespeed, 0, 0);
                    if (!field.GetComponent<Field>().BlockMove(this.gameObject))    //�̵��� ���⿡ �ٸ� ���� �ִ��� && ���� �ʺ� ������� Ȯ��
                        this.transform.position += new Vector3(-movespeed, 0, 0);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))  
                {
                    this.transform.RotateAround(this.transform.GetChild(4).position, new Vector3(0, 0, 1), 90.0f);  //������ �߽��� �������� �� ȸ��
                    if (!field.GetComponent<Field>().BlockMove(this.gameObject))    //ȸ���� ���⿡ �ٸ� ���� �ִ��� && ���� �ʺ� ������� Ȯ��
                    {
                        this.transform.RotateAround(this.transform.GetChild(4).position, new Vector3(0, 0, 1), -90.0f);
                    }
                    field.GetComponent<Field>().UnderBlock(this.gameObject);        //���� ���̸� ����� ��ġ����

                }
                if (Input.GetKeyDown(KeyCode.DownArrow))                            //������ ���� ���� �������� �ӵ� ���
                {
                    fallspeed = 0.02f;
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))                              //���� ���� ���� �ӵ��� ����
                {
                    fallspeed = 1f;
                }
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    while(field.GetComponent<Field>().UnderBlock(this.gameObject))
                    {
                        this.transform.position += new Vector3(0, -1, 0);
                    }
                    GameObject.Find("CreateShape").GetComponent<CreateShape>().change = false;
                    target = false;     //���� ����
                    ChangeTarget();
                }
            }
            if (this.transform.childCount == 1)                                     //����� �����ϴ� ���� ��� ������ٸ� �߽��� ����ϴ� ������Ʈ ����
            {
                Destroy(this.gameObject);
            }
        }
        
        
    }
    public void FallShape()        //���� ����߸��� �Լ�
    {
        if (target)
        {
            this.transform.position += new Vector3(0, -1, 0);
            if (!field.GetComponent<Field>().UnderBlock(this.gameObject))     //�ؿ� ���� ������ 1�� �� ��� ����
            {
                GameObject.Find("CreateShape").GetComponent<CreateShape>().change = false;
                Invoke("ChangeTarget", 1f);
            }
            else
            {
                Invoke("FallShape", fallspeed);
            }
            /*else if(!save)                                                                  //����� ���°� �ƴϸ� �ݺ� ����
            {
                target = true;
                //this.transform.position += new Vector3(0, -1, 0);
                Invoke("FallShape", fallspeed);
            }
            else if (target == false)                                             //Ÿ���� �ƴϸ� �� ������
            {

            }*/
        }

    }
    void ChangeTarget()     //��� ���� �Լ�
    {
        //field.GetComponent<Field>().CheckLine();    //�� ���� ��� ä�������� Ȯ�� �� ����
        
       field.GetComponent<Field>().CheckLine();    //�� ���� ��� ä�������� Ȯ�� �� ����
        target = false;     //���� ����
    }

}
