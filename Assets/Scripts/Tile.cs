using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WhoAmI { partOfBarrack, partOfPowerPlant, emptyTile };

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

    public bool IsBusy { get; set; }
    public bool IsBusyAffordance { get; set; }

    WhoAmI whoAmI;

    public WhoAmI GetWhoAmI()
    {
        return whoAmI;
    }
    public void SetWhoAmI(WhoAmI value)
    {
        whoAmI = value;
    }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        IsBusy = false;
        IsBusyAffordance = false;
        SetWhoAmI(WhoAmI.emptyTile);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("TileAffordance") && IsBusy == false && GetWhoAmI() == WhoAmI.emptyTile)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = busyTileAffordanceSprite;
            IsBusyAffordance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("TileAffordance") && IsBusyAffordance == true && GetWhoAmI() == WhoAmI.emptyTile)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;
            IsBusyAffordance = false;
        }
    }
    #endregion

    #region Custom Function
    //listener function updates tile
    public void UpdateTile()
    {
        if (GetWhoAmI() == WhoAmI.partOfBarrack)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = partOfBarrack;
        }
        else if (GetWhoAmI() == WhoAmI.partOfPowerPlant)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = partOfPowerPlant;
        }

        //reset tile's affordance if it is empty
        IsBusyAffordance = false;
    }
    #endregion
}
