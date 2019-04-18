using System.Collections.Generic;
using UnityEngine;

namespace Single.MahjongDataType
{
    [CreateAssetMenu(menuName = "Mahjong/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("General settings")] public int InitialPoints = 25000;
        public int RichiMortgagePoints = 1000;
        public int ExtraRoundBonusPerPlayer = 100;
        public int NotReadyPunishPerPlayer = 1000;
        public bool GameEndsWhenLoseAllPoints = true;
        public bool AllowDiscardSameAfterOpen = false;
        public bool AllowChows = true;
        
        [Header("Tile drawing settings")] public int InitialDrawRound = 3;
        public int TilesEveryRound = 4;
        public int TilesLastRound = 1;

        [Header("Time settings")] public int BaseTurnTime = 5;
        public int BonusTurnTime = 20;
        // public int ServerTimeOut = 30;
        
        [Header("Mahjong settings")]
        public int DiceMin = 2;
        public int DiceMax = 12;
        public int MountainReservedTiles = 14;
        public int LingshangTilesCount = 4;
        public int InitialDora = 1;
        public int MaxDora = 5;

        public bool IsChowAllowed => AllowChows;

        public Tile[] allTiles = MahjongConstants.FullTiles.ToArray();
        
        public Tile[] redTiles = new Tile[] {
            new Tile(Suit.M, 5, true), 
            new Tile(Suit.P, 5, true), 
            new Tile(Suit.S, 5, true)
        };
    }
}