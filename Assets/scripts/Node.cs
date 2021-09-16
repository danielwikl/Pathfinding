using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX;// checks what position the nodes are at in the array that will be created later
    public int gridY;// checks what position the nodes are at in the array that will be created later

    public bool isWall; // check if the node is beeing obstructed or not

    public Vector3 Position;// checks the nodes real world position
    public Node Parent;// tracks the path back to the beginning for the algoritm

    public int gCost;//Gcost = distance from starting node
    public int hCost;//Hcost = distance from end node
    public int FCost {get { return gCost + hCost; } }//Fcost = Gcost + Hcost   with a get function

    public Node(bool a_isWall, Vector3 a_Pos, int A_gridX, int a_gridY) //Constructor the two ints to set the gridposition
    {
        isWall = a_isWall; // tells the program if the node is beeing obstructed
        Position = a_Pos; // the world position of the node
        gridX = A_gridX; // x postion in the array
        gridY = a_gridY; // y position in the array
    }
}
