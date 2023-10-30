using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class audioON : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private AudioSource m_Source;
    public void OnPointerClick(PointerEventData eventData)
    {
        m_Source.volume = 0.4f;

    }
}