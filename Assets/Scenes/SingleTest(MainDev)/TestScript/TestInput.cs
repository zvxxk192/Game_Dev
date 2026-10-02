using DG.Tweening;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public VertexPath vertexPath;
    public BezierPath bezierPath;
    public float speed;
    float dstTravelled;

    void Update()
    {
        dstTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        transform.rotation = pathCreator.path.GetRotationAtDistance(dstTravelled, end);
        
    }
}
