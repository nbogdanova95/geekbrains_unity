using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace Geekbrains.Editor
{
    public class UniqueNames
    {
        private static readonly Dictionary<string, int> _nameDictionary = new Dictionary<string, int>();
        [MenuItem("Lesson5/Проверка на уникальность имен", false, 0)]
        private static void NewMenuOption()
        {
            var sceneObj = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];                                
            if (sceneObj != null)
            {
                foreach (var obj in sceneObj)
                {
                    DataCollection(obj);
                }
            }
            foreach (var i in _nameDictionary)
            {
                for (var j = 0; j < i.Value; j++)
                {
                    var gameObj = GameObject.Find(i.Key);
                    if (gameObj)
                    {
                        gameObj.name += "(" + j + ")";
                    }
                }
            }
            _nameDictionary.Clear();           
        }
       
        private static void DataCollection(GameObject sceneObj)
        {
            string[] tempName = sceneObj.name.Split('(');
            tempName[0] = tempName[0].Trim();  
            if (!_nameDictionary.ContainsKey(tempName[0]))
            {
                _nameDictionary.Add(tempName[0], 0);
            }
            else
            {
                _nameDictionary[tempName[0]]++;
            }
            sceneObj.name = tempName[0];
        }
    }
}
