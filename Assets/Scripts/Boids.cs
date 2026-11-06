using UnityEngine;

public class Boid : MonoBehaviour
{
    [HideInInspector] public BoidManager manager;
    private Vector3 velocity;
    private Renderer boidRenderer;
    private MaterialPropertyBlock propBlock;

    public void Initialize(BoidManager boidManager, Vector3 spawnPosition)
    {
        manager = boidManager;
        transform.position = spawnPosition;
        velocity = Random.insideUnitSphere * manager.moveSpeed;

        boidRenderer = GetComponentInChildren<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }

    public void UpdateBoid(System.Collections.Generic.List<Boid> allBoids)
    {
        Vector3 acceleration = Vector3.zero;

        Vector3 cohesion = ComputeCohesion(allBoids);
        Vector3 separation = ComputeSeparation(allBoids);
        Vector3 alignment = ComputeAlignment(allBoids);

        //adds a pull to the tower
        if (manager.towerTarget != null)
        {
            Vector3 toTower = manager.towerTarget.position - transform.position;
            toTower.y = 0; //keeps them orbiting horizontally
            float dist = toTower.magnitude;
            
            //maintains orbit
            Vector3 orbitDir = Vector3.Cross(toTower.normalized, Vector3.up);
            acceleration += orbitDir * 2f;

            if (dist > manager.orbitRadius + 3f)
                acceleration += toTower.normalized * 1.5f;
            else if (dist < manager.orbitRadius - 3f)
                acceleration -= toTower.normalized * 1.5f;
        }

        acceleration += cohesion * manager.cohesionWeight;
        acceleration += separation * manager.separationWeight;
        acceleration += alignment * manager.alignmentWeight;

        Vector3 steeringForce = Vector3.ClampMagnitude(acceleration, manager.maxSteeringForce);

        velocity += steeringForce * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, manager.moveSpeed);

        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        transform.position += velocity * Time.deltaTime;
        UpdateColorByHeight();
    }

    private void UpdateColorByHeight() //updates the color based on height
    {
        if (boidRenderer == null) return;

        float minY = manager.towerTarget != null ? manager.towerTarget.position.y : 0f;
        float maxY = minY + 15f;
        float heightLerp = Mathf.Clamp01((transform.position.y - minY) / (maxY - minY));

        Color dynamicColor = Color.Lerp(manager.baseColor, manager.heightColor, heightLerp);

        boidRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", dynamicColor);
        propBlock.SetColor("_BaseColor", dynamicColor); 
        boidRenderer.SetPropertyBlock(propBlock);
    }

    private Vector3 ComputeCohesion(System.Collections.Generic.List<Boid> allBoids)
    {
        Vector3 centerOfMass = Vector3.zero;
        int count = 0;

        foreach (var boid in allBoids)
        {
            if (boid != this)
            {
                float d = Vector3.Distance(transform.position, boid.transform.position);
                if (d < manager.perception)
                {
                    centerOfMass += boid.transform.position;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            centerOfMass /= count;
            Vector3 desired = centerOfMass - transform.position;
            return desired.normalized * manager.moveSpeed - velocity;
        }

        return Vector3.zero;
    }

    private Vector3 ComputeSeparation(System.Collections.Generic.List<Boid> allBoids)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (var boid in allBoids)
        {
            if (boid != this)
            {
                float d = Vector3.Distance(transform.position, boid.transform.position);
                if (d < manager.perception && d > 0f)
                {
                    Vector3 diff = transform.position - boid.transform.position;
                    diff.Normalize();
                    diff /= d;
                    steer += diff;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            steer /= count;
            if (steer != Vector3.zero)
            {
                steer = steer.normalized * manager.moveSpeed - velocity;
            }
        }

        return steer;
    }

    private Vector3 ComputeAlignment(System.Collections.Generic.List<Boid> allBoids)
    {
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;

        foreach (var boid in allBoids)
        {
            if (boid != this)
            {
                float d = Vector3.Distance(transform.position, boid.transform.position);
                if (d < manager.perception)
                {
                    averageVelocity += boid.velocity;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            averageVelocity /= count;
            return averageVelocity.normalized * manager.moveSpeed - velocity;
        }

        return Vector3.zero;
    }
}