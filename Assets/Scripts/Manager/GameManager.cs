using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("골드/생명/턴 수/승리 수")]
    public int gold = 10;
    public int life = 5;
    public int turn = 1;
    public int win = 0;

    #region UI
    [Header("UI")]
    public Text text_gold;
    public Text text_life;
    public Text text_turn;
    public Text text_win;
    #endregion

    private void Start()
    {
        Game();
    }

    public void Game()
    {
        //팀 준비 창
        //팀 이름 정하기
        
        //상점 세팅
        TurnStart();

        //시작버튼 누르면 게임 시작
        //라운드 진행
        //라운드 종료
        //결과창
        //다음 라운드 준비
        //게임 종료
        //메인 메뉴로 돌아가기
        //다시 시작

    }
    
    void TurnStart()
    {
        gold = 10;
        if (turn > 1) turn++;

        gameObject.GetComponent<ItemMaker>().ResetShop();
        ResetUI();
    }

    IEnumerator Battle()
    {
        yield return null;
    }

    void TurnEnd()
    {

    }

    void GameOver()
    {
    }

    void GameOut()
    {
    }
    void GameRestart()
    {
    }

    void ResetUI()
    {
        text_gold.text = gold.ToString();
        text_life.text = life.ToString();
        text_turn.text = turn.ToString();
        text_win.text = win.ToString();
    }
}
