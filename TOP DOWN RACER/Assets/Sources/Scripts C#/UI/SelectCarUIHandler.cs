using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car prefab")]
    public GameObject carPrefab;

    [Header("Spawn on")]
    public Transform spawnOnTransform;

    bool isChangingCar = false;

    CarUIHandler carUIHandler = null;

    CarData[] carDatas;

    int selectedCarIndex = 0;

    private void Start()
    {
        carDatas = Resources.LoadAll<CarData>("CarData/");

        StartCoroutine(SpawnCarCO(true));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            OnPreviousCar();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            OnNextCar();
        }

        if(Input.GetKey(KeyCode.KeypadEnter))
        {
            OnSelectCar();
        }
    }

    public void OnPreviousCar()
    {
        if (isChangingCar)
            return;

        selectedCarIndex--;

        if (selectedCarIndex < 0)
            selectedCarIndex = carDatas.Length - 1;

        StartCoroutine(SpawnCarCO(true));
    }

    public void OnNextCar()
    {
        if (isChangingCar)
            return;

        selectedCarIndex++;

        if (selectedCarIndex > carDatas.Length - 1)
            selectedCarIndex = 0;

        StartCoroutine(SpawnCarCO(false));
    }

    public void OnSelectCar()
    {
        GameManager.Instance.ClearDriversList();

        GameManager.Instance.AddDriverToList(1, "P1", carDatas[selectedCarIndex].CarUniqueID, false);

        List<CarData> uniqueCars = new List<CarData>(carDatas);

        uniqueCars.Remove(carDatas[selectedCarIndex]);

        string[] names = {"Osama", "P.Diddy", "Ye", "Adolf.H","Max"};
        List<string> uniqueNames = names.ToList<string>();

        for(int i = 2; i < 5; i++)
        {
            string driverName = uniqueNames[Random.Range(0, uniqueCars.Count)];
            uniqueNames.Remove(driverName);

            CarData carData = uniqueCars[Random.Range(0, uniqueCars.Count)];

            GameManager.Instance.AddDriverToList(i, driverName, carData.CarUniqueID, true);
        }

        SceneManager.LoadScene(1);
    }

    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isChangingCar = true;

        if (carUIHandler != null)
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);

        GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);

        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.SetUpCar(carDatas[selectedCarIndex]);
        carUIHandler.StartCarEnteranceAnimation(isCarAppearingOnRightSide);

        yield return new WaitForSeconds(0.4f);

        isChangingCar = false;
    }

}
