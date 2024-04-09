using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeKeyController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public string shapeKeyName;
    public float shapeSpeed = 1f;

    private int shapeKeyIndex;
    private float t = 0;
    public float value;



    void Start()
    {
        shapeKeyIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shapeKeyName);    
    }


    void Update()
    {
        t += Time.deltaTime * shapeSpeed;
        value = (Mathf.Sin(t) + 1) / 2; // Veivaa edestakaisin 0 ja 1 välillä.
        skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyIndex, value * 100);
    }
}
