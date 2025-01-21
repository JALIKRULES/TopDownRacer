using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderboardItemInfo[] setLeaderBoardItemInfo;

    private void Awake()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        setLeaderBoardItemInfo = new SetLeaderboardItemInfo[carLapCounterArray.Length];

        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

            setLeaderBoardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();

            setLeaderBoardItemInfo[i].SetPositionText($"{i + 1}.");
        }

    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderBoardItemInfo[i].SetDriverName(lapCounters[i].gameObject.name);
        }
    }
}
