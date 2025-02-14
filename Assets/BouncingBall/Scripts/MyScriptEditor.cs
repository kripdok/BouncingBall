using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Example))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Отображаем стандартный интерфейс инспектора

        Example myScript = (Example)target; // Получаем ссылку на ваш скрипт

        if (GUILayout.Button("Нажми меня!")) // Создаем кнопку
        {
            myScript.SetForce(); // Вызываем метод при нажатии
        }
    }
}
