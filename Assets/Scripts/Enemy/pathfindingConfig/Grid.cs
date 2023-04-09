using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private float nodeDiameter;
    int gridSizeX, gridSizeY;
    public List<Node> finalPath;
    Node[,] grid;  
    public bool onShowGrid = false;
    
    void Awake(){
        nodeDiameter = 2*nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        CreateGrid();
    }
    public int MaxSize{
        get{
            return gridSizeX*gridSizeY;
        }
    }
    void CreateGrid(){
        grid = new Node[gridSizeX, gridSizeY];
                                                    //Direction * magnitude
        Vector3 worldBottomLeft = transform.position - Vector3.right*(gridWorldSize.x/2) - Vector3.up*(gridWorldSize.y/2);
        //Collision check for cells
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right*(x*nodeDiameter+nodeRadius) + Vector3.up*(y*nodeDiameter+nodeRadius); 
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }    
    }
    public List<Node> GetNeighbours(Node currentNode){
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++){
            for(int y = -1; y <= 1; y++){
                if(x == 0 && y == 0)
                    continue;
                int checkX = currentNode.gridX + x;
                int checkY = currentNode.gridY + y;
                if(checkX>=0 && checkX<gridSizeX && checkY>=0 && checkY<gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition){
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }   

    void OnDrawGizmos(){
        if(onShowGrid){
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
            if(grid != null){
                foreach (Node n in grid){
                    Gizmos.color = n.isWalkable?Color.white:Color.red;
                    if(finalPath != null){
                        if(finalPath.Contains(n))
                            Gizmos.color = Color.black;}
                    Gizmos.DrawCube(n.WorldPos, Vector3.one*(nodeDiameter - .1f));
                }
            }
        }
    }
}
