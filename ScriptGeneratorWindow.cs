using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptGeneratorWindow : EditorWindow
{
    private string scriptName = "NewScript";
    private int scriptType = 0;
    private string[] scriptOptions = { "Player Controller", "Singleton Pattern", "UI Button Handler", "Basic Enemy AI", "Empty Template" };

    [MenuItem("Tools/Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptGeneratorWindow>("Script Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Generator", EditorStyles.boldLabel);

        scriptName = EditorGUILayout.TextField("Script Name:", scriptName);
        scriptType = EditorGUILayout.Popup("Select Script Type:", scriptType, scriptOptions);

        if (GUILayout.Button("Generate Script"))
        {
            GenerateScript();
        }
    }

    private void GenerateScript()
    {
        string path = "Assets/" + scriptName + ".cs";

        if (File.Exists(path))
        {
            EditorUtility.DisplayDialog("Error", "Script already exists!", "OK");
            return;
        }

        string scriptContent = GetScriptTemplate(scriptType);
        File.WriteAllText(path, scriptContent);
        AssetDatabase.Refresh();
    }

    private string GetScriptTemplate(int type)
    {
        switch (type)
        {
            case 0:
                return "using UnityEngine;\n\npublic class " + scriptName + " : MonoBehaviour\n{\n    public float speed = 5f;\n    void Update()\n    {\n        float move = Input.GetAxis(\"Horizontal\") * speed * Time.deltaTime;\n        transform.Translate(move, 0, 0);\n    }\n}";

            case 1:
                return "using UnityEngine;\n\npublic class " + scriptName + " : MonoBehaviour\n{\n    public static " + scriptName + " Instance;\n    void Awake()\n    {\n        if (Instance == null) Instance = this;\n        else Destroy(gameObject);\n    }\n}";

            case 2:
                return "using UnityEngine;\nusing UnityEngine.UI;\n\npublic class " + scriptName + " : MonoBehaviour\n{\n    public Button myButton;\n    void Start()\n    {\n        myButton.onClick.AddListener(TaskOnClick);\n    }\n    void TaskOnClick()\n    {\n        Debug.Log(\"Button Clicked!\");\n    }\n}";

            case 3:
                return "using UnityEngine;\n\npublic class " + scriptName + " : MonoBehaviour\n{\n    public Transform player;\n    public float speed = 2f;\n    void Update()\n    {\n        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);\n    }\n}";

            default:
                return "using UnityEngine;\n\npublic class " + scriptName + " : MonoBehaviour\n{\n    void Start() { }\n    void Update() { }\n}";
        }
    }
}
