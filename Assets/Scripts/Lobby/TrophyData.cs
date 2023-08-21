using UnityEngine;


[CreateAssetMenu(fileName = "TrophyData", menuName = "Trophy/TrophyData")]
public class TrophyData : ScriptableObject
{
    public new string name;
    public string description;
    public RuntimeAnimatorController panelRewardAnimatorController;
}
