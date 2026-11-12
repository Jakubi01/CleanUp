using Item.Garbage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public enum GameOverType { CollectAllGarbage, LossAllScore }

        public static GameManager Instance { get; private set; }

        private List<GarbageBase> garbageCache;
        private int _gameScore;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Register(List<GarbageBase> list)
        {
            if (list == null || list.Count == 0) return;

            foreach(var g in list)
            {
                if (garbageCache.Contains(g)) continue;
                garbageCache.Add(g);
            }
        }

        public void UnRegister(GarbageBase garbageBase)
        {
            if (!garbageBase) return;
            if (!garbageCache.Contains(garbageBase)) return;

            garbageCache.Remove(garbageBase);
            ValidateList();
        }

        public void ValidateList()
        {
            if (garbageCache == null) return;

            if(garbageCache.Count <= 0)
            {
                GameOver(GameOverType.CollectAllGarbage);
            }
        }

        public string GameOver(GameOverType gameOverType)
        {
            var message = gameOverType switch
            {
                GameOverType.CollectAllGarbage => "Thank you for your efforts.\n You collected all the trash.",
                GameOverType.LossAllScore => "Study recycling again.",
                _ => ""
            };

            return message;
        }

        public void AddGameScore(int scoreValue)
        {
            if (scoreValue <= 0) return;

            _gameScore += scoreValue;
        }

        public void RemoveGameScore(int scoreValue)
        {
            if(scoreValue <= 0) return;

            _gameScore -= scoreValue;
            if(_gameScore < 0)
            {
                GameOver(GameOverType.LossAllScore);
            }
        }
    }
}
