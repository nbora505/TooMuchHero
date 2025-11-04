using UnityEngine;

[CreateAssetMenu]
public class Table_SO : ScriptableObject
{
    [System.Serializable]
    public class TurnRange
    {
        public int minTurn;
        public int maxTurn;
        public int itemCnt;

        [Range(0f, 1f)] public float[] probability = new float[6];
    }

    public TurnRange[] ranges;
}
