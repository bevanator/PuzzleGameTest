using System;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryGame
{
    public class SaveManager : MonoBehaviour
    {
        private const string GridKey = "GRID_DATA";
        private const string RowsKey = "GRID_ROWS";
        private const string ColumnsKey = "GRID_COLUMNS";
        private const string ScoreKey = "SCORE_VALUE";
        private const string TurnKey = "TURN_COUNT";



        private Dictionary<int, string> _gridData = new();
        private bool _hasSaved;
        
        private static SaveManager _instance;


        private void Awake()
        {
            _instance = this;
        }

        void OnEnable()
        {
            CardSpawner.GridInitEvent += InitializeGrid;
            CardSpawner.GridSpawnEvent += SaveGridSize;
            CardMatchManager.MatchedEvent += OnMatched;
        }

        void OnDisable()
        {
            CardSpawner.GridInitEvent -= InitializeGrid;
            CardSpawner.GridSpawnEvent -= SaveGridSize;
            CardMatchManager.MatchedEvent -= OnMatched;
        }



        public static bool TryLoadGridSize(out int rows, out int columns)
        {
            rows = 0;
            columns = 0;

            if (!PlayerPrefs.HasKey(RowsKey) || !PlayerPrefs.HasKey(ColumnsKey))
                return false;

            rows = PlayerPrefs.GetInt(RowsKey);
            columns = PlayerPrefs.GetInt(ColumnsKey);
            return true;
        }
        
        public static void SaveGameState(int score, int turns)
        {
            PlayerPrefs.SetInt(ScoreKey, score);
            PlayerPrefs.SetInt(TurnKey, turns);
        }
        
        private void SaveGridSize(int rows, int columns)
        {
            PlayerPrefs.SetInt(RowsKey, rows);
            PlayerPrefs.SetInt(ColumnsKey, columns);
        }

        private void InitializeGrid(Dictionary<int, int> initialGrid)
        {
            _gridData.Clear();

            foreach (var kvp in initialGrid)
                _gridData[kvp.Key] = kvp.Value.ToString();

            _hasSaved = PlayerPrefs.HasKey(GridKey);
        }
        
        public void InitializeGrid(Dictionary<int, string> loadedGrid)
        {
            _gridData.Clear();

            foreach (var kvp in loadedGrid)
                _gridData[kvp.Key] = kvp.Value;

            _hasSaved = true;
        }


        public static void LoadGrid(Dictionary<int, string> gridData)
        {
            _instance.InitializeGrid(gridData);
        }

        private void OnMatched(List<int> matchedIndices)
        {
            for (int i = 0; i < matchedIndices.Count; i++)
                _gridData[matchedIndices[i]] = "x";

            Save();
        }

        private void Save()
        {
            if (!_hasSaved)
                _hasSaved = true;

            string data = Serialize();
            PlayerPrefs.SetString(GridKey, data);
            PlayerPrefs.Save();
        }


        public static bool TryLoad(out Dictionary<int, string> gridData) => _instance.TryLoadInternal(out gridData);

        private bool TryLoadInternal(out Dictionary<int, string> gridData)
        {
            gridData = null;

            if (!PlayerPrefs.HasKey(GridKey))
                return false;

            string data = PlayerPrefs.GetString(GridKey);
            gridData = Deserialize(data);
            return true;
        }

        private string Serialize()
        {
            int count = _gridData.Count;
            string[] values = new string[count];

            for (int i = 0; i < count; i++)
                values[i] = _gridData[i];

            return string.Join(",", values);
        }

        private Dictionary<int, string> Deserialize(string data)
        {
            Dictionary<int, string> result = new();
            string[] values = data.Split(',');

            for (int i = 0; i < values.Length; i++)
                result[i] = values[i];

            return result;
        }


        public static bool TryGetState(out int score, out int turns)
        {
            score = PlayerPrefs.GetInt(ScoreKey, 0);
            turns = PlayerPrefs.GetInt(TurnKey, 0);
            return PlayerPrefs.HasKey(TurnKey);
        }

        public static void ResetState()
        {
            PlayerPrefs.DeleteKey(ScoreKey);
            PlayerPrefs.DeleteKey(TurnKey);
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteAll();
            _instance._hasSaved = false;
        }
    }
}
