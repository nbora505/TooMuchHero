using Firebase.Firestore;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

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
            db.Collection("deck")
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
            db.Collection("deck")
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
}
