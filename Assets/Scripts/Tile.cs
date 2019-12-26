using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Sprite tileSprite;
    [SerializeField]
    Sprite busyTileAffordanceSprite;
    [SerializeField]
    Sprite partOfPowerPlant;
    [SerializeField]
    Sprite partOfBarrack;

    public bool isBusy;
    public bool isBusyAffordance;

    public enum WhoAmI { partOfBarrack, partOfPowerPlant, emptyTile };
    public WhoAmI whoAmI;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        isBusy = false;
        isBusyAffordance = false;
        whoAmI = WhoAmI.emptyTile;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("TileAffordance") && isBusy == false && whoAmI == WhoAmI.emptyTile)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = busyTileAffordanceSprite;
            isBusyAffordance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("TileAffordance") && isBusyAffordance == true && whoAmI == WhoAmI.emptyTile)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;
            isBusyAffordance = false;
        }
    }
    #endregion

    #region Custom Function
    //listener function updates tile
    public void UpdateTile()
    {
        if (whoAmI == WhoAmI.partOfBarrack)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = partOfBarrack;
        }
        else if (whoAmI == WhoAmI.partOfPowerPlant)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = partOfPowerPlant;
        }

        //reset tile's affordance if it is empty
        isBusyAffordance = false;
    }
    #endregion
}
