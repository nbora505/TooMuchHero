using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [SerializeField] private Table_SO table;
    [SerializeField] private Table_SO itemTable;

    public GameObject characterShop;
    public GameObject itemShop;

    public GameObject characterPrefab;
    public GameObject itemPrefab;

    public int curTurn;

    public void ResetShop()
    {
        for (int i = 0; i < characterShop.transform.childCount; i++)
        {
            Destroy(characterShop.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < itemShop.transform.childCount; i++)
        {
            Destroy(itemShop.transform.GetChild(i).gameObject);
        }

        MakeCharacterTokenInShop(curTurn);
        MakeItemTokenInShop(curTurn);
    }

    #region Character Token
    public void MakeCharacterTokenInShop(int turn)
    {
        var range = table.ranges.FirstOrDefault(r =>
        turn >= r.minTurn && turn <= r.maxTurn);

        for (int i = 0; i < range.itemCnt; i++)
        {
            int star = HowMuchCharacterStar(range.probability);

            GameObject characterToken = Instantiate(characterPrefab, characterShop.transform);
            SetCharacterToken(characterToken, star);

        }
    }

    void SetCharacterToken(GameObject characterToken, int star)
    {
        TokenController TC = characterToken.GetComponent<TokenController>();

        TC.data = DataManager.instance.GetRandomCharacterByStar(star);
        TC.tokenImage.sprite = TC.data.characterSprite;
        TC.text_star.text = TC.data.star.ToString();
        TC.text_lv.text = TC.data.lv.ToString();
        TC.text_atk.text = TC.data.atk.ToString();
        TC.text_hp.text = TC.data.hp.ToString();
        TC.text_name.text = TC.data.characterName;
        TC.text_decription.text = TC.data.skillDescription[0];

        TC.curAtk = TC.data.atk;
        TC.curHp = TC.data.hp;
    }

    int HowMuchCharacterStar(float[] probs)
    {
        float r = Random.value;
        float a = 0;

        for (int i = 0; i < probs.Length; i++)
        {
            a += probs[i];
            if (r <= a) return i + 1;
        }
        return probs.Length;
    }

    #endregion

    #region Item Token
    public void MakeItemTokenInShop(int turn)
    {
        var range = itemTable.ranges.FirstOrDefault(r =>
        turn >= r.minTurn && turn <= r.maxTurn);

        for (int i = 0; i < range.itemCnt; i++)
        {
            int star = HowMuchCharacterStar(range.probability);

            GameObject itemToken = Instantiate(itemPrefab, itemShop.transform);
            SetItemToken(itemToken, star);

        }
    }

    void SetItemToken(GameObject itemToken, int star)
    {
        TokenController TC = itemToken.GetComponent<TokenController>();

        TC.itemData = DataManager.instance.GetRandomItemByStar(star);
        TC.tokenImage.sprite = TC.itemData.itemSprite;
        TC.text_star.text = TC.itemData.star.ToString();
        TC.text_name.text = TC.itemData.itemName;
        TC.text_decription.text = TC.itemData.itemDescription;
    }
    #endregion
}
