
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetBlock : MonoBehaviour
{

    public LayerMask hitLayers;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //checks if the player has leftclicked (0) represents leftclick
        {
            hitLayers = LayerMask.NameToLayer(null);
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse); //Ray	Creates a ray starting at origin along direction. casts a ray to get were the mouse is poiting at.
            RaycastHit hit; //stores the position where the ray hit


            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) // checks if the ray hits a layer (WALL) or travels to infinity?
            {
                this.transform.position = hit.point; //Moves the target to the mouse position
            }

        }

    }
}

