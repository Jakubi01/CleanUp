using Item.Garbage;
using System.Collections.Generic;
using UnityEngine;
using UserInterface;

namespace Assets.Scripts.Manager
{
    public enum GameOverType { CollectAllGarbage, LossAllScore }
    
    public class GameManager : MonoBehaviour
    {

        [Header("UI")]
        [SerializeField] private TypeMatchResult typeMatchResult;
        [SerializeField] private GameOver gameOver;
        
        public static GameManager Instance { get; private set; }

        private readonly List<GarbageBase> _garbageCache = new();
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
                if (g == null) continue;
                
                if (_garbageCache.Contains(g)) continue;
                _garbageCache.Add(g);
            }
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
            if (_garbageCache == null || gameOver is null) return;
            if (_garbageCache.Count > 0) return;
            
            gameOver.SetText(GameOverType.CollectAllGarbage);
            gameOver.Show();
        }

        public void AddGameScore(int scoreValue)
        {
            if (scoreValue <= 0) return;

            _gameScore += scoreValue;
        }

        public void RemoveGameScore(int scoreValue)
        {
            if(scoreValue <= 0 || gameOver is null) return;

            _gameScore -= scoreValue;
            if (_gameScore >= 0) return;
            
            gameOver.SetText(GameOverType.LossAllScore);
            gameOver.Show();
        }

        public void ShowTypeMatchResult(bool isTypeMatch)
        {
            if (typeMatchResult is null) return;

            typeMatchResult.SetText(isTypeMatch);
            typeMatchResult.ShowText();
        }
    }
}
