using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Installer
{
    public struct InstallerPackageView : IWindowView
    {
        private readonly InstallerWindow window;
        private readonly List<PackageConfig> packages;

        private Vector2 categoriesScroll;
        private Vector2 packageScroll;

        private int selectedCategory;
        private string searchString;

        private readonly IEnumerable<string> categories;

        private const float TOP_PADDING = 20f;
        private const string PACKAGES_PATH = "Assets/Framework/Editor/Installer/Packages/";
        
        public InstallerPackageView(InstallerWindow window)
        {
            this.window = window;
            
            window.position = new Rect(window.position.x, window.position.y, 700f, 600f);
            
            window.minSize = new Vector2(650f, 550f);
            window.maxSize = new Vector2(700f, 700f);

            packages = new List<PackageConfig>();
            categoriesScroll = Vector2.zero;
            packageScroll = Vector2.zero;

            selectedCategory = 0;
            searchString = null;

            categories = InstallerHelper.GetConstantsValues(typeof(InstallerCategories));

            ImportPackages();
        }

        private void ImportPackages()
        {
            packages.Clear();
            string[] packageFilesPath = Directory.GetFiles(PACKAGES_PATH, "*.asset", SearchOption.AllDirectories);

            foreach (var packagePath in packageFilesPath)
            {
                var package = AssetDatabase.LoadAssetAtPath(packagePath, typeof(PackageConfig));
                packages.Add(package as PackageConfig);
            }
        }

        public void OnGUI()
        {
            DrawCategories();
            DrawHeader();
        }

        private void DrawCategories()
        {
            const float width = 180f;

            var rect = new Rect(0, TOP_PADDING, width, window.position.height);
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
            categoriesScroll = GUI.BeginScrollView(rect, categoriesScroll, rect);

            InstallerStyle.DrawVerticalLine(rect);

            var headerRect = rect;
            headerRect.height = 30f;
            headerRect.width -= 1;
            EditorGUI.DrawRect(headerRect, new Color(0.19f, 0.19f, 0.19f));

            InstallerStyle.DrawHorizontalLine(headerRect);

            GUI.Label(headerRect, "Categories", InstallerStyle.HEADER);

            var enumerable = categories as string[] ?? categories.ToArray();
            for (var index = 0; index < enumerable.Count(); index++)
            {
                var category = enumerable.ElementAt(index);
                if (OnCategoryButton(index, category, rect))
                {
                }
            }

            GUI.EndScrollView();

            DrawPackages(rect);
        }

        private void DrawPackages(Rect lastRect)
        {
            var scrollRect = new Rect(lastRect.width, TOP_PADDING, window.position.width - lastRect.width,
                window.position.height - TOP_PADDING);

            var categoriesList = categories;
            var currentCategory = selectedCategory;
            var categoryPackages = packages.Where(x => x.category.Value == categoriesList.ElementAt(currentCategory))
                .ToList();

            packageScroll = GUI.BeginScrollView(scrollRect, packageScroll,
                new Rect(scrollRect.position.x, 0, scrollRect.width - 13, categoryPackages.Count * 60f), false, true);

            scrollRect.width -= 13;

            for (var index = 0; index < categoryPackages.Count; index++)
            {
                if (OnPackageButton(index, categoryPackages[index], scrollRect))
                {
                }
            }

            GUI.EndScrollView();
        }

        private bool OnCategoryButton(int index, string title, Rect lastRect)
        {
            lastRect.height = 30f;
            var rect = new Rect(lastRect.position.x, 1 + lastRect.height + index * 31 + TOP_PADDING, lastRect.width - 1,
                30f);
            if (GUI.Button(rect, new GUIContent(title),
                    selectedCategory == index
                        ? InstallerStyle.CATEGORY_BUTTON_SELECTED
                        : InstallerStyle.CATEGORY_BUTTON))
            {
                selectedCategory = index;
            }

            InstallerStyle.DrawHorizontalLine(rect);

            rect.y += 6f;
            rect.x += rect.width - 25f;
            rect.height = 20f;
            GUI.Label(rect, InstallerStyle.CreateNamedIcon("", "d_tab_next"));

            return false;
        }

        private bool OnPackageButton(int index, PackageConfig packageConfig, Rect lastRect)
        {
            const float padding = 0f;
            var rect = new Rect(lastRect.x + padding, index * 60f + padding, lastRect.width - padding * 2, 60f);

            EditorGUI.DrawRect(rect, new Color(0.19f, 0.19f, 0.19f));
            InstallerStyle.DrawHorizontalLine(rect);

            // Icon
            var iconRect = rect;

            iconRect.width = 50;
            iconRect.height = 50;
            iconRect.y += 5;
            iconRect.x += 5;
            GUI.Label(iconRect, InstallerStyle.CreateNamedIcon("", packageConfig.GetIconKey()));

            rect.x += 60;

            // Header
            var headerRect = rect;
            headerRect.y -= 5;

            var headerGUI = new GUIContent(packageConfig.packageName);
            GUI.Label(headerRect, headerGUI, InstallerStyle.PACKAGE_HEADER);

            headerRect.width = InstallerStyle.PACKAGE_HEADER.CalcSize(headerGUI).x;

            // Installed label
            if (InstallerRegistry.IsInstalled(packageConfig.packageName))
            {
                var installedRect = headerRect;
                installedRect.height = 15;
                installedRect.width = 60;
                installedRect.x += headerRect.width;
                installedRect.y += 11;
                InstallerStyle.DrawUIBox(installedRect, new Color(0.41f, 0.89f, 0.62f), new Color(0.11f, 0.12f, 0.12f),
                    1);
                GUI.Label(installedRect, "Installed", InstallerStyle.PACKAGE_INSTALLED);
            }

            // Footer - Version and date
            GUI.Label(rect, new GUIContent($"Version - {packageConfig.Version:yyyy.MM.dd}"),
                InstallerStyle.PACKAGE_FOOTER);

            var descriptionRect = rect;
            descriptionRect.height = 20;
            descriptionRect.width = 350;
            descriptionRect.y += 24;

            // Description
            GUI.Label(descriptionRect, new GUIContent(packageConfig.description), InstallerStyle.PACKAGE_DESCRIPTION);

            rect.x += rect.width - 100;
            rect.y += 15;
            rect.width = 30;
            rect.height = 30;

            // Download button
            var icon = InstallerRegistry.IsInstalled(packageConfig.packageName)
                ? "winbtn_win_close_h@2x"
                : "Download-Available@2x";
            if (GUI.Button(rect, InstallerStyle.CreateNamedIcon("", icon), GUIStyle.none))
            {
                if (InstallerRegistry.IsInstalled(packageConfig.packageName))
                {
                    if (EditorUtility.DisplayDialog("Delete selected package?",
                            $"{packageConfig.packageName}.package \n " + $"\n" +
                            $"You cannot undo the delete packages action.", "Delete", "Cancel"))
                    {
                        PackageImporter.DeletePackage(packageConfig);  
                    }
                }
                else
                {
                    PackageImporter.ImportPackage(packageConfig);   
                }
            }

            return false;
        }

        private void DrawHeader()
        {
            var rect = new Rect(0, 0, window.position.width, 25f);

            GUI.Box(rect, "", GUI.skin.FindStyle("Toolbar"));

            var backRect = rect;
            backRect.width = 70;
            if (GUI.Button(backRect, InstallerStyle.CreateNamedIcon("Back", "d_Profiler.PrevFrame"), GUI.skin.FindStyle("ToolbarButton")))
            {
                window.ActiveWindow = new InstallerView(window);
            }
            
            var addRect = rect;
            addRect.width = 40;
            addRect.x += backRect.width;
            if (GUI.Button(addRect, InstallerStyle.CreateNamedIcon("", "d_CreateAddNew@2x"), GUI.skin.FindStyle("ToolbarButton")))
            {
                var exportMennu = new GenericMenu();
                InstallerPackageView tmpThis = this;
                
                exportMennu.AddItem(new GUIContent("Export folder"), false,
                    () => { exportPackage(true);  });
                exportMennu.AddItem(new GUIContent("Export files"), false,
                    () => { exportPackage(false); });

                exportMennu.ShowAsContext();
                
                void exportPackage(bool folderOnly)
                {
                    PackagePopup.Show("Add new package", "Insert package name", "Create",(c) => PackageImporter.ExportPackage(c, folderOnly));
                    tmpThis.ImportPackages();
                }
            }

            rect.y += 2;
            rect.width = 200;
            rect.x = window.position.width - rect.width;
            searchString = GUI.TextField(rect, searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));

            // Show menu

            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Open packages folder"), false,
                () => { InstallerHelper.PingObject(PACKAGES_PATH); });

            rect.height = 18f;
            rect.width = 18f;
            rect.x -= 25;
            if (GUI.Button(rect, InstallerStyle.CreateNamedIcon("", "d_Settings"), GUIStyle.none))
            {
                menu.ShowAsContext();
            }
        }
    }
}