using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dir : MonoBehaviour
{
    GameObject f;           //맵을 다루는 오브젝트
    public GameObject p;    //일시정지 팝업
    public GameObject g;    //게임 오버 팝업
    public GameObject m;    //준비 화면에서의 UI
    public GameObject u;    //게임 화면에서의 UI
    public bool IsPause;    //일시 정지 확인
    public bool restart;    //게임 다시 시작 확인
    public bool IsEnd;      //게임이 끝났는지 확인
    AudioSource click;      //버튼 효과음
    AudioSource bgm;        //배경음
    // Start is called before the first frame update
    private void Awake()
    {
        Screen.SetResolution(720, 800, false);
    }
    void Start()
    {
        restart = false;    //재시작 x
        IsEnd = false;      //끝나지 않음
        IsPause = false;    //일시정지 x
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
        if (IsPause == false)       //일시정지 상태가 아닐 때
        {
            Time.timeScale = 0.0f;  //시간 정지
            IsPause = true;         //일시 정지
            p.SetActive(true);      //일시 정지 팝업 on
            bgm.Pause();            //배경음 일시 정지
        }
    }
    public void Continue()
    {
        click.Play();
        if (IsPause == true)        //일시정지 상태일 때
        {
            Time.timeScale = 1.0f;  //시간 재생
            IsPause = false;        //일시 정지 해제
            p.SetActive(false);     //일시 정지 팝업 off
            bgm.Play();
        }
    }
    public void ReStart()
    {
        click.Play();
        bgm.Stop();
        restart = true;             //재시작 on
        Time.timeScale = 1.0f;      //시간 재생
        f.GetComponent<Field>().ResetBlock();       //블럭 제거
        if (p.activeSelf == true && IsPause == true)  //일시정지 상태이고 팝업이 있을 경우
        {
            IsPause = false;                        //일시 정지 해제
            p.SetActive(false);                     //팝업 해제
        }
        if (g.activeSelf == true && IsEnd == true)      //게임 오버 상태이고 팝업이 있을 경우
        {
            IsEnd = false;                          //게임 오버 해제
            g.SetActive(false);                     //게임 오버 팝업 해제
        }
        //GameObject.Find("CreateShape").GetComponent<CreateShape>().Createshape();   //시작 블럭 생성
        u.GetComponent<Score>().score = 0;          //점수 초기화
        bgm.Play();
        bgm.volume = 0;
        GameObject.Find("CreateShape").GetComponent<CreateShape>().next.Clear();
        GameObject.Find("CreateShape").GetComponent<CreateShape>().change = true;


    }
    public void Gameover()
    {
        if (IsEnd == false)                         //게임 오버 상태가 아닐 때
        {
            f.GetComponent<Field>().ResetBlock();   //블럭 제거
            IsEnd = true;                           //게임 오버 
            g.SetActive(true);                      //게임 오버 팝업 on
        }
    }
    public void GameStart()
    {
        Time.timeScale = 1.0f;                      //시간 재생
        u.SetActive(true);                          //게임 ui on
        m.SetActive(false);                         //준비 ui off
        f.GetComponent<Field>().gameover = false;   //게임 오버 상태 해제
        ReStart();                                  //재시작
        click.Play();                               //효과음 재생
        bgm.Play();                                 //배경음 재생
        bgm.volume = 0;
        GameObject.Find("CreateShape").GetComponent<CreateShape>().next.Clear();
        Destroy(GameObject.Find("CreateShape").GetComponent<CreateShape>().saveblock);
        GameObject.Find("CreateShape").GetComponent<CreateShape>().blocksave = false;
        GameObject.Find("CreateShape").GetComponent<CreateShape>().change = true;
    }
    public void GameExit()
    {
        f.GetComponent<Field>().gameover = true;    //게임 오버(게임 종료)on
        Time.timeScale = 0.0f;                      //시간 정지
        u.SetActive(false);                         //게임 ui off
        m.SetActive(true);                          //메인 ui on
        f.GetComponent<Field>().ResetBlock();       //블럭 제거

        bgm.Stop();

        click.Play();
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
