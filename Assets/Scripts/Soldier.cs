using System.Collections;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    #region Variables
    GameObject selectedGameObject;
    bool isClicked = false;
    AStarPathFinding2D pathFinder;

    [SerializeField]
    float walkCoroutineTime;

    GameObject[] tiles;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        //initialize
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        pathFinder = gameObject.AddComponent<AStarPathFinding2D>();
        walkCoroutineTime = 0.5f;
    }
    #endregion

    #region Custom Functions
    //Updates selected object (listener function)
    public void UpdateSelected()
    {
        selectedGameObject = GameObject.Find("GameManager").GetComponent<SelectingObject>().SelectedGameObject;

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
                selectedGameObject = GameObject.Find("GameManager").GetComponent<SelectingObject>().SelectedGameObject;
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
