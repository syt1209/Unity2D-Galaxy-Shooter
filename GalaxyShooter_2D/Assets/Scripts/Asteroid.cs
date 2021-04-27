using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // state variables
    [SerializeField] private float _rotateSpeed = 100f;

    // cached reference
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.position =new Vector3(0, 5f, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager is null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            _spawnManager.StartSpawnRoutine();
            
            Destroy(this.gameObject, 0.1f);
        }
    }
}
