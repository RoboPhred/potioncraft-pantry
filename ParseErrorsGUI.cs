using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    public static class ParseErrorsGUI
    {
        public static List<YamlFileException> ParseErrors = new();
        private static readonly Lazy<int> WindowId = new(() => GUIUtility.GetControlID(FocusType.Passive));

        private static Vector2 errorsScrollPosition = default;

        /// <summary>
        /// Gets or sets a value indicating whether the Goals gui is being shown.
        /// </summary>
        public static bool IsShowing { get; set; }

        /// <summary>
        /// Draw the gui.
        /// </summary>
        public static void OnGUI()
        {
            if (!IsShowing)
            {
                return;
            }

            var width = Mathf.Min(Screen.width, 500);
            var height = Mathf.Min(Screen.height, 700);
            var offsetX = (Screen.width * 3 / 4) - (width / 2);
            var offsetY = 10;
            GUILayout.Window(WindowId.Value, new Rect(offsetX, offsetY, width, height), ParseErrorsWindow, "Pantry Parse Errors");
        }

        public static void ShowIfErrors()
        {
            if (ParseErrors.Count > 0)
            {
                IsShowing = true;
            }
        }

        private static void ParseErrorsWindow(int id)
        {
            errorsScrollPosition = GUILayout.BeginScrollView(errorsScrollPosition, GUILayout.Height(600));

            foreach (var ex in ParseErrors)
            {
                GUILayout.Label($"{ex.FilePath}:{ex.Start.Line}");
                GUILayout.Label(ex.GetInnermostMessage());
                if (GUILayout.Button("Copy to clipboard"))
                {
                    var textEditor = new TextEditor { text = ex.ToString() };

                    textEditor.SelectAll();
                    textEditor.Copy();
                }
            }

            GUILayout.EndScrollView();

            if (GUILayout.Button("Close"))
            {
                IsShowing = false;
            }
        }
    }
}
