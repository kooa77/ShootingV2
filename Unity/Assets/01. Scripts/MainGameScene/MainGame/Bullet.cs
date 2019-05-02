using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	void Start ()
    {
        Destroy(gameObject, 3.0f);
	}

    float _curTestRot = 0;

    void Update ()
    {
        // z 축을 변경 (양의 값으로)
        /*
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(curPos.x, curPos.y, curPos.z + 0.1f);
        transform.position = nextPos;
        */

        // for test
        transform.position = transform.position + (transform.forward * 0.1f);
        transform.Rotate(Vector3.up, _curTestRot);
        _curTestRot -= 0.01f;
        //_curTestRot = Mathf.Cos(Time.deltaTime) * 100.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
