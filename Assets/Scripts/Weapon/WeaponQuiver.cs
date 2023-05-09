using UnityEngine;
using UnityEngine.Rendering;

public class WeaponQuiver : MonoBehaviour {
    public SortingGroup SortingGroup { get; private set; }

    void Awake() {
        SortingGroup = GetComponent<SortingGroup>();
    }
}
