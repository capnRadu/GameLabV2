using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockPuzzleScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    public Camera _camera;
    [SerializeField] private BlocksScript[] tiles;
    private int emptySpaceIndex = 11;
    [NonSerialized] public bool _isFinished = false;
    [SerializeField] private GameObject endPanel;

    ProductManagementManager productManagementManager;

    private void Start()
    {
        productManagementManager = GetComponent<ProductManagementManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isFinished)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                if (Vector2.Distance(emptySpace.position, hit.transform.position) < 3.5)
                {
                    Vector2 lastEmptySpacePosition = emptySpace.position;
                    BlocksScript thisBlock = hit.transform.GetComponent<BlocksScript>();
                    emptySpace.position = thisBlock.targetPosition;
                    thisBlock.targetPosition = lastEmptySpacePosition;
                    int tileIndex = findIndex(thisBlock);
                    tiles[emptySpaceIndex] = tiles[tileIndex];
                    tiles[tileIndex] = null;
                    emptySpaceIndex = tileIndex;
                }
            }
        }
        if (!_isFinished)
        {
            int correctBlocks = 0;
            foreach (var a in tiles)
            {
                if (a != null)
                {
                    if (a.inRightPlace)
                        correctBlocks++;
                }
            }
            if (correctBlocks == tiles.Length - 1)
            {
                _isFinished = true;
                Debug.Log("finished");
                StartCoroutine(SignalNextRound());
            }
        }
    }

    public void Shuffle()
    {
        foreach (var a in tiles)
        {
            if (a != null)
            {
                a.wasInRightPlace = false;
                a.hasMoved = false;
            }
        }

        if (emptySpaceIndex != 11)
        {
            var tileOn11LastPos = tiles[11].targetPosition;
            tiles[11].targetPosition = emptySpace.position;
            emptySpace.position = tileOn11LastPos;
            tiles[emptySpaceIndex] = tiles[11];
            tiles[11] = null;
            emptySpaceIndex = 11;
        }
        int invertion;
        do
        {
            for (int i = 0; i <= 10; i++)
            {

                var lastPos = tiles[i].targetPosition;
                int randomIndex = UnityEngine.Random.Range(0, 10);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;

            }
            invertion = GetInversions();
            Debug.Log("");
        } while (invertion%2 != 0);

        StartCoroutine(ToggleIsFinished());
    }
    public int findIndex(BlocksScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                return i;
            }
        }
        return -1;
    }
    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }

    IEnumerator SignalNextRound()
    {
        yield return new WaitForSeconds(2f);

        productManagementManager.NextRound();
    }

    private IEnumerator ToggleIsFinished()
    {
       yield return new WaitForSeconds(0.5f);

        _isFinished = false;
    }
}