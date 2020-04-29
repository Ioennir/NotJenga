﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    #region Private Variables
    private GameObject _tower;
    private Pool _piecePool;

    [Header("Configuration")]
    [SerializeField] private uint towerHeight = 17;
    [SerializeField] [Range(0.0f, 1.0f)] private float buildInterval = 0.5f;
    [SerializeField] private float pieceHeight = 0.5f;
    #endregion

    #region Public Variables

    #endregion

    #region Properties

    #endregion

    #region MonoBehaviour
    private void Start()
    {
        _tower = new GameObject("Tower");
        _tower.transform.parent = transform;
        _piecePool = GetComponent<Pool>();
        StartCoroutine("BuildTower", buildInterval);
    }
    
    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private IEnumerator BuildTower(float secondInterval)
    {
        for(int y = 0; y < towerHeight; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                GameObject piece = _piecePool.Instantiate();
                piece.transform.parent = _tower.transform;
                if (y % 2 == 0)
                {
                    piece.transform.localPosition = new Vector3(x, pieceHeight + pieceHeight * y, 0.0f);
                }
                else
                {
                    piece.transform.localRotation = Quaternion.AngleAxis(90.0f, piece.transform.up);
                    piece.transform.localPosition = new Vector3(1.0f, pieceHeight + pieceHeight * y, x - 1.0f);
                }
                yield return new WaitForSeconds(secondInterval);
            }
        }

        yield return 0;
    }

    #endregion

}