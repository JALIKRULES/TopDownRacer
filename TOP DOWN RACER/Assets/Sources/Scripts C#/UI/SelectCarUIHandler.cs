using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car prefab")]
    public GameObject carPrefab;

    [Header("Spawn on")]
    public Transform spawnOnTransform;

    bool isChangingCar = false;

    CarUIHandler carUIHandler = null;
    
    private void Start()
    {
        StartCoroutine(SpawnCarCO(true));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !isChangingCar)
        {
            StartCoroutine(SpawnCarCO(true));
        }
    }

    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isChangingCar = true;

        if(carUIHandler != null) 
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);

        GameObject instantiatedCar =  Instantiate(carPrefab, spawnOnTransform);

        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.StartCarEnteranceAnimation(isCarAppearingOnRightSide);

        yield return new WaitForSeconds(0.2f);

        isChangingCar = false;
    }

}
