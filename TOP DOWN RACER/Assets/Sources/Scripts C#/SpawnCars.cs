using Cinemachine;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    

    void Start()
    {
        

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            int playerSelectedCarID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");

            foreach (CarData carData in carDatas)
            {
                if (carData.CarUniqueID == playerSelectedCarID)
                {
                    GameObject car = Instantiate(carData.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    int playerNumber = i + 1;

                    car.GetComponent<CarInputHandler>().playerNumber = i + 1;

                    if(PlayerPrefs.GetInt($"P{playerNumber}_isAI") == 1)
                    {
                        car.GetComponent<CarInputHandler>().enabled = false;
                        car.GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
                        car.tag = "AI";

                    }
                    else
                    {
                        car.GetComponent<CarAIHandler>().enabled = false;
                        car.GetComponent<AStarLite>().enabled = false;
                        car.tag = "Player";

                        
                    }

                    break;
                }
            }
        }
    }


}
