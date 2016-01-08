
// This file has been generated by the GUI designer. Do not modify.
namespace LongoMatch.Gui
{
	public partial class MainWindow
	{
		private global::Gtk.UIManager UIManager;
		
		private global::Gtk.Action FileAction;
		
		private global::Gtk.Action NewPojectAction;
		
		private global::Gtk.Action OpenProjectAction;
		
		private global::Gtk.Action QuitAction;
		
		private global::Gtk.Action CloseProjectAction;
		
		private global::Gtk.Action ToolsAction;
		
		private global::Gtk.Action ProjectsManagerAction;
		
		private global::Gtk.Action CategoriesTemplatesManagerAction;
		
		private global::Gtk.Action ViewAction;
		
		private global::Gtk.ToggleAction FullScreenAction;
		
		private global::Gtk.Action SaveProjectAction;
		
		private global::Gtk.Action HelpAction;
		
		private global::Gtk.Action AboutAction;
		
		private global::Gtk.Action ExportProjectToCSVFileAction;
		
		private global::Gtk.Action TeamsTemplatesManagerAction;
		
		private global::Gtk.Action HelpAction1;
		
		private global::Gtk.Action ImportProjectAction;
		
		private global::Gtk.Action ExportProjectAction;
		
		private global::Gtk.Action ExportProjectAction1;
		
		private global::Gtk.Action Action;
		
		private global::Gtk.Action ExportToProjectFileAction;
		
		private global::Gtk.Action ImportFromFileAction;
		
		private global::Gtk.Action VideoConverterToolAction;
		
		private global::Gtk.Action DatabasesManagerAction;
		
		private global::Gtk.Action PreferencesAction;
		
		private global::Gtk.Action ShowProjectStatsAction;
		
		private global::Gtk.Action MigrationToolAction;
		
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.VBox menubox;
		
		private global::Gtk.MenuBar menubar1;
		
		private global::Gtk.HBox centralbox;
		
		private global::LongoMatch.Gui.Component.RenderingStateBar renderingstatebar1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget LongoMatch.Gui.MainWindow
			this.UIManager = new global::Gtk.UIManager ();
			global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
			this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("_File"), null, null);
			this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_File");
			w1.Add (this.FileAction, null);
			this.NewPojectAction = new global::Gtk.Action ("NewPojectAction", global::Mono.Unix.Catalog.GetString ("_New Project"), null, "gtk-new");
			this.NewPojectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_New Project");
			w1.Add (this.NewPojectAction, null);
			this.OpenProjectAction = new global::Gtk.Action ("OpenProjectAction", global::Mono.Unix.Catalog.GetString ("_Open Project"), null, "gtk-open");
			this.OpenProjectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Open Project");
			w1.Add (this.OpenProjectAction, null);
			this.QuitAction = new global::Gtk.Action ("QuitAction", global::Mono.Unix.Catalog.GetString ("_Quit"), null, "gtk-quit");
			this.QuitAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Quit");
			w1.Add (this.QuitAction, null);
			this.CloseProjectAction = new global::Gtk.Action ("CloseProjectAction", global::Mono.Unix.Catalog.GetString ("_Close Project"), null, "gtk-close");
			this.CloseProjectAction.Sensitive = false;
			this.CloseProjectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Close Project");
			w1.Add (this.CloseProjectAction, null);
			this.ToolsAction = new global::Gtk.Action ("ToolsAction", global::Mono.Unix.Catalog.GetString ("_Tools"), null, null);
			this.ToolsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Tools");
			w1.Add (this.ToolsAction, null);
			this.ProjectsManagerAction = new global::Gtk.Action ("ProjectsManagerAction", global::Mono.Unix.Catalog.GetString ("Projects Manager"), null, null);
			this.ProjectsManagerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Database Manager");
			w1.Add (this.ProjectsManagerAction, "<Primary>m");
			this.CategoriesTemplatesManagerAction = new global::Gtk.Action ("CategoriesTemplatesManagerAction", global::Mono.Unix.Catalog.GetString ("Analysis Dashboards Manager"), null, null);
			this.CategoriesTemplatesManagerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Dashboards Manager");
			w1.Add (this.CategoriesTemplatesManagerAction, "<Primary>d");
			this.ViewAction = new global::Gtk.Action ("ViewAction", global::Mono.Unix.Catalog.GetString ("_View"), null, null);
			this.ViewAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_View");
			w1.Add (this.ViewAction, "<Control>t");
			this.FullScreenAction = new global::Gtk.ToggleAction ("FullScreenAction", global::Mono.Unix.Catalog.GetString ("Full Screen"), null, "gtk-fullscreen");
			this.FullScreenAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Full Screen");
			w1.Add (this.FullScreenAction, "<Primary>f");
			this.SaveProjectAction = new global::Gtk.Action ("SaveProjectAction", global::Mono.Unix.Catalog.GetString ("_Save Project"), null, "gtk-save");
			this.SaveProjectAction.Sensitive = false;
			this.SaveProjectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Save Project");
			w1.Add (this.SaveProjectAction, null);
			this.HelpAction = new global::Gtk.Action ("HelpAction", global::Mono.Unix.Catalog.GetString ("_Help"), null, null);
			this.HelpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Help");
			w1.Add (this.HelpAction, null);
			this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("_About"), null, "gtk-about");
			this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_About");
			w1.Add (this.AboutAction, null);
			this.ExportProjectToCSVFileAction = new global::Gtk.Action ("ExportProjectToCSVFileAction", global::Mono.Unix.Catalog.GetString ("Export Project To CSV File"), null, null);
			this.ExportProjectToCSVFileAction.Sensitive = false;
			this.ExportProjectToCSVFileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Export Project To CSV File");
			this.ExportProjectToCSVFileAction.Visible = false;
			w1.Add (this.ExportProjectToCSVFileAction, null);
			this.TeamsTemplatesManagerAction = new global::Gtk.Action ("TeamsTemplatesManagerAction", global::Mono.Unix.Catalog.GetString ("Teams Manager"), null, null);
			this.TeamsTemplatesManagerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Teams Manager");
			w1.Add (this.TeamsTemplatesManagerAction, "<Primary>t");
			this.HelpAction1 = new global::Gtk.Action ("HelpAction1", global::Mono.Unix.Catalog.GetString ("_Help"), null, "gtk-help");
			this.HelpAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Help");
			w1.Add (this.HelpAction1, null);
			this.ImportProjectAction = new global::Gtk.Action ("ImportProjectAction", global::Mono.Unix.Catalog.GetString ("_Import Project"), null, "stock-import");
			this.ImportProjectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("_Import Project");
			w1.Add (this.ImportProjectAction, "<Primary>i");
			this.ExportProjectAction = new global::Gtk.Action ("ExportProjectAction", global::Mono.Unix.Catalog.GetString ("Export Project"), null, null);
			this.ExportProjectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Export Project");
			w1.Add (this.ExportProjectAction, null);
			this.ExportProjectAction1 = new global::Gtk.Action ("ExportProjectAction1", global::Mono.Unix.Catalog.GetString ("Export Project"), null, null);
			this.ExportProjectAction1.Sensitive = false;
			this.ExportProjectAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("Export Project");
			w1.Add (this.ExportProjectAction1, null);
			this.Action = new global::Gtk.Action ("Action", null, null, null);
			w1.Add (this.Action, null);
			this.ExportToProjectFileAction = new global::Gtk.Action ("ExportToProjectFileAction", global::Mono.Unix.Catalog.GetString ("Export to project file"), null, null);
			this.ExportToProjectFileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Export to project file");
			w1.Add (this.ExportToProjectFileAction, null);
			this.ImportFromFileAction = new global::Gtk.Action ("ImportFromFileAction", global::Mono.Unix.Catalog.GetString ("Import from file"), null, null);
			this.ImportFromFileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Import from file");
			w1.Add (this.ImportFromFileAction, null);
			this.VideoConverterToolAction = new global::Gtk.Action ("VideoConverterToolAction", global::Mono.Unix.Catalog.GetString ("Video Converter Tool"), null, null);
			this.VideoConverterToolAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Video Converter Tool");
			w1.Add (this.VideoConverterToolAction, null);
			this.DatabasesManagerAction = new global::Gtk.Action ("DatabasesManagerAction", global::Mono.Unix.Catalog.GetString ("Databases Manager"), null, null);
			this.DatabasesManagerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Databases Manager");
			w1.Add (this.DatabasesManagerAction, "<Primary>b");
			this.PreferencesAction = new global::Gtk.Action ("PreferencesAction", global::Mono.Unix.Catalog.GetString ("Preferences"), null, null);
			this.PreferencesAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Preferences");
			w1.Add (this.PreferencesAction, "<Primary>p");
			this.ShowProjectStatsAction = new global::Gtk.Action ("ShowProjectStatsAction", global::Mono.Unix.Catalog.GetString ("Show project stats"), null, null);
			this.ShowProjectStatsAction.Sensitive = false;
			this.ShowProjectStatsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Show project stats");
			w1.Add (this.ShowProjectStatsAction, "<Primary>h");
			this.MigrationToolAction = new global::Gtk.Action ("MigrationToolAction", global::Mono.Unix.Catalog.GetString ("Migration Tool"), null, null);
			this.MigrationToolAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Migration Tool");
			w1.Add (this.MigrationToolAction, null);
			this.UIManager.InsertActionGroup (w1, 0);
			this.AddAccelGroup (this.UIManager.AccelGroup);
			this.Name = "LongoMatch.Gui.MainWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("LongoMatch");
			this.WindowPosition = ((global::Gtk.WindowPosition)(1));
			this.Gravity = ((global::Gdk.Gravity)(5));
			// Container child LongoMatch.Gui.MainWindow.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			// Container child vbox1.Gtk.Box+BoxChild
			this.menubox = new global::Gtk.VBox ();
			this.menubox.Name = "menubox";
			this.menubox.Spacing = 6;
			// Container child menubox.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='NewPojectAction' action='NewPojectAction'/><menuitem name='OpenProjectAction' action='OpenProjectAction'/><menuitem name='SaveProjectAction' action='SaveProjectAction'/><menuitem name='CloseProjectAction' action='CloseProjectAction'/><separator/><menuitem name='ImportProjectAction' action='ImportProjectAction'/><separator/><menuitem name='PreferencesAction' action='PreferencesAction'/><separator/><menuitem name='QuitAction' action='QuitAction'/></menu><menu name='ToolsAction' action='ToolsAction'><menuitem name='ProjectsManagerAction' action='ProjectsManagerAction'/><menuitem name='CategoriesTemplatesManagerAction' action='CategoriesTemplatesManagerAction'/><menuitem name='TeamsTemplatesManagerAction' action='TeamsTemplatesManagerAction'/><menuitem name='DatabasesManagerAction' action='DatabasesManagerAction'/><separator/><menuitem name='ShowProjectStatsAction' action='ShowProjectStatsAction'/><menu name='ExportProjectAction1' action='ExportProjectAction1'><menuitem name='ExportToProjectFileAction' action='ExportToProjectFileAction'/></menu><separator/><menuitem name='VideoConverterToolAction' action='VideoConverterToolAction'/><menuitem name='MigrationToolAction' action='MigrationToolAction'/></menu><menu name='ViewAction' action='ViewAction'><menuitem name='FullScreenAction' action='FullScreenAction'/><separator/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction' action='AboutAction'/><menuitem name='HelpAction1' action='HelpAction1'/></menu></menubar></ui>");
			this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
			this.menubar1.Name = "menubar1";
			this.menubox.Add (this.menubar1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.menubox [this.menubar1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			this.vbox1.Add (this.menubox);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubox]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.centralbox = new global::Gtk.HBox ();
			this.centralbox.Name = "centralbox";
			this.centralbox.Spacing = 6;
			this.vbox1.Add (this.centralbox);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.centralbox]));
			w4.Position = 1;
			// Container child vbox1.Gtk.Box+BoxChild
			this.renderingstatebar1 = new global::LongoMatch.Gui.Component.RenderingStateBar ();
			this.renderingstatebar1.Events = ((global::Gdk.EventMask)(256));
			this.renderingstatebar1.Name = "renderingstatebar1";
			this.renderingstatebar1.Fraction = 0;
			this.vbox1.Add (this.renderingstatebar1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.renderingstatebar1]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 938;
			this.DefaultHeight = 604;
			this.renderingstatebar1.Hide ();
			this.Show ();
			this.AboutAction.Activated += new global::System.EventHandler (this.OnAboutActionActivated);
			this.HelpAction1.Activated += new global::System.EventHandler (this.OnHelpAction1Activated);
			this.VideoConverterToolAction.Activated += new global::System.EventHandler (this.OnVideoConverterToolActionActivated);
			this.MigrationToolAction.Activated += new global::System.EventHandler (this.OnMigrationToolActionActivated);
		}
	}
}
