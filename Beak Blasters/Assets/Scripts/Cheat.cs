using System.Text;
using SaveSystems;
using UnityEngine;
using Zenject;

public class Cheat : MonoBehaviour
{
    [Inject] private SaveSystem _saveSystem;
    
    private readonly StringBuilder _code = new();
    private int _counter;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _code.Append("Q");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _code.Append("W");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _code.Append("E");

            if (_code.ToString() == "QWE")
            {
                Debug.Log("Cheater!");
                _saveSystem.SavePlayerCoins(50000);
            }
            else
            {
                _code.Clear();
            }
        }
    }

    public void ClickLogo()
    {
        _counter++;
        if (_counter % 10 == 0)
        {
            Debug.Log("Cheater!");
            _saveSystem.SavePlayerCoins(50000);
        }
    }
}
