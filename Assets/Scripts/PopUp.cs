using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject popUpPanel;
    public GameObject popUpWindow;

    public void OnClickCloseBtn()
    {
        popUpPanel.SetActive(false);
    }

}
