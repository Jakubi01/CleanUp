using Item.Garbage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UserInterface;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public enum GameOverType { CollectAllGarbage, LossAllScore }

        [SerializeField] private TypeMatchResult typeMatchResult;
        [SerializeField, Multiline] private string winMessage = "Thank you for your efforts.\n You collected all the trash.";
        [SerializeField, Multiline] private string loseMessage = "Study recycling again.";
        [SerializeField, Multiline] private string typeMatch = "Type Match!";
        [SerializeField, Multiline] private string typeMismatch = "Oops! Wrong type";
        
        public static GameManager Instance { get; private set; }

        private List<GarbageBase> garbageCache = new();
        private int _gameScore;

        private void Awake()
        {
            if (!Instance)
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
                if (!g) continue;
                
                if (garbageCache.Contains(g)) continue;
                garbageCache.Add(g);
            }
        }

        public void UnRegister(GarbageBase garbageBase)
        {
            if (!garbageBase || !garbageCache.Contains(garbageBase)) return;

            garbageCache.Remove(garbageBase);
            garbageBase.CompleteRemove();
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
                GameOverType.CollectAllGarbage => winMessage,
                GameOverType.LossAllScore => loseMessage,
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

        public void ShowTypeMatchResult(bool typeMatch)
        {
            if (!typeMatchResult) return;

            typeMatchResult.SetText(typeMatch ? this.typeMatch : typeMismatch, typeMatch);
            typeMatchResult.ShowText();
        }
    }
}
