using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    #region Variables
    GameObject selectedGameObject;
    bool isClicked = false;
    AStarPathFinding2D pathFinder;

    [SerializeField]
    float walkCoroutineTime;

    [SerializeField]
    GameObject gameBoard;

    [SerializeField]
    GameObject gameManager;

    List<GameObject> tiles;

    public GameObject GameBoard { get => gameBoard; set => gameBoard = value; }
    public GameObject GameManager { get => gameManager; set => gameManager = value; }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<GameObject>();
        //initialize
        for (int i = 0; gameBoard.transform.childCount != i; i++)
        {
            tiles.Add(gameBoard.transform.GetChild(i).gameObject);
            //Debug.Log("Child Added!" + i);
        }
        pathFinder = gameObject.AddComponent<AStarPathFinding2D>();
        walkCoroutineTime = 0.5f;
    }
    #endregion

    #region Custom Functions
    //Updates selected object (listener function)
    public void UpdateSelected()
    {
        selectedGameObject = gameManager.GetComponent<SelectingObject>().SelectedGameObject;

        //if selected object is soldier, then we will wait for user input as destination
        if (selectedGameObject != null && selectedGameObject.CompareTag("Player") && selectedGameObject.transform.position == gameObject.transform.position)
        {
            StartCoroutine(WaitForMouseClick());
        }
    }

    //waits for mouse click (changing selected object)
    private IEnumerator WaitForMouseClick()
    {
        isClicked = false;

        while (isClicked == false)
        {
            if (selectedGameObject != gameObject)
            {
                isClicked = true;
                selectedGameObject = gameManager.GetComponent<SelectingObject>().SelectedGameObject;
                Debug.Log("Waited for mouse click!");

                //find tile that player currently stands
                GameObject _startingTile = new GameObject();
                foreach (var tile in tiles)
                {
                    if (gameObject.transform.position == tile.transform.position)
                    {
                        _startingTile = tile;
                        continue;
                    }
                }

                //Pathfinding algorithm is called here
                pathFinder.FindPath(tiles, _startingTile, selectedGameObject, gameObject, walkCoroutineTime);
            }
            yield return null;
        }
    }
    #endregion
}
