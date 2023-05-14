using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class DragDroppableUI : MonoBehaviour 
{
	// COMPONENTS
	private RectTransform rectTransform;
	private Camera cam;
	
	// VARIABLES
	public bool IsDragging { get; private set; }
	private Vector3 curScreenPos;
	private Vector3 snapBackAnchoredPosition;
	private Tweener snapBackTween;

    // FUNCS
    static public HUDOverlay.GetUIClickStatus IsUIClickedAction;
	public event Action OnSnapBackDoneAction;

	private Vector3 WorldPos {
		get {
			float z = cam.WorldToScreenPoint(transform.position).z;
			return cam.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
		}
	}

	public bool IsClickedOn {
		get {
			bool ClickedOn = false;
            IsUIClickedAction?.Invoke( gameObject, out ClickedOn );
 
            if( ClickedOn )
				return true;
			else
				return false;
		}
	}

	public void SetSnapBackAnchoredPosition( Vector3 snapBackAnchoredPosition ) {
		this.snapBackAnchoredPosition = snapBackAnchoredPosition;
	}

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
		cam = Camera.main;
	}

	public void OnClickPerformed() {
		IsDragging = true;

		StartCoroutine(Drag());
	}

	public void OnClickCanceled() {
		IsDragging = false;
	}

	public void OnScreenPositionPerformed( Vector3 position ) {
		curScreenPos = position;
	}

	private IEnumerator Drag() {
		snapBackTween?.Kill();

		Vector3 offset = transform.position - WorldPos;
		while( IsDragging ) {
			transform.position = WorldPos + offset;

			yield return null;
		}

		if( snapBackAnchoredPosition != null ) {
			snapBackTween = rectTransform
				.DOAnchorPos( endValue: snapBackAnchoredPosition, duration: 0.5f )
				.SetEase( ease: Ease.OutExpo )
				.OnComplete( () => OnSnapBackDoneAction?.Invoke() );
		}
	}
}
