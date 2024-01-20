#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


namespace WorldGeneration
{
    namespace WaxWorldGeneration
    {
        [CustomEditor(typeof(WaxWorldGenMain) ) ]
        public class WaxWorldGenInspector : Editor
        {
            public override void OnInspectorGUI ( ) {
                DrawDefaultInspector ( );
                
                WaxWorldGenMain myScript = ( WaxWorldGenMain ) target;
                
                if(GUILayout.Button ( "Generate Map" ) )
                    myScript.GenerateMap ( );
            }
        }
    }
}

#endif