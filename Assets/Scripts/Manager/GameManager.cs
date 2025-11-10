using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("골드/생명/턴 수/승리 수")]
    public int gold = 10;
    public int life = 5;
    public int turn = 1;
    public int win = 0;

    [Header("리스트")]
    public List<GameObject> playerDeck = new List<GameObject>();
    public List<GameObject> enemyDeck = new List<GameObject>();
    public List<TokenController> battleOrderList = new List<TokenController>();

    #region UI
    [Header("상점 화면 UI")]
    public Text text_gold;
    public Text text_life;
    public Text text_turn;
    public Text text_win;

    [Header("전투 화면 UI")]
    public Transform playerDeck_Parent;
    public Transform enemyDeck_Parent;

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
        SetBattleOrder(); //전투 순서

        //전투시작 능력 발동
        //전투 진행
        while (playerDeck.Count > 0 && enemyDeck.Count > 0)
        {
            //양측 첫번째 토큰 기울이기
            //그 상태로 대기
            //yield return WaitUntil(() => )

            //서로서로 공격
            //공격 애니메이션 재생
        }

        //전투 종료
        //결과창

        yield return null;
    }

    void SetBattleOrder()
    {
        battleOrderList.Clear();

        for (int i = 0; i < playerDeck.Count; i++)
            battleOrderList.Add(playerDeck[i].GetComponent<TokenController>());
        for (int i = 0; i < enemyDeck.Count; i++)
            battleOrderList.Add(enemyDeck[i].GetComponent<TokenController>());

        battleOrderList = battleOrderList
            .OrderByDescending(token => token.curAtk)
            .ToList();
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

    #region Button Methods
    public void OnClick_PlayBattleButton()
    {
        //매칭매니저 가져와서 매칭 시도
        //상대 덱 정보 가져왔으면 배틀 시작
        StartCoroutine(Battle());
    }
    #endregion
}
