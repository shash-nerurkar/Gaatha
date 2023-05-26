using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class CustomButtonEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action onPointerDown;
    public event Action onPointerUp;
    public event Action whilePointerPressed;

    // COMPONENTS
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
    }

    private IEnumerator WhilePressed() {
        while(true) {
             whilePointerPressed?.Invoke();
             yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(!_button.interactable) return;

        StopAllCoroutines();

        onPointerDown?.Invoke();

        StartCoroutine( WhilePressed() );
    }

    public void OnPointerUp(PointerEventData eventData) {
        StopAllCoroutines();
        onPointerUp?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopAllCoroutines();
    }

    public void OnPointerEnter(PointerEventData eventData) {}
}