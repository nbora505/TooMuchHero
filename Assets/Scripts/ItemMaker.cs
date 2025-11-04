using System.Linq;
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
            //characterToken.GetComponent<CharacterController>().data = 
        }
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
