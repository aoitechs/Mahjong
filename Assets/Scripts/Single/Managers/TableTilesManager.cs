﻿using System.Collections.Generic;
using Single.MahjongDataType;
using Single.MahjongDataType.Interfaces;
using UnityEngine;
using Utils;

namespace Single.Managers
{
    public class TableTilesManager : MonoBehaviour, IObserver<ClientRoundStatus>
    {
        public PlayerHandManager[] HandManagers;
        public OpenMeldManager[] OpenManagers;
        public PlayerRiverManager[] RiverManagers;
        public PlayerBeiDoraManager[] BeiManagers;

        private void UpdateHands(ClientRoundStatus status)
        {
            for (int placeIndex = 0; placeIndex < HandManagers.Length; placeIndex++)
            {
                var hand = HandManagers[placeIndex];
                hand.Count = status.GetTileCount(placeIndex);
                hand.LastDraw = status.GetLastDraw(placeIndex);
            }
            HandManagers[0].HandTiles = status.LocalPlayerHandTiles;
        }

        public void SetMelds(int placeIndex, IList<OpenMeld> melds)
        {
            OpenManagers[placeIndex].SetMelds(melds);
        }

        public void ClearMelds(int placeIndex)
        {
            OpenManagers[placeIndex].ClearMelds();
        }

        public void ClearMelds()
        {
            System.Array.ForEach(OpenManagers, m => m.ClearMelds());
        }

        private void UpdateRivers(ClientRoundStatus status)
        {
            for (int placeIndex = 0; placeIndex < RiverManagers.Length; placeIndex++)
            {
                var manager = RiverManagers[placeIndex];
                manager.RiverTiles = status.GetRiverTiles(placeIndex);
            }
        }

        private void UpdateBeiDoras(ClientRoundStatus status)
        {
            if (status.BeiDoras == null) return;
            for (int placeIndex = 0; placeIndex < BeiManagers.Length; placeIndex++)
            {
                var manager = BeiManagers[placeIndex];
                manager.SetBeiDoras(status.BeiDoras[placeIndex]);
            }
        }

        public void DiscardTile(int placeIndex, bool discardingLastDraw)
        {
            HandManagers[placeIndex].DiscardTile(discardingLastDraw);
        }

        public void SetHandTiles(int placeIndex, IList<Tile> handTiles)
        {
            HandManagers[placeIndex].HandTiles = handTiles;
        }

        public void StandUp(int placeIndex)
        {
            HandManagers[placeIndex].StandUp();
        }

        public void StandUp()
        {
            System.Array.ForEach(HandManagers, manager => manager.StandUp());
        }

        public void OpenUp(int placeIndex)
        {
            HandManagers[placeIndex].OpenUp();
        }

        public void OpenUp()
        {
            System.Array.ForEach(HandManagers, m => m.OpenUp());
        }

        public void CloseDown(int placeIndex)
        {
            HandManagers[placeIndex].CloseDown();
        }

        public void CloseDown()
        {
            System.Array.ForEach(HandManagers, m => m.CloseDown());
        }

        public void UpdateStatus(ClientRoundStatus subject)
        {
            if (subject == null) return;
            UpdateHands(subject);
            UpdateRivers(subject);
            UpdateBeiDoras(subject);
        }
    }
}
