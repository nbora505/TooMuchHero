using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public DeckManager deckManager;
    [Tooltip("¿ÞÂÊ¡æ¿À¸¥ÂÊ = 0..4")] public int leftToRightIndex;

    public TokenController TC =>
        transform.childCount > 0 ? transform.GetChild(0).GetComponent<TokenController>() : null;


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        var draggable = eventData.pointerDrag.GetComponent<TokenController>();
        if (draggable == null) return;

        if (draggable.gameObject.tag == "character")
            deckManager.HandleDrop(this, draggable);
        else
            deckManager.ItemDrop();
    }
}
