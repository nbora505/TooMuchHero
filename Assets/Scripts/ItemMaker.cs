using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [SerializeField] private Table_SO table;

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

        MakeCharacterTokenInShop(curTurn);
    }

    public void MakeCharacterTokenInShop(int turn)
    {
        var range = table.ranges.FirstOrDefault(r =>
        turn >= r.minTurn && turn <= r.maxTurn);

        for (int i = 0; i < range.itemCnt; i++)
        {
            int star = HowMuchCharacterStar(range.probability);

            Debug.Log(star);

            GameObject characterToken = Instantiate(characterPrefab, characterShop.transform);
            SetToken(characterToken, star);

        }
    }

    void SetToken(GameObject characterToken, int star)
    {
        TokenController TC = characterToken.GetComponent<TokenController>();

        TC.data = DataManager.instance.GetRandomByStar(star);
        TC.tokenImage.sprite = TC.data.characterSprite;
        TC.text_star.text = TC.data.star.ToString();
        TC.text_lv.text = "1";
        TC.text_atk.text = TC.data.atk.ToString();
        TC.text_hp.text = TC.data.hp.ToString();
        TC.text_name.text = TC.data.characterName;
        TC.text_decription.text = TC.data.skillDescription[0];
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
}
