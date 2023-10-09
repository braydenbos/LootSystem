using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(LootDrop))]
public class LootDropEditor : Editor
{

    public List<Roll> rolls = new List<Roll>();
    public int dropAmount= 1;
    public class Roll
    {
        public List<GameObject> items = new List<GameObject>();
    }

    public override void OnInspectorGUI()
    {
        GUIStyle horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
        horizontalLine.fixedHeight = 1;
 
        static void HorizontalLine ( Color color ,GUIStyle horizontalLine) {
            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box( GUIContent.none, horizontalLine );
            GUI.color = c;
        }
        DrawDefaultInspector();

        LootDrop lootDrop = (LootDrop)target; 

        EditorGUILayout.Separator();
        HorizontalLine(Color.white, horizontalLine);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Roll loot drop",GUILayout.Height(30)))
        {
            for (int i = 0; i < dropAmount ; i++)
            {
                var roll = new Roll();

                foreach (var item in LootSystem.Instance.RollLoot(lootDrop.lootTable,lootDrop.lootForce,lootDrop.transform))
                {
                    roll.items.Add(item);
                }
                rolls.Add(roll);

            }
        }
        dropAmount = EditorGUILayout.IntSlider("",dropAmount,1,100,GUILayout.Width(150),GUILayout.Height(30));
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Clear roll",GUILayout.Height(30)))
        {
            rolls.Clear();
        }
        
        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Drops:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Total rolls: "+rolls.Count, EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        for (int i = 0; i < rolls.Count; i++)
        {
            List<string> items = new List<string>();
            EditorGUILayout.LabelField("Roll " + (i+1) + ":");
            for (int j = 0; j < rolls[i].items.Count; j++)
            {
                int amount = 1;
                string name = rolls[i].items[j].name;

                for (int k = 0; k < rolls[i].items.Count; k++)
                {
                    if (name == rolls[i].items[k].name && j != k)
                    {
                        amount++;
                    }
                }
                if (!items.Contains(amount+"x "+name))
                {
                    items.Add(amount+"x "+name);
                    EditorGUILayout.LabelField(amount+"x "+name);
                }
            }
            if (GUILayout.Button("delete", GUILayout.Height(30)))
            {
                rolls.RemoveAt(i);
            }
            HorizontalLine(Color.white, horizontalLine);
            EditorGUILayout.Separator();
        }
    }
}