using System.Collections.Generic;
using Item.Garbage;
using UnityEngine;
using UserInterface;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TypeMatchResult typeMatchResult;
        [SerializeField] private GameOver gameOver;
        [SerializeField] private GameHUD gameHUD;
        
        public static GameManager Instance { get; private set; }

        private readonly List<GarbageBase> _garbageCache = new();
        private int _cacheElemCount;
        private int _gameScore;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Register(List<GarbageBase> list)
        {
            if (list == null || list.Count == 0) return;

            foreach(var g in list)
            {
                if (g == null || _garbageCache.Contains(g)) continue;
                _garbageCache.Add(g);
            }
            
            _cacheElemCount = _garbageCache.Count;
        }

        public void UnRegister(GarbageBase garbageBase)
        {
            if (garbageBase is null|| !_garbageCache.Contains(garbageBase)) return;

            _garbageCache.Remove(garbageBase);
            garbageBase.CompleteRemove();
            ValidateList();
        }

        private void ValidateList()
        {
            if (_garbageCache == null || _garbageCache.Count > 0) return;
            if (gameOver is null || gameHUD is null) return;

            gameHUD.Hide();
            gameOver.SetResultText(_gameScore, _cacheElemCount);
            gameOver.Show();
        }

        public void AddGameScore(int scoreValue)
        {
            if (scoreValue <= 0 || gameHUD is null) return;

            _gameScore += scoreValue;
            gameHUD.UpdateScoreText(_gameScore);
        }

        public void RemoveGameScore(int scoreValue)
        {
            if(scoreValue <= 0 || gameHUD is null) return;

            _gameScore -= scoreValue;
            gameHUD.UpdateScoreText(_gameScore);
        }

        public void ShowTypeMatchResult(bool isTypeMatch)
        {
            if (typeMatchResult is null) return;

            typeMatchResult.SetText(isTypeMatch);
            typeMatchResult.ShowText();
        }

        public void ClearFlags()
        {
            _gameScore = 0;
            _garbageCache.Clear();
        }
    }
}
