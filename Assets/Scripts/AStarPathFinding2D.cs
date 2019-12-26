using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding2D : MonoBehaviour
{
    #region Variables
    private static List<TileLogic> closedList = new List<TileLogic>();
    private static List<TileLogic> openList = new List<TileLogic>();
    private static bool isDestFound;
    public static int pathfindingStatus = 0;

    //custom class made to hold parent-child relationship and f,g,h values.
    private class TileLogic
    {
        public TileLogic parentLogicalTile;
        public GameObject thisTile;
        public float fValue;
        public float hValue;
        public float gValue;

        public TileLogic(GameObject t, float f, float h, float g)
        {
            parentLogicalTile = null;
            thisTile = t;
            fValue = f;
            hValue = h;
            gValue = g;
        }
    };
    #endregion

    #region Custom Functions
    //Executive function
    public bool FindPath(GameObject[] map, GameObject source, GameObject destination, GameObject soldier, float walkCoroutineTime)
    {
        //status of algorithm. 0 = initialize. 1 = start. 2 = finish.
        pathfindingStatus = 1;

        //reset variables
        closedList = new List<TileLogic>(); //visited tiles
        openList = new List<TileLogic>(); //possible movable tiles
        isDestFound = false;

        //if destination is not valid
        if (destination == null || destination.GetComponent<Tile>() == null)
        {
            Debug.Log("You chose invalid destination!");
            pathfindingStatus = 2;
            return false;
        }
        //if we are already on destination tile
        else if (source.transform.position == destination.transform.position)
        {
            Debug.Log("You are already at the destination!");
            pathfindingStatus = 2;
            return true;
        }
        //if destination is not empty.
        else if (destination.GetComponent<Tile>().isBusy == true)
        {
            Debug.Log("Destination tile is not empty!");
            pathfindingStatus = 2;
            return false;
        }

        Debug.Log("Pathfinding started from " + source.transform.position + " to " + destination.transform.position);

        //create Tilelogic from map. This extra data structure is defined due to single purpose concern of Tile class.
        TileLogic[] tiles = new TileLogic[map.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileLogic(map[i], float.MaxValue, float.MaxValue, float.MaxValue); //initializing
        }

        //turn source and destination into TileLogic
        TileLogic sourceTileLogic = new TileLogic(source, 0, 0, 0);
        TileLogic destTileLogic = new TileLogic(destination, 0, float.MaxValue, float.MaxValue);

        //put starting point in open list
        openList.Add(sourceTileLogic);
        //destination is not found at the beginning
        isDestFound = false;

        //start algorithm
        while (openList.Count != 0)
        {
            //find tile with smallest f value from open list
            TileLogic _currentTile = openList[0];

            foreach (var logicalTile in openList)
            {
                if (logicalTile.fValue < _currentTile.fValue)
                {
                    _currentTile = logicalTile;
                }
            }

            closedList.Add(_currentTile);
            openList.RemoveAt(openList.FindIndex(x => x.thisTile == _currentTile.thisTile));

            //find successors with raycasting (4 sides of player)
            bool _isUpSuccessorAvailable = false;
            bool _isDownSuccessorAvailable = false;
            bool _isRightSuccessorAvailable = false;
            bool _isLeftSuccessorAvailable = false;
            //then check is they are available or not
            TileLogic _upSuccessor = FindSuccessorTile(_currentTile.thisTile, Vector2.up, tiles, ref _isUpSuccessorAvailable);
            TileLogic _downSuccessor = FindSuccessorTile(_currentTile.thisTile, Vector2.down, tiles, ref _isDownSuccessorAvailable);
            TileLogic _rightSuccessor = FindSuccessorTile(_currentTile.thisTile, Vector2.right, tiles, ref _isRightSuccessorAvailable);
            TileLogic _leftSuccessor = FindSuccessorTile(_currentTile.thisTile, Vector2.left, tiles, ref _isLeftSuccessorAvailable);

            //check up successor
            if (_isUpSuccessorAvailable == true)
            {
                //destination is found
                if (_upSuccessor.thisTile == destTileLogic.thisTile)
                {
                    _upSuccessor.parentLogicalTile = _currentTile;

                    isDestFound = true;
                    Debug.Log("Destination is found!");
                    StartCoroutine(TraceRoute(closedList, _upSuccessor, sourceTileLogic, soldier, walkCoroutineTime));

                    return true;
                }
                //if successor is not visited yet
                else if (closedList.Find(x => x.thisTile == _upSuccessor.thisTile) == null)
                {
                    float _tempG = _currentTile.gValue + 1f;
                    float _tempH = CalculateH(_upSuccessor.thisTile, destTileLogic.thisTile);
                    float _tempF = _tempG + _tempH;

                    //max value means it is not considered yet
                    if (_upSuccessor.fValue == float.MaxValue)
                    {
                        _upSuccessor.fValue = _tempF;
                        _upSuccessor.gValue = _tempG;
                        _upSuccessor.hValue = _tempH;
                        _upSuccessor.parentLogicalTile = _currentTile;

                        openList.Add(_upSuccessor);
                    }
                    //_tempF is less means this path is better
                    else if (_upSuccessor.fValue > _tempF)
                    {
                        _upSuccessor.fValue = _tempF;
                        _upSuccessor.gValue = _tempG;
                        _upSuccessor.hValue = _tempH;
                        _upSuccessor.parentLogicalTile = _currentTile;
                    }
                }
            }

            //check down successor
            if (_isDownSuccessorAvailable == true)
            {
                //destination is found
                if (_downSuccessor.thisTile == destTileLogic.thisTile)
                {
                    _downSuccessor.parentLogicalTile = _currentTile;

                    isDestFound = true;
                    Debug.Log("Destination is found!");
                    StartCoroutine(TraceRoute(closedList, _downSuccessor, sourceTileLogic, soldier, walkCoroutineTime));

                    return true;
                }
                //if successor is not visited yet
                else if (closedList.Find(x => x.thisTile == _downSuccessor.thisTile) == null)
                {
                    float _tempG = _currentTile.gValue + 1f;
                    float _tempH = CalculateH(_downSuccessor.thisTile, destTileLogic.thisTile);
                    float _tempF = _tempG + _tempH;

                    //max value means it is not considered yet
                    if (_downSuccessor.fValue == float.MaxValue)
                    {
                        _downSuccessor.fValue = _tempF;
                        _downSuccessor.gValue = _tempG;
                        _downSuccessor.hValue = _tempH;
                        _downSuccessor.parentLogicalTile = _currentTile;

                        openList.Add(_downSuccessor);
                    }
                    //_tempF is less means this path is better
                    else if (_downSuccessor.fValue > _tempF)
                    {
                        _downSuccessor.fValue = _tempF;
                        _downSuccessor.gValue = _tempG;
                        _downSuccessor.hValue = _tempH;
                        _downSuccessor.parentLogicalTile = _currentTile;
                    }
                }
            }

            //check right successor
            if (_isRightSuccessorAvailable == true)
            {
                //destination is found
                if (_rightSuccessor.thisTile == destTileLogic.thisTile)
                {
                    _rightSuccessor.parentLogicalTile = _currentTile;

                    isDestFound = true;
                    Debug.Log("Destination is found!");
                    StartCoroutine(TraceRoute(closedList, _rightSuccessor, sourceTileLogic, soldier, walkCoroutineTime));

                    return true;
                }
                //if successor is not visited yet
                else if (closedList.Find(x => x.thisTile == _rightSuccessor.thisTile) == null)
                {
                    float _tempG = _currentTile.gValue + 1f;
                    float _tempH = CalculateH(_rightSuccessor.thisTile, destTileLogic.thisTile);
                    float _tempF = _tempG + _tempH;

                    //max value means it is not considered yet
                    if (_rightSuccessor.fValue == float.MaxValue)
                    {
                        _rightSuccessor.fValue = _tempF;
                        _rightSuccessor.gValue = _tempG;
                        _rightSuccessor.hValue = _tempH;
                        _rightSuccessor.parentLogicalTile = _currentTile;

                        openList.Add(_rightSuccessor);
                    }
                    //_tempF is less means this path is better
                    else if (_rightSuccessor.fValue > _tempF)
                    {
                        _rightSuccessor.fValue = _tempF;
                        _rightSuccessor.gValue = _tempG;
                        _rightSuccessor.hValue = _tempH;
                        _rightSuccessor.parentLogicalTile = _currentTile;
                    }
                }
            }

            //check left successor
            if (_isLeftSuccessorAvailable == true)
            {
                //destination is found
                if (_leftSuccessor.thisTile == destTileLogic.thisTile)
                {
                    _leftSuccessor.parentLogicalTile = _currentTile;

                    isDestFound = true;
                    Debug.Log("Destination is found!");
                    StartCoroutine(TraceRoute(closedList, _leftSuccessor, sourceTileLogic, soldier, walkCoroutineTime));

                    return true;
                }
                //if successor is not visited yet
                else if (closedList.Find(x => x.thisTile == _leftSuccessor.thisTile) == null)
                {
                    float _tempG = _currentTile.gValue + 1f;
                    float _tempH = CalculateH(_leftSuccessor.thisTile, destTileLogic.thisTile);
                    float _tempF = _tempG + _tempH;

                    //max value means it is not considered yet
                    if (_leftSuccessor.fValue == float.MaxValue)
                    {
                        _leftSuccessor.fValue = _tempF;
                        _leftSuccessor.gValue = _tempG;
                        _leftSuccessor.hValue = _tempH;
                        _leftSuccessor.parentLogicalTile = _currentTile;

                        openList.Add(_leftSuccessor);
                    }
                    //_tempF is less means this path is better
                    else if (_leftSuccessor.fValue > _tempF)
                    {
                        _leftSuccessor.fValue = _tempF;
                        _leftSuccessor.gValue = _tempG;
                        _leftSuccessor.hValue = _tempH;
                        _leftSuccessor.parentLogicalTile = _currentTile;
                    }
                }
            }
        }

        //if destination is not found after search, then it is not reachable
        if (isDestFound == false)
        {
            Debug.Log("Destination is not reachable!");
            pathfindingStatus = 2;
        }
        return false;
    }

    //Manhattan Distance as H value, because player can only move in 4 directions.
    private static float CalculateH(GameObject start, GameObject destination)
    {
        return Mathf.Abs(start.transform.position.x - destination.transform.position.x) + Mathf.Abs(start.transform.position.y - destination.transform.position.y);
    }

    //Finds possible ways (successors). Returns null if successor is not available
    private static TileLogic FindSuccessorTile(GameObject raySource, Vector2 rayDirection, TileLogic[] tiles, ref bool isSuccessorAvailable)
    {
        Debug.DrawRay(new Vector3(raySource.transform.position.x + rayDirection.x / 1.5f, raySource.transform.position.y + rayDirection.y / 1.5f, raySource.transform.position.z), rayDirection * .1f, Color.red, 2f);
        RaycastHit2D _hit2D = Physics2D.Raycast(new Vector3(raySource.transform.position.x + rayDirection.x / 1.5f, raySource.transform.position.y + rayDirection.y / 1.5f, raySource.transform.position.z), rayDirection * .1f);

        if (_hit2D.collider == null || _hit2D.collider.gameObject.GetComponent<Tile>() == null || _hit2D.collider.gameObject.GetComponent<Tile>().isBusy == true)
        {
            isSuccessorAvailable = false;
            return null;
        }
        else
        {
            isSuccessorAvailable = true;
            return FindLogicalTile(_hit2D.collider.gameObject, tiles);
        }     
    }

    //Returns logical tile (class instance) when tile is given
    private static TileLogic FindLogicalTile(GameObject tileToFind, TileLogic[] tiles)
    {
        //o(n)
        foreach (var tile in tiles)
        {
            if (tileToFind == tile.thisTile)
            {
                return tile;
            }
        }

        Debug.LogError("tile not found!");
        Debug.Break();
        return null;
    }
    
    //Moves player through the way
    IEnumerator TraceRoute(List<TileLogic> closedTiles, TileLogic logicalDestinationTile, TileLogic logicalStartingTile, GameObject soldier, float walkCoroutineTime)
    {
        //limit user input
        if (walkCoroutineTime <= 0)
        {
            walkCoroutineTime = 0.1f;
        }
        else if (walkCoroutineTime > 10f)
        {
            walkCoroutineTime = 10f;
        }

        Stack<TileLogic> _pathStack = new Stack<TileLogic>();
        TileLogic _currentTile = logicalDestinationTile;

        while (_currentTile.thisTile != logicalStartingTile.thisTile)
        {
            _pathStack.Push(_currentTile);
            _currentTile = _currentTile.parentLogicalTile;
        }

        while (_pathStack.Count != 0)
        {
            //move one step
            _currentTile.thisTile.GetComponent<Tile>().isBusy = false;
            _currentTile = _pathStack.Pop();
            _currentTile.thisTile.GetComponent<Tile>().isBusy = true;

            soldier.gameObject.transform.position = _currentTile.thisTile.transform.position;
            yield return new WaitForSeconds(walkCoroutineTime);
        }

        if (_pathStack.Count == 0)
        {
            pathfindingStatus = 2;
        }
    }
    #endregion
}
