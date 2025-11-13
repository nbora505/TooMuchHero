using Firebase.Firestore;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

[FirestoreData]
public class TeamData
{
    [FirestoreProperty] public string teamName { get; set; }
    [FirestoreProperty] public int turn { get; set; }
    [FirestoreProperty] public int[] characters { get; set; }
    [FirestoreProperty] public int[] LV { get; set; }
    [FirestoreProperty] public int[] ATK { get; set; }
    [FirestoreProperty] public int[] HP { get; set; }
    [FirestoreProperty] public float rand { get; set; }
}

public class MatchingManager : MonoBehaviour
{
    public async Task<TeamData> RandomUserMatching(int myTurn)
    {
        var db = FirebaseFirestore.DefaultInstance;
        double r = Random.value;
        var q1 =
            db.Collection("decks")
            .WhereEqualTo("turn", myTurn)
            .WhereGreaterThanOrEqualTo("rand", r)
            .OrderBy("rand")
            .Limit(1);

        var s1 = await q1.GetSnapshotAsync();
        DocumentSnapshot picked = null;

        if (s1.Count > 0) 
        {
            picked = s1.Documents.FirstOrDefault();
        }
        else
        {
            var q2 =
            db.Collection("decks")
            .WhereEqualTo("turn", myTurn)
            .WhereLessThanOrEqualTo("rand", r)
            .OrderBy("rand")
            .Limit(1);

            var s2 = await q2.GetSnapshotAsync();
            if (s2.Count > 0)
            {
                picked = s2.Documents.FirstOrDefault();
            }
        }

        if (picked == null) return null;

        var data = picked.ConvertTo<TeamData>();
        return data;
    }

    public async Task SavePlayerDeck(List<TokenController> playerDeck, GameManager gameManager)
    {
        // 0개일 때 예외 처리
        if (playerDeck == null || playerDeck.Count == 0)
        {
            Debug.LogWarning("SavePlayerDeck : 덱에 캐릭터가 0명!");
            return;
        }

        int MaxSize = 5;

        int[] characters = new int[MaxSize];
        int[] lvs = new int[MaxSize];
        int[] atks = new int[MaxSize];
        int[] hps = new int[MaxSize];


        int count = Mathf.Min(playerDeck.Count, MaxSize);
        for (int i = 0; i < count; i++)
        {
            if (i < playerDeck.Count && playerDeck[i] != null)
            {
                TokenController t = playerDeck[i];

                characters[i] = t.characterId;
                lvs[i] = t.curLv;
                atks[i] = t.curAtk;
                hps[i] = t.curHp;
            }
            else
            {
                characters[i] = 0;
                lvs[i] = 0;
                atks[i] = 0;
                hps[i] = 0;
            }
        }

        // GameManager에서 팀 이름 / 턴 가져오기
        string teamName = gameManager.teamName;
        int turn = gameManager.turn;

        TeamData data = new TeamData
        {
            teamName = teamName,
            turn = turn,
            characters = characters,
            LV = lvs,
            ATK = atks,
            HP = hps,
            rand = Random.value
        };

        var db = FirebaseFirestore.DefaultInstance;

        // 문서 ID를 자동으로 생성하고 싶으면 AddAsync
        await db.Collection("decks").AddAsync(data);

        Debug.Log("덱 저장 완료");
    }

}
