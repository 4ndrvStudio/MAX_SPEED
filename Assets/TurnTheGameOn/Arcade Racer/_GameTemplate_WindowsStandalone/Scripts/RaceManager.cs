namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.Events;
    using System.Linq;
    using System;

    public class RaceManager : MonoBehaviour
    {
        public static RaceManager Instance = null;
        public List<RaceWaypointData> waypointDataList;
        public RaceManager_Circuit_SpawnPoint[] spawnPoints;
        public List<GameObject> disableObjectsOnRaceComplete;
        public GameObject defaultCamera;
        public GameObject raceTimerAudioClip;
        public int playerIndex { get; private set; }
        public List<RaceManager_QuickRace_RacerInformation> racerInformation;
        public RaceInformation raceInformationUI;
        public UnityEvent onRaceFinished;
        public delegate void PlayerCompletedRace();
        public event PlayerCompletedRace OnPlayerCompletedRace;
        private int[] toSort;
        private float[] toSort2;
        private float gameTime;
        private float lapTime;
        private bool isPreRace = true;
        private bool playedRaceTimerAudio;
        public bool isChampionshipRace { get; private set; }
        public RacerInfo[] runtimeRacerInfo { get; private set; }
        public RaceData raceData { get; private set; }

        public void LoadRaceData(RaceData _raceData)
        {
            raceData = _raceData;
            StartCoroutine(InitializeRace());
            RaceInformation_LapTimes.Instance.Initialize();
            if (raceData.useDefeatTimer)
            {
                RaceCountDownTimer.Instance.InitializeTimer(raceData.defeatTimer);
            }
            else
            {
                RaceCountDownTimer.Instance.countDownTinerText.text = "";
                RaceCountDownTimer.Instance.addTimeText.text = "";
            }
        }

        void OnEnable()
        {
            Instance = this;
            OnPlayerCompletedRace += PlayerCompletedRaceCallback;
            if (RaceDataManager.Instance == null)
            {
                Debug.LogWarning("This scene requires a RaceDataManager, add one to this scene for testing or load this scene from the main menu.");
                enabled = false;
            }
            if (PlayerVehicleManager.Instance == null)
            {
                Debug.LogWarning("This scene requires a PlayerVehicleManager, add one to this scene for testing or load this scene from the main menu.");
                enabled = false;
            }
        }

        private void OnDisable()
        {
            OnPlayerCompletedRace -= PlayerCompletedRaceCallback;
            Instance = null;
        }

        IEnumerator InitializeRace()
        {
            while (raceData == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (SeriesManager.Instance)
            {
                isChampionshipRace = true;
                runtimeRacerInfo = SeriesManager.Instance.seriesData.racerInfo;
            }
            else
            {
                runtimeRacerInfo = raceData.racerInfo;
            }
            SetupRace();
        }

        void SetupRace()
        {
            for (int i = 0; i < runtimeRacerInfo.Length; i++)
            {
                if (spawnPoints.Length > i)
                {
                    RaceManager_QuickRace_RacerInformation newRacerInformationElement = new RaceManager_QuickRace_RacerInformation();
                    if (runtimeRacerInfo[i].player)
                    {
                        playerIndex = i;
                        GameObject racerObject = Instantiate(PlayerVehicleManager.Instance.VehiclePrefab(), spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                        if (defaultCamera != null)
                        {
                            defaultCamera.SetActive(false);
                        }
                        racerObject.name = GameData.GetProfileName();
                        newRacerInformationElement.racerObject = racerObject;
                        newRacerInformationElement.racerName = GameData.GetProfileName();
                        newRacerInformationElement.isPlayer = true;
                        newRacerInformationElement.gameTemplate_Player = racerObject.GetComponent<GameTemplate_Player>();
                        if (raceData.miniMapSettings.enabled)
                        {
                            newRacerInformationElement.gameTemplate_Player.SetupMiniMap
                            (
                                false,
                                raceData.miniMapSettings.lockPositionAndRotation,
                                raceData.miniMapSettings.lockPosition,
                                raceData.miniMapSettings.lockRotation,
                                raceData.miniMapSettings.cameraSize
                            );
                        }
                        if (raceData.showWrongWayWarning)
                        {
                            newRacerInformationElement.gameTemplate_Player.SetupWrongWayDetection();
                        }
                        racerInformation.Add(newRacerInformationElement);
                    }
                    else
                    {
                        GameObject racerObject = Instantiate(runtimeRacerInfo[i].vehiclePrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                        racerObject.name = runtimeRacerInfo[i].name;
                        newRacerInformationElement.racerObject = racerObject;
                        newRacerInformationElement.racerName = runtimeRacerInfo[i].name;
                        newRacerInformationElement.gameTemplate_AI = racerObject.GetComponent<AIRacer>();
                        newRacerInformationElement.gameTemplate_AI.AIController.waypointRoute = spawnPoints[i].startingAIRoute;
                        newRacerInformationElement.gameTemplate_AI.AIController.customResetPoint = spawnPoints[i].customResetPoint;
                        newRacerInformationElement.gameTemplate_AI.AIController.stuckResetRoute = spawnPoints[i].stuckResetRoute;
                        newRacerInformationElement.gameTemplate_AI.AIController.stuckResetPoint = spawnPoints[i].stuckResetPoint;


                        racerInformation.Add(newRacerInformationElement);
                    }
                    Array.Resize(ref newRacerInformationElement.lapTimes, raceData.laps);
                    newRacerInformationElement.previousWaypoint = spawnPoints[i].transform;
                    newRacerInformationElement.currentWaypoint = waypointDataList[0]._transform;
                    newRacerInformationElement.positionScore = 0;
                    RaceInformation.Instance.AddNewElement();
                }
            }
            Array.Resize(ref toSort, runtimeRacerInfo.Length);
            Array.Resize(ref toSort2, runtimeRacerInfo.Length);
            if (LoadingScreen.Instance != null) LoadingScreen.Instance.DisableLoadingScreen(0.1f);
            
            StartCoroutine("StartGame", 0.3f);
        }

        IEnumerator StartGame(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameTime = raceData.preRaceTime;
            StartCoroutine("PositionCalculation", 0.01f);
            while (isPreRace)
            {
                if (gameTime <= 2 && !playedRaceTimerAudio)
                {
                    playedRaceTimerAudio = true;
                    Instantiate(raceTimerAudioClip);
                }
                else if (gameTime <= 0)
                {
                    for (int i = 0; i < racerInformation.Count; i++)
                    {
                        if (racerInformation[i].isPlayer)
                        {
                            racerInformation[i].gameTemplate_Player.EnableControl();
                            if (PauseScreen.Instance != null) PauseScreen.Instance.canPause = true;
                        }
                        else
                        {
                            racerInformation[i].gameTemplate_AI.EnableControl();
                        }
                    }
                    isPreRace = false;
                    gameTime = 0;
                    if (raceData.useDefeatTimer)
                    {
                        RaceCountDownTimer.Instance.StartTimer();
                    }
                }
                yield return new WaitForSeconds(0.001f);
            }
        }

        private void Update()
        {
            GameTime();
        }

        void GameTime()
        {
            if (isPreRace)
            {
                gameTime -= 1 * Time.deltaTime;
            }
            else
            {
                gameTime += 1 * Time.deltaTime;
                lapTime += 1 * Time.deltaTime;
            }
            if (!racerInformation[playerIndex].finishedRace)
            {
                raceInformationUI.raceTimeText.text = FormatGameTime.ConvertFromSeconds((double)gameTime);
                raceInformationUI.lapTimeText.text = FormatGameTime.ConvertFromSeconds((double)lapTime);
            }
        }

        IEnumerator PositionCalculation()
        {
            while (true)
            {
                float distance;
                for (int i = 0; i < racerInformation.Count; i++)
                {
                    racerInformation[i].checkpointDistance = Vector3.Distance(racerInformation[i].racerObject.transform.position, racerInformation[i].currentWaypoint.position);
                    distance = (0.00001f * racerInformation[i].checkpointDistance);
                    racerInformation[i].distanceScore = -(distance);
                    racerInformation[i].totalScore = racerInformation[i].positionScore + racerInformation[i].distanceScore;
                    for (int j = 0; j < runtimeRacerInfo.Length; j++)
                    {
                        toSort[j] = racerInformation[j].position;
                    }
                }
                foreach (int sort in toSort.OrderBy(sorted => sorted)) { }
                for (int i = 0; i < runtimeRacerInfo.Length; i++)
                {
                    toSort2[i] = racerInformation[i].totalScore;
                }
                for (int i = 0; i < racerInformation.Count; i++)
                {
                    var sort2 = toSort2.OrderByDescending(sorted => sorted).ToArray();
                    float scoreCheck = racerInformation[i].totalScore;
                    for (int j = 0; j < toSort2.Length; j++)
                    {
                        if (scoreCheck == sort2[j]) racerInformation[i].position = j + 1;
                    }
                }
                for (int i = 0; i < raceInformationUI.racerInformationList.Count; i++)
                {
                    for (int j = 0; j < racerInformation.Count; j++)
                    {
                        if ((i + 1) == racerInformation[j].position)
                        {
                            raceInformationUI.racerInformationList[i].nameText.text = racerInformation[j].racerName;
                        }
                    }
                }
                raceInformationUI.playerPositionText.text = racerInformation[playerIndex].position.ToString() + " / " + runtimeRacerInfo.Length.ToString();
                raceInformationUI.playerStandingText.text = racerInformation[playerIndex].position.DisplayWithSuffix();
                raceInformationUI.playerLapText.text = racerInformation[playerIndex].lap.ToString() + " / " + raceData.laps.ToString();
                yield return new WaitForSeconds(0.01f);
            }
        }

        public void ChangeTarget(int racerNumber, int waypointNumber)
        {
            if (waypointNumber == racerInformation[racerNumber].nextWP + 1)
            {
                float check = racerInformation[racerNumber].nextWP;
                if (check < waypointDataList.Count)
                {
                    racerInformation[racerNumber].nextWP += 1;
                    racerInformation[racerNumber].positionScore += 2;
                    if (racerInformation[racerNumber].nextWP != waypointDataList.Count)
                    {
                        racerInformation[racerNumber].previousWaypoint = racerInformation[racerNumber].currentWaypoint;
                        racerInformation[racerNumber].currentWaypoint = waypointDataList[racerInformation[racerNumber].nextWP]._transform;
                    }
                    else
                    {
                        racerInformation[racerNumber].nextWP = 0;
                        if (raceData.laps > racerInformation[racerNumber].lap)
                        {
                            racerInformation[racerNumber].nextWP = 0;
                            racerInformation[racerNumber].previousWaypoint = racerInformation[racerNumber].currentWaypoint;
                            racerInformation[racerNumber].currentWaypoint = waypointDataList[racerInformation[racerNumber].nextWP]._transform;
                            racerInformation[racerNumber].positionScore += 100;
                            if (racerInformation[racerNumber].isPlayer)
                            {
                                racerInformation[racerNumber].lapTimes[racerInformation[racerNumber].lap - 1] = lapTime;
                                lapTime = 0;
                                CheckpointManager.Instance.ResetAllDisabledCheckpoints();
                            }
                            racerInformation[racerNumber].lap += 1;
                        }
                        else if (!racerInformation[racerNumber].finishedRace)
                        {
                            
                            racerInformation[racerNumber].finishedRace = true;
                            racerInformation[racerNumber].finalStanding = racerInformation[racerNumber].position;
                            racerInformation[racerNumber].finishTime = gameTime;
                            RaceInformation_FinalStandings.Instance.AddNewElement(
                                racerInformation[racerNumber].racerName,
                                FormatGameTime.ConvertFromSeconds((double)racerInformation[racerNumber].finishTime)
                                );
                            if (racerNumber == playerIndex) OnPlayerCompletedRace();
                        }
                    }
                }
            }
        }

        void PlayerCompletedRaceCallback()
        {
            for (int i = 0; i < disableObjectsOnRaceComplete.Count; i++) // disable ui and any other objects registered to be disabled
            {
                disableObjectsOnRaceComplete[i].SetActive(false);
            }
            if (PauseScreen.Instance != null) PauseScreen.Instance.canPause = false;
            for (int i = 0; i < racerInformation.Count; i++) // player race reward
            {
                if (racerInformation[i].isPlayer)
                {
                    racerInformation[i].lapTimes[racerInformation[i].lap - 1] = lapTime;
                    racerInformation[i].finishTime = gameTime;
                    raceInformationUI.raceTimeText.text = FormatGameTime.ConvertFromSeconds(gameTime);
                    switch (racerInformation[i].finalStanding)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                    onRaceFinished.Invoke();
                    for (int j = 0; j < racerInformation[i].lapTimes.Length; j++)
                    {
                        string _lap = (j + 1).ToString();
                        RaceInformation_LapTimes.Instance.AddNewElement(_lap, racerInformation[i].lapTimes[j]);
                    }

                    RaceCompleteScreen.Instance.prizeText.text = isChampionshipRace && racerInformation[i].finalStanding != racerInformation.Count ? "waiting for other racers to finish the race.." : "";
                    if (isChampionshipRace == false)
                    {
                        if (racerInformation[i].finalStanding - 1 < raceData.prize.Length)
                        {

                            RaceCompleteScreen.Instance.prizeText.text = "Earnings: " + raceData.prize[racerInformation[i].finalStanding - 1].ToString("c0");
                            int playerBank = GameData.GetPlayerBank();
                            int newBankBalance = playerBank + raceData.prize[racerInformation[i].finalStanding - 1];
                            GameData.SetPlayerBank(newBankBalance);
                        }
                        else if (racerInformation[i].finalStanding > 1)
                        {
                            GameTemplate_Player.Instance.SetDefeated();
                        }
                    }
                    
                    if (PauseScreen.Instance != null) PauseScreen.Instance.canPause = false;
                    defaultCamera.SetActive(true);
                    RaceCompleteScreen.Instance.OnRaceComplete();
                    if (!isChampionshipRace)
                    {
                        RaceCompleteScreen.Instance.SetCanContinue();
                    }
                }

            }
        }

        public void ForceFinishRace()
        {
            int playerIndex = 0;
            for (int i = 0; i <racerInformation.Count; i++)
            {
                if (racerInformation[i].isPlayer)
                {
                    playerIndex = i;
                    break;
                }
            }
            racerInformation[playerIndex].finishedRace = true;
            racerInformation[playerIndex].finalStanding = racerInformation[playerIndex].position;
            racerInformation[playerIndex].finishTime = gameTime;
            RaceInformation_FinalStandings.Instance.AddNewElement(
                racerInformation[playerIndex].racerName,
                FormatGameTime.ConvertFromSeconds((double)racerInformation[playerIndex].finishTime)
                );
            OnPlayerCompletedRace();
        }

        #region Editor Waypoint Spawners
        bool IsCBetweenAB(Vector3 A, Vector3 B, Vector3 C)
        {
            return (
                Vector3.Dot((B - A).normalized, (C - B).normalized) < 0f && Vector3.Dot((A - B).normalized, (C - A).normalized) < 0f &&
                Vector3.Distance(A, B) >= Vector3.Distance(A, C) &&
                Vector3.Distance(A, B) >= Vector3.Distance(B, C)
                );
        }

        public void ClickToSpawnNextWaypoint(Vector3 _position)
        {
            GameObject newWaypoint = Instantiate(Resources.Load("RaceWaypoint"), _position, Quaternion.identity, gameObject.transform) as GameObject;
            RaceWaypointData newPoint = new RaceWaypointData();
            newPoint._name = newWaypoint.name = "RaceWaypoint " + (waypointDataList.Count + 1);
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<RaceWaypoint>();
            newPoint._waypoint.waypointNumber = waypointDataList.Count + 1;
            waypointDataList.Add(newPoint);
        }

        public void ClickToInsertSpawnNextWaypoint(Vector3 _position)
        {
            bool isBetweenPoints = false;
            int insertIndex = 0;
            if (waypointDataList.Count >= 2)
            {
                for (int i = 0; i < waypointDataList.Count - 1; i++)
                {
                    Vector3 point_A = waypointDataList[i]._transform.position;
                    Vector3 point_B = waypointDataList[i + 1]._transform.position;
                    isBetweenPoints = IsCBetweenAB(point_A, point_B, _position);
                    insertIndex = i + 1;
                    if (isBetweenPoints) break;
                }
            }

            GameObject newWaypoint = Instantiate(Resources.Load("RaceWaypoint"), _position, Quaternion.identity, gameObject.transform) as GameObject;
            RaceWaypointData newPoint = new RaceWaypointData();
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<RaceWaypoint>();
            if (isBetweenPoints)
            {
                newPoint._transform.SetSiblingIndex(insertIndex);
                newPoint._name = newWaypoint.name = "RaceWaypoint " + (insertIndex + 1);
                waypointDataList.Insert(insertIndex, newPoint);
                for (int i = 0; i < waypointDataList.Count; i++)
                {
                    int newIndexName = i + 1;
                    waypointDataList[i]._transform.gameObject.name = "RaceWaypoint " + newIndexName;
                    waypointDataList[i]._waypoint.waypointNumber = i + 1;
                }
            }
            else
            {
                newPoint._name = newWaypoint.name = "RaceWaypoint " + (waypointDataList.Count + 1);
                newPoint._waypoint.waypointNumber = waypointDataList.Count + 1;
                waypointDataList.Add(newPoint);
            }
        }
        #endregion
    }
}