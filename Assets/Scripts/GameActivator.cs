﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameActivator : MonoBehaviour
{
    public Camera menuCamera;
    public Camera gameCamera;

    public GameObject Tower;
    public Text textrows;
    public Dropdown dropdownGames;

    private bool _loadGames = true;
    [SerializeField]
    private float loadGamesTime = 5.0f;

    private SaveSystem.Informer<SavedGamesData> _informerGames;
    private SavedGamesData _currentData = null;
    private PlayerInGame _player;
    
    private void Start()
    {
        _player = FindObjectOfType<PlayerInGame>();
    }

    private void Update()
    {
        if (_informerGames != null && _informerGames.loaded)
        {
            dropdownGames.options = new List<Dropdown.OptionData>();
            dropdownGames.options.Add(new Dropdown.OptionData("Choose one"));
            foreach (JengaData data in _informerGames.data.games)
            {
                dropdownGames.options.Add(
                    new Dropdown.OptionData($"{data.id} - {new DateTime(data.date)}")
                );
            }
            _currentData = _informerGames.data;
            _informerGames = null;
            return;
        }

        if (!_loadGames) return;
        _loadGames = false;
        _informerGames = Config.LoadGamesData();
    }

    public void ResetTower()
    {
        int rows = 7;
        int parsedrows = int.Parse(textrows.text);
        if (parsedrows > 2 && parsedrows <= 17)
        {
            rows = parsedrows;
        }
        Tower.GetComponent<TowerGenerator>().Reset(rows);
    }

    public void SelectGame()
    {
        if (dropdownGames.value == 0 || 
            _player.StateCurrentTurn == PlayerInGame.State.WaitingForGameToStart ||
            !Tower.GetComponent<Tower>().towerAlreadyBuilt
        ) return;
        
        Tower.GetComponent<TowerGenerator>().ResetWithLoad(_currentData.games[dropdownGames.value-1]);
    }
    

    public void LoadGames()
    {
        _loadGames = true;
    }
    
}
