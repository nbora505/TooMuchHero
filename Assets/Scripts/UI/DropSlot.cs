using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var draggable = eventData.pointerDrag.GetComponent<TokenController>();
        if (draggable == null) return;

        // 이 슬롯이 비어있는지 체크(자식이 없는 경우)
        if (transform.childCount == 0)
        {
            draggable.SnapTo(transform);
        }
        else
        {
            // 교체 로직이 필요하면 여기서 처리
            var existing = transform.GetChild(0);
            existing.SetParent(draggable.transform.parent, true);
            draggable.SnapTo(transform);
        }
    }
}
