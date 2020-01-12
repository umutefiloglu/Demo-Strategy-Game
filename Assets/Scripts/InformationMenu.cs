using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InformationMenu : MonoBehaviour
{
    #region Variables
    RaycastHit2D hit2D;

    GameObject selectedGameObject;

    public UnityEvent NewObjectCreatedEvent;

    [SerializeField]
    GameObject barrackPanel;
    [SerializeField]
    GameObject powerPlantPanel;

    [SerializeField]
    GameObject soldierPrefab;

    [SerializeField]
    GameObject gameBoard;

    [SerializeField]
    GameObject gameManager;

    GameObject newSoldier;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        selectedGameObject = null;
        hit2D = new RaycastHit2D();

        NewObjectCreatedEvent.AddListener(delegate { gameManager.GetComponent<SelectingObject>().NewObjectCreated(newSoldier); });
    }
    #endregion

    #region Custom Function
    //Updates selected object (listener function)
    public void UpdateSelected()
    {
        selectedGameObject = gameManager.GetComponent<SelectingObject>().SelectedGameObject;

        if (selectedGameObject == null)
        {
            barrackPanel.SetActive(false);
            powerPlantPanel.SetActive(false);
        }
        else if (selectedGameObject.CompareTag("Barrack"))
        {
            barrackPanel.SetActive(true);
            powerPlantPanel.SetActive(false);
        }
        else if (selectedGameObject.CompareTag("PowerPlant"))
        {
            barrackPanel.SetActive(false);
            powerPlantPanel.SetActive(true);
        }
        else if (selectedGameObject.CompareTag("UI") != true)
        {
            barrackPanel.SetActive(false);
            powerPlantPanel.SetActive(false);
        }
    }

    //Instantiate soldier if pathfinding is not running
    public void DeploySoldier()
    {
        if (AStarPathFinding2D.pathfindingStatus != 1)
        {
            if (CheckForSpace() == true)
            {
                newSoldier = Instantiate(soldierPrefab, hit2D.collider.transform.position, Quaternion.identity);
                newSoldier.GetComponent<Soldier>().GameBoard = gameBoard;
                newSoldier.GetComponent<Soldier>().GameManager = gameManager;
                hit2D.collider.GetComponent<Tile>().IsBusy = true;
                NewObjectCreatedEvent.Invoke();
            }
            else
            {
                Debug.LogWarning("There is no empty adjacent space to deploy soldier from this barrack.");
            }
        }
    }

    //Looks for available spaces
    bool CheckForSpace()
    {
        //looks for adjacent squares (including tiles on the corner). This function achieves this by sending rays to all possible tile locations.
        //rays' sequence can be change according to design concerns.
        //up rays
        for (float i = -1.5f; i <= 1.5f; i++)
        {
            Debug.DrawRay(new Vector3(selectedGameObject.transform.position.x + i, selectedGameObject.transform.position.y, selectedGameObject.transform.position.z) + new Vector3(0, 2.1f, 0), Vector2.up * 1000, Color.green, 2f);

            hit2D = Physics2D.Raycast(new Vector3(selectedGameObject.transform.position.x + i, selectedGameObject.transform.position.y, selectedGameObject.transform.position.z) + new Vector3(0, 2.1f, 0), Vector2.up * 1000);
            if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
            {
                Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
                //deploy soldier and make tile busy
                return true;
            }
        }

        //down rays
        for (float i = -1.5f; i <= 1.5f; i++)
        {
            Debug.DrawRay(new Vector3(selectedGameObject.transform.position.x + i, selectedGameObject.transform.position.y, selectedGameObject.transform.position.z) + new Vector3(0, -2.1f, 0), Vector2.down * 1000, Color.green, 100f);

            hit2D = Physics2D.Raycast(new Vector3(selectedGameObject.transform.position.x + i, selectedGameObject.transform.position.y, selectedGameObject.transform.position.z) + new Vector3(0, -2.1f, 0), Vector2.down * 1000);
            if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
            {
                Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
                //deploy soldier and make tile busy
                return true;
            }
        }

        //right rays
        for (float i = -1.5f; i <= 1.5f; i++)
        {
            Debug.DrawRay(new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y + i, selectedGameObject.transform.position.z) + new Vector3(2.1f, 0, 0), Vector2.right * 1000, Color.green, 100f);

            hit2D = Physics2D.Raycast(new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y + i, selectedGameObject.transform.position.z) + new Vector3(2.1f, 0, 0), Vector2.right * 1000);
            if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
            {
                Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
                //deploy soldier and make tile busy
                return true;
            }
        }

        //left rays
        for (float i = -1.5f; i <= 1.5f; i++)
        {
            Debug.DrawRay(new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y + i, selectedGameObject.transform.position.z) + new Vector3(-2.1f, 0, 0), Vector2.left * 1000, Color.green, 100f);

            hit2D = Physics2D.Raycast(new Vector3(selectedGameObject.transform.position.x, selectedGameObject.transform.position.y + i, selectedGameObject.transform.position.z) + new Vector3(-2.1f, 0, 0), Vector2.left * 1000);
            if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
            {
                Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
                //deploy soldier and make tile busy
                return true;
            }
        }

        //diagonal rays
        Debug.DrawRay(selectedGameObject.transform.position + new Vector3(2.1f, 2.1f, 0), new Vector3(1, 1, 0) * 1000, Color.green, 100f);
        hit2D = Physics2D.Raycast(selectedGameObject.transform.position + new Vector3(2.1f, 2.1f, 0), new Vector3(1, 1, 0) * 1000);
        if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
        {
            Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
            //deploy soldier and make tile busy
            return true;
        }


        Debug.DrawRay(selectedGameObject.transform.position + new Vector3(-2.1f, 2.1f, 0), new Vector3(-1, 1, 0) * 1000, Color.green, 100f);
        hit2D = Physics2D.Raycast(selectedGameObject.transform.position + new Vector3(-2.1f, 2.1f, 0), new Vector3(-1, 1, 0) * 1000);
        if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
        {
            Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
            //deploy soldier and make tile busy
            return true;
        }


        Debug.DrawRay(selectedGameObject.transform.position + new Vector3(-2.1f, -2.1f, 0), new Vector3(-1, -1, 0) * 1000, Color.green, 100f);
        hit2D = Physics2D.Raycast(selectedGameObject.transform.position + new Vector3(-2.1f, -2.1f, 0), new Vector3(-1, -1, 0) * 1000);
        if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
        {
            Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
            //deploy soldier and make tile busy
            return true;
        }


        Debug.DrawRay(selectedGameObject.transform.position + new Vector3(2.1f, -2.1f, 0), new Vector3(1, -1, 0) * 1000, Color.green, 100f);
        hit2D = Physics2D.Raycast(selectedGameObject.transform.position + new Vector3(2.1f, -2.1f, 0), new Vector3(1, -1, 0) * 1000);
        if (hit2D.collider != null && hit2D.collider.GetComponent<Tile>() != null && hit2D.collider.GetComponent<Tile>().IsBusy == false)
        {
            Debug.Log(hit2D.collider.name + " is usable to deploy soldier.");
            //deploy soldier and make tile busy
            return true;
        }

        return false;
    }
    #endregion
}
