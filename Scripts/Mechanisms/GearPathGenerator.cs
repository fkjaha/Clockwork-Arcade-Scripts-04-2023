using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GearPathGenerator : MonoBehaviour
{
    [SerializeField] private List<ChildGear> gearPrefabs;
    [SerializeField] private List<GearLayout> gearPrefabsLayouts;
    [SerializeField] private float distanceBetweenMainGears;
    [SerializeField] private float minDistanceBetweenGears;
    [Space(20f)] 
    [SerializeField] private GearPath gearPath;
    [SerializeField] private Gear fromGear;
    [SerializeField] private Gear toGear;
    [SerializeField] private Transform fromTransform;
    [SerializeField] private Transform toTransform;

    private Vector3 _fromPos;
    private Vector3 _toPos;
    
#if UNITY_EDITOR


    [ContextMenu("Spawn")]
    public void Spawn()
    {
        Vector3 toPosition = toGear == null ? toTransform.position :
            (toGear.transform.position + 
             ((fromGear == null ? fromTransform.position : fromGear.transform.position) - toGear.transform.position).normalized * toGear.GetRadius);
        Vector3 fromPosition = fromGear == null ? fromTransform.position :
            (fromGear.transform.position + 
             ((toGear == null ? toTransform.position : toGear.transform.position) - fromGear.transform.position).normalized * fromGear.GetRadius);
        _fromPos = fromPosition;
        _toPos = toPosition;
        // SpawnGears(GeneratePath(fromTransform.position, toTransform.position, gearPrefabsLayouts));
        SpawnGears(GeneratePath(fromPosition, toPosition, gearPrefabsLayouts));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_fromPos, .2f);
        Gizmos.DrawSphere(_toPos, .2f);
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        gearPath.ClearPath();
    }
    
    [ContextMenu("Clear and Spawn")]
    public void ClearAndSpawn()
    {
        Clear();
        Spawn();
    }

    private void SpawnGears(List<GearLayout> layouts)
    {
        foreach (var gearLayout in layouts)
        {
            var layout = gearLayout;
            ChildGear gearPrefab = gearPrefabs.Find(g => Math.Abs(g.GetRadius - layout.radius) < .001f);
            if (gearPrefab == null) return;
            // ChildGear spawned = Instantiate(gearPrefab, gearLayout.position, Quaternion.identity);
            ChildGear spawned = PrefabUtility.InstantiatePrefab(gearPrefab) as ChildGear;
            spawned.transform.position = gearLayout.position;
            gearPath.AddToPath(spawned);
        }
    }
    private List<GearLayout> GeneratePath(Vector3 fromPosition, Vector3 toPosition, List<GearLayout> possibleGears, int numberOfGears = 0)
    {
        Vector3 lineVector = toPosition - fromPosition;
        Vector3 lineVectorNormalized = lineVector.normalized;
        GearLayout randomGearLayout;
        List<GearLayout> results = new();
        List<GearLayout> additionalResults = new();
        float distanceLeft = lineVector.magnitude;
        Vector3 nextPivotPosition = fromPosition;

        while (true)
        {
            List<GearLayout> filteredLayouts = possibleGears.Where(g => g.radius * 2 <= distanceLeft).ToList();
            if (distanceLeft <= 0 || filteredLayouts.Count == 0) break;
            
            randomGearLayout = filteredLayouts.GetRandom();

            randomGearLayout.position = nextPivotPosition + lineVectorNormalized * randomGearLayout.radius;
            distanceLeft -= randomGearLayout.radius * 2;
            distanceLeft -= distanceBetweenMainGears;
            results.Add(randomGearLayout);
            nextPivotPosition = randomGearLayout.position + lineVectorNormalized * (randomGearLayout.radius + distanceBetweenMainGears);
        }

        GearLayout lastGear = new GearLayout(0, Vector3.one * Mathf.Infinity);

        for (int i = 0; i < results.Count; i++)
        {
            GearLayout firstGear = results[i];
            GearLayout secondGear = (i != results.Count - 1) ? results[i+1] :
                (toGear == null ? new GearLayout(0f, toPosition) : new GearLayout(toGear.GetRadius, toGear.transform.position));

            int answerIndex = Random.Range(0, 2);
            
            List<GearLayout> filteredLayouts = possibleGears
                .Where(g =>
                {
                    Vector3 position = GetPositionOfSideCircle(g.radius, firstGear, secondGear, answerIndex == 1);
                    
                    return Vector3.Distance(position, lastGear.position) >=
                           g.radius + lastGear.radius + minDistanceBetweenGears;
                }).ToList();

            if (filteredLayouts.Count <= 0)
                return GeneratePath(fromPosition, toPosition, possibleGears, numberOfGears);
            
            randomGearLayout = filteredLayouts.GetRandom();
            
            randomGearLayout.position = GetPositionOfSideCircle(randomGearLayout.radius, firstGear, secondGear, answerIndex == 1);
            additionalResults.Add(randomGearLayout);
            lastGear = randomGearLayout;
        }

        results.AddRange(additionalResults);

        results = results.OrderBy(r => Vector3.Distance(fromPosition, r.position)).ToList();
        
        return results;
    }

    private Vector3 GetPositionOfSideCircle(float circleRadius, GearLayout firstLayout, GearLayout secondLayout, bool getFirstResult)
    {
        firstLayout.radius += circleRadius;
        secondLayout.radius += circleRadius;
        float distanceBetweenLayouts = Vector3.Distance(firstLayout.position, secondLayout.position);
        float a = (firstLayout.radius * firstLayout.radius - secondLayout.radius * secondLayout.radius +
                   distanceBetweenLayouts * distanceBetweenLayouts);
        a /= 2 * distanceBetweenLayouts;
        float h = Mathf.Sqrt((firstLayout.radius * firstLayout.radius - a * a));

        Vector3 p3 = firstLayout.position + a * (secondLayout.position - firstLayout.position) / distanceBetweenLayouts;
        
        float x4;
        float z4;
        
        if (getFirstResult)
        {
            x4 = p3.x + (h / distanceBetweenLayouts) * (secondLayout.position.z - firstLayout.position.z);
            z4 = p3.z - (h / distanceBetweenLayouts) * (secondLayout.position.x - firstLayout.position.x);
        }
        else
        {
            x4 = p3.x - (h / distanceBetweenLayouts) * (secondLayout.position.z - firstLayout.position.z);
            z4 = p3.z + (h / distanceBetweenLayouts) * (secondLayout.position.x - firstLayout.position.x);
        }
        
        Vector3 result = new Vector3(x4, firstLayout.position.y, z4);
        
        return result;
    }
#endif

}

[Serializable]
public struct GearLayout
{
    public float radius;
    public Vector3 position;

    public GearLayout(float r, Vector3 p)
    {
        radius = r;
        position = p;
    }
    
    public GearLayout(GearLayout copyTarget)
    {
        this = copyTarget;
    }
    
}
