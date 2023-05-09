using System.Collections;
using UnityEngine;

public class DragDroppableUI : MonoBehaviour 
{
	// COMPONENTS
	private Camera cam;
	
	// VARIABLES
	private bool isDragging;
	private Vector3 curScreenPos;

    // FUNCS
    static public HUDOverlay.GetUIClickStatus IsUIClickedAction;

	private Vector3 WorldPos {
		get {
			float z = cam.WorldToScreenPoint(transform.position).z;
			return cam.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
		}
	}

	private bool isClickedOn {
		get {
			bool isClickedOn = false;
            IsUIClickedAction?.Invoke( gameObject, out isClickedOn );
 
            if( isClickedOn )
				return true;
			else
				return false;
		}
	}
	
	private void Awake() {
		cam = Camera.main;
	}

	public void OnClickPerformed() {
		isDragging = true;

		if(isClickedOn)	StartCoroutine(Drag());
	}

	public void OnClickCanceled() {
		isDragging = false;
	}

	public void OnScreenPositionPerformed( Vector3 position ) {
		curScreenPos = position;
	}

	private IEnumerator Drag() {
		Vector3 offset = transform.position - WorldPos;
		while( isDragging ) {
			transform.position = WorldPos + offset;

			yield return null;
		}
	}
}
