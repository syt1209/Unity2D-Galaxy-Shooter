using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : SmartEnemy
{
    private float _speed;
    private Vector3 _direction;
    private float _maxX = 9.6f, _minX = -9.6f, _maxY = 6.0f, _minY = -2f;

    protected override void Start()
    {
        base.Start();
        _enemyPoints = 50;
        _speed = 0f;
        StartCoroutine(ChangeDirectionRoutine());
    }
    // Update is called once per frame
    protected override void Update()
    {
        Wonder();
    }

    private void Wonder()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
        transform.position =new Vector3(Mathf.Clamp(transform.position.x, _minX, _maxX), Mathf.Clamp(transform.position.y, _minY, _maxY),0);
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            _speed = Random.Range(2.0f, 5.0f);
            yield return new WaitForSeconds(2.0f);
        }
    }
}
