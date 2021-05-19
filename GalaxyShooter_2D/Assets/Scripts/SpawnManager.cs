using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // config variables
    [SerializeField]
    private WaitForSeconds _enemySpawnDelay = new WaitForSeconds(3.0f);
    private WaitForSeconds _waveSpawnDelay = new WaitForSeconds(3.0f);
    private WaitForSeconds _levelSpawnDelay = new WaitForSeconds(3.0f);
    private WaitForSeconds _powerupSpawnDelay = new WaitForSeconds(7.0f);
    private bool _stopSpawning = false;
    private int _powerupID;
    private int _selectedWeight;
    private int[] _weightsOfPowerups =
    {
        50, // Ammo (ID=0)
        20, // Triple Shot (ID=1), Multi-Direction Shot (ID=2), Homing Missile (ID=3)
        15, // Shield (ID=4)
        10, // Speed Boost (ID=5), Speed Slow Down (negative) (ID=6)
        5   // Life (ID=7)
    };

    
    private enum SpawnTypes
    { 
        Wave,         // level I to III
        Once          // level IV
    }
    // End of Enums

    // Enemy level and wave configs
    [SerializeField]
    private int _level = 1;

    private int _currentWaveID = 0;
    private int _totalWaves = 3;
    private int[] _enemiesInWaves = { 3, 5, 7 };


    // End of enemy level and wave configs

    // cached reference
    [SerializeField]
    private GameObject _enemyPrefab, _smartEnemyPrefab, _aggressiveEnemyPrefab, _bossPrefab;
    private Transform _enemyContainer;
    private Dictionary<int, GameObject> Enemies = new Dictionary<int, GameObject>(4);
    [SerializeField]
    private GameObject[] _powerupPrefabs;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _enemyContainer = transform.Find("EnemyContainer").GetComponent<Transform>();

        Enemies.Add(1, _enemyPrefab);
        Enemies.Add(2, _smartEnemyPrefab);
        Enemies.Add(3, _aggressiveEnemyPrefab);
        Enemies.Add(4, _bossPrefab);

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager is null)
        {
            Debug.LogError("UIManager is NULL");
        }
    }


    // PRIVATE METHODS

    // Enemy spawning  
    private IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
  
            while (_stopSpawning is false)
            {
                int spawnedEnemyInWave = 0;
                while (_currentWaveID < _totalWaves)
                {
                    int totalEnemyInWave = _enemiesInWaves[_currentWaveID];
                    _uiManager.UpdateLevelText(_level);
                    _uiManager.UpdateWaveText(_currentWaveID + 1);

                    while (spawnedEnemyInWave < totalEnemyInWave)
                    {
                        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                        GameObject _newEnemy = Instantiate(EnemyForLevel(_level), posToSpawn, Quaternion.identity, _enemyContainer);
                        //Debug.Log(EnemyForLevel(_level).name);

                        spawnedEnemyInWave++;
                        //Debug.Log("spawned Enemy in wave: " + spawnedEnemyInWave);

                        yield return _enemySpawnDelay;
                    }

                    int enemyAlive = _enemyContainer.childCount;
                    if (enemyAlive == 0)
                    {
                        _currentWaveID++;
                        spawnedEnemyInWave = 0;
                    }

                    yield return _waveSpawnDelay;
                }

                _level++;
                _currentWaveID = 0;

                yield return _levelSpawnDelay;
            }
    }
    // End of enemy spawning settings

    // Enemy for different levels
    private GameObject EnemyForLevel(int level)
    {
        GameObject currentEnemy = Enemies[level];
        return currentEnemy;
    }

    // Powerup spawning
    private IEnumerator PowerUpSpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning is false)
        { 
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newTripleShotPowerup = Instantiate(_powerupPrefabs[PowerupSelector()], posToSpawn, Quaternion.identity);

            yield return _powerupSpawnDelay;
        }
    }

    private int PowerupSelector()
    {
        int totalWeights = 0;
        foreach (var item in _weightsOfPowerups)
        { 
            totalWeights += item;
        }
        
        int randomNum = Random.Range(0, totalWeights);

        foreach (var weight in _weightsOfPowerups)
        {
            if (randomNum < weight)
            {
                _selectedWeight = weight;
                break;
            }

            else
            {
                randomNum -= weight;
            }
        }

        switch (_selectedWeight)
        {
            case 50:
                _powerupID = 0;
                break;
            case 20:
                _powerupID = Random.Range(1, 4);
                break;
            case 15:
                _powerupID = 4;
                break;
            case 10:
                _powerupID = Random.Range(5, 7);
                break;
            case 5:
                _powerupID = 7;
                break;
            default:
                Debug.LogError("No matching weight for powerup");
                break;
        }

        return _powerupID;
    }
    // End of powerup settings

    // public methods
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void StartSpawnRoutine()
    {

        if (_level == 4) // Boss level -- spawn once
        {
            _uiManager.UpdateLevelText(_level);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newEnemy = Instantiate(EnemyForLevel(_level), posToSpawn, Quaternion.identity, _enemyContainer);

            int enemyAlive = _enemyContainer.childCount;
            if (enemyAlive == 0)
            {
                Debug.Log("Congratulations! You Are the NEW BOSS!");
                _stopSpawning = true;
            }
        }

        else if (_level < 4)
        {
            StartCoroutine(EnemySpawnRoutine());
        }
        StartCoroutine(PowerUpSpawnRoutine());
    }

}
