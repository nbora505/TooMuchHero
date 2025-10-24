using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{
    private string id;
    private string pw;
    private bool isExist = false;

    public InputField idInput;
    public InputField pwInput;

    public Text popUpMsg;

    public GameObject popUpPanel;
    public GameObject logInPanel;
    public GameObject loadingPanel;
    //public GameObject rankingPanel;

    //private int gold;
    //private int score;

    //public Text goldText;
    //public Text scoreText;
    public Text userIdText;

    //public GameObject rankingPrefab;
    //public Transform content;

    public void OnClickSignUp()
    {
        if (string.IsNullOrEmpty(idInput.text) || string.IsNullOrEmpty(pwInput.text))
        {
            Debug.Log("아이디와 패스워드를 모두 일력해주세요.");
            popUpPanel.SetActive(true);
            popUpMsg.text = "아이디와 패스워드를 모두 일력해주세요.";
        }
        else
        {
            Debug.Log("ID :: " + idInput.text);
            Debug.Log("PW :: " + pwInput.text);

            id = idInput.text;
            pw = pwInput.text;
            StartCoroutine(CreateUser());
        }
    }

    IEnumerator CreateUser()
    {
        loadingPanel.SetActive(true);
        FirebaseFirestore db;
        yield return db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef;
        yield return docRef = db.Collection("users").Document(id);

        Dictionary<string, object> user = new Dictionary<string, object>
        {
            {"score", 0 },
            {"updateTime", FieldValue.ServerTimestamp },
            {"userID", id },
            {"userPW", pw }
        };

        yield return docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log($"{id} 의 회원가입이 완료되었습니다.");
                popUpMsg.text = $"{id}의 회원가입이 완료되었습니다.";
            }
            else
            {
                Debug.Log(task.Exception);
                popUpMsg.text = task.Exception.ToString();
            }
        });
        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(idInput.text) || string.IsNullOrEmpty(pwInput.text))
        {
            Debug.Log("아이디와 패스워드를 모두 입력해주세요.");
            popUpPanel.SetActive(true);
            popUpMsg.text = "아이디와 패스워드를 모두 입력해주세요.";
        }
        else
        {
            //Debug.Log("ID :: " + emailInput.text);
            //Debug.Log("PW :: " + pwInput.text);
            // FireStore에 ID 생성...
            id = idInput.text;
            pw = pwInput.text;
            StartCoroutine(ReadUserData());
        }
    }

    IEnumerator ReadUserData()
    {
        loadingPanel.SetActive(true);
        FirebaseFirestore db;
        yield return db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef;
        yield return docRef = db.Collection("users").Document(id);

        yield return docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                isExist = true;
                Dictionary<string, object> doc = snapshot.ToDictionary();

                //Debug.Log(doc["userPW"]);

                foreach (KeyValuePair<string, object> pair in doc)
                {
                    if (pair.Key == "userPW")
                    {
                        Debug.Log(pair.Value.ToString());
                        if (pair.Value.ToString() != pw)
                        {
                            Debug.Log("HERE!!!");
                            popUpPanel.SetActive(true);
                            popUpMsg.text = "아이디 혹은 비밀번호를 확인해주세요...";
                            break;
                        }
                        else
                        {
                            logInPanel.SetActive(false);
                            //scoreText.text = "Score : " + score;
                            userIdText.text = id;
                        }
                    }

                    //if (pair.Key == "score")
                        //score = int.Parse(pair.Value.ToString());

                }
            }
            else
            {
                Debug.Log($"{id} 은 존재하지 않습니다...");
                popUpPanel.SetActive(true);
                popUpMsg.text = $"아이디와 비밀번호를 확인해주세요...";
            }
        });
        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
    }

    public void OnClickUpdate()
    {
        //StartCoroutine(UpdateUserData(id,score));
    }

    IEnumerator UpdateUserData(string userEmail,int userScore)
    {
        loadingPanel.SetActive(true);
        FirebaseFirestore db;
        yield return db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef;
        yield return docRef = db.Collection("users").Document(userEmail);

        Dictionary<string, object> dic = new Dictionary<string, object>
        {
            {"score", userScore },
            {"updateTime", FieldValue.ServerTimestamp }
        };

        yield return docRef.SetAsync(dic, SetOptions.MergeAll);
        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
    }

    public void OnClickDelete()
    {
        StartCoroutine(DeleteUser(id));
    }

    IEnumerator DeleteUser(string userEmail)
    {
        loadingPanel.SetActive(true);
        FirebaseFirestore db;
        yield return db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef;
        yield return docRef = db.Collection("users").Document(userEmail);

        yield return docRef.DeleteAsync();
        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
        Debug.Log($"{userEmail} 삭제완료...");
        SceneManager.LoadScene(0);
    }

    /*
    public void OnClickRanking()
    {
        StartCoroutine(GetRanking());
    }

    IEnumerator GetRanking()
    {
        loadingPanel.SetActive(true);
        FirebaseFirestore db;
        yield return db = FirebaseFirestore.DefaultInstance;

        CollectionReference userRef;
        yield return userRef = db.Collection("users");

        Query query;
        yield return query = userRef.OrderByDescending("score").Limit(10);
        int i = 1;
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshot) => {
            rankingPanel.SetActive(true);

            foreach (DocumentSnapshot docSnapshot in querySnapshot.Result.Documents)
            {
                Dictionary<string, object> doc = docSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in doc)
                {
                    if (pair.Key == "score")
                    {
                        Debug.Log($"{docSnapshot.Id} : {pair.Value}");

                        GameObject rankingItem = Instantiate(rankingPrefab);
                        rankingItem.transform.SetParent(content);
                        rankingItem.transform.localScale = Vector3.one;

                        if (i < 4)
                        {
                            rankingItem.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
                            i++;
                        }
                        else
                        {
                            rankingItem.transform.GetChild(0).GetComponent<Text>().text = "";
                        }

                        rankingItem.transform.GetChild(1).GetComponent<Text>().text = docSnapshot.Id;
                        rankingItem.transform.GetChild(2).GetComponent<Text>().text = pair.Value.ToString();
                    }
                }
            }

        });

        loadingPanel.SetActive(false);
    }

    public void ControllGold(bool value)
    {
        if (value)
            gold += 10;
        else
            gold -= 10;

        goldText.text = $"Gold : {gold}";
    }

    public void ControllScore(bool value)
    {
        if (value)
            score += 100;
        else
            score -= 100;

        scoreText.text = $"Score : {score}";
    }

    public void DeleteAllRankingItems()
    {
        //Transform content = GameObject.Find("Content").transform;
        //GameObject[] items = content.GetComponentsInChildren<GameObject>();

        GameObject[] itemTag = GameObject.FindGameObjectsWithTag("RankingItem");
        for (int i = 0; i < itemTag.Length; i++)
        {
            Destroy(itemTag[i]);
        }
        rankingPanel.SetActive(false);
    }
    */

    public void TestAddBtn()
    {
        StartCoroutine(TestAddData());
    }

    IEnumerator TestAddData()
    {
        FirebaseFirestore db;
        yield return db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef;
        yield return docRef = db.Collection("users").Document("alovelace");

        Dictionary<string, object> user = new Dictionary<string, object>
        {
            {"First", "Ada" },
            {"Last", "Lovelace" },
            {"Born", 1815 }
        };

        docRef.SetAsync(user).ContinueWithOnMainThread(task =>
        {
            Debug.Log("데이터가 추가 되었습니다!!!");
        });
    }
}
