using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static int w = 10;           //맵 너비
    public static int h = 16;           //맵 높이
    GameObject[] block;                 //하위 블럭을 불러올 배열
    public GameObject s;                //점수를 저장할 오브젝트
    GameObject d;                       //시스템 관리 오브젝트
    List<GameObject> line = new List<GameObject>();     //한줄이 다 찾는지 확인할 리스트
    List<GameObject> lineblock = new List<GameObject>();//지워야 할 블럭
    List<int> deleteH = new List<int>();                //지워야 할 라인
    List<GameObject> upline = new List<GameObject>();   //블럭을 내릴 때 사용할 리스트
    bool IsPause;                                       //일시정지 확인
    public bool gameover;                               //게임 오버 확인
    int count;                                          //블럭 제거 시 깜빡임에 사용
    bool alpha;                                         //블럭 제거 시 깜빡임에 사용
    AudioSource delete;                                 //블럭 제거 효과음
    // Start is called before the first frame update
    void Start()
    {
        d = GameObject.Find("Director");
        IsPause = false;                                //일시정지 x
        gameover = true;                                //게임오버(준비화면) 확인
        alpha = true;
        count = 0;
        delete = GameObject.Find("Delete").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool BlockMove(GameObject b)         //블럭 이동 후 겹치는지 확인
    {
        for (int i = 0; i < 4; i++)             //형태 블럭을 구성하는 하위 블럭 수 만큼 반복(중심점 제외)
        {
            block = GameObject.FindGameObjectsWithTag("Block");             //맵에 있는 모든 하위 블럭 저장
            if (Mathf.Round(b.transform.GetChild(i).position.x) < 0)        //맵 너비를 벗어나지 못하게 작동
            {
                b.transform.position += new Vector3(1, 0, 0);

            }
            else if (Mathf.Round(b.transform.GetChild(i).position.x) > 9)   //맵 너비를 벗어나지 못하게 작동
            {
                b.transform.position -= new Vector3(1, 0, 0);
            }
            for (int x = 0; x < block.Length; x++)                          
            {
                if (Mathf.Round(block[x].transform.position.y) == Mathf.Round(b.transform.GetChild(i).position.y)       //블럭의 y값이 같은지 확인
                    && Mathf.Round(block[x].transform.position.x) == Mathf.Round(b.transform.GetChild(i).position.x))   //블럭의 x값이 같은지 확인
                {
                    if (block[x].transform.parent != b.transform)           //같은 블럭인지 확인
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public bool UnderBlock(GameObject b)        //아래 블럭 확인
    {
        for (int i = 0; i < 4; i++)
        {
            block = GameObject.FindGameObjectsWithTag("Block");
            if (Mathf.Round(b.transform.GetChild(i).position.y) < 1)    //맵 높이를 벗어나지 못하게 작용
            {
                b.transform.position += new Vector3(0, 1, 0);
                return false;
            }
            if (Mathf.Round(b.transform.GetChild(i).position.y) > 16)   //맵 높이를 벗어나지 못하게 작용
            {
                b.transform.position -= new Vector3(0, 1, 0);
                return false;
            }
            for (int x = 0; x < block.Length; x++)
            {
                if (Mathf.Round(block[x].transform.position.y) == Mathf.Round(b.transform.GetChild(i).position.y)       //블럭의 y값이 같은지 확인
                    && Mathf.Round(block[x].transform.position.x) == Mathf.Round(b.transform.GetChild(i).position.x))   //블럭의 x값이 같은지 확인
                {
                    if (block[x].transform.parent != b.transform)       //같은 블럭인지 확인
                    {
                        b.transform.position += new Vector3(0, 1, 0);
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public void CheckLine()                 //한 줄의 블럭이 채워졌는지 확인 후 제거
    {
        deleteH.Clear();
        lineblock.Clear();
        for (int i = h+1; i > 0; i--)        //높이 만큼 반복
        {
            block = GameObject.FindGameObjectsWithTag("Block"); //맵에 있는 모든 블럭
            line.Clear();                   //리스트 초기화
            
            for (int x = 0; x < block.Length; x++)  
            {
                if ((int)Mathf.Round(block[x].transform.position.x) > -1 && (int)Mathf.Round(block[x].transform.position.x) < 10)
                {
                    if ((int)Mathf.Round(block[x].transform.position.y) == i)   //지정된 높이에 있는 블럭 수 확인
                    {
                        line.Add(block[x]);
                    }
                }
            }
            if (line.Count >= 10)       //블럭 수 >=너비 ==> 지우는 줄
            {
                deleteH.Add(i);
                for (int y = 0; y < line.Count; y++)    //지워야 할 블럭 저장
                {
                    lineblock.Add(line[y]);
                }
                s.GetComponent<Score>().score += 10;    //제거 시 점수 획득
                //LineDown(i);                            //지정된 높이 위의 모든 블럭을 내리는 함수
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
        GameObject.Find("CreateShape").GetComponent<CreateShape>().Createshape();   //블럭 생성
    }
    void deleteLine()   //블럭 제가 함수
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

    void LineDown(int h)        //지정된 높이 위의 모든 블럭을 내리는 함수
    {
        block = GameObject.FindGameObjectsWithTag("Block");
        upline.Clear();         //리스트 클리어
        for (int x = 0; x < block.Length; x++)
        {
            if ((int)Mathf.Round(block[x].transform.position.x) > -1 && (int)Mathf.Round(block[x].transform.position.x) < 10)
            {
                if (Mathf.Round(block[x].transform.position.y) > h) //지정된 높이 위의 모든 블럭 저장
                {
                    upline.Add(block[x]);
                }
            }
        }
        for (int y = 0; y < upline.Count; y++)                  //블럭 내리기
        {
            upline[y].transform.position -= new Vector3(0, 1, 0);
        }
        //upline.Clear();         
    }

    public bool CheckCreate(GameObject b)                       //블럭이 생성되었을 때 겹치는 지 확인
    {        
        for (int i = 0; i < 4; i++)
        {
            block = GameObject.FindGameObjectsWithTag("Block");

            for (int x = 0; x < block.Length; x++)
            {
                if (Mathf.Round(block[x].transform.position.y) == Mathf.Round(b.transform.GetChild(i).position.y)       //블럭의 y값이 같은지 확인
                    && Mathf.Round(block[x].transform.position.x) == Mathf.Round(b.transform.GetChild(i).position.x))   //블럭의 x값이 같은지 확인
                {
                    if (block[x].transform.parent != b.transform)   //같은 블럭인지 확인
                    {
                        Destroy(b);                                 //생성된 도형 블럭 제거
                        gameover = true;                            //게임 오버 확인
                        d.GetComponent<Dir>().Gameover();           //게임 오버 함수 실행
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void ResetBlock()                    //블럭 리셋
    {
        Destroy(GameObject.Find("CreateShape").GetComponent<CreateShape>().t);  
        
        block = GameObject.FindGameObjectsWithTag("Block");
        for (int i=0;i<block.Length;i++)        //모든 블럭 삭제
        {
            Destroy(block[i]);
        }
        gameover = false;                       //게임 오버 해제
        
    }
}
