using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public bool isMoving { get; private set; }
    public MapNode currentNode;
    [SerializeField] private float moveSpeed = 10.0f;

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, moveSpeed * Time.deltaTime);
        isMoving = transform.position != currentNode.transform.position;
    }
}
