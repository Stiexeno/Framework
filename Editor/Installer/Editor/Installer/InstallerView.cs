using UnityEditor;
using UnityEngine;

namespace Framework.Installer
{
	public readonly struct InstallerView : IWindowView
	{
		private readonly InstallerWindow window;

		private const float TOP_PADDING = 20f;

		public InstallerView(InstallerWindow window) : this()
		{
			window.position = new Rect(window.position.x, window.position.y, 450f, 600f);
			window.minSize = new Vector2(400f, 550f);
			window.maxSize = new Vector2(400f, 550f);

			window.Open(this);

			this.window = window;
		}

		public void OnGUI()
		{
			DrawHeader();
		}

		private void DrawHeader()
		{
			var rect = new Rect(0, 0, window.position.width, 20f);
			GUI.Box(rect, "", GUI.skin.FindStyle("Toolbar"));

			var packageRect = rect;

			packageRect.width = 100f;
			packageRect.x = rect.width - packageRect.width + 1;
			if (GUI.Button(packageRect, InstallerStyle.CreateNamedIcon("Packages", "d_Package Manager"), EditorStyles.toolbarButton))
			{
				window.Open(new InstallerPackageView(window));
			}

			const float buttonHeight = 30f;
			rect.y += TOP_PADDING + 1;
			if (OnButton(rect, "Setup Scene"))
			{
				InstallerActions.Setup();
			}

			rect.y += buttonHeight;
			if (OnButton(rect, "Setup Vertical UIManager"))
			{
				InstallerActions.CreateUIManager(true);
			}

			rect.y += buttonHeight;
			if (OnButton(rect, "Create folders"))
			{
				InstallerActions.CreateFolders();
			}
		}

		// Styles

		private bool OnButton(Rect rect, string title)
		{
			rect.height = 30f;
			EditorGUI.DrawRect(rect, new Color(0.20f, 0.20f, 0.20f));
			InstallerStyle.DrawHorizontalLine(rect);
			if (GUI.Button(rect, title, InstallerStyle.INSTALLER_BUTTON))
			{
				return true;
			}

			return false;
		}
	}
}