using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    // cached reference
    [SerializeField]
    private GameObject _enemyPrefab;
    private Transform _enemyContainer;

    // config variables
    [SerializeField]
    private WaitForSeconds _enemySpawnDelay = new WaitForSeconds(3.0f);
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        _enemyContainer = transform.Find("EnemyContainer").GetComponent<Transform>();
        StartCoroutine(EnemySpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawning is false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity, _enemyContainer);
            //_newEnemy.transform.parent = _enemyContainer;

            yield return _enemySpawnDelay;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    
}
