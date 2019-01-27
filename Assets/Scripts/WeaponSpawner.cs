using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weaponList;

    public float weaponDensity;

    private PlanetMeshGenerator planetMeshGenerator;

    private List<GameObject> spawnedWeaponList = new List<GameObject>();

    private void Start()
    {
        planetMeshGenerator = GameManager.GetInstance().GetPlanet().GetComponent<PlanetMeshGenerator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(spawnedWeaponList.Count < weaponDensity)
        {
            spawnedWeaponList.Add(this.spawnSingleWeapon());
        }

        for(int i=0; i<spawnedWeaponList.Count; i++)
        {
            if(spawnedWeaponList[i] == null)
            {
                Debug.Log("spawn new weapon");
                spawnedWeaponList[i] = this.spawnSingleWeapon();
            }
        }
    }

    GameObject spawnSingleWeapon()
    {
        //  Rand weapon
        int weaponIndex = Random.Range(0, this.weaponList.Length);

        //  Rand position
        Vector3 newPos = Random.insideUnitCircle.normalized * (planetMeshGenerator.radius + planetMeshGenerator.terrainFluctuationMagnitude);

        //  spawn obj
        GameObject spawned = Instantiate(weaponList[weaponIndex], newPos, Quaternion.identity);

        return spawned;
    }
}
