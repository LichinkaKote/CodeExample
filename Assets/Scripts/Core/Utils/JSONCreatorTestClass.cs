using Assets.Scripts.Core.Data;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Core.Utils
{
    public class JSONCreatorTestClass : MonoBehaviour
    {
        public void Create()
        {
            var path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "TestClass.json";
            var obj = new EnemyData();
            obj.id = 1;
            obj.damageResistance = new EnemyDamageResistanceData { physRes = 0.5f };
            FileManager.SaveObject(obj, path);
        }
    }
}