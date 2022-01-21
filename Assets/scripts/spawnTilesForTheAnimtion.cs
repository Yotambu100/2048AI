using UnityEngine;


/// <summary>
/// class that responsible of spawning tiles from the sky to the screen
/// </summary>
public class spawnTilesForTheAnimtion : MonoBehaviour
{
    public GameObject[] cubesSpawn;
    private float timeToNextSpawnCube;
    private int numOfCubeThatSpawn = 0;
    private bool isStopSpawnCubes=false;
    private int bigCube = 0;
    private int numCubeToSpawn = 40;
    private float minTimeToSpawnCube = 0.1f;
    private float maxTimeToSpawnCube = 0.6f;
    
    /// <summary>
    /// it initiation the time for the first tile to spawn
    /// </summary>
    void Start()
    {
        timeToNextSpawnCube = Random.Range(minTimeToSpawnCube, maxTimeToSpawnCube);
    }

    /// <summary>
    /// the function spawns a random tile every random couple of seconds
    /// </summary>
    void Update()
    {
        timeToNextSpawnCube -= Time.deltaTime;
        if(timeToNextSpawnCube <= 0 && !isStopSpawnCubes)
        {
            spawnTile();
            timeToNextSpawnCube = Random.Range(minTimeToSpawnCube, maxTimeToSpawnCube);
            numOfCubeThatSpawn += 1; 
        } 
        else if(numOfCubeThatSpawn == numCubeToSpawn)
        {
            isStopSpawnCubes = true;
        } 
    }
    
    /// <summary>
    /// function that spawn a cube
    /// </summary>
    void spawnTile()
    {
        int tileNumSpawn = Random.Range(0, cubesSpawn.Length);
        GameObject tileThatSpawn =  Instantiate(cubesSpawn[tileNumSpawn], transform, true);
        if(bigCube != 2)
        {
            int percentToBeBigCube = Random.Range(1, 4); 
            if(percentToBeBigCube == 3)
            {
                TileStartAnimtions cubeStartAnimationScript = tileThatSpawn.GetComponent<TileStartAnimtions>(); 
                cubeStartAnimationScript.IsThisBigTile = true;
                bigCube += 1;
            }
        }
    }
}
