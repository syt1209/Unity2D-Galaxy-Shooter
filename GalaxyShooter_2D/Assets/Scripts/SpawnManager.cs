using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // config variables
    [SerializeField]
    private WaitForSeconds _enemySpawnDelay = new WaitForSeconds(3.0f);
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

    // cached reference
    [SerializeField]
    private GameObject _enemyPrefab;
    private Transform _enemyContainer;
    [SerializeField]
    private GameObject[] _powerupPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        _enemyContainer = transform.Find("EnemyContainer").GetComponent<Transform>();
    }

    // private methods

    // Enemy related
    private IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning is false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity, _enemyContainer);

            yield return _enemySpawnDelay;
        }
    }

    // Powerup spawning
    private IEnumerator PowerUpSpawnRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning is false)
        { 
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newTripleShotPowerup = Instantiate(_powerupPrefabs[PowerupSelector()], posToSpawn, Quaternion.identity);
            Debug.Log(_powerupID);

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
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

}
