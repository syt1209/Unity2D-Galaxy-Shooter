using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    // cached reference
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private WaitForSeconds _enemySpawnDelay = new WaitForSeconds(3.0f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            yield return _enemySpawnDelay;
        }
    }
    
}
