using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockPuzzleScript : MonoBehaviour
{
    [SerializeField] private Transform emptySpace = null;
    private Camera _camera;
    [SerializeField] private BlocksScript[] tiles;
    private int emptySpaceIndex = 11;
    private bool _isFinished;
    [SerializeField] private GameObject endPanel;
    
    void Start()
    {
        _camera = Camera.main;

        Shuffle();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                if (Vector2.Distance(emptySpace.position, hit.transform.position) < 3)
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
                endPanel.SetActive(true);
                StartCoroutine(WaitforEnd());
            }
        }
    }

    public void Shuffle()
    {
        if(emptySpaceIndex != 11)
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
                int randomIndex = Random.Range(0, 10);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;

            }
            invertion = GetInversions();
            Debug.Log("");
        } while (invertion%2 != 0);
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
    IEnumerator WaitforEnd()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("MainScene");
    }
}
