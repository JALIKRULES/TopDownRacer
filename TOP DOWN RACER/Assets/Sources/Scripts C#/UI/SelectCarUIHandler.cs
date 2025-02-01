using System.Collections;
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
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P1_isAI", 0);

        PlayerPrefs.SetInt("P2SelectedCarID", carDatas[Random.Range(0,carDatas.Length)].CarUniqueID);
        PlayerPrefs.SetInt("P2_isAI", 1);
        
        PlayerPrefs.SetInt("P3SelectedCarID", carDatas[Random.Range(0, carDatas.Length)].CarUniqueID);
        PlayerPrefs.SetInt("P3_isAI", 1);

        PlayerPrefs.SetInt("P4SelectedCarID", carDatas[Random.Range(0, carDatas.Length)].CarUniqueID);
        PlayerPrefs.SetInt("P4_isAI", 1);

        PlayerPrefs.Save();

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
