using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksScript : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 correctPosition;
    private SpriteRenderer _sprite;
    public int number;
    public bool inRightPlace;
    [NonSerialized] public bool wasInRightPlace = false;
    [NonSerialized] public bool hasMoved = false;

    ProductManagementManager productManagementManager;

    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        _sprite = GetComponent<SpriteRenderer>();

        productManagementManager = GetComponentInParent<ProductManagementManager>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);

        if (Vector2.Distance(targetPosition, correctPosition) < 0.5f)
        {
            _sprite.color = Color.green;
            inRightPlace = true;

            if (!wasInRightPlace && hasMoved)
            {
                productManagementManager.AddPoints();
                wasInRightPlace = true;
            }
        }
        else
        {
            _sprite.color = Color.white;
            inRightPlace = false;
            hasMoved = true;
        }
    }
}
