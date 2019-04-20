﻿using System.Collections;
using System.Collections.Generic;
using Single.MahjongDataType;
using UnityEngine;
using Debug = Single.Debug;

namespace Single
{
    public class PlayerHandManager : MonoBehaviour
    {
        public Transform handHolder;
        public Transform drawnHolder;
        [HideInInspector] public int Count;
        [HideInInspector] public List<Tile> Tiles = null;
        [HideInInspector] public Tile? LastDraw = null;
        private Transform[] handTileTransforms;
        private TileInstance[] handTileInstances;
        private Transform lastDrawTransform;
        private TileInstance lastDrawInstance;
        private bool discarding = false;
        private WaitForSeconds discardingWait = new WaitForSeconds(MahjongConstants.PlayerHandTilesSortDelay);

        private void OnEnable()
        {
            handTileTransforms = new Transform[handHolder.childCount];
            handTileInstances = new TileInstance[handHolder.childCount];
            for (int i = 0; i < handHolder.childCount; i++)
            {
                handTileTransforms[i] = handHolder.GetChild(i);
                handTileInstances[i] = handTileTransforms[i].GetComponent<TileInstance>();
            }
            lastDrawTransform = drawnHolder.GetChild(0);
            lastDrawInstance = lastDrawTransform.GetComponent<TileInstance>();
        }

        private void Update()
        {
            if (!discarding)
            {
                HoldTiles();
                LastDrawTile();
            }
        }

        private void HoldTiles()
        {
            if (Count > handHolder.childCount)
            {
                Debug.LogWarning($"Not enough tiles to show, cap to {handHolder.childCount}", false);
            }
            for (int i = 0; i < handHolder.childCount; i++)
            {
                handTileTransforms[i].gameObject.SetActive(i < Count);
            }
            if (Tiles == null) return;
            for (int i = 0; i < Tiles.Count; i++)
            {
                handTileInstances[i].SetTile(Tiles[i]);
            }
        }

        private void LastDrawTile()
        {
            if (LastDraw == null)
            {
                lastDrawTransform.gameObject.SetActive(false);
                return;
            }
            lastDrawTransform.gameObject.SetActive(true);
            lastDrawInstance.SetTile((Tile)LastDraw);
            var p = drawnHolder.transform.localPosition;
            drawnHolder.transform.localPosition = new Vector3(
                Count * MahjongConstants.HandTileWidth + MahjongConstants.LastDrawGap, p.y, p.z);
        }

        public void DiscardTile(bool discardingLastDraw)
        {
            discarding = true;
            if (discardingLastDraw) lastDrawTransform.gameObject.SetActive(false);
            else
            {
                int tileIndex = Random.Range(0, Count);
                handTileTransforms[tileIndex].gameObject.SetActive(false);
            }
            StartCoroutine(StopDiscarding());
        }

        private IEnumerator StopDiscarding()
        {
            yield return discardingWait;
            discarding = false;
            Debug.Log("Animation done");
        }

        public void Reveal()
        {
            // Reveal hand tiles
            handHolder.localRotation = Quaternion.Euler(90, 0, 0);
            var p = handHolder.localPosition;
            handHolder.localPosition = new Vector3(p.x, MahjongConstants.TileThickness / 2, p.z);
            // Reveal last draw
            drawnHolder.localRotation = Quaternion.Euler(90, 0, 0);
            p = drawnHolder.localPosition;
            drawnHolder.localPosition = new Vector3(p.x, MahjongConstants.TileThickness / 2, p.z);
        }

        public void StandUp()
        {
            // Un-reveal hand tiles
            handHolder.localRotation = Quaternion.Euler(0, 0, 0);
            var p = handHolder.localPosition;
            handHolder.localPosition = new Vector3(p.x, 0, p.z);
            // Un-reveal last draw
            drawnHolder.localRotation = Quaternion.Euler(0, 0, 0);
            p = drawnHolder.localPosition;
            drawnHolder.localPosition = new Vector3(p.x, 0, p.z);
        }

        public void CloseDown()
        {
            // Close hand tiles
            handHolder.localRotation = Quaternion.Euler(-90, 0, 0);
            var p = handHolder.localPosition;
            handHolder.localPosition = new Vector3(p.x, MahjongConstants.TileThickness / 2, p.z);
            // Close last draw
            drawnHolder.localRotation = Quaternion.Euler(-90, 0, 0);
            p = drawnHolder.localPosition;
            drawnHolder.localPosition = new Vector3(p.x, MahjongConstants.TileThickness / 2, p.z);
        }
    }
}
