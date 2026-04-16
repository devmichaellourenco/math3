using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonColorChanger : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Color p1Color;
    [SerializeField] private Color p2Color;

    private Image button;

    void Start()
    {
        button = GetComponent<Image>();   
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            button.color = p1Color;
        }else if(eventData.button == PointerEventData.InputButton.Right)
        {
            button.color = p2Color;
        }
    }
}
