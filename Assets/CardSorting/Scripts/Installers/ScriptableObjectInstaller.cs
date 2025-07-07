using CardSorting;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableObjectInstaller", menuName = "Installers/ScriptableObjectInstaller")]
public class ScriptableObjectInstaller : ScriptableObjectInstaller<ScriptableObjectInstaller>
{
    [SerializeField]
    private ScriptableObject[] scriptableObjects;
    
    public override void InstallBindings()
    {
        foreach (var scriptableObject in scriptableObjects)
        {
            Container.BindInterfacesAndSelfTo(scriptableObject.GetType()).FromInstance(scriptableObject);
        }
    }
}