using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public float fallspeed;             //떨어지는 속도(한 번 이동하는데 걸리는 시간)
    float movespeed;                    //좌우 이동 거리
    public bool target;                        //조작가능한 블럭 확인
    GameObject field;                   //맵을 관리하는 오브젝트
    GameObject dir;                     //시스템 관리 오브젝트
    public bool save;
    // Start is called before the first frame update
    void Start()
    {
        save = false;
        dir = GameObject.Find("Director");
        field = GameObject.Find("Field");
        target = true;                  //생성되면 조작가능한 블럭으로 시작
        fallspeed = 1;                  
        movespeed = 1;
        Invoke("FallShape", fallspeed); //블럭이 떨어지는 함수
    }

    // Update is called once per frame
    void Update()
    {
        if (!field.GetComponent<Field>().gameover)      //게임오버가 아닐 때 작동
        {
            if (target && !dir.GetComponent<Dir>().IsPause)     //움직이는 대상이고 일시정지가 아닐 때 작동
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))        
                {
                    this.transform.position += new Vector3(-movespeed, 0, 0);
                    if (!field.GetComponent<Field>().BlockMove(this.gameObject))    //이동한 방향에 다른 블럭이 있는지 && 맵의 너비를 벗어나는지 확인
                        this.transform.position += new Vector3(+movespeed, 0, 0);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    this.transform.position += new Vector3(movespeed, 0, 0);
                    if (!field.GetComponent<Field>().BlockMove(this.gameObject))    //이동한 방향에 다른 블럭이 있는지 && 맵의 너비를 벗어나는지 확인
                        this.transform.position += new Vector3(-movespeed, 0, 0);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))  
                {
                    this.transform.RotateAround(this.transform.GetChild(4).position, new Vector3(0, 0, 1), 90.0f);  //지정한 중심을 기준으로 블럭 회전
                    if (!field.GetComponent<Field>().BlockMove(this.gameObject))    //회전한 방향에 다른 블럭이 있는지 && 맵의 너비를 벗어나는지 확인
                    {
                        this.transform.RotateAround(this.transform.GetChild(4).position, new Vector3(0, 0, 1), -90.0f);
                    }
                    field.GetComponent<Field>().UnderBlock(this.gameObject);        //맵의 높이를 벗어나면 위치조정

                }
                if (Input.GetKeyDown(KeyCode.DownArrow))                            //누르는 동안 블럭이 내려가는 속도 상승
                {
                    fallspeed = 0.02f;
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))                              //손을 때면 기존 속도로 복구
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
                    target = false;     //지정 해제
                    ChangeTarget();
                }
            }
            if (this.transform.childCount == 1)                                     //모양을 구성하는 블럭이 모두 사라진다면 중심을 담당하는 오브젝트 제거
            {
                Destroy(this.gameObject);
            }
        }
        
        
    }
    public void FallShape()        //블럭을 떨어뜨리는 함수
    {
        if (target)
        {
            this.transform.position += new Vector3(0, -1, 0);
            if (!field.GetComponent<Field>().UnderBlock(this.gameObject))     //밑에 블럭이 있으면 1초 후 대상 변경
            {
                GameObject.Find("CreateShape").GetComponent<CreateShape>().change = false;
                Invoke("ChangeTarget", 1f);
            }
            else
            {
                Invoke("FallShape", fallspeed);
            }
            /*else if(!save)                                                                  //저장된 상태가 아니면 반복 실행
            {
                target = true;
                //this.transform.position += new Vector3(0, -1, 0);
                Invoke("FallShape", fallspeed);
            }
            else if (target == false)                                             //타겟이 아니면 안 떨어짐
            {

            }*/
        }

    }
    void ChangeTarget()     //대상 변경 함수
    {
        //field.GetComponent<Field>().CheckLine();    //한 줄이 모두 채워졌는지 확인 후 제거
        
       field.GetComponent<Field>().CheckLine();    //한 줄이 모두 채워졌는지 확인 후 제거
        target = false;     //지정 해제
    }

}
