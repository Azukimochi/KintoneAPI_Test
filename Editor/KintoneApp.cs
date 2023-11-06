using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace io.github.azukimochi
{
    public class KintoneApp : EditorWindow
    {
        private static string KINTONE_USER = "";
        private static string KINTONE_MAIL = "";
        private static string KINTONE_API_URL = "";
        private static string KINTONE_API_KEY = "";
        
        private Tab _tab = Tab.Main;
        private bool _UpdateSolution = false;

        private string projectName = "";
        private string todoContetnt = "";

        // Start is called before the first frame update
        enum Tab
        {
            Main,
            Settings,
        }
        
        [MenuItem("Window/Kintone TodoApp")]
        public static void ShowWindow()
        {
            GetWindow<KintoneApp>("Kintone TodoApp");
        }
        private void Update()
        {
            if(_UpdateSolution)
            {
                _UpdateSolution = false;
                Repaint();
            }
        }
        private void OnEnable (){
            KINTONE_USER = Utils.DecryptAES(EditorPrefs.GetString("io.github.Azukimochi.KINTONE_USER", ""));
            KINTONE_MAIL = Utils.DecryptAES(EditorPrefs.GetString("io.github.Azukimochi.KINTONE_MAIL", ""));
            KINTONE_API_URL = Utils.DecryptAES(EditorPrefs.GetString("io.github.Azukimochi.KINTONE_API_URL", ""));
            KINTONE_API_KEY = Utils.DecryptAES(EditorPrefs.GetString("io.github.Azukimochi.KINTONE_API_KEY", ""));
        }
        void OnGUI()
        {
            _tab = (Tab)GUILayout.Toolbar((int)_tab, Styles.TabToggles, Styles.TabButtonStyle, Styles.TabButtonSize);
            
            switch (_tab)
            {
                case Tab.Main:
                    DrawMainTab();
                    break;
                case Tab.Settings:
                    DrawSettingsTab();
                    break;
            }
        }

        public void DrawMainTab()
        {
            GUILayout.Label("ToDo List", EditorStyles.boldLabel);
            GUILayout.Space(20);
            
            GUILayout.Label("プロジェクト名" , EditorStyles.boldLabel);
            projectName = EditorGUILayout.TextField(projectName);
            GUILayout.Label("ToDo" , EditorStyles.boldLabel);
            todoContetnt = EditorGUILayout.TextArea(todoContetnt, GUILayout.Height(50));
            if (GUILayout.Button("送信"))
            {
                kintoneAPIHundler.sendKintoneAPI(KINTONE_USER, KINTONE_MAIL, KINTONE_API_KEY, KINTONE_API_URL, projectName, todoContetnt);
            }
        }

        public void DrawSettingsTab()
        {
            GUILayout.Label("kintone User Name" , EditorStyles.boldLabel);
            KINTONE_USER = EditorGUILayout.TextArea(KINTONE_USER);
            GUILayout.Label("kintone Mail" , EditorStyles.boldLabel);
            KINTONE_MAIL = EditorGUILayout.TextArea(KINTONE_MAIL);
            GUILayout.Label("kintone API URL" , EditorStyles.boldLabel);
            KINTONE_API_URL = EditorGUILayout.PasswordField(KINTONE_API_URL);
            GUILayout.Label("kintone API Key" , EditorStyles.boldLabel);
            KINTONE_API_KEY = EditorGUILayout.PasswordField(KINTONE_API_KEY);
            GUILayout.Space(20);
            
            if (GUILayout.Button("適用"))
            {
                Debug.Log("save key and url");
                EditorPrefs.SetString("io.github.Azukimochi.KINTONE_USER", Utils.EncryptAES(KINTONE_USER));
                EditorPrefs.SetString("io.github.Azukimochi.KINTONE_MAIL", Utils.EncryptAES(KINTONE_MAIL));
                EditorPrefs.SetString("io.github.Azukimochi.KINTONE_API_KEY", Utils.EncryptAES(KINTONE_API_KEY));
                EditorPrefs.SetString("io.github.Azukimochi.KINTONE_API_URL", Utils.EncryptAES(KINTONE_API_URL));
            }
        }
        private static class Styles
        {
            private static GUIContent[] _tabToggles = null;
            public static GUIContent[] TabToggles{
                get {
                    if (_tabToggles == null) {
                        _tabToggles = System.Enum.GetNames(typeof(Tab)).Select(x => new GUIContent(x)).ToArray();
                    }
                    return _tabToggles;
                }
            }
            public static readonly GUIStyle TabButtonStyle = "LargeButton";
            public static readonly GUI.ToolbarButtonSize TabButtonSize = GUI.ToolbarButtonSize.Fixed;
        }
        
    }
}
