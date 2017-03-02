using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    //Waves of Enemies
    public Wave[] waves;
    //Enemy GameObjects
    public Enemy enemy;
    //CurrentWave
    Wave currentWave;
    //Currently Number of Waves
    int currentWaveNumber;
    //Enemies remainging
    int enemiesRemainingToSpawn;
    //Next Time for Enemy To Spawn
    float nextSpawnTime;
    //Enemies Ramaining Alive
    int enemiesRemainingAlive;

    LivingEntity playerEntity;
    Transform playerT;

    MapGenerator map;

    float timeBetweenCampingChecks = 2;
    float nextCampCheckTime;
    float campThreshholdDistance = 1.5f;
    bool isCamping;
    Vector3 campingPositionOld;

    public bool isDisabled;

    public event System.Action<int> OnNewWave;

    void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;
        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campingPositionOld = playerT.position;

        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();

        //Start Waves
        NextWave();
    }

    void Update()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campingPositionOld) < campThreshholdDistance);
                campingPositionOld = playerT.position;

            }

            //If Enemies remainging is Greater Than Zero, and Time is greater that Next Spawn Time
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                //Subtracting Number of Enemies That Need to be Spawned In The Level
                enemiesRemainingToSpawn--;
                //Next Spawn Time = Time Plus the Current Wave.TimeBeween Spawns
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }
        Material tilemat = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = tilemat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while(spawnTimer < spawnDelay)
        {
            tilemat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;

            yield return null;
        }

        //Instantiate Enemies In Spawn Location
        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        //Once Enemy Dies, Add Enemy Death
        spawnedEnemy.OnDeath += OnEnemyDeath;


    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    void OnEnemyDeath()
    {
        print("Enemy died");
        //Once Enemy Dies, Enemies Remaining is Subtracted
        enemiesRemainingAlive--;

        //If Enemy Remainging Alive == 0, Do Next Wave
        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void NextWave()
    {

        currentWaveNumber++;
        print("Wave:" + currentWaveNumber);

        if(currentWaveNumber - 1 < waves.Length){
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if(OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
            if (currentWaveNumber >= 2)
            {
                Debug.Log("Working?");
                ResetPlayerPosition();
            }
        }
    }

    //Inspector
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

}
