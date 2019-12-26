using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantButton : ProductionButton
{
    #region Variables
    readonly int sizeOfPowerPlant = 6;

    [SerializeField]
    GameObject powerPlantPrefab;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        affordance = GameObject.Find("PowerPlantTileAffordance");
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
        affordance.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + .1f);
    }

    //checks if area is suitable to build
    public override bool IsBuildable()
    {
        int _currentSize = 0;

        //o(n)
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile>().isBusyAffordance == true)
            {
                ++_currentSize;
            }
        }

        if (_currentSize == sizeOfPowerPlant)
        {
            //change sprites
            //o(n)
            foreach (var tile in tiles)
            {
                if (tile.GetComponent<Tile>().isBusyAffordance == true)
                {
                    tile.GetComponent<Tile>().whoAmI = Tile.WhoAmI.partOfPowerPlant;
                }
            }

            return true;
        }
        return false;
    }

    //Instantiates power plant
    public override void CreateBuilding()
    {
        Vector3 _buildLocation = new Vector3(0, 0, 0);

        //o(n)
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile>().isBusyAffordance == true)
            {
                _buildLocation = new Vector3(_buildLocation.x + tile.transform.position.x, _buildLocation.y + tile.transform.position.y, _buildLocation.z + tile.transform.position.z - 1);
            }
        }
        _buildLocation = new Vector3(_buildLocation.x / sizeOfPowerPlant, _buildLocation.y / sizeOfPowerPlant, _buildLocation.z / sizeOfPowerPlant);

        Instantiate(powerPlantPrefab, _buildLocation, Quaternion.identity);
    }
    #endregion
}
