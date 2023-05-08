using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private float _timer;

    private int _count;
    // Start is called before the first frame update
    void Start()
    {
        _count = 0;
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _count)
        {
            _count++;
            Debug.Log(_count);
        
        }
    }
}