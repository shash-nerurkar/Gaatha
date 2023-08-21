using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Helpers 
{
    public static class Vector3IntHelper
    {
        public static Vector2Int GetRandomVectorBetween ( Vector2Int minVector, Vector2Int maxVector ) {
            return ( Vector2Int ) GetRandomVectorBetween ( ( Vector3Int ) minVector, ( Vector3Int ) maxVector );
        }

        public static Vector3Int GetRandomVectorBetween ( Vector3Int minVector, Vector3Int maxVector ) {
            return new Vector3Int ( 
                Random.Range ( minVector.x, maxVector.x ),
                Random.Range ( minVector.y, maxVector.y ),
                Random.Range ( minVector.z, maxVector.z )
            );
        }

        public static Vector3Int Vector3IntLerp ( Vector3Int a, Vector3Int b, float t ) {
            int x = Mathf.RoundToInt ( Mathf.Lerp ( a.x, b.x, t ) );
            int y = Mathf.RoundToInt ( Mathf.Lerp ( a.y, b.y, t ) );
            int z = Mathf.RoundToInt ( Mathf.Lerp ( a.z, b.z, t ) );

            return new Vector3Int(x, y, z);
        }

        public static List < Vector2Int > ToVector2IntList ( List < Vector3Int > list ) {
            List < Vector2Int > resultList = new List < Vector2Int > ( );

            foreach ( Vector3Int point in list )
                resultList.Add ( ( Vector2Int ) point );
            
            return resultList;
        }


        public static void SortVector3IntArray ( Vector3Int[ ] array ) {
            Array.Sort ( array, new Vector3IntComparer ( ) );
        }
    
        public static bool ArrayContains ( Vector3Int[ ] array, Vector3Int target ) {
            if ( array == null || array.Length == 0 )
                return false;

            for ( int i = 0; i < array.Length; i++ )
                if ( target.Equals ( array[ i ] ) )
                    return true;

            return false;
        }
    
        public static string ArrayToString ( Vector3Int[ ] array ) {
            StringBuilder s = new StringBuilder ( "{" );

            foreach ( Vector3Int vector in array )
                s.Append ( "new Vector3Int " + vector.ToString ( ) + ", " );

            s.Append( "}" );

            return s.ToString ( );
        } 

        public static string ArrayToString ( Vector2Int[ ] array ) {
            StringBuilder s = new StringBuilder ( "{" );

            foreach ( Vector2Int vector in array )
                s.Append ( "new Vector2Int " + vector.ToString ( ) + ", " );

            s.Append( "}" );

            return s.ToString ( );
        } 



        public static Vector3Int GetMinPositionValues ( Vector3Int[ ] vectorArray ) {
            Vector3Int minVector = new Vector3Int ( );

            if ( vectorArray != null && vectorArray.Length != 0 ) {
                minVector = vectorArray [ 0 ];

                for ( int index = 1; index < vectorArray.Length; index++ ) {
                    if ( vectorArray [ index ].x  <  minVector.x )
                        minVector.x = vectorArray [ index ].x;

                    if ( vectorArray [ index ].y  <  minVector.y )
                        minVector.y = vectorArray [ index ].y;

                    if ( vectorArray [ index ].z  <  minVector.z )
                        minVector.z = vectorArray [ index ].z;
                }
            }

            return minVector;
        }

        public static Vector3Int GetMaxPositionValues ( Vector3Int[ ] vectorArray ) {
            Vector3Int maxVector = new Vector3Int ( );

            if ( vectorArray != null && vectorArray.Length != 0 ) {
                maxVector = vectorArray [ 0 ];

                for ( int index = 1; index < vectorArray.Length; index++ ) {
                    if ( vectorArray [ index ].x > maxVector.x )
                        maxVector.x = vectorArray [ index ].x;

                    if ( vectorArray [ index ].y > maxVector.y )
                        maxVector.y = vectorArray [ index ].y;

                    if ( vectorArray [ index ].z > maxVector.z )
                        maxVector.z = vectorArray [ index ].z;
                }
            }

            return maxVector;
        }   
    }
    
    public class Vector3IntComparer : IComparer < Vector3Int > 
    {
        public int Compare( Vector3Int a, Vector3Int b ) {
            if (a.x != b.x)
                return a.x.CompareTo(b.x);

            if (a.y != b.y)
                return a.y.CompareTo(b.y);

            if (a.z != b.z)
                return a.z.CompareTo(b.z);

            return 0;
        }
    }
}




