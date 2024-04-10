using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public MapNode upNeighbor;
    public MapNode rightNeighbor;
    public MapNode downNeighbor;
    public MapNode leftNeighbor;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (MapNode neighbor in new MapNode[] { upNeighbor, rightNeighbor, downNeighbor, leftNeighbor })
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }
    }

    public List<MapNode> NeighborsCount()
    {
        List<MapNode> neighbors = new List<MapNode>();

        foreach (MapNode neighbor in new MapNode[] { upNeighbor, rightNeighbor, downNeighbor, leftNeighbor })
        {
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }
        
        return neighbors;
    }  
}
