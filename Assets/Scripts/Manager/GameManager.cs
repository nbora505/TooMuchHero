using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Public Data

    [Header("인게임 데이터")]
    public string teamName;
    public int gold = 10;
    public int life = 5;
    public int turn = 1;
    public int win = 0;

    [Header("리스트")]
    public List<TokenController> playerDeck = new List<TokenController>();
    public List<TokenController> enemyDeck = new List<TokenController>();
    public List<TokenController> battleOrderList = new List<TokenController>();

    #endregion

    #region Private Data

    TeamData enemyTeam;
    bool isContinue, isPlay, isAuto, isSpeed, isGameOver;

    #endregion

    #region UI

    [Header("이름짓기 화면 UI")]
    public GameObject namePanel;
    public InputField inputField_teamName;

    [Header("상점 화면 UI")]
    public GameObject readyPanel;
    public Text text_teamName;
    public Text text_gold;
    public Text text_life;
    public Text text_turn;
    public Text text_win;

    [Header("전투 화면 UI")]
    public GameObject battlePanel;
    public Transform[] playerSlots;
    public Transform[] enemySlots;
    public Image image_autoBtn;
    public Image image_speedBtn;
    public Text text_myName;
    public Text text_enemyName;

    [Header("결과 화면 UI")]
    public GameObject resultPanel;
    public Text text_result;
    public Transform remainLife_parent;
    public Transform remainWin_parent;

    [Header("로딩 화면 UI")]
    public GameObject loadingPanel;

    #endregion


    private void Start()
    {
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        //팀 준비 창
        //팀 이름 버튼 누르면 시작
        yield return new WaitUntil(() => isContinue);
        isContinue = false;

        while (!isGameOver)
        {
            //상점 세팅
            TurnStart();


            //시작버튼 누르면 전투 시작
            yield return new WaitUntil(() => isContinue);
            isContinue = false;


            //데이터 매칭
            SetMyDeck();
            SetEnemyDeck();


            //전투화면 세팅
            SetBattlePage();


            //전투
            StartCoroutine(Battle());


            //라운드 종료
        }

        //게임 종료
        //메인 메뉴로 돌아가기

    }

    #region 상점 함수

    void TurnStart()
    {
        gold = 10;
        if (turn > 1) turn++;

        gameObject.GetComponent<ItemMaker>().ResetShop();
        ResetUI();
    }
    
    #endregion

    #region 전투 준비 함수

    void SetMyDeck()
    {
        playerDeck.Clear();
        playerDeck = GetComponent<DeckManager>().deck.ToList();
    }
    async void SetEnemyDeck()
    {
        enemyDeck.Clear();

        MatchingManager mm = GetComponent<MatchingManager>();

        enemyTeam = await mm.RandomUserMatching(turn);
        if (enemyTeam == null)
            enemyTeam = await mm.RandomUserMatching(12);

    }
    void SetBattlePage()
    {
        text_myName.text = teamName;
        text_enemyName.text = enemyTeam.teamName;

        for (int i = 0; i < playerDeck.Count; i++)
        {
            Instantiate(playerDeck[i], playerSlots[i]);
        }
        for (int i = 0; i < enemyDeck.Count; i++)
        {
            Instantiate(enemyDeck[i], enemySlots[i]);
        }

    }
    void SetBattleOrder()
    {
        battleOrderList.Clear();

        for (int i = 0; i < playerDeck.Count; i++)
            battleOrderList.Add(playerDeck[i]);
        for (int i = 0; i < enemyDeck.Count; i++)
            battleOrderList.Add(enemyDeck[i]);

        battleOrderList = battleOrderList
            .OrderByDescending(token => token.curAtk)
            .ToList();
    }

    #endregion

    #region 전투 진행 함수

    IEnumerator Battle()
    {
        SetBattleOrder(); //전투 순서

        //[전투 시작 시] 능력 체크

        //전투 진행
        while (playerDeck.Count > 0 && enemyDeck.Count > 0)
        {
            //빈자리 있을 때 자리 이동

            //양측 첫번째 토큰 기울이기
            
            //그 상태로 대기
            yield return new WaitUntil(() => isPlay || isAuto);

            TokenController p1 = playerDeck[0], p2 = enemyDeck[0];

            //서로서로 공격
            p1.curHp -= p2.curAtk;
            p2.curHp -= p1.curAtk;

            //공격 애니메이션 재생

            //체력 UI 변경
            p1.text_hp.text = p1.curHp.ToString();
            p2.text_hp.text = p2.curHp.ToString();
            yield return new WaitForSeconds(1f);

            //사망 체크
            if (p1.curHp <= 0) StartCoroutine(DestroyToken(p1));
            if (p2.curHp <= 0) StartCoroutine(DestroyToken(p2));


            //[사망 시] 능력 체크

            isPlay = false;
        }

        //전투 종료
        //결과창

        yield return null;
    }

    IEnumerator DestroyToken(TokenController token)
    {
        playerDeck.Remove(token);
        battleOrderList.Remove(token);
        Destroy(token.gameObject);
        yield break;
    }

    #endregion

    #region 전투 이후 함수

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

    #endregion

    #region 기타 함수

    void ResetUI()
    {
        text_teamName.text = teamName;
        text_gold.text = gold.ToString();
        text_life.text = life.ToString();
        text_turn.text = turn.ToString();
        text_win.text = win.ToString();
    }

    IEnumerator Loading(bool on)
    {
        if (on)
        {
            loadingPanel.SetActive(true);
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(1);
            loadingPanel.SetActive(false);
        }
    }

    #endregion

    #region 버튼 함수
    public void OnClick_TeamNameSubmitButton()
    {
        int rand = Random.Range(0, 10);

        if (inputField_teamName.text != null)
            teamName = inputField_teamName.text;
        else teamName = $"여명 길드 헌터 {rand}팀";

        StartCoroutine(Loading(true));
        
        namePanel.SetActive(false);
        readyPanel.SetActive(true);
        isContinue = true;

        StartCoroutine(Loading(false));

    }
    public void OnClick_PlayBattleButton()
    {
        readyPanel.SetActive(false);
        battlePanel.SetActive(true);

        isContinue = true;

    }
    public void OnClick_NoneAutoPlayButton()
    {
        isPlay = true;
    }
    public void OnClick_AutoPlayButton()
    {
        if (!isAuto)
        {
            image_autoBtn.color = Color.white;
            isAuto = true;
        }
        else
        {
            image_autoBtn.color= Color.gray;
            isAuto = false;
        }
    }
    public void OnClick_SpeedButton()
    {
        if (!isSpeed)
        {
            image_speedBtn.color = Color.white;
            Time.timeScale = 2f;
        }
        else
        {
            image_speedBtn.color= Color.gray;
            Time.timeScale = 1f;
        }
    }
    #endregion
}
