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

    // cached reference
    [SerializeField]
    private GameObject _enemyPrefab;
    private Transform _enemyContainer;
    [SerializeField]
    private GameObject _tripleShotPowerupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _enemyContainer = transform.Find("EnemyContainer").GetComponent<Transform>();
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    // private methods

    // Enemy related
    private IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawning is false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity, _enemyContainer);

            yield return _enemySpawnDelay;
        }
    }

    // Powerup related
    private IEnumerator PowerUpSpawnRoutine()
    {
        while (_stopSpawning is false)
        { 
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newTripleShotPowerup = Instantiate(_tripleShotPowerupPrefab, posToSpawn, Quaternion.identity);

            yield return _powerupSpawnDelay;
        }
    }

    // public methods
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    
}
