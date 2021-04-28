using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.localPosition;
    }

    public IEnumerator CameraShakeRoutine(float shakeDuration)
    {
        float timePassed = 0;

        while (timePassed < shakeDuration)
        {
            ShakeCamera();
            timePassed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPos;
    }

    private void ShakeCamera()
    { 
        transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), initialPos.z);
    }
}
