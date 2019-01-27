using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(PlanetMeshGenerator))]
public class PlanetMeshGeneratorCustomEditor : Editor
{
    //  -----
    //  private member
    //  -----
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
            targetPlanetMeshGenerator.randomSeed();
        }

        if (GUILayout.Button("Generate Planet"))
        {
            targetPlanetMeshGenerator.startGenPlanetMeshProcess();
        }
    }
    
}
#endif

public class PlanetMeshGenerator : MonoBehaviour
{
    //  -----
    //  public member
    //  -----
    public float radius = 50;
    public int numOfVerticies = 300;

    public float terrainFluctuationMagnitude = 7;

    public float noiseAmplitude = 128;
    public float noiseFrequency = 4;
    public int octaveCount = 12;
    public float octavePersistence = 0.3f;

    public Material material;
    
    //  -----
    //  private member
    //  -----
    private Mesh mesh;
    private List<Vector3> verticies = new List<Vector3>();
    private List<Vector2> textureCoordinates = new List<Vector2>();
    private List<Vector2> colliderVerticies = new List<Vector2>();
    private List<int> indicies = new List<int>();
    private float minSurfaceHeight;
    float perlinSeed;

    //  -----
    //  member function
    //  -----
    
    public void Start()
    {
        this.randomSeed();
        GameManager gameManagerInstance = GameManager.GetInstance();
        this.noiseFrequency = gameManagerInstance.noiseFrequency;
        this.terrainFluctuationMagnitude = gameManagerInstance.terrainFluctuationMagnitude;
        this.octavePersistence = gameManagerInstance.octavePersistence;
        
        

        startGenPlanetMeshProcess();
    }

    public void Update()
    {
        //  Update material uniform
        material.SetVector("_CoreCenter", new Vector4(this.transform.position.x, this.transform.position.y));
        
        material.SetFloat("_numSurfaceVerticies", colliderVerticies.Count );

        material.SetFloat("_minSurfaceHeight", this.minSurfaceHeight);

        material.SetFloat("_Speed", GameManager.GetInstance().planetSpeed);
    }

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
        mesh.SetUVs(0, textureCoordinates);

        //  Send data to components
        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<PolygonCollider2D>().points = colliderVerticies.ToArray();

        GetComponent<MeshRenderer>().material = material;
    }

    public float octavePerlin( float x )
    {
        float total = 0;
        float maxValue = 0;
        float frequency = this.noiseFrequency;
        float amplitude = this.noiseAmplitude;

        for (int i = 0; i < this.octaveCount; i++)
        {
            //total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;

            total += (Mathf.PerlinNoise(x * frequency, this.perlinSeed) * 2 - 1) * amplitude;

            maxValue += amplitude;
            
            amplitude *= this.octavePersistence;
            frequency *= 2;
        }

        return total / maxValue;
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

        //  Clear UV
        textureCoordinates.Clear();
        //  Append center of UV as the origin in local space
        textureCoordinates.Add(new Vector2(0, 0));

        //  Clear collider verticies
        colliderVerticies.Clear();

        this.minSurfaceHeight = Mathf.Infinity;

        //  Generate mesh in full circle
        /*
         * Note: Since Unity coordinate system is left-handed (clockwise) but Math function coordinate system is right-handed
         * , we will have to generate verticies in clockwise direction by decreasing accumulatedAngle until it reach -2Pi
         */
        while( accumulatedAngle > -2 * Mathf.PI )
        {
            Vector3 dirToSurface = new Vector3(Mathf.Cos(accumulatedAngle), Mathf.Sin(accumulatedAngle));

            float noise = this.octavePerlin( accumulatedAngle );

            float terrainHeight = radius + noise * this.terrainFluctuationMagnitude;

            //  Cache min surface height for shader
            if(this.minSurfaceHeight > terrainHeight)
                minSurfaceHeight = terrainHeight;

            Vector3 surfacePosition = dirToSurface * terrainHeight;

            //  Add generated point to vertex array
            verticies.Add(surfacePosition);

            //  Add texture coordinate
            textureCoordinates.Add(new Vector2( - accumulatedAngle / ( 2 * Mathf.PI )
                                                , 1));

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
