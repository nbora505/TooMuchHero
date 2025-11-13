using UnityEngine;
[CreateAssetMenu]
public class Character_SO : ScriptableObject
{
    public int id;
    public string characterName;
    public int atk;
    public int hp;
    public int star;
    public int lv;
    public int exp;

    public enum When { turnStart, battleStart, dead, selled, bought, frontAttack, damaged, lose }
    public enum What { getHp, getAtk, getBoth, getBuff, extraAtk, summon, getGold, getExp, reduceAtk, reduceDamage }
    public enum Target { self, randomEnemy, randomFriend, range, backFriend, frontFriend, firstEnemy, firstFriend, all }

    public When when;
    public What what;
    public Target target;

    public int[] skillValue = new int[3];
    public int[] targetValue = new int[3];

    [TextArea]
    public string[] skillDescription = new string[3];
    public Sprite characterSprite;
}
