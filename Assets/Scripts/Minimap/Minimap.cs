using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public static Minimap Instance;
    public RectTransform contentRectTransform;
    public Vector2 worldSize;
    public float skala;
    public RectTransform minimapIcon;
    public Transform playerTransform;

    private Matrix4x4 transformationMatrix;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CalculateTransformationMatrix();
    }

    private void CalculateTransformationMatrix()
    {
        var minimapSize = contentRectTransform.rect.size;
        var worldSize = new Vector2(this.worldSize.x, this.worldSize.y);

        var translation = -minimapSize / 2;
        var scaleRatio = minimapSize / worldSize;

        transformationMatrix = Matrix4x4.TRS(translation, Quaternion.identity, scaleRatio);

        //  {scaleRatio.x,   0,           0,   translation.x},
        //  {  0,        scaleRatio.y,    0,   translation.y},
        //  {  0,            0,           1,            0},
        //  {  0,            0,           0,            1}
    }

    private Vector2 WorldPositionToMapPosition(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z);
        return transformationMatrix.MultiplyPoint3x4(pos);
    }

    private void Update()
    {
        minimapIcon.localPosition = WorldPositionToMapPosition(playerTransform.position);
    }
}
