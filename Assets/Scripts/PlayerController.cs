using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public float speed;
    
    private Vector3 _lookDirection = Vector3.forward;
    
    private void Start()
    {
        var materials = meshRenderer.materials;
        materials[0] = new Material(materials[0]);
    }

    private void Update()
    {
        var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0,Input.GetAxis("Vertical"));

        _lookDirection = Vector3.RotateTowards(_lookDirection, moveDirection, 0.1f, 0);
       
        meshRenderer.materials[0].SetVector("_Movement", -moveDirection);

        transform.position += moveDirection  * (speed * Time.deltaTime);

        transform.forward = _lookDirection;
    }
}