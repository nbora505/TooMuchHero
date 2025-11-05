using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DeckManager : MonoBehaviour
{
    [Header("Slots (왼→오 순서로 배열)")]
    public DropSlot[] slotsLeftToRight; // size=5, 인스펙터에서 왼쪽→오른쪽 순으로 할당

    [Header("Data Deck (오른쪽 끝=0)")]
    public Character_SO[] deck; // 런타임에서 갱신됨

    [Header("UI")]
    public GameObject purchaseBlockedPopup; // 꽉 찼을 때 켜는 팝업
    public LevelUpManager LM;

    void Awake()
    {
        if (deck == null || deck.Length != slotsLeftToRight.Length)
            deck = new Character_SO[slotsLeftToRight.Length];

        for (int i = 0; i < slotsLeftToRight.Length; i++)
        {
            slotsLeftToRight[i].deckManager = this;
            slotsLeftToRight[i].leftToRightIndex = i;
        }
    }

    // 오른쪽 끝 슬롯 = deck[0]
    public int DeckIndexFor(DropSlot slot)
    {
        int n = slotsLeftToRight.Length;
        return (n - 1) - slot.leftToRightIndex;
    }

    public void HandleDrop(DropSlot target, TokenController drag)
    {
        var token = drag.GetComponent<TokenController>();
        if (token == null) return;

        var existing = target.TC;

        // 드래그 출발 슬롯의 데이터는 일단 비워둘 준비
        var fromSlot = drag.OriginalSlotOnDrag;

        if (existing == null)
        {
            // 빈 칸 → 그냥 배치
            RemoveFromDeckIfStillThere(token, fromSlot);
            PlaceToken(target, token);
            return;
        }

        // 2-1) 동일 아이디 → 합성: 새 토큰 제거 + 기존 토큰 경험치 증가
        if (existing.data.id == token.data.id)
        {
            if (existing.data.lv >= 3) return;
            
            Debug.Log("경험치 추가");
            LM.AddExp(existing);
            // 새 토큰 날리고, 데이터쪽도 fromSlot에서 제거
            RemoveFromDeckIfStillThere(token, fromSlot);
            Destroy(token.gameObject);
            return;
        }

        // 2-2) 다른 토큰 → 왼쪽으로 밀어넣기 시도, 안되면 오른쪽, 둘다 안되면 실패 팝업
        if (TryShift(target, dir: -1) || TryShift(target, dir: +1))
        {
            RemoveFromDeckIfStillThere(token, fromSlot);
            PlaceToken(target, token);
        }
        else
        {
            // 실패 → 드래그는 원위치 복귀 (SnapTo 호출 안 함)
            if (purchaseBlockedPopup != null) purchaseBlockedPopup.SetActive(true);
        }
    }

    // 드래그 시작 슬롯에 여전히 이 토큰이 남아 있다면 덱에서 지움
    private void RemoveFromDeckIfStillThere(TokenController token, DropSlot fromSlot)
    {
        if (fromSlot == null) return;
        if (fromSlot.TC == token)
        {
            deck[DeckIndexFor(fromSlot)] = null;
        }
    }

    private void PlaceToken(DropSlot slot, TokenController token)
    {
        token.SnapTo(slot.transform, slot);
        deck[DeckIndexFor(slot)] = token.data;
    }

    // dir:-1 = 왼쪽으로 밀기, dir:+1 = 오른쪽으로 밀기
    private bool TryShift(DropSlot pivot, int dir)
    {
        int pivotIdx = pivot.leftToRightIndex;
        var path = new List<int>(); // 비어있는 칸을 찾을 때까지 인덱스 기록

        int i = pivotIdx + dir;
        int n = slotsLeftToRight.Length;
        while (i >= 0 && i < n)
        {
            path.Add(i);
            if (slotsLeftToRight[i].TC == null)
            {
                // 빈 칸 발견 → path를 역순으로 토큰 이동 (밀어넣기)
                for (int k = path.Count - 1; k > 0; k--)
                {
                    int from = path[k - 1];
                    int to = path[k];
                    MoveToken(slotsLeftToRight[from], slotsLeftToRight[to]);
                }
                // pivot칸(시작칸)도 한 칸 밀기
                MoveToken(slotsLeftToRight[pivotIdx], slotsLeftToRight[pivotIdx + dir]);
                return true;
            }
            i += dir;
        }
        // 끝까지 갔는데 빈 칸 없음 → 실패
        return false;
    }

    private void MoveToken(DropSlot from, DropSlot to)
    {
        var t = from.TC;
        if (t == null) return;

        // UI 이동
        t.SnapTo(to.transform, to);

        // 데이터 이동
        deck[DeckIndexFor(to)] = t.data;
        deck[DeckIndexFor(from)] = null;
    }
}
