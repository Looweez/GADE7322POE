using UnityEngine;

public class PathNode
{
    public int x;
    public int z;
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;
    public PathNode parent;

    public PathNode(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    
    //this is a helper class for grid coordinate representatin when making pathways
}
