using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using GameEvents;
using TMPro;

public class PlayFabGameManager : Singleton<PlayFabGameManager>
{
    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform rowsParent;
    [SerializeField] Transform currentPlayerRowParent;

    private string player_Id;
    private bool isOnTopScoreBoard;

    private void OnEnable()
    {
        GameplayEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameplayEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver()
    {

        if (PlayFabClientAPI.IsClientLoggedIn() && GamePlayManager.Instance.isNormalMode == false)
        {
            StartCoroutine(SendAndGetLeaderBoard());
        }
    }

    IEnumerator SendAndGetLeaderBoard()
    {
        SendLeaderboard(ScoreManager.Instance.totalScoreCurrentRun);
        yield return new WaitForSeconds(0.5f);
        GetLeaderboard();
    }

    private void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = Const.SCOREBOARD_NAME,
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    private void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = Const.SCOREBOARD_NAME,
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var player in result.Leaderboard)
        {
            player_Id = SaveManager.Instance.LoadFile()._playerID;

            GameObject newGO = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newGO.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (player.Position + 1).ToString();
            texts[1].text = player.DisplayName;
            texts[2].text = player.StatValue.ToString();

            if (player_Id == player.PlayFabId)
            {
                texts[0].color = Color.cyan;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;

                isOnTopScoreBoard = true;
            }
           
            Debug.Log(player.Position + " " + player.DisplayName + " " + player.StatValue);
        }

        GetLeaderboardAroundPlayer();
    }

    private void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = Const.SCOREBOARD_NAME,
            MaxResultsCount = 1
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        player_Id = SaveManager.Instance.LoadFile()._playerID;

        if (isOnTopScoreBoard) return;

        foreach (var player in result.Leaderboard)
        {
            if (player.PlayFabId == player_Id)
            {
                GameObject newGO = Instantiate(rowPrefab, currentPlayerRowParent);
                TMP_Text[] texts = newGO.GetComponentsInChildren<TMP_Text>();
                texts[0].text = (player.Position + 1).ToString();
                texts[1].text = player.DisplayName;
                texts[2].text = player.StatValue.ToString();

                texts[0].color = Color.cyan;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;

                Debug.Log(player.Position + " " + player.DisplayName + " " + player.StatValue);

                return;
            }
        }
    }

    public void OnUpdateDisplayNameRequest(string newName)
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = newName};
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.LogWarning("NOME CERTO " + result.DisplayName);

    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
