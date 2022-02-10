using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGrid : MonoBehaviour, IProcedural
{
    public float gridSize;
    public int gridResolution;
    public bool doRandomRotation = true;
    public float yOffset = 0f;
    [Range(0,8)]public float randomOffsetAmount = 3;
    public Vector2 randomScaleAmount = new Vector2(1,1);
    [Range(0,1)]public float density;
    public List<GameObject> proceduralAssets;
    public void Generate()
    {
        for (int y = 0; y <= gridResolution; y++)
        {
            for (int x = 0; x <= gridResolution; x++)
            {
                if (Random.Range(0, 1f) > (1 - density))
                {
                    Vector3 p = new Vector3(transform.position.x - gridSize / 2 + (gridSize * x) / gridResolution, transform.position.y, transform.position.z - gridSize / 2 + (gridSize * y) / gridResolution);

                    Vector3 pOffset = new Vector3(Random.Range(-randomOffsetAmount, randomOffsetAmount), 0, Random.Range(-randomOffsetAmount, randomOffsetAmount));

                    p += pOffset;

                    Debug.Log("Trying Generate Point against Mask");
                    bool maskCheck = (ProceduralManager.instance.spawnMasks.pointFallsWithinMask(p));


                    Ray r = new Ray(p, Vector3.down);

                    RaycastHit hit;

                    if (Physics.Raycast(r, out hit) && !maskCheck)
                    {
                        if (hit.collider.tag == "Ground")
                        {
                            GameObject prefab = proceduralAssets[Random.Range(0, proceduralAssets.Count)];
                            
                            GameObject t = Instantiate(prefab, new Vector3(p.x, hit.point.y + yOffset, p.z), prefab.transform.rotation, transform);

                            float scale = Random.Range(randomScaleAmount.x, randomScaleAmount.y);
                            t.transform.localScale = new Vector3(scale * t.transform.localScale.x, scale * t.transform.localScale.y, scale * t.transform.localScale.z);

                            if (doRandomRotation) 
                            {
                                t.transform.RotateAround(Vector3.up, Random.Range(0, 360));
                            }

                            IProcedural branch = t.GetComponent<IProcedural>();
                            
                            if (branch != null) 
                            {
                                branch.Generate();
                            }

                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize, 1, gridSize));
        Gizmos.color = Color.red;

        for (int y = 0; y <= gridResolution; y++) 
        {
            for(int x = 0; x <= gridResolution; x++) 
            {
                Gizmos.DrawSphere(new Vector3(transform.position.x - gridSize/2 + (gridSize * x)/gridResolution , transform.position.y, transform.position.z - gridSize / 2 + (gridSize * y)/gridResolution ), 2f);        
            }
        }
    }
}
