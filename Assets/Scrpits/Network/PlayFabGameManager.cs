using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using GameEvents;

public class PlayFabGameManager : Singleton<PlayFabGameManager>
{
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
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SendLeaderboard(ScoreManager.Instance.totalScoreCurrentRun);
            GetLeaderboard();
        }
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

    private void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = Const.SCOREBOARD_NAME,
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }


    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var player in result.Leaderboard)
        {
            Debug.Log(player.Position + " " + player.DisplayName + " " + player.StatValue);
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }
}
