using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    #region Private Variables
    
    private GameObject _tower;
    private Pool _piecePool;
    [Header("Configuration")]
    [SerializeField] private uint towerHeight = 17;
    [SerializeField] [Range(0.0f, 1.0f)] private float buildInterval = 0.5f;
    private float pieceHeight = 0.1f;
    private float pieceWidth = 0.1f;
    private Tower _towerData;
    private Vector3 towerCenter;
    private JengaData _dataLoaded;
    
    #endregion

    #region Public Variables
    public Material[] pieceMaterials;
    #endregion

    #region Properties

    public float PieceHeight => pieceHeight;

    public Vector3 TowerCenter => towerCenter;

    public float PieceWidth => pieceWidth;
    
    
    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _towerData = gameObject.AddComponent<Tower>();
        pieceHeight *= 0.1f;
        towerCenter = transform.position;
        // Uncomment for test
        //Config.LoadInConfig(SaveSystem.Load<SavedGamesData>("game_data.json").games[0]);
    }

    private void Start()
    {
        InitTower();
    }

    #endregion

    #region Public Methods

    public void Reset(int rows)
    {
        //Despawn all pieces or explode them and then reconstruct the tower
        _piecePool.DeactivateAll();
        towerHeight = (uint)rows;
        //For some reason, the first piece instantiated after doing this has got the wrong rotation.
        if (_dataLoaded == null)
        {
            StartCoroutine("BuildTower", buildInterval);
            return;
        }
        StartCoroutine(SpawnLoad());
        _towerData.towerAlreadyBuilt = true;
    }

    private IEnumerator SpawnLoad()
    { 
        for (int i = 0; i < _dataLoaded.jengas.Length; ++i)
        {
            JengaPieceData piece = _dataLoaded.jengas[i];
            GameObject instance = _piecePool.Instantiate();
            Transform transformPiece = instance.transform;
            instance.transform.parent = _tower.transform;
            transformPiece.position = piece.position;
            transformPiece.rotation = Quaternion.Euler(piece.rotation);
            transformPiece.localScale = piece.scale;
            pieceHeight = piece.scale.y + 0.05f;
            pieceWidth = piece.scale.x + 0.05f;
            instance.GetComponent<MeshRenderer>().material = pieceMaterials[i % 2];
            _towerData.AddPiece(instance);
        }
        yield return new WaitForSeconds(2f);
        _towerData.towerAlreadyBuilt = true;
    }

    public float CalculateTop(float y)
    {
        return pieceHeight + pieceHeight * y;
    }

    /// <summary>
    /// Adds jenga piece into the tower hierarchy
    /// </summary>
    /// <param name="jenga"></param>
    public void AddPiece(GameObject jenga)
    {
        jenga.transform.parent = _tower.transform;
    }

    #endregion

    #region Private Methods

    private void InitTower()
    {
        _tower = new GameObject("Tower");
        _tower.transform.parent = transform;
        _piecePool = GetComponent<Pool>();
        _dataLoaded = Config.GetJengaConfig();
        if (_dataLoaded == null)
        {
            StartCoroutine("BuildTower", buildInterval);
            return;
        }
        StartCoroutine(SpawnLoad());
        _towerData.towerAlreadyBuilt = true;
    }

    private IEnumerator BuildTower(float secondInterval)
    {
        for(int y = 0; y < towerHeight; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                GameObject piece = _piecePool.Instantiate();
                piece.transform.parent = _tower.transform;
                //piece.transform.localScale *= 0.5f;
                pieceHeight = piece.transform.localScale.y + 0.05f;
                pieceWidth = piece.transform.localScale.x + 0.05f;
                var original = PieceOriginalMaterial.Get(piece);
                original.originalMaterial = pieceMaterials[x % 2];
                piece.GetComponent<MeshRenderer>().material = original.originalMaterial;
                if (y % 2 == 0)
                {
                    piece.transform.localPosition = new Vector3(towerCenter.x + (x * pieceWidth - pieceWidth),
                        towerCenter.y + y * pieceHeight + pieceHeight,
                        towerCenter.z);
                }
                else
                {
                    piece.transform.localRotation = Quaternion.AngleAxis(90.0f, piece.transform.up);
                    piece.transform.localPosition = new Vector3(towerCenter.x,
                        towerCenter.y + y * pieceHeight + pieceHeight,
                        towerCenter.z + (x * pieceWidth - pieceWidth));
                }
                _towerData.AddPiece(piece);
                
                yield return new WaitForSeconds(secondInterval);
            }
            Material temp = pieceMaterials[0];
            pieceMaterials[0] = pieceMaterials[1];
            pieceMaterials[1] = temp;
        }

        // This is a delay because Tower might bug
        yield return new WaitForSeconds(2f);
        _towerData.towerAlreadyBuilt = true;

        yield return 0;
    }
    

    #endregion

}
