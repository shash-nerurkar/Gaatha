using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class OutputGrid
{
    private Dictionary<int, HashSet<int>> indexPossiblePatternsDictionary = new Dictionary<int, HashSet<int>>();
    public int Width { get; set; }
    public int Height { get; set; }
    private int maxPatternCount = 0;

    public OutputGrid(int width, int height, int maxPatternCount ) {
        Width = width;
        Height = height;
        this.maxPatternCount = maxPatternCount;

        ResetAllPossibilities();
    }

    public void ResetAllPossibilities() {
        HashSet<int> allPossiblePatternsList = new HashSet<int>();

        allPossiblePatternsList.UnionWith( Enumerable.Range( 0, maxPatternCount ).ToList() );

        indexPossiblePatternsDictionary.Clear();

        for( int index = 0; index < Height * Width; index++ )
            indexPossiblePatternsDictionary.Add( index, new HashSet<int>( allPossiblePatternsList ) );
    }

    public bool CheckCell( Vector2Int position ) => indexPossiblePatternsDictionary.ContainsKey( GetIndex( position ) );

    private int GetIndex( Vector2Int position ) => position.x + Width * position.y;

    private Vector2Int GetCoordinates( int index ) {
        Vector2Int coords = Vector2Int.zero;

        coords.x = index % this.Width;
        coords.y = index / this.Height;

        return coords;
    }

    public bool IsCellCollapsed( Vector2Int position ) => GetPossibleValues( position ).Count <= 1;

    public HashSet<int> GetPossibleValues( Vector2Int position ) {
        int index = GetIndex( position );
        if( indexPossiblePatternsDictionary.ContainsKey( index ) )
            return indexPossiblePatternsDictionary[ index ];
        else
            return new HashSet<int>();
    }

    public bool IsGridSolved() => indexPossiblePatternsDictionary.Any( x => x.Value.Count > 1 ) == false;

    internal bool CheckPositionValidity( Vector2Int position ) => MyCollectionExtension.ValidateCoordinates( position.x, position.y, Width, Height );

    public Vector2Int GetRandomCell() {
        int randomIndex = UnityEngine.Random.Range( 0, indexPossiblePatternsDictionary.Count );

        return GetCoordinates( randomIndex );
    }

    public void SetPatternOnPosition( Vector2Int position, int patternIndex ) {
        int index = GetIndex( position );

        indexPossiblePatternsDictionary[ index ] = new HashSet<int>() { patternIndex };
    }

    public int[][] GetSolvedOutputGrid() {
        if( IsGridSolved() == false ) {
            return MyCollectionExtension.CreateJaggedArray<int[][]>( 0, 0 );
        }
        else {
            int[][] returnGrid = MyCollectionExtension.CreateJaggedArray<int[][]>( Height, Width );

            for( int row = 0; row < Height; row++ )
                for( int col = 0; col < Width; col++ ) {
                    int index = GetIndex( new Vector2Int( col, row ) );
                    returnGrid[ row ][ col ] = indexPossiblePatternsDictionary[ index ].First();
                }
            
            return returnGrid;
        }
    }

    internal void PrintResultsToConsole() {
        // DEBUG START
        StringBuilder builder = null;
        List<string> list = new List<string>();

        for ( int row = 0; row < Height; row++ ) {
            builder = new StringBuilder();

            for ( int col = 0; col < Width; col++ ) {
                HashSet<int> result = GetPossibleValues( new Vector2Int( row, col ) );

                if( result.Count == 1 ) {
                    builder.Append( result.First() );
                }
                else {
                    foreach ( int value in result ) {
                        builder.Append( value + "," );
                    }
                }
                builder.Append( " " );
            }
            
            list.Add( builder.ToString() );
        }
        list.Reverse();

        Debug.Log("---Print WFC Results To Console---");
        foreach( string str in list )
            Debug.Log( str );
        Debug.Log("------");
        // DEBUG END
    }
}