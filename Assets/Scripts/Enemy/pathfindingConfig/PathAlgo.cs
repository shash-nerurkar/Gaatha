using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PathAlgo : MonoBehaviour
{   
    PathRequestManager requestManager;
    Grid grid;
    void Awake(){
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos){
         StartCoroutine(FindPath(startPos, targetPos));
    }
    IEnumerator FindPath(Vector3 startPos, Vector3 TargetPos){
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(TargetPos);
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0){
            Node currentNode = openSet.RemoveFirst();
            closeSet.Add(currentNode);

            if(currentNode == endNode){
                
                pathSuccess = true;
                break;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode)){
                if(!neighbour.isWalkable || closeSet.Contains(neighbour))
                    continue;

                int newNeighbourCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newNeighbourCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)){
                    neighbour.gCost = newNeighbourCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }
        yield return null;
        if(pathSuccess){
            waypoints = RetracePath(startNode, endNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode){
        List<Node> path =  new List<Node>();
        Node currentNode = endNode;
        while(currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        } 
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }
    Vector3[] SimplifyPath(List<Node> path){
        if(path.Count<1)
            return new Vector3[0];
        List<Vector3> waypoints = new List<Vector3>();
        waypoints.Add(path[0].WorldPos);
        for(int i=1; i<path.Count-1; i++){
            Vector2 directionNew = new Vector2(path[i+1].gridX-path[i].gridX, path[i+1].gridY-path[i].gridY);
            Vector2 directionOld = new Vector2(path[i-1].gridX-path[i].gridX, path[i-1].gridY-path[i].gridY);
            if(directionNew != directionOld){
                waypoints.Add(path[i].WorldPos);
            }
        }
        waypoints.Add(path[path.Count-1].WorldPos);
        return waypoints.ToArray();
    }
    public int GetDistance(Node nodeA, Node nodeB){
        int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (disX>disY)
            return 14*disY + 10*(disX - disY);
        return 14*disX + 10*(disY - disX);
    }
}
