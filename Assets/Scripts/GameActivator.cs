using System;
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
    
    private void Start()
    {
        
    }

    private void Update()
    {
        if (_informerGames != null && _informerGames.loaded)
        {
            dropdownGames.options = new List<Dropdown.OptionData>();
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
        if (parsedrows > 2 && parsedrows < 17)
        {
            rows = parsedrows;
        }
        Tower.GetComponent<TowerGenerator>().Reset(rows);
    }

    public void SelectGame()
    {
        Tower.GetComponent<TowerGenerator>().ResetWithLoad(_currentData.games[dropdownGames.value]);
    }
    

    public void LoadGames()
    {
        _loadGames = true;
    }
    
}
