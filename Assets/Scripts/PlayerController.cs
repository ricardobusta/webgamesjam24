using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera camera;
    
    public MeshRenderer meshRenderer;

    public float speed;
    public float rotateSpeed;

    private Vector3 _lookDirection = Vector3.forward;
    
    private int trailSize = 30;
    private Vector3 cameraOffset;

    private Queue<Vector3> _followPosition = new Queue<Vector3>(130);

    private Vector3 _lastMoveDirection;
    private static readonly int Movement = Shader.PropertyToID("_Movement");

    private void Start()
    {
        var materials = meshRenderer.materials;
        materials[0] = new Material(materials[0]);
        cameraOffset = camera.transform.position;
        var pos = transform.position;
        for (var i = 0; i < trailSize; i++)
        {
            _followPosition.Enqueue(pos);
        }
    }

    private void Update()
    {
        var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDirection != Vector3.zero)
        {
            _lastMoveDirection = moveDirection;
        }

        var tr = transform;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hit, 1000,
            LayerMask.GetMask("Control")))
        {
            var lookDirection = (tr.position - hit.point).normalized;
            lookDirection.y = 0;
            tr.forward = lookDirection;
        }
        
        tr.position += moveDirection * (speed * Time.deltaTime);

        _followPosition.Enqueue(tr.position);
        if (_followPosition.Count > trailSize)
        {
            var pos = _followPosition.Dequeue();
            var dir = pos - tr.position;
            meshRenderer.materials[0].SetVector(Movement, dir);
        }
    }
}