using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    Grid grid;
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Awake()
    {
        grid = GetComponent<Grid>();//Get a reference to the game manager
    }

    private void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);//Find a path to the goalA
    }

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = grid.NodeFromWorldPosition(a_StartPos);
        Node TargetNode = grid.NodeFromWorldPosition(a_TargetPos);

        //list that stores set of current discoverd nodes that havent been valuated yet
        // hashset same as list but doesent hold any values, once a node is in the closed set we dont need to check it anymore
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();


        OpenList.Add(StartNode);//Add the starting node to the open list to begin the program

        // runs while count is > 0
        while (OpenList.Count > 0)
        {
            //creating a node and setting it to the first object in the OpenList
            Node CurrentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i]; // set the currnt node to the current OpenList object since that object is closer to the goal than the current node
                }
            }
            // after the forloop is finnished we can remove the current node from the OpenList to the closed list since we dont need to check it anymore
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if(CurrentNode == TargetNode) // if the currnet node is equal to targetnode then the program has found a path towards the final goal
            {
                GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            foreach (Node NeighbourNode in grid.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighbourNode.isWall || ClosedList.Contains(NeighbourNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighbourNode);//Get the F cost of that neighbor

                if (MoveCost < CurrentNode.gCost || !OpenList.Contains(NeighbourNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighbourNode.gCost = MoveCost;//Set the g cost to the f cost
                    NeighbourNode.hCost = GetManhattenDistance(NeighbourNode, TargetNode);//Set the h cost
                    NeighbourNode.Parent = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighbourNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighbourNode);//Add it to the list
                    }
                }
            }
        }
    }


    // okay i need to readup on whats happening here 
    void GetFinalPath(Node startNode, Node targetNode)
    {
        // a list that stores the finalPath
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = targetNode;//Node to store the current node being checked

        while (CurrentNode != startNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
        FinalPath.Reverse();//Reverse the path to get the correct order
        grid.FinalPath = FinalPath;//Set the final path
    }


    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);


        return ix + iy;

    }
}

