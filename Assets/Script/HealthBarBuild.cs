using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBuild : MonoBehaviour
{
    private  MaterialPropertyBlock matBlock;
    private  MeshRenderer meshRenderer;
    private  Camera mainCamera;
    private  Build unit;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        matBlock = new MaterialPropertyBlock();
        // get the damageable parent we're attached to
        unit = GetComponentInParent<Build>();
    }

    private void Start()
    {
        // Cache since Camera.main is super slow
        mainCamera = Camera.main;
        meshRenderer.enabled = true;
    }

    private void Update()
    {
        // Only display on partial health
        meshRenderer.enabled = true;
        AlignCamera();
        UpdateParams();

    }

    private void UpdateParams()
    {
        meshRenderer.GetPropertyBlock(matBlock);
        matBlock.SetFloat("_Fill", unit.CurrentHealth / (float)unit.MaxHealth + 0.005f);
        meshRenderer.SetPropertyBlock(matBlock);
    }

    private void AlignCamera()
    {
        if (mainCamera != null)
        {
            var camXform = mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

}