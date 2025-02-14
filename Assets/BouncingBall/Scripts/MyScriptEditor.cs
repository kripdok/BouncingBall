using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Example))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // ���������� ����������� ��������� ����������

        Example myScript = (Example)target; // �������� ������ �� ��� ������

        if (GUILayout.Button("����� ����!")) // ������� ������
        {
            myScript.SetForce(); // �������� ����� ��� �������
        }
    }
}
