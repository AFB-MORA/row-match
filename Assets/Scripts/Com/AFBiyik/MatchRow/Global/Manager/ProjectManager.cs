using Com.AFBiyik.MatchRow.Global.LevelSystem;
using UnityEngine;
using Zenject;

public class ProjectManager : MonoBehaviour
{
    [Inject]
    private ILevelManager levelmanager;

    // Start is called before the first frame update
    async void Start()
    {
        var level = await levelmanager.GetLevel(1);
        Debug.Log("Level");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
