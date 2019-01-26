using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetMeshGenerator))]
public class PlanetMeshGeneratorCustomEditor : Editor
{
    private PlanetMeshGenerator targetPlanetMeshGenerator;
    
    public void OnEnable()
    {
        targetPlanetMeshGenerator = (PlanetMeshGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Random Seed"))
        {
            Debug.Log("random seed");
            targetPlanetMeshGenerator.randomSeed();
        }

        if (GUILayout.Button("Generate Planet"))
        {
            Debug.Log("gen planet");
            targetPlanetMeshGenerator.startGenPlanetMeshProcess();
        }
    }
    
}

public class PlanetMeshGenerator : MonoBehaviour
{
    //  -----
    //  public member
    //  -----
    public float radius = 10;
    public int numOfVerticies = 200;
    public float terrainFluctuationMagnitude = 3;
    public float terrainFluctuationLength = 2;
    public Material material;
    
    //  -----
    //  private member
    //  -----
    private Mesh mesh;
    private List<Vector3> verticies = new List<Vector3>();
    private List<int> indicies = new List<int>();
    private List<Vector2> colliderVerticies = new List<Vector2>();
    float perlinSeed;

    //  -----
    //  member function
    //  -----
    /* overide */
    

    public void randomSeed()
    {
        //  Random new seed
        perlinSeed = Random.Range(0.0f, 10000.0f);

        //  Update planet
        this.startGenPlanetMeshProcess();
    }

    public void startGenPlanetMeshProcess()
    {
        /*
         * Note: We should probably change to generate only at the start of the game but since it's testing
         * I'll just put it here so we can play around
         */
        //  Generate planet mesh
        this.genPlanetMesh();
        
        //  Set mesh data 
        mesh = new Mesh();
        mesh.vertices = verticies.ToArray();
        mesh.triangles = indicies.ToArray();

        //  Send data to components
        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<PolygonCollider2D>().points = colliderVerticies.ToArray();

        GetComponent<MeshRenderer>().material = material;
    }

    private void genPlanetMesh()
    {
        //
        //  Set up parameters
        //

        float radianStepSize = 2 * Mathf.PI / numOfVerticies;
        
        float accumulatedAngle = 0;

        //
        //  Verticies
        //

        //  Clear verticies buffer
        verticies.Clear();
        //  Append center of transform as the origin in local space
        verticies.Add(new Vector3(0, 0, 0));

        //  Generate mesh in full circle
        /*
         * Note: Since Unity coordinate system is left-handed (clockwise) but Math function coordinate system is right-handed
         * , we will have to generate verticies in clockwise direction by decreasing accumulatedAngle until it reach -2Pi
         */
        while( accumulatedAngle > -2 * Mathf.PI )
        {
            Vector3 dirToSurface = new Vector3(Mathf.Cos(accumulatedAngle), Mathf.Sin(accumulatedAngle));
            
            float terrainHeight = radius 
                                    + terrainFluctuationMagnitude * Mathf.PerlinNoise(accumulatedAngle * terrainFluctuationLength, perlinSeed);

            Vector3 surfacePosition = dirToSurface * terrainHeight;

            //  Add generated point to vertex array
            verticies.Add(surfacePosition);

            //  Add generated point to collider mesh vertex array
            colliderVerticies.Add( new Vector2(surfacePosition.x, surfacePosition.y) );

            accumulatedAngle -= radianStepSize;
        }

        //
        //  Indicies
        //

        indicies.Clear();

        //  Generate triangle fan index buffer
        for( int index=1; index <= numOfVerticies; index++)
        {
            //  Center index
            indicies.Add(0);

            //  Current index
            indicies.Add(index);

            //  Next index
            indicies.Add( (index % numOfVerticies) + 1 );
        }
    }
}
