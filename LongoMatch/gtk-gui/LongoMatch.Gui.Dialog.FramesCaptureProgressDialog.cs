// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace LongoMatch.Gui.Dialog {
    
    
    public partial class FramesCaptureProgressDialog {
        
        private Gtk.VBox vbox2;
        
        private Gtk.ProgressBar progressbar;
        
        private Gtk.Button buttonCancel;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget LongoMatch.Gui.Dialog.FramesCaptureProgressDialog
            this.Name = "LongoMatch.Gui.Dialog.FramesCaptureProgressDialog";
            this.Title = Mono.Unix.Catalog.GetString("Capture Progress");
            this.Icon = Gdk.Pixbuf.LoadFromResource("longomatch_logo.png");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Modal = true;
            this.BorderWidth = ((uint)(3));
            this.Resizable = false;
            this.AllowGrow = false;
            this.Gravity = ((Gdk.Gravity)(5));
            this.SkipPagerHint = true;
            this.SkipTaskbarHint = true;
            this.HasSeparator = false;
            // Internal child LongoMatch.Gui.Dialog.FramesCaptureProgressDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.progressbar = new Gtk.ProgressBar();
            this.progressbar.Name = "progressbar";
            this.vbox2.Add(this.progressbar);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.progressbar]));
            w2.Position = 0;
            w2.Fill = false;
            w1.Add(this.vbox2);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(w1[this.vbox2]));
            w3.Position = 0;
            // Internal child LongoMatch.Gui.Dialog.FramesCaptureProgressDialog.ActionArea
            Gtk.HButtonBox w4 = this.ActionArea;
            w4.Name = "dialog1_ActionArea";
            w4.Spacing = 6;
            w4.BorderWidth = ((uint)(5));
            w4.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w5 = ((Gtk.ButtonBox.ButtonBoxChild)(w4[this.buttonCancel]));
            w5.Expand = false;
            w5.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 108;
            this.Show();
            this.buttonCancel.Clicked += new System.EventHandler(this.OnButtonCancelClicked);
        }
    }
}
