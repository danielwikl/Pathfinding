

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// this class creates the grid and stores the node arrays
public class Grid : MonoBehaviour
{
    public Transform StartPostition; // this is where the program starts pathfinding from
    public LayerMask WallMask; //for the program to compare against when looking for obstrictions on the map 
    public Vector2 gridWorldSize; // stores the width and height of the graph in real world units
    public float nodeRadious; // the size of the nodes
    public float distance; // how far the nodes spawn from eachother

    Node[,] grid; //a two dimensional array to store the grid in
    public List<Node> FinalPath; // stores the completed path that A* finds

    float nodeDiameter; //stores the diameter of the node
    int gridSizeX, gridSizeY; // stores the two positions of the array unit

    private void Start()
    {
        nodeDiameter = nodeRadious * 2; // sets the diameter to be twice the radious
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

      void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];// create the node array that we calculated in the start function
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; // finding the bottom left of the graph in world coordinates

        // forloop to loop trough the nodes one by one
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // get the world position of the current node
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadious) + Vector3.forward * (y * nodeDiameter + nodeRadious);
                // is the current node a wall node?
                bool Wall = true;

                if(Physics.CheckSphere(worldPoint, nodeRadious, WallMask)) // check if its colliding with anything with a wallmass, if so boolean sets to true
                {
                    Wall = false;
                }
                // we then create a new node in the array using the variables that we just calculated
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPosition(Vector3 a_WorldPosition)
    {
        // two variables that convert the wolrd position to a position in node array

        float xPoint = ((a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((a_WorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.x);
        
        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);

        // gives us the current position in hte node array
        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, y];
    }


    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node neighboringNode)
    {
        List<Node> NeighborList = new List<Node>();
        int xCheck;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int yCheck;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        // check right side
        xCheck = neighboringNode.gridX + 1;
        yCheck = neighboringNode.gridY;

        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }
            
        }

        //Left side
        xCheck = neighboringNode.gridX - 1;
        yCheck = neighboringNode.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }

        }

        //Top side
        xCheck = neighboringNode.gridX;
        yCheck = neighboringNode.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }

        }
        //bottom Side
        xCheck = neighboringNode.gridX;
        yCheck = neighboringNode.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighborList.Add(grid[xCheck, yCheck]);
            }

        }

        return NeighborList;
    }



    // drawing the grid using unity gizmos
    private void OnDrawGizmos()
    {
        // declare the gridbox so we can controll the size of the grid
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); // DrawWireCube creates a wirecube in unity with the set dimensions in unity editor
        
        // Checks if the grid has been initialized to avoid null reference errors
        if(grid !=null)
        {
            // loop trough the array to check if the node is a wall or not
            foreach(Node node in grid)
            {
                if (node.isWall) // s
                {
                    Gizmos.color = Color.white; //if its not a wall color is set to white
                }
                else
                {
                    Gizmos.color = Color.yellow; // if its a wall color = yellow
                }
                if(FinalPath != null)//If the final path is not empty
                {
                    if (FinalPath.Contains(node))
                    {
                        Gizmos.color = Color.red; // if it does color = red
                    }
                }
                // draw out the cubes with the drawCube function
                Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}
