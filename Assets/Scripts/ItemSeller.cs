using UnityEngine;
using UnityEngine.UI;

public class ItemSeller : MonoBehaviour
{
    [HideInInspector]
    public GameObject selectedToken;

    public ItemMaker IM;
    public DeckManager DM;
    public Animator leftBtn;
    public Button btn_sell;
    public Button btn_keep;
    public Text text_sell;
    public Text text_keep;
    public Image image_sell;
    public Image image_keep;

    public void Sell()
    {
        if (selectedToken != null)
        {
            DM.deck[DM.DeckIndexFor(selectedToken.GetComponent<TokenController>().CurrentSlot)] = null;
            Destroy(selectedToken);
        }
    }

    public void Keep()
    {
        if (selectedToken != null)
        {
            if(selectedToken.tag == "character")
            {
                if (!IM.keepList_C.Contains(selectedToken))
                    IM.keepList_C.Add(selectedToken);
                else
                    IM.keepList_C.Remove(selectedToken);
            }
            else
            {
                if (!IM.keepList_I.Contains(selectedToken))
                    IM.keepList_I.Add(selectedToken);
                else
                    IM.keepList_I.Remove(selectedToken);
            }
        }
    }

    public bool ChangeSelectedToken(GameObject newSelected)
    {
        if (selectedToken != null)
        {
            selectedToken.GetComponent<TokenController>().window_description.SetActive(false);
            
            if (selectedToken == newSelected)
            {
                selectedToken = null;

                return false;
            }
        }

        selectedToken = newSelected;
        return true;
    }

    public void ChangeSellOrKeepButton(bool isInDeck)
    {
        btn_sell.gameObject.SetActive(false);
        btn_keep.gameObject.SetActive(false);
        SetButtonOff();

        if (isInDeck)
        {
            btn_sell.gameObject.SetActive(true);
            btn_sell.interactable = true;
            text_sell.color = Color.white;
            image_sell.color = Color.white;
            leftBtn.SetTrigger("SELL");
        }
        else
        {
            btn_keep.gameObject.SetActive(true);
            btn_keep.interactable = true;
            text_keep.color = Color.white;
            image_keep.color = Color.white;
            leftBtn.SetTrigger("KEEP");
        }
    }

    public void SetButtonOff()
    {
        btn_sell.interactable = false;
        btn_keep.interactable = false;

        text_keep.color = Color.gray;
        text_sell.color = Color.gray;
        image_keep.color = Color.gray;
        image_sell.color = Color.gray;

    }
}
