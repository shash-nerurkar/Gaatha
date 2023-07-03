using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDroppable : MonoBehaviour 
{
	// COMPONENTS
	[SerializeField] private InputAction press, screenPos;
	private Camera cam;
	
	// VARIABLES
	private bool isDragging;
	private Vector3 curScreenPos;

	private Vector3 WorldPos {
		get {
			float z = cam.WorldToScreenPoint(transform.position).z;
			return cam.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
		}
	}

	private bool isClickedOn {
		get {
			Ray ray = cam.ScreenPointToRay(curScreenPos);
			RaycastHit hit;

			if(Physics.Raycast(
				ray: ray, 
				hitInfo: out hit,
				maxDistance: Mathf.Infinity,
				layerMask: LayerMask.NameToLayer(Constants.LAYERMASK_UI)
			)) 	
				return hit.transform == transform;
			else 								
				return false;
		}
	}
	
	private void Awake() 
	{
		cam = Camera.main;
		
		screenPos.Enable();
		press.Enable();
		
		screenPos.performed += context => {
			curScreenPos = context.ReadValue<Vector2>();
		};
		
		press.performed += _ => {
			if(isClickedOn) 
				StartCoroutine(Drag());
		};

		press.canceled += _ => {
			isDragging = false;
		};
	}

	private IEnumerator Drag()
	{
		isDragging = true;
		
		GetComponent<Rigidbody>().useGravity = false;
		
		Vector3 offset = transform.position - WorldPos;
		while( isDragging ) {
			transform.position = WorldPos + offset;
			yield return null;
		}
		
		GetComponent<Rigidbody>().useGravity = true;
	}
}
