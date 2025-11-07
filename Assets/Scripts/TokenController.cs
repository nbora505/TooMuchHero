using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public Character_SO data;
    [HideInInspector] public Item_SO itemData;

    [Header("캐릭터 데이터")]
    public int curExp;
    public int curLv;
    public int curAtk;
    public int curHp;

    #region UI Elements
    [Header("UI 요소")]
    public Image tokenImage;
    public Text text_star;
    public Text text_lv;
    public Text text_atk;
    public Text text_hp;
    public Text text_name;
    public Text text_decription;

    [Space]
    public GameObject icon_star;
    public GameObject icon_lv;
    public GameObject window_description;
    public Slider slider_exp;

    [HideInInspector]
    public Canvas canvas;                    // 최상위 캔버스 참조
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private GameObject manager;

    #endregion

    [Header("Bool")]
    public bool isDragable = true;
    private bool isDroppedInSlot = false;
    public bool isInDeck = false;

    public DropSlot CurrentSlot { get; set; }     // 지금 끼워져 있는 슬롯
    public DropSlot OriginalSlotOnDrag { get; private set; } // 드래그 시작 시 슬롯

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();

        manager = GameObject.Find("GameManager");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDroppedInSlot = false;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform, true); // 최상단으로 올려서 가리기 방지
        canvasGroup.blocksRaycasts = false; // 드래그 중에는 슬롯이 Raycast를 받게
        canvasGroup.alpha = 0.8f;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor; // 캔버스 스케일 보정
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // 드롭에 실패했으면 원위치
        if (!isDroppedInSlot)
        {
            transform.SetParent(originalParent, true);
            rect.anchoredPosition = Vector2.zero;
        }
    }

    public void SnapTo(Transform parent, DropSlot slot)
    {
        isDroppedInSlot = true;
        transform.SetParent(parent, true);
        rect.anchoredPosition = Vector2.zero;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        transform.SetAsLastSibling();
        CurrentSlot = slot;

        OnDroppedInSlot();
    }

    void OnDroppedInSlot()
    {
        icon_star.SetActive(false);
        icon_lv.SetActive(true);
        isInDeck = true;

        OnClickedToken();

    }

    public void OnClickedToken()
    {
        ItemSeller IS = manager.GetComponent<ItemSeller>();

        bool selected = IS.ChangeSelectedToken(this.gameObject);

        if (selected)
        {
            window_description.SetActive(true);
            IS.ChangeSellOrKeepButton(isInDeck);
        }
        else
        {
            window_description.SetActive(false);
            IS.SetButtonOff();
        }

    }

}
