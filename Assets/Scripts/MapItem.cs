using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapItem : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
    /// <summary>
    /// Загрузка изображения в префаб
    /// </summary>
    /// <param name="sprite"></param>
    public void SetImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Clicked the Collider!");
    }
}
