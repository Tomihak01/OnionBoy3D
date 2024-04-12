using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeKeyController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public string shapeKeyName;
    public string shapeKeyName2;

    public float shapeSpeed = 1f;

    private int shapeKeyIndex;
    private int shapeKeyIndex2;
    private float t = 0;
    public float value;



    void Start()
    {
        shapeKeyIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shapeKeyName);
        shapeKeyIndex2 = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(shapeKeyName2);
    }


    void Update()
    {
        t += Time.deltaTime * shapeSpeed;
        value = (Mathf.Sin(t) + 1) / 2; // Veivaa edestakaisin 0 ja 1 välillä.
        skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyIndex, value * 100);
        skinnedMeshRenderer.SetBlendShapeWeight(shapeKeyIndex2, value * 100);
    }
}
