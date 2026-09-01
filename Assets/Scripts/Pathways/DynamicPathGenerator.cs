using System.Collections.Generic;
using UnityEngine;

public class DynamicPathGenerator : MonoBehaviour
{
    public MeshGenerator meshGenerator;
    
    public List<List<Vector3>> enemyPaths = new List<List<Vector3>>();

    public void GeneratePathways()
    {
        enemyPaths.Clear();
        Vector2Int centerNode = new Vector2Int(meshGenerator.xSize / 2, meshGenerator.zSize / 2);
        List<Vector2Int> spawnNodes = GetThreeRandomEdgeNodes();

        foreach (Vector2Int spawnNode in spawnNodes)
        {
            List<Vector3> worldPath = FindPathAStar(spawnNode, centerNode);
            if (worldPath != null && worldPath.Count > 0)
            {
                enemyPaths.Add(worldPath);
                Debug.Log($"Path generated successfully! Total points: {worldPath.Count}");
            }
            else
            {
                Debug.LogWarning($"Failed to generate path from {spawnNode} to {centerNode}.");
            }
        }

        Debug.Log($"Total valid paths created: {enemyPaths.Count}");
    }

    private List<Vector2Int> GetThreeRandomEdgeNodes()
    {
        List<Vector2Int> edgeNodes = new List<Vector2Int>();

        int xMax = meshGenerator.xSize;
        int zMax = meshGenerator.zSize;
        
        edgeNodes.Add(new Vector2Int(0, Random.Range(2, zMax-2)));
        edgeNodes.Add(new Vector2Int(xMax, Random.Range(2, zMax-2)));
        edgeNodes.Add(new Vector2Int(Random.Range(2,xMax-2), 0));
        
        return edgeNodes;
    }

    private List<Vector3> FindPathAStar(Vector2Int start, Vector2Int target)
{
    PathNode[,] grid = new PathNode[meshGenerator.xSize + 1, meshGenerator.zSize + 1];

    for (int x = 0; x <= meshGenerator.xSize; x++)
        for (int z = 0; z <= meshGenerator.zSize; z++)
            grid[x, z] = new PathNode(x, z);

    List<PathNode> openSet = new List<PathNode>();
    HashSet<PathNode> closedSet = new HashSet<PathNode>();

    PathNode startNode = grid[start.x, start.y];
    PathNode targetNode = grid[target.x, target.y];

    openSet.Add(startNode);

    // SAFETY COUNTER: Prevents Unity from freezing if pathing gets stuck
    int maxIterations = 5000;
    int iterations = 0;

    while (openSet.Count > 0)
    {
        iterations++;
        if (iterations > maxIterations)
        {
            Debug.LogError($"Pathfinding timed out between {start} and {target}!");
            return null;
        }

        PathNode current = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].fCost < current.fCost || (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                current = openSet[i];
        }

        openSet.Remove(current);
        closedSet.Add(current);

        if (current.x == targetNode.x && current.z == targetNode.z)
        {
            return RetracePath(startNode, targetNode);
        }

        foreach (PathNode neighbor in GetNeighbours(grid, current))
        {
            if (closedSet.Contains(neighbor)) continue;

            float currentY = meshGenerator.GetTerrainHeightAt(current.x, current.z);
            float neighborY = meshGenerator.GetTerrainHeightAt(neighbor.x, neighbor.z);
            float slopePenalty = Mathf.Abs(neighborY - currentY) * 5f;

            float newCostToNeighbor = current.gCost + Vector2.Distance(new Vector2(current.x, current.z), new Vector2(neighbor.x, neighbor.z)) + slopePenalty;

            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
            {
                neighbor.gCost = newCostToNeighbor;
                neighbor.hCost = Vector2.Distance(new Vector2(neighbor.x, neighbor.z), new Vector2(targetNode.x, targetNode.z));
                neighbor.parent = current;

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
            }
        }
    }

    return null;
}
    
    private List<Vector3> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        PathNode current = endNode;
    
        int safetyLimit = 1000;
        int steps = 0;

        while (current != startNode && current != null)
        {
            steps++;
            if (steps > safetyLimit)
            {
                Debug.LogError("Infinite loop detected in RetracePath!");
                break;
            }

            float y = meshGenerator.GetTerrainHeightAt(current.x, current.z);
            path.Add(new Vector3(current.x, y + 0.2f, current.z));
            current = current.parent;
        }

        if (current == startNode)
        {
            float startY = meshGenerator.GetTerrainHeightAt(startNode.x, startNode.z);
            path.Add(new Vector3(startNode.x, startY + 0.2f, startNode.z));
        }

        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbours(PathNode[,] grid, PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();
        
        int[] dx = { -1, 1, 0, 0 };
        int[] dz = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.x + dx[i];
            int checkZ = node.z + dz[i];

            if (checkX >= 0 && checkX <= meshGenerator.xSize && checkZ >= 0 && checkZ <= meshGenerator.zSize)
            {
                neighbours.Add(grid[checkX, checkZ]);
            }
        }
        return neighbours;
    }
}
