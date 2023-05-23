using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;

    private SpriteRenderer _spriteRenderer;
    private Transform[] _points;
    private int _currentPoint;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _points = new Transform[_path.childCount];

        for (int i = 0; i < _points.Length; ++i)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    private void Update()
    {
        Transform targetPoint = _points[_currentPoint];

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, _speed * Time.deltaTime);

        if (transform.position == targetPoint.position)
        {
            _currentPoint++;

            _spriteRenderer.flipX = false;

            if(_currentPoint >= _points.Length)
            {
                _spriteRenderer.flipX = true;
                _currentPoint = 0;
            }
        }
    }
}
