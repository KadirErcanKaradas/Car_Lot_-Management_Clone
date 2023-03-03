using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameStage GameStage { get; private set; }
    public Transform carParent;
    public Transform carGoTo;

    protected override void Awake()
    {
        base.Awake();
            HandleData();
    }
    public void SetGameStage(GameStage gameStage)
    {
        GameStage = gameStage;
    }
    private void HandleData()
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("Money", 10000);
        }
    }
    
}
public enum GameStage
{
    NotLoaded, Loaded, Started, Win, Fail
}