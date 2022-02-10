using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAsset : MonoBehaviour, IProcedural
{
    [Range(0, 1)] public float spawnChance = .5f;
    public bool spawnGeneric = false;
    public GameObject generic;
    public ProceduralAssetPool pool;
    public List<GameObject> spawnPoints = new List<GameObject>();

    public void Generate() 
    {
        float roll = Random.Range(0, 1f);

        if (roll < spawnChance && spawnGeneric) 
        {

            IProcedural ip = Instantiate(generic, transform.position, generic.transform.rotation).GetComponent<IProcedural>();

        }

        else  if (roll < spawnChance && pool != null) 
        {
            GameObject assetToSpawn = generic;

            if (spawnGeneric)
            {
                GameObject s = Instantiate(assetToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), assetToSpawn.transform.rotation);
            }

            if (pool != null)
            {
                assetToSpawn = pool.DrawAsset();
            }

            


            Ray r = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if(Physics.Raycast(r, out hit)) 
            {
                GameObject s = Instantiate(assetToSpawn, new Vector3(transform.position.x, hit.point.y, transform.position.z), assetToSpawn.transform.rotation);
                IProcedural branch = s.GetComponent<IProcedural>();
                if(branch != null) 
                {
                    branch.Generate();
                }
            }

            
        }

       

        List<IProcedural> spProcedurals = new List<IProcedural>();

        foreach(GameObject sp in spawnPoints) 
        {
            spProcedurals.Add(sp.GetComponent<IProcedural>());
        }

        foreach (IProcedural sp in spProcedurals) 
        {
            sp.Generate();
        }
    }
}
