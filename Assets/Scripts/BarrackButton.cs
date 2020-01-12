﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackButton : ProductionButton
{
    #region Variables
    readonly int sizeOfBarrack = 16;

    [SerializeField]
    GameObject barrackPrefab;

    [SerializeField]
    GameObject barrackTileAffordance;

    [SerializeField]
    GameObject gameManagerOnScene;

    [SerializeField]
    GameObject gameBoard;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        affordance = barrackTileAffordance;
    }
     
    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    #endregion

    #region Custom Functions
    //shows build area as affordance
    public override void ShowBuildArea()
    {
        affordance.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + .1f);
    }

    //checks if area is suitable to build
    public override bool IsBuildable()
    {
        int _currentSize = 0;

        //o(n)
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile>().IsBusyAffordance == true)
            {
                ++_currentSize;
            }
        }

        if (_currentSize == sizeOfBarrack)
        {
            //change sprites
            //o(n)
            foreach (var tile in tiles)
            {
                if (tile.GetComponent<Tile>().IsBusyAffordance == true)
                {
                    tile.GetComponent<Tile>().SetWhoAmI(WhoAmI.partOfBarrack);
                }
            }

            return true;
        }
        return false;
    }

    //Instantiates barracks
    public override void CreateBuilding()
    {
        Vector3 _buildLocation = new Vector3(0, 0, 0);

        //o(n)
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile>().IsBusyAffordance == true)
            {
                _buildLocation = new Vector3(_buildLocation.x + tile.transform.position.x, _buildLocation.y + tile.transform.position.y, _buildLocation.z + tile.transform.position.z - 1);
            }
        }
        _buildLocation = new Vector3(_buildLocation.x / sizeOfBarrack, _buildLocation.y / sizeOfBarrack, _buildLocation.z / sizeOfBarrack);

        Instantiate(barrackPrefab, _buildLocation, Quaternion.identity);
    }

    public override void AssignGameManager()
    {
        gameManager = gameManagerOnScene;
    }

    public override void AssignTiles()
    {
        tiles = new List<GameObject>();
        //initialize
        for (int i = 0; gameBoard.transform.childCount != i; i++)
        {
            tiles.Add(gameBoard.transform.GetChild(i).gameObject);
            //Debug.Log("Child Added!" + i);
        }
    }
    #endregion
}
