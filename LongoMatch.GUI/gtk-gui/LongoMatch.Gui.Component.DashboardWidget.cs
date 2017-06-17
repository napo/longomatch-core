
// This file has been generated by the GUI designer. Do not modify.
namespace LongoMatch.Gui.Component
{
	public partial class DashboardWidget
	{
		private global::Gtk.HBox hbox2;

		private global::Gtk.VBox vbox2;

		private global::Gtk.HButtonBox hbuttonbox2;

		private global::Gtk.Button addcatbutton;

		private global::VAS.UI.Helpers.ImageView addcatbuttonimage;

		private global::Gtk.Button addtimerbutton;

		private global::VAS.UI.Helpers.ImageView addtimerbuttonimage;

		private global::Gtk.Button addscorebutton;

		private global::VAS.UI.Helpers.ImageView addscorebuttonimage;

		private global::Gtk.Button addcardbutton;

		private global::VAS.UI.Helpers.ImageView addcardbuttonimage;

		private global::Gtk.Button addtagbutton;

		private global::VAS.UI.Helpers.ImageView addtagbuttonimage;

		private global::Gtk.ScrolledWindow dashscrolledwindow;

		private global::Gtk.DrawingArea drawingarea;

		private global::Gtk.VBox rightbox;

		private global::Gtk.Frame propertiesframe;

		private global::Gtk.Alignment propertiesalignment;

		private global::Gtk.VBox vbox10;

		private global::Gtk.HBox positionsbox;

		private global::Gtk.VBox fieldvbox;

		private global::Gtk.Frame fieldframe;

		private global::Gtk.Alignment fieldalignment;

		private global::Gtk.EventBox fieldeventbox;

		private global::Gtk.VBox vbox12;

		private global::VAS.UI.Helpers.ImageView fieldimage;

		private global::Gtk.Label fieldlabel1;

		private global::Gtk.Label fieldlabel2;

		private global::Gtk.Button resetfieldbutton;

		private global::Gtk.VBox hfieldvbox;

		private global::Gtk.Frame hfieldframe;

		private global::Gtk.Alignment halffieldalignment;

		private global::Gtk.EventBox hfieldeventbox;

		private global::Gtk.VBox vbox14;

		private global::VAS.UI.Helpers.ImageView hfieldimage;

		private global::Gtk.Label hfieldlabel1;

		private global::Gtk.Label hfieldlabel2;

		private global::Gtk.Button resethfieldbutton;

		private global::Gtk.VBox goalvbox;

		private global::Gtk.Frame goalframe;

		private global::Gtk.Alignment goalalignment;

		private global::Gtk.EventBox goaleventbox;

		private global::Gtk.VBox vbox16;

		private global::VAS.UI.Helpers.ImageView goalimage;

		private global::Gtk.Label goallabel1;

		private global::Gtk.Label goallabel2;

		private global::Gtk.Button resetgoalbutton;

		private global::Gtk.HBox periodsbox;

		private global::Gtk.Label periodslabel;

		private global::Gtk.Entry periodsentry;

		private global::Gtk.Button applybutton;

		private global::VAS.UI.Helpers.ImageView applyimage;

		private global::Gtk.ScrolledWindow propertiesscrolledwindow;

		private global::Gtk.Alignment tagpropertiesalignment;

		private global::Gtk.Notebook propertiesnotebook;

		private global::Gtk.Label label2;

		private global::LongoMatch.Gui.Component.CategoryProperties tagproperties;

		private global::Gtk.Label label3;

		private global::LongoMatch.Gui.Component.LinkProperties linkproperties;

		private global::Gtk.Label label5;

		private global::Gtk.VBox vbox4;

		private global::Gtk.ToggleButton editbutton;

		private global::Gtk.HSeparator hseparator1;

		private global::Gtk.ToggleButton linksbutton;

		private global::Gtk.HSeparator hseparator3;

		private global::Gtk.ToggleButton popupbutton;

		private global::Gtk.HSeparator hseparator5;

		private global::Gtk.ToggleButton fitbutton;

		private global::Gtk.ToggleButton fillbutton;

		private global::Gtk.ToggleButton d11button;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget LongoMatch.Gui.Component.DashboardWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "LongoMatch.Gui.Component.DashboardWidget";
			// Container child LongoMatch.Gui.Component.DashboardWidget.Gtk.Container+ContainerChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 12;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.CanFocus = true;
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbuttonbox2 = new global::Gtk.HButtonBox ();
			this.hbuttonbox2.Name = "hbuttonbox2";
			this.hbuttonbox2.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(1));
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.addcatbutton = new global::Gtk.Button ();
			this.addcatbutton.CanFocus = true;
			this.addcatbutton.Name = "addcatbutton";
			// Container child addcatbutton.Gtk.Container+ContainerChild
			this.addcatbuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.addcatbuttonimage.WidthRequest = 0;
			this.addcatbuttonimage.Name = "addcatbuttonimage";
			this.addcatbutton.Add (this.addcatbuttonimage);
			this.hbuttonbox2.Add (this.addcatbutton);
			global::Gtk.ButtonBox.ButtonBoxChild w2 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2 [this.addcatbutton]));
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.addtimerbutton = new global::Gtk.Button ();
			this.addtimerbutton.CanFocus = true;
			this.addtimerbutton.Name = "addtimerbutton";
			// Container child addtimerbutton.Gtk.Container+ContainerChild
			this.addtimerbuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.addtimerbuttonimage.WidthRequest = 0;
			this.addtimerbuttonimage.Name = "addtimerbuttonimage";
			this.addtimerbutton.Add (this.addtimerbuttonimage);
			this.hbuttonbox2.Add (this.addtimerbutton);
			global::Gtk.ButtonBox.ButtonBoxChild w4 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2 [this.addtimerbutton]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.addscorebutton = new global::Gtk.Button ();
			this.addscorebutton.CanFocus = true;
			this.addscorebutton.Name = "addscorebutton";
			// Container child addscorebutton.Gtk.Container+ContainerChild
			this.addscorebuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.addscorebuttonimage.WidthRequest = 0;
			this.addscorebuttonimage.Name = "addscorebuttonimage";
			this.addscorebutton.Add (this.addscorebuttonimage);
			this.hbuttonbox2.Add (this.addscorebutton);
			global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2 [this.addscorebutton]));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.addcardbutton = new global::Gtk.Button ();
			this.addcardbutton.CanFocus = true;
			this.addcardbutton.Name = "addcardbutton";
			// Container child addcardbutton.Gtk.Container+ContainerChild
			this.addcardbuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.addcardbuttonimage.WidthRequest = 0;
			this.addcardbuttonimage.Name = "addcardbuttonimage";
			this.addcardbutton.Add (this.addcardbuttonimage);
			this.hbuttonbox2.Add (this.addcardbutton);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2 [this.addcardbutton]));
			w8.Position = 3;
			w8.Expand = false;
			w8.Fill = false;
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.addtagbutton = new global::Gtk.Button ();
			this.addtagbutton.CanFocus = true;
			this.addtagbutton.Name = "addtagbutton";
			// Container child addtagbutton.Gtk.Container+ContainerChild
			this.addtagbuttonimage = new global::VAS.UI.Helpers.ImageView ();
			this.addtagbuttonimage.WidthRequest = 0;
			this.addtagbuttonimage.Name = "addtagbuttonimage";
			this.addtagbutton.Add (this.addtagbuttonimage);
			this.hbuttonbox2.Add (this.addtagbutton);
			global::Gtk.ButtonBox.ButtonBoxChild w10 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2 [this.addtagbutton]));
			w10.Position = 4;
			w10.Expand = false;
			w10.Fill = false;
			this.vbox2.Add (this.hbuttonbox2);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbuttonbox2]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.dashscrolledwindow = new global::Gtk.ScrolledWindow ();
			this.dashscrolledwindow.CanFocus = true;
			this.dashscrolledwindow.Name = "dashscrolledwindow";
			// Container child dashscrolledwindow.Gtk.Container+ContainerChild
			global::Gtk.Viewport w12 = new global::Gtk.Viewport ();
			w12.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.drawingarea = new global::Gtk.DrawingArea ();
			this.drawingarea.CanFocus = true;
			this.drawingarea.Name = "drawingarea";
			w12.Add (this.drawingarea);
			this.dashscrolledwindow.Add (w12);
			this.vbox2.Add (this.dashscrolledwindow);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.dashscrolledwindow]));
			w15.Position = 1;
			this.hbox2.Add (this.vbox2);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox2]));
			w16.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.rightbox = new global::Gtk.VBox ();
			this.rightbox.Name = "rightbox";
			this.rightbox.Spacing = 6;
			// Container child rightbox.Gtk.Box+BoxChild
			this.propertiesframe = new global::Gtk.Frame ();
			this.propertiesframe.Name = "propertiesframe";
			this.propertiesframe.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child propertiesframe.Gtk.Container+ContainerChild
			this.propertiesalignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.propertiesalignment.Name = "propertiesalignment";
			this.propertiesalignment.BorderWidth = ((uint)(6));
			// Container child propertiesalignment.Gtk.Container+ContainerChild
			this.vbox10 = new global::Gtk.VBox ();
			this.vbox10.Name = "vbox10";
			this.vbox10.Spacing = 6;
			// Container child vbox10.Gtk.Box+BoxChild
			this.positionsbox = new global::Gtk.HBox ();
			this.positionsbox.Name = "positionsbox";
			this.positionsbox.Spacing = 6;
			// Container child positionsbox.Gtk.Box+BoxChild
			this.fieldvbox = new global::Gtk.VBox ();
			this.fieldvbox.Name = "fieldvbox";
			this.fieldvbox.Spacing = 6;
			// Container child fieldvbox.Gtk.Box+BoxChild
			this.fieldframe = new global::Gtk.Frame ();
			this.fieldframe.Name = "fieldframe";
			this.fieldframe.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child fieldframe.Gtk.Container+ContainerChild
			this.fieldalignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.fieldalignment.Name = "fieldalignment";
			this.fieldalignment.LeftPadding = ((uint)(6));
			this.fieldalignment.RightPadding = ((uint)(6));
			// Container child fieldalignment.Gtk.Container+ContainerChild
			this.fieldeventbox = new global::Gtk.EventBox ();
			this.fieldeventbox.Name = "fieldeventbox";
			// Container child fieldeventbox.Gtk.Container+ContainerChild
			this.vbox12 = new global::Gtk.VBox ();
			this.vbox12.Name = "vbox12";
			this.vbox12.Spacing = 2;
			// Container child vbox12.Gtk.Box+BoxChild
			this.fieldimage = new global::VAS.UI.Helpers.ImageView ();
			this.fieldimage.WidthRequest = 0;
			this.fieldimage.HeightRequest = 50;
			this.fieldimage.Name = "fieldimage";
			this.vbox12.Add (this.fieldimage);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox12 [this.fieldimage]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child vbox12.Gtk.Box+BoxChild
			this.fieldlabel1 = new global::Gtk.Label ();
			this.fieldlabel1.Name = "fieldlabel1";
			this.fieldlabel1.LabelProp = global::VAS.Core.Catalog.GetString ("<span font_desc=\"10\">Field</span>");
			this.fieldlabel1.UseMarkup = true;
			this.fieldlabel1.Wrap = true;
			this.fieldlabel1.Justify = ((global::Gtk.Justification)(2));
			this.vbox12.Add (this.fieldlabel1);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox12 [this.fieldlabel1]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child vbox12.Gtk.Box+BoxChild
			this.fieldlabel2 = new global::Gtk.Label ();
			this.fieldlabel2.Name = "fieldlabel2";
			this.fieldlabel2.LabelProp = global::VAS.Core.Catalog.GetString ("<span font_desc=\"8\">click to change...</span>");
			this.fieldlabel2.UseMarkup = true;
			this.vbox12.Add (this.fieldlabel2);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox12 [this.fieldlabel2]));
			w19.Position = 2;
			w19.Expand = false;
			w19.Fill = false;
			this.fieldeventbox.Add (this.vbox12);
			this.fieldalignment.Add (this.fieldeventbox);
			this.fieldframe.Add (this.fieldalignment);
			this.fieldvbox.Add (this.fieldframe);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.fieldvbox [this.fieldframe]));
			w23.Position = 0;
			w23.Expand = false;
			w23.Fill = false;
			// Container child fieldvbox.Gtk.Box+BoxChild
			this.resetfieldbutton = new global::Gtk.Button ();
			this.resetfieldbutton.CanFocus = true;
			this.resetfieldbutton.Name = "resetfieldbutton";
			this.resetfieldbutton.UseStock = true;
			this.resetfieldbutton.UseUnderline = true;
			this.resetfieldbutton.Label = "gtk-refresh";
			this.fieldvbox.Add (this.resetfieldbutton);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.fieldvbox [this.resetfieldbutton]));
			w24.Position = 1;
			w24.Expand = false;
			w24.Fill = false;
			this.positionsbox.Add (this.fieldvbox);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.positionsbox [this.fieldvbox]));
			w25.Position = 0;
			w25.Expand = false;
			w25.Fill = false;
			// Container child positionsbox.Gtk.Box+BoxChild
			this.hfieldvbox = new global::Gtk.VBox ();
			this.hfieldvbox.Name = "hfieldvbox";
			this.hfieldvbox.Spacing = 6;
			// Container child hfieldvbox.Gtk.Box+BoxChild
			this.hfieldframe = new global::Gtk.Frame ();
			this.hfieldframe.Name = "hfieldframe";
			this.hfieldframe.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child hfieldframe.Gtk.Container+ContainerChild
			this.halffieldalignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.halffieldalignment.Name = "halffieldalignment";
			this.halffieldalignment.LeftPadding = ((uint)(6));
			this.halffieldalignment.RightPadding = ((uint)(6));
			// Container child halffieldalignment.Gtk.Container+ContainerChild
			this.hfieldeventbox = new global::Gtk.EventBox ();
			this.hfieldeventbox.Name = "hfieldeventbox";
			// Container child hfieldeventbox.Gtk.Container+ContainerChild
			this.vbox14 = new global::Gtk.VBox ();
			this.vbox14.Name = "vbox14";
			this.vbox14.Spacing = 2;
			// Container child vbox14.Gtk.Box+BoxChild
			this.hfieldimage = new global::VAS.UI.Helpers.ImageView ();
			this.hfieldimage.WidthRequest = 0;
			this.hfieldimage.HeightRequest = 50;
			this.hfieldimage.Name = "hfieldimage";
			this.vbox14.Add (this.hfieldimage);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox14 [this.hfieldimage]));
			w26.Position = 0;
			w26.Expand = false;
			w26.Fill = false;
			// Container child vbox14.Gtk.Box+BoxChild
			this.hfieldlabel1 = new global::Gtk.Label ();
			this.hfieldlabel1.Name = "hfieldlabel1";
			this.hfieldlabel1.LabelProp = global::VAS.Core.Catalog.GetString ("<span font_desc=\"10\">Half field</span>");
			this.hfieldlabel1.UseMarkup = true;
			this.vbox14.Add (this.hfieldlabel1);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox14 [this.hfieldlabel1]));
			w27.Position = 1;
			w27.Expand = false;
			w27.Fill = false;
			// Container child vbox14.Gtk.Box+BoxChild
			this.hfieldlabel2 = new global::Gtk.Label ();
			this.hfieldlabel2.Name = "hfieldlabel2";
			this.hfieldlabel2.LabelProp = global::VAS.Core.Catalog.GetString ("<span font_desc=\"8\">click to change...</span>");
			this.hfieldlabel2.UseMarkup = true;
			this.vbox14.Add (this.hfieldlabel2);
			global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.vbox14 [this.hfieldlabel2]));
			w28.Position = 2;
			w28.Expand = false;
			w28.Fill = false;
			this.hfieldeventbox.Add (this.vbox14);
			this.halffieldalignment.Add (this.hfieldeventbox);
			this.hfieldframe.Add (this.halffieldalignment);
			this.hfieldvbox.Add (this.hfieldframe);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.hfieldvbox [this.hfieldframe]));
			w32.Position = 0;
			w32.Expand = false;
			w32.Fill = false;
			// Container child hfieldvbox.Gtk.Box+BoxChild
			this.resethfieldbutton = new global::Gtk.Button ();
			this.resethfieldbutton.CanFocus = true;
			this.resethfieldbutton.Name = "resethfieldbutton";
			this.resethfieldbutton.UseStock = true;
			this.resethfieldbutton.UseUnderline = true;
			this.resethfieldbutton.Label = "gtk-refresh";
			this.hfieldvbox.Add (this.resethfieldbutton);
			global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hfieldvbox [this.resethfieldbutton]));
			w33.Position = 1;
			w33.Expand = false;
			w33.Fill = false;
			this.positionsbox.Add (this.hfieldvbox);
			global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.positionsbox [this.hfieldvbox]));
			w34.Position = 1;
			w34.Expand = false;
			w34.Fill = false;
			// Container child positionsbox.Gtk.Box+BoxChild
			this.goalvbox = new global::Gtk.VBox ();
			this.goalvbox.Name = "goalvbox";
			this.goalvbox.Spacing = 6;
			// Container child goalvbox.Gtk.Box+BoxChild
			this.goalframe = new global::Gtk.Frame ();
			this.goalframe.Name = "goalframe";
			this.goalframe.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child goalframe.Gtk.Container+ContainerChild
			this.goalalignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.goalalignment.Name = "goalalignment";
			this.goalalignment.LeftPadding = ((uint)(6));
			this.goalalignment.RightPadding = ((uint)(6));
			// Container child goalalignment.Gtk.Container+ContainerChild
			this.goaleventbox = new global::Gtk.EventBox ();
			this.goaleventbox.Name = "goaleventbox";
			// Container child goaleventbox.Gtk.Container+ContainerChild
			this.vbox16 = new global::Gtk.VBox ();
			this.vbox16.Name = "vbox16";
			this.vbox16.Spacing = 2;
			// Container child vbox16.Gtk.Box+BoxChild
			this.goalimage = new global::VAS.UI.Helpers.ImageView ();
			this.goalimage.WidthRequest = 0;
			this.goalimage.HeightRequest = 50;
			this.goalimage.Name = "goalimage";
			this.vbox16.Add (this.goalimage);
			global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.vbox16 [this.goalimage]));
			w35.Position = 0;
			w35.Expand = false;
			w35.Fill = false;
			// Container child vbox16.Gtk.Box+BoxChild
			this.goallabel1 = new global::Gtk.Label ();
			this.goallabel1.Name = "goallabel1";
			this.goallabel1.LabelProp = global::VAS.Core.Catalog.GetString ("<span font_desc=\"10\">Goal</span>");
			this.goallabel1.UseMarkup = true;
			this.vbox16.Add (this.goallabel1);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.vbox16 [this.goallabel1]));
			w36.Position = 1;
			w36.Expand = false;
			w36.Fill = false;
			// Container child vbox16.Gtk.Box+BoxChild
			this.goallabel2 = new global::Gtk.Label ();
			this.goallabel2.Name = "goallabel2";
			this.goallabel2.LabelProp = global::VAS.Core.Catalog.GetString ("<span font_desc=\"8\">click to change...</span>");
			this.goallabel2.UseMarkup = true;
			this.vbox16.Add (this.goallabel2);
			global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.vbox16 [this.goallabel2]));
			w37.Position = 2;
			w37.Expand = false;
			w37.Fill = false;
			this.goaleventbox.Add (this.vbox16);
			this.goalalignment.Add (this.goaleventbox);
			this.goalframe.Add (this.goalalignment);
			this.goalvbox.Add (this.goalframe);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.goalvbox [this.goalframe]));
			w41.Position = 0;
			w41.Expand = false;
			w41.Fill = false;
			// Container child goalvbox.Gtk.Box+BoxChild
			this.resetgoalbutton = new global::Gtk.Button ();
			this.resetgoalbutton.CanFocus = true;
			this.resetgoalbutton.Name = "resetgoalbutton";
			this.resetgoalbutton.UseStock = true;
			this.resetgoalbutton.UseUnderline = true;
			this.resetgoalbutton.Label = "gtk-refresh";
			this.goalvbox.Add (this.resetgoalbutton);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.goalvbox [this.resetgoalbutton]));
			w42.Position = 1;
			w42.Expand = false;
			w42.Fill = false;
			this.positionsbox.Add (this.goalvbox);
			global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.positionsbox [this.goalvbox]));
			w43.Position = 2;
			w43.Expand = false;
			w43.Fill = false;
			this.vbox10.Add (this.positionsbox);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.vbox10 [this.positionsbox]));
			w44.Position = 0;
			w44.Expand = false;
			w44.Fill = false;
			// Container child vbox10.Gtk.Box+BoxChild
			this.periodsbox = new global::Gtk.HBox ();
			this.periodsbox.Name = "periodsbox";
			this.periodsbox.Spacing = 6;
			// Container child periodsbox.Gtk.Box+BoxChild
			this.periodslabel = new global::Gtk.Label ();
			this.periodslabel.Name = "periodslabel";
			this.periodslabel.LabelProp = global::VAS.Core.Catalog.GetString ("Periods");
			this.periodsbox.Add (this.periodslabel);
			global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.periodsbox [this.periodslabel]));
			w45.Position = 0;
			w45.Expand = false;
			w45.Fill = false;
			// Container child periodsbox.Gtk.Box+BoxChild
			this.periodsentry = new global::Gtk.Entry ();
			this.periodsentry.CanFocus = true;
			this.periodsentry.Name = "periodsentry";
			this.periodsentry.IsEditable = true;
			this.periodsentry.InvisibleChar = '•';
			this.periodsbox.Add (this.periodsentry);
			global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.periodsbox [this.periodsentry]));
			w46.Position = 1;
			// Container child periodsbox.Gtk.Box+BoxChild
			this.applybutton = new global::Gtk.Button ();
			this.applybutton.CanFocus = true;
			this.applybutton.Name = "applybutton";
			// Container child applybutton.Gtk.Container+ContainerChild
			this.applyimage = new global::VAS.UI.Helpers.ImageView ();
			this.applyimage.WidthRequest = 0;
			this.applyimage.Name = "applyimage";
			this.applybutton.Add (this.applyimage);
			this.periodsbox.Add (this.applybutton);
			global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.periodsbox [this.applybutton]));
			w48.Position = 2;
			w48.Expand = false;
			w48.Fill = false;
			this.vbox10.Add (this.periodsbox);
			global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.vbox10 [this.periodsbox]));
			w49.Position = 1;
			w49.Expand = false;
			w49.Fill = false;
			// Container child vbox10.Gtk.Box+BoxChild
			this.propertiesscrolledwindow = new global::Gtk.ScrolledWindow ();
			this.propertiesscrolledwindow.CanFocus = true;
			this.propertiesscrolledwindow.Name = "propertiesscrolledwindow";
			this.propertiesscrolledwindow.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.propertiesscrolledwindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child propertiesscrolledwindow.Gtk.Container+ContainerChild
			global::Gtk.Viewport w50 = new global::Gtk.Viewport ();
			w50.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport1.Gtk.Container+ContainerChild
			this.tagpropertiesalignment = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.tagpropertiesalignment.Name = "tagpropertiesalignment";
			this.tagpropertiesalignment.BorderWidth = ((uint)(6));
			// Container child tagpropertiesalignment.Gtk.Container+ContainerChild
			this.propertiesnotebook = new global::Gtk.Notebook ();
			this.propertiesnotebook.CanFocus = true;
			this.propertiesnotebook.Name = "propertiesnotebook";
			this.propertiesnotebook.CurrentPage = 2;
			this.propertiesnotebook.ShowBorder = false;
			this.propertiesnotebook.ShowTabs = false;
			// Notebook tab
			global::Gtk.Label w51 = new global::Gtk.Label ();
			w51.Visible = true;
			this.propertiesnotebook.Add (w51);
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::VAS.Core.Catalog.GetString ("page1");
			this.propertiesnotebook.SetTabLabel (w51, this.label2);
			this.label2.ShowAll ();
			// Container child propertiesnotebook.Gtk.Notebook+NotebookChild
			this.tagproperties = new global::LongoMatch.Gui.Component.CategoryProperties ();
			this.tagproperties.Events = ((global::Gdk.EventMask)(256));
			this.tagproperties.Name = "tagproperties";
			this.tagproperties.Edited = false;
			this.propertiesnotebook.Add (this.tagproperties);
			global::Gtk.Notebook.NotebookChild w52 = ((global::Gtk.Notebook.NotebookChild)(this.propertiesnotebook [this.tagproperties]));
			w52.Position = 1;
			// Notebook tab
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::VAS.Core.Catalog.GetString ("page2");
			this.propertiesnotebook.SetTabLabel (this.tagproperties, this.label3);
			this.label3.ShowAll ();
			// Container child propertiesnotebook.Gtk.Notebook+NotebookChild
			this.linkproperties = new global::LongoMatch.Gui.Component.LinkProperties ();
			this.linkproperties.Events = ((global::Gdk.EventMask)(256));
			this.linkproperties.Name = "linkproperties";
			this.linkproperties.Edited = false;
			this.propertiesnotebook.Add (this.linkproperties);
			global::Gtk.Notebook.NotebookChild w53 = ((global::Gtk.Notebook.NotebookChild)(this.propertiesnotebook [this.linkproperties]));
			w53.Position = 2;
			// Notebook tab
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.LabelProp = global::VAS.Core.Catalog.GetString ("page3");
			this.propertiesnotebook.SetTabLabel (this.linkproperties, this.label5);
			this.label5.ShowAll ();
			this.tagpropertiesalignment.Add (this.propertiesnotebook);
			w50.Add (this.tagpropertiesalignment);
			this.propertiesscrolledwindow.Add (w50);
			this.vbox10.Add (this.propertiesscrolledwindow);
			global::Gtk.Box.BoxChild w57 = ((global::Gtk.Box.BoxChild)(this.vbox10 [this.propertiesscrolledwindow]));
			w57.Position = 2;
			this.propertiesalignment.Add (this.vbox10);
			this.propertiesframe.Add (this.propertiesalignment);
			this.rightbox.Add (this.propertiesframe);
			global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.rightbox [this.propertiesframe]));
			w60.Position = 0;
			this.hbox2.Add (this.rightbox);
			global::Gtk.Box.BoxChild w61 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.rightbox]));
			w61.Position = 1;
			w61.Expand = false;
			w61.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.editbutton = new global::Gtk.ToggleButton ();
			this.editbutton.CanFocus = true;
			this.editbutton.Name = "editbutton";
			this.editbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.vbox4.Add (this.editbutton);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.editbutton]));
			w62.Position = 0;
			w62.Expand = false;
			w62.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox4.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w63 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hseparator1]));
			w63.Position = 1;
			w63.Expand = false;
			w63.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.linksbutton = new global::Gtk.ToggleButton ();
			this.linksbutton.CanFocus = true;
			this.linksbutton.Name = "linksbutton";
			this.linksbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.vbox4.Add (this.linksbutton);
			global::Gtk.Box.BoxChild w64 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.linksbutton]));
			w64.Position = 2;
			w64.Expand = false;
			w64.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.hseparator3 = new global::Gtk.HSeparator ();
			this.hseparator3.Name = "hseparator3";
			this.vbox4.Add (this.hseparator3);
			global::Gtk.Box.BoxChild w65 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hseparator3]));
			w65.Position = 3;
			w65.Expand = false;
			w65.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.popupbutton = new global::Gtk.ToggleButton ();
			this.popupbutton.CanFocus = true;
			this.popupbutton.Name = "popupbutton";
			this.popupbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.vbox4.Add (this.popupbutton);
			global::Gtk.Box.BoxChild w66 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.popupbutton]));
			w66.Position = 4;
			w66.Expand = false;
			w66.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.hseparator5 = new global::Gtk.HSeparator ();
			this.hseparator5.Name = "hseparator5";
			this.vbox4.Add (this.hseparator5);
			global::Gtk.Box.BoxChild w67 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hseparator5]));
			w67.Position = 5;
			w67.Expand = false;
			w67.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.fitbutton = new global::Gtk.ToggleButton ();
			this.fitbutton.CanFocus = true;
			this.fitbutton.Name = "fitbutton";
			this.fitbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.vbox4.Add (this.fitbutton);
			global::Gtk.Box.BoxChild w68 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.fitbutton]));
			w68.Position = 6;
			w68.Expand = false;
			w68.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.fillbutton = new global::Gtk.ToggleButton ();
			this.fillbutton.CanFocus = true;
			this.fillbutton.Name = "fillbutton";
			this.fillbutton.Relief = ((global::Gtk.ReliefStyle)(2));
			this.vbox4.Add (this.fillbutton);
			global::Gtk.Box.BoxChild w69 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.fillbutton]));
			w69.Position = 7;
			w69.Expand = false;
			w69.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.d11button = new global::Gtk.ToggleButton ();
			this.d11button.CanFocus = true;
			this.d11button.Name = "d11button";
			this.d11button.Relief = ((global::Gtk.ReliefStyle)(2));
			this.vbox4.Add (this.d11button);
			global::Gtk.Box.BoxChild w70 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.d11button]));
			w70.Position = 8;
			w70.Expand = false;
			w70.Fill = false;
			this.hbox2.Add (this.vbox4);
			global::Gtk.Box.BoxChild w71 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox4]));
			w71.Position = 2;
			w71.Expand = false;
			this.Add (this.hbox2);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
		}
	}
}
