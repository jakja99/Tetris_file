using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    GameObject s;                   //게임 진행 중 나타낼 현재점수
    GameObject hs;                  //게임 진행 중 나타낼 지난 최고 점수
    public GameObject sco;          //결과창에서 나타낼 현재 점수
    public GameObject hsc;          //결과창에서 나타낼 최고 점수
    GameObject d;                   //시스템 관리 오브젝트
    public GameObject sc;           //일시 정지 시 보여줄 현재 점수
    public int score;               //현재 점수 저장
    private int highscore = 0;      //최고 점수 저장
    string KeyName = "HighScore";   //최고점수를 메모리에 저장하는 키워드
    int lasthighscore = 0;          //지난 최고점수
    // Start is called before the first frame update
    void Awake()
    {
        //PlayerPrefs.DeleteAll();  //메모리에 저장된 최고점수 삭제
        s = GameObject.Find("Score");
        hs = GameObject.Find("HighScore");
        d = GameObject.Find("Director");
        score = 0;                  //시작 점수 초기화
        highscore = PlayerPrefs.GetInt(KeyName,0);  //메모리에 저장된 최고 점수 불러오기
        hs.GetComponent<Text>().text = $"{highscore.ToString("0")}";    //최고점수 출력
        
    }

    // Update is called once per frame
    void Update()
    {
        s.GetComponent<Text>().text = score.ToString("0");          //현재점수 표시(진행중)
        sco.GetComponent<Text>().text = score.ToString("0");        //현재점수 표시(결과창)
        
        if (score>highscore)           //게임이 종료되었을 때 점수가 최고점수보다 높을 경우
        {
            hs.GetComponent<Text>().text = score.ToString("0");
            if (d.GetComponent<Dir>().IsEnd)
            {
                PlayerPrefs.SetInt(KeyName, score);
                highscore = PlayerPrefs.GetInt(KeyName, 0);//최고점수 저장(메모리)
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
