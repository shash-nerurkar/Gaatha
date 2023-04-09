using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: IHeapItem<Node>{
    public bool isWalkable;
    public Vector3 WorldPos;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;
    int heapIndex;

    public Node(bool isWalkable, Vector3 worldPos, int gridX, int gridY){
        this.isWalkable = isWalkable;
        this.WorldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost{
        get{
            return gCost+hCost;
        }
    }
    public int HeapIndex{
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }
    public int CompareTo(Node nodeToCOmpare){
        int compare = fCost.CompareTo(nodeToCOmpare.fCost);
        if(compare == 0){
            compare = hCost.CompareTo(nodeToCOmpare.hCost);
        }
        return -compare;
    }
}
