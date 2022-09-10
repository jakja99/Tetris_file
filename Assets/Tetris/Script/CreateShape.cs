using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShape : MonoBehaviour
{
    public GameObject[] shapes;     //블럭의 종류를 저장할 배열
    public GameObject t;            //움직일 블럭
    GameObject f;                   //맵을 관리하는 오브젝트
    GameObject d;
    public List<GameObject> next = new List<GameObject>();
    public GameObject[] nextblock;
    public bool blocksave;
    public GameObject s;
    public bool change;
    GameObject temp;
    public GameObject saveblock;
    // Start is called before the first frame update
    private void Awake()
    {
        change = true;
        blocksave = false;
    }
    void Start()
    {
        f = GameObject.Find("Field");
        d = GameObject.Find("Director");
    }

    // Update is called once per frame
    void Update()
    {
        if (d.GetComponent<Dir>().restart && !GameObject.FindGameObjectWithTag("Block"))
        {
            Createshape();
            Destroy(saveblock);
            blocksave = false;
            d.GetComponent<Dir>().restart = false;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            if (change)
            {
                t.transform.rotation = Quaternion.Euler(0, 0, 0);
                if (blocksave == false)
                {
                    blocksave = true;
                    t.GetComponent<Fall>().target = false;
                    if(t.name.Contains("0"))
                    {
                        t.transform.position = new Vector3(s.transform.position.x - 0.5f, s.transform.position.y - 0.5f, s.transform.position.z);
                    }
                    else if(t.name.Contains("1"))
                    {
                        t.transform.position = new Vector3(s.transform.position.x - 0.55f, s.transform.position.y, s.transform.position.z);
                    }
                    else
                    {
                        t.transform.position = new Vector3(s.transform.position.x, s.transform.position.y - 0.5f, s.transform.position.z);
                    }
                    
                    saveblock = t;
                    Createshape();
                }
                else if (blocksave == true)
                {
                    ChangeBlock();
                }
            }
            change = false;
        }
    }
    public void Createshape()   //블럭 생성
    {
        if(next.Count ==0)
        {
            for(int i=0;i<3;i++)
            {
                next.Add(Instantiate((shapes[Random.Range(0, shapes.Length)]), nextblock[i].transform.position, Quaternion.identity));
            }
        }
        if (!f.GetComponent<Field>().gameover)  //게임 오버가 아닐 때 작동
        {
            //t = Instantiate((shapes[Random.Range(0, shapes.Length)]), this.transform.position+new Vector3(0,1,0), Quaternion.identity);    //랜덤 블럭 생성
            t = next[0];
            t.transform.position = new Vector3(4, 15, 0);
            for(int i =0;i<2;i++)
            {
                next[i] = next[i + 1];
                next[i].transform.position = nextblock[i].transform.position;
            }
            next[2] = Instantiate((shapes[Random.Range(0, shapes.Length)]), nextblock[2].transform.position, Quaternion.identity);
            for(int i =0;i<3;i++)
            {
                if (next[i].name.Contains("0"))
                {
                    next[i].transform.position = new Vector3(nextblock[i].transform.position.x, nextblock[i].transform.position.y - 0.5f, nextblock[i].transform.position.z);
                }
                else if (next[i].name.Contains("1"))
                {
                    next[i].transform.position = new Vector3(nextblock[i].transform.position.x, nextblock[i].transform.position.y, nextblock[i].transform.position.z);
                }
                else
                {
                    next[i].transform.position = new Vector3(nextblock[i].transform.position.x+0.55f, nextblock[i].transform.position.y - 0.5f, nextblock[i].transform.position.z);
                }
            }
            t.GetComponent<Fall>().enabled = true;
            f.GetComponent<Field>().CheckCreate(t);     //블럭이 생성되었을 때 다른 블럭과 겹치는지 확인
            change = true;
        }
    }
    void ChangeBlock()
    {
        temp = saveblock;
        saveblock = t;
        t = temp;
        t.transform.position = new Vector3(4, 15, 0);
        if (saveblock.name.Contains("0"))
        {
            saveblock.transform.position = new Vector3(s.transform.position.x - 0.5f, s.transform.position.y - 0.5f, s.transform.position.z);
        }
        else if (saveblock.name.Contains("1"))
        {
            saveblock.transform.position = new Vector3(s.transform.position.x - 0.55f, s.transform.position.y, s.transform.position.z);
        }
        else
        {
            saveblock.transform.position = new Vector3(s.transform.position.x, s.transform.position.y - 0.5f, s.transform.position.z);
        }
        saveblock.GetComponent<Fall>().target = false;
        t.GetComponent<Fall>().target = true;
        t.GetComponent<Fall>().FallShape();
    }

}
