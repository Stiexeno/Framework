using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework.Installer
{
	public class InstallerWindow : EditorWindow
	{
		private readonly List<PackageConfig> packages = new List<PackageConfig>();

		private Vector2 packageScrollView;
		private int windowIndex;

		public IWindowView ActiveWindow { get; set; }

		[MenuItem("Framework/Installer", false)]
		public static void OpenWindow()
		{
			var window = GetWindow<InstallerWindow>(false, "Installer", true);
			window.titleContent = InstallerStyle.CreateNamedIcon("Installer", "d_Package Manager");
			window.Show();
		}

		public void Open(IWindowView window)
		{
			ActiveWindow = window;

			if (ActiveWindow is InstallerView)
			{
				windowIndex = 0;
			}
			else if (ActiveWindow is InstallerPackageView)
			{
				windowIndex = 1;
			}

			EditorPrefs.SetInt("Installer", windowIndex);
		}

		private void OnGUI()
		{
			Repaint();

			ActiveWindow.OnGUI();
			//packageView.OnGUI();
		}

		private void OnEnable()
		{
			if (EditorPrefs.HasKey("Installer") == false)
			{
				windowIndex = 0;
			}
			else
			{
				windowIndex = EditorPrefs.GetInt("Installer");
			}

			if (windowIndex == 0)
			{
				ActiveWindow = new InstallerView(this);
				return;
			}

			if (windowIndex == 1)
			{
				ActiveWindow = new InstallerPackageView(this);
				return;
			}
		}

		// Styles

		private bool OnButton(string title, bool smallSpace = false)
		{
			GUILayout.Space(smallSpace ? 5 : 20);
			return GUILayout.Button(title, EditorStyles.toolbarButton);
		}
	}
}