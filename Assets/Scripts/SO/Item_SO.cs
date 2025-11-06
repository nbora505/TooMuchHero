using UnityEngine;

[CreateAssetMenu]
public class Item_SO : ScriptableObject
{
    public int id;
    public int star;
    public string itemName;

    public enum What { getAtk, getBoth, getOneTurn, getExp, buff_extraAtk, buff_summon, buff_reduceDamage, buff_firstAtk, buff_shield }
    public enum Target { one, random2, random3, shop, }

    public What what;
    public Target target;

    public int value;

    [TextArea]
    public string itemDescription;

    public Sprite itemSprite;
}
