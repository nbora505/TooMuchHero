using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite normalImage;
    public Sprite pressedImage;

    Button btn;
    Image btnImage;
    
    Transform[] childTransforms;

    private void Start()
    {
        btn = GetComponent<Button>();
        btnImage = GetComponent<Image>();
        btnImage.sprite = normalImage;

        int childCount = transform.childCount;
        childTransforms = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (btn.interactable)
        {
            btnImage.sprite = pressedImage;

            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i].localScale = new Vector3(0.8f, 0.65f, 0.8f);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn.interactable)
        {
            btnImage.sprite = normalImage;

            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i].localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
