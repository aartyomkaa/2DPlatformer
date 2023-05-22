using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class GemSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gems;

    private float _startTime = 0.0f;
    private float _delay = 2f;
    private bool _increaseTime;

    private void Update()
    {
        _increaseTime = IncreaseTime();

        if (_increaseTime)
        {
            _startTime += Time.deltaTime;

            if (_startTime > _delay)
            {
                for (int i = 0; i < _gems.Count; i++)
                {
                    _gems[i].SetActive(true);
                }

                _increaseTime = false;
                _startTime = 0f;
            }
        }
    }

    private bool IncreaseTime()
    {
        if (_gems.Any(gem => gem.activeSelf == true))
        {
            return false;
        }

        return true;
    }
}