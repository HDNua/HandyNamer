

using System;
using System.IO;
using System.Collections.Generic;

namespace HandyComicNamer
{
    public partial class App: Gtk.Window
    {
        #region constants
        const string s_file = "파일";
        const string s_clear = "목록 비우기";
        
        const string s_reorder = "정렬";
        const string s_apply = "적용";
		const string s_remFileOfExt = "지정한 확장자를 갖는 파일 제외";
		const string s_remFileNotOfExt = "지정한 확장자 외의 것을 갖는 파일 제외";

        const string s_quit = "닫기";
        
		const string s_setting = "설정";
        const string s_style = "스타일";
		const string s_developer = "개발자";
        
        const string sd_accept = "확인";
        const string sd_cancel = "취소";
        const string sd_continue = "진행";
        
        const string sd_folder = "폴더";
        const string sd_file = "파일";
        
        const string Old_Name = "이전 이름";
        const string New_Name = "새 이름";
        
        enum SortType
        {
            Ascending,
            Descending,
            Unsorted
        }
        enum TargetType
        {
            File
        }
        
#endregion
        
        List<FileInfo> filelist;
        Gtk.ListStore treestore;
        Gtk.TreeView tview;
        
        Gtk.AccelGroup ag;
        Gtk.TargetEntry[] entry;
        
        public App (): base("Handy Comic Namer")
        {
            int appWidth, appHeight;
            SetWidthHeight (out appWidth, out appHeight);
            this.SetDefaultSize (appWidth, appHeight);
            this.SetPosition (Gtk.WindowPosition.Center);
            this.DeleteEvent += OnDeleteEvent;
            //            this.SetIconFromFile ("Icon.png");
            
            this.ag = new Gtk.AccelGroup ();
            this.AddAccelGroup (this.ag);
            
            this.entry = new Gtk.TargetEntry[] {
                new Gtk.TargetEntry
                ("text/uri-list", Gtk.TargetFlags.OtherApp,
                 (uint)TargetType.File)
            };
            
            this.filelist = new List<FileInfo> ();
            this.treestore = new Gtk.ListStore
                (typeof (string), typeof (string));
            this.treestore.SortColumnChanged
                += new EventHandler
                    (OnSortColumnChanged);
            this.tview = CreateTree (this.treestore, this.entry);
            Gtk.MenuBar bar = CreateMenu ();
            
            Gtk.ScrolledWindow swnd = new Gtk.ScrolledWindow ();
            swnd.SetPolicy 
                (Gtk.PolicyType.Automatic, Gtk.PolicyType.Automatic);
            swnd.Add (this.tview);
            
            Gtk.VBox vbox = new Gtk.VBox ();
            vbox.PackStart (bar, false, false, 0);
            vbox.PackStart (swnd, true, true, 0);
            
            this.Add (vbox);
            this.ShowAll ();
        }
        
        #region methods
        void SetWidthHeight (out int width, out int height)
        {
            int scrWidth = Screen.Width;
            int scrHeight = Screen.Height;
            
            int appWidth = 0;
            int appHeight = 0;
            
            if (scrWidth == 1920) {
                appWidth = 1024;
                appHeight = 768;
            } else if (scrWidth == 1280) {
                appWidth = 800;
                appHeight = 600;
            } else if (scrWidth == 1024) {
                appWidth = 640;
                appHeight = 480;
            } else if (scrWidth == 800) {
                appWidth = 480;
                appHeight = 360;
            } else {
                appWidth = scrWidth;
                appHeight = scrHeight;
            }
            
            width = appWidth;
            height = appHeight;
        }
        Gtk.MenuBar CreateMenu ()
        {
            Gtk.MenuBar bar = new Gtk.MenuBar ();
            Gtk.MenuItem item;
            Gtk.Menu menu;
            
            item = new Gtk.MenuItem (s_file);
            menu = new Gtk.Menu ();
            item.Submenu = menu;
            bar.Append (item);
            
            item = new Gtk.MenuItem (s_clear);
            item.Activated += OnClearAll;
            item.AddAccelerator
                ("activate", this.ag,
                 new Gtk.AccelKey (Gdk.Key.c, Gdk.ModifierType.ControlMask,
                                  Gtk.AccelFlags.Visible));
            menu.Append (item);
            
            item = new Gtk.MenuItem (s_reorder);
            item.Activated += OnSort;
            item.AddAccelerator
                ("activate", this.ag,
                 new Gtk.AccelKey (Gdk.Key.r, Gdk.ModifierType.ControlMask,
                                  Gtk.AccelFlags.Visible));
            menu.Append (item);
            
            item = new Gtk.MenuItem (s_apply);
            item.Activated += OnApply;
            item.AddAccelerator
                ("activate", this.ag,
                 new Gtk.AccelKey (Gdk.Key.a, Gdk.ModifierType.ControlMask, 
                                  Gtk.AccelFlags.Visible));
            menu.Append (item);

			item = new Gtk.MenuItem (s_remFileOfExt);
			item.Activated += OnRemFileOfExt;
			item.AddAccelerator
				("activate", this.ag,
				 new Gtk.AccelKey (Gdk.Key.e, Gdk.ModifierType.ControlMask,
				                  Gtk.AccelFlags.Visible));
			menu.Append (item);

			item = new Gtk.MenuItem (s_remFileNotOfExt);
			item.Activated += OnRemFileNotOfExt;
			item.AddAccelerator
				("activate", this.ag,
				 new Gtk.AccelKey (Gdk.Key.w, Gdk.ModifierType.ControlMask,
				                  Gtk.AccelFlags.Visible));
			menu.Append (item);
            
            item = new Gtk.SeparatorMenuItem ();
            menu.Append (item);
            
            item = new Gtk.MenuItem (s_quit);
            item.Activated += OnDeleteEvent;
            item.AddAccelerator
                ("activate", this.ag,
                 new Gtk.AccelKey (Gdk.Key.q, Gdk.ModifierType.ControlMask,
                                  Gtk.AccelFlags.Visible));
            menu.Append (item);
            
            item = new Gtk.MenuItem (s_setting);
            menu = new Gtk.Menu ();
            item.Submenu = menu;
            bar.Append (item);
            
            item = new Gtk.MenuItem (s_style);
            item.Activated += OnSetStyle;
            item.AddAccelerator
                ("activate", this.ag,
                 new Gtk.AccelKey (Gdk.Key.s, Gdk.ModifierType.ControlMask,
                                  Gtk.AccelFlags.Visible));
            menu.Append (item);
            
            item = new Gtk.MenuItem (s_developer);
            item.Activated += OnShowDeveloper;
            menu.Append (item);
            
            return bar;
        }
        Gtk.TreeView CreateTree 
            (Gtk.ListStore store, Gtk.TargetEntry[] entry)
        {
            Gtk.TreeView tview = new Gtk.TreeView (store);
            tview.RulesHint = true;
            
            tview.DragDataReceived
                += new Gtk.DragDataReceivedHandler
                    (OnTreeDragDataReceived);
            
            Gtk.TreeViewColumn column;
            
            column = CreateColumn (Old_Name, 0);
            column.Clickable = false;
            tview.AppendColumn (column);
            
            column = CreateColumn (New_Name, 1);
            column.Clickable = false;
            tview.AppendColumn (column);
            
            Gtk.Drag.DestSet
                (tview, Gtk.DestDefaults.All, entry, Gdk.DragAction.Copy);
            
            return tview;
        }
        Gtk.TreeViewColumn CreateColumn
            (string title, int columnid)
        {
            Gtk.CellRendererText rtext;
            Gtk.TreeViewColumn column;
            
            rtext = new Gtk.CellRendererText ();
            column = new Gtk.TreeViewColumn
                (title, rtext, "text", columnid);
            column.SortColumnId = columnid;
            column.Resizable = true;
            
            return column;
        }
#endregion
        
        #region event handlers
        // general
        void OnDeleteEvent (object sender, EventArgs args)
        {
            Gtk.Application.Quit ();
        }
        void OnClearAll (object sender, EventArgs args)
        {
            this.index = 0;
            this.volume = 0;
            this.bpath = "";
            this.sort_order = SortType.Unsorted;
            
            this.filelist.Clear ();
            this.treestore.Clear ();
        }
        void OnSort (object sender, EventArgs args)
        {
            Console.WriteLine ("Sort");
            
            switch (this.sort_order) {
            case SortType.Descending:
                this.filelist.Sort (FileNameCompareAscending);
                this.treestore.SetSortColumnId (0, Gtk.SortType.Ascending);
                this.sort_order = SortType.Ascending;
                break;
                
            case SortType.Ascending:
            case SortType.Unsorted:
            default:
                this.filelist.Sort (FileNameCompareDescending);
                this.treestore.SetSortColumnId (0, Gtk.SortType.Descending);
                this.sort_order = SortType.Descending;
                break;
            }
        }
        
        void OnApply (object sender, EventArgs args)
        {
            Gtk.Dialog dialog = new Gtk.Dialog
                ("이름 변경 적용", this, Gtk.DialogFlags.Modal,
                 sd_cancel, Gtk.ResponseType.Cancel,
                 sd_continue, Gtk.ResponseType.Accept);
            
            Gtk.VBox vbox = new Gtk.VBox ();
            vbox.PackStart 
                (new Gtk.Label ("계속 진행하시겠습니까?"), true, true, 0);
            vbox.ShowAll ();
            dialog.VBox.Add (vbox);
            
            if ((Gtk.ResponseType)dialog.Run ()
                == Gtk.ResponseType.Accept) {
                if (this.filelist.Count>0)
                {
                    List<string> newnames = new List<string> ();
                    foreach (object[] arr in this.treestore)
                        newnames.Add ((string) arr[1]);
                    
                    int store_index = 0;
                    foreach (FileInfo info in this.filelist)
                    {
                        string path = info.DirectoryName;
                        string newpath = path + '/' + newnames[store_index];
                        
                        try {
                            info.MoveTo (newpath);
                            ++store_index;
                        } catch /*(Exception e)*/ {
                            //                            Console.WriteLine ("Error: {0}", e);
                            //                            Console.WriteLine ("Message: {0}", e.Message);
                            return;
                        }
                    }
                    
                    StyleDialog.Style = ":o";
                    this.SyncFileInfoWithTree
                        (this.filelist, this.treestore);
                }
                //                Console.WriteLine ("Complete");
            } else {
                //                Console.WriteLine ("Cancelled");
            }
            dialog.Destroy ();
        }

		void OnRemFileOfExt (object sender, EventArgs args)
		{
			Gtk.Dialog dialog = new Gtk.Dialog
				("제외할 확장자 입력", this, Gtk.DialogFlags.Modal,
				 sd_cancel, Gtk.ResponseType.Cancel,
				 sd_continue, Gtk.ResponseType.Accept);
			Gtk.VBox vbox = new Gtk.VBox ();
			Gtk.Entry entry = new Gtk.Entry ();
			vbox.PackStart (entry);
			vbox.ShowAll ();
			dialog.VBox.Add (vbox);

			if ((Gtk.ResponseType)dialog.Run ()
				== Gtk.ResponseType.Accept) {
				string ext = entry.Text;
				if (ext[0] != '.')
					ext = "." + ext;

				for (int i=0; i<this.filelist.Count; ++i)
					if (this.filelist[i].Extension == ext)
						this.filelist.Remove (this.filelist[i--]);
			}
			dialog.Destroy ();

			this.SyncFileInfoWithTree
				(this.filelist, this.treestore);
		}
		void OnRemFileNotOfExt (object sender, EventArgs args)
		{
			Gtk.Dialog dialog = new Gtk.Dialog
				("제외하지 않을 확장자 입력", this, Gtk.DialogFlags.Modal,
				 sd_cancel, Gtk.ResponseType.Cancel,
				 sd_continue, Gtk.ResponseType.Accept);
			Gtk.VBox vbox = new Gtk.VBox ();
			Gtk.Entry entry = new Gtk.Entry ();
			vbox.PackStart (entry);
			vbox.ShowAll ();
			dialog.VBox.Add (vbox);
			
			if ((Gtk.ResponseType)dialog.Run ()
			    == Gtk.ResponseType.Accept) {
				string ext = entry.Text;
				if (ext[0] != '.')
					ext = "." + ext;

				for (int i=0; i<this.filelist.Count; ++i)
					if (this.filelist[i].Extension != ext)
						this.filelist.Remove (this.filelist[i--]);
			}
			dialog.Destroy ();
			
			this.SyncFileInfoWithTree
				(this.filelist, this.treestore);
		}

        void OnSetStyle (object sender, EventArgs args)
        {
            //            Console.WriteLine ("Style setting");
            
            const string Cancel = "취소";
            const string Accept = "선택";
            StyleDialog style = new StyleDialog
                ("스타일 선택", this, Gtk.DialogFlags.Modal,
                 Cancel, Gtk.ResponseType.Cancel,
                 Accept, Gtk.ResponseType.Accept);
            
            style.Run ();
            style.Destroy ();
            
            if (StyleDialog.IsValid (StyleDialog.Style)) {
                this.index = 0;
                this.volume = 0;
                this.SyncFileInfoWithTree
                    (this.filelist, this.treestore);
            }
        }
        void OnShowDeveloper (object sender, EventArgs args)
        {
            Gtk.AboutDialog about = new Gtk.AboutDialog ();
            
            about.ProgramName = "Handy Namer";
            about.Version = "0.1";
            about.Copyright = "한 도영 (aka HDNua, C. 탄소)";
//            about.Logo = new Gdk.Pixbuf ("Handy.png");
            about.Comments = @"Handy namer는 파일의 이름을
원하는 대로 정렬하도록 돕는 프로그램입니다.
라이센스 정보를 보시려면 readme.txt 파일을 참조하시거나
아래의 링크로 이동해주세요.";
            about.Website = "http://blog.naver.com/rbfwmqwntm";
            about.SetPosition (Gtk.WindowPosition.Mouse);
            
            about.Run ();
            about.Destroy ();
        }
        
        SortType sort_order = SortType.Unsorted;
        void OnSortColumnChanged (object sender, EventArgs args)
        {
            if (this.sort_order != SortType.Ascending) {
                this.filelist.Sort (FileNameCompareAscending);
                this.sort_order = SortType.Ascending;
            } else {
                this.filelist.Sort (FileNameCompareDescending);
                this.sort_order = SortType.Descending;
            }
        }
        
        // drop
        void OnTreeDragDataReceived
            (object sender, Gtk.DragDataReceivedArgs args)
        {
            if (args.SelectionData.Length >= 0
                && args.SelectionData.Format == 8) {
                
                string[] paths = Convert.ToConvertedPathArray 
                    (args.SelectionData.Data);
                
                // 1. check whether selected data
                //  contains information about directory
                if (this.DirectoryIncluded (paths))
                {
                    //                    Console.WriteLine ("Directory included");
                    
                    Gtk.Dialog dialog = new Gtk.Dialog
                        ("확인", this, Gtk.DialogFlags.Modal,
                         sd_folder, Gtk.ResponseType.Cancel,
                         sd_file, Gtk.ResponseType.Accept);
                    
                    Gtk.VBox vbox = new Gtk.VBox (false, 4);
                    Gtk.HBox hbox = new Gtk.HBox (false, 4);
                    vbox.PackStart
                        (new Gtk.Label (@"
폴더 자체에 대해 이름을 변경하려면 폴더,
폴더 내의 파일들에 대해 변경하려면 파일
"), false, false, 0);
                    hbox.PackStart (vbox);
                    hbox.ShowAll ();
                    dialog.VBox.Add (hbox);
                    
                    Gtk.ResponseType rsp
                        = (Gtk.ResponseType)dialog.Run ();
                    if (rsp == Gtk.ResponseType.Accept)
                    {
                        List<string> dlist = new List<string> ();
                        List<string> flist = new List<string> ();
                        
                        foreach (string path in paths)
                            if (Directory.Exists (path))
                                dlist.Add (path);
                        else
                            flist.Add (path);
                        
                        foreach (string dir in dlist)
                        {
                            string[] dfiles = Directory.GetFiles (dir);
                            AddPathNotDup (dfiles);
                        }
                        AddPathNotDup (flist.ToArray ());
                    }
                    else if (rsp == Gtk.ResponseType.Cancel)
                    {
                        AddPathNotDup (paths);
                    }
                    dialog.Destroy ();
                }
                else
                {
                    AddPathNotDup (paths);
                }
                
                Gtk.Drag.Finish (args.Context, true, false, args.Time);
            }
            Gtk.Drag.Finish (args.Context, false, false, args.Time);
        }
#endregion
        
        #region event handler methods
        bool DirectoryIncluded (string[] paths)
        {
            foreach (string path in paths)
                if (Directory.Exists (path))
                    return true;
            return false;
        }
        
        int FileNameCompareAscending (FileInfo info1, FileInfo info2)
        {
            return string.Compare (info1.FullName, info2.FullName);
        }
        int FileNameCompareDescending (FileInfo info1, FileInfo info2)
        {
            return string.Compare (info2.FullName, info1.FullName);
        }
        
        void AddPathNotDup (string[] paths)
        {
            if (this.filelist.Count>0) {
                foreach (string path in paths)
                {
                    bool exists = false;
                    foreach (FileInfo info in this.filelist)
                    {
                        if (IsPathDuplicated (info, path))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        this.filelist.Add (new FileInfo (path));
                }
            } else {
                foreach (string path in paths)
                    this.filelist.Add (new FileInfo (path));
            }
            this.SyncFileInfoWithTree
                (this.filelist, this.treestore);
        }
        bool IsPathDuplicated (FileInfo info, string path)
        {
            if (path == info.FullName)
                return true;
            else if (path == info.FullName.Replace ("\\", "/"))
                return true;
            else
                return false;
        }
        void SyncFileInfoWithTree
            (List<FileInfo> flist, Gtk.ListStore ts)
        {
            ts.Clear ();
            foreach (FileInfo info in flist) {
                string style_applied = StyleAppliedString
                    (info, StyleDialog.Style, "Test");
                ts.AppendValues (info.Name, style_applied);
            }
        }
        
        int volume = 0;
        int index = 0;
        string bpath = "";
        string StyleAppliedString 
            (FileInfo info, string style, string name)
        {
            string result = "";
            string origin = (string)info.Name.Clone ();
            string ext = info.Extension;
            if (ext.Length > 0)
                origin = origin.Remove (origin.Length - ext.Length);
            
            string path = info.DirectoryName;
            path = path.Replace ("\\", "/");
            if (bpath != path) {
                ++volume;
                index = 0;
                bpath = path;
            }
            
            string[] dirs = path.Split ('/');
            string dir = dirs[dirs.Length-1];
            
            for (int i=0; i<style.Length; ++i) {
                if (style[i]==':')
                {
                    ++i;
                    char ch = style[i];
                    
                    if (ch=='o' || ch=='O')
                        result += origin;
                    else if (ch=='d' || ch=='D')
                        result += dir;
                    else if (ch=='n' || ch=='N')
                        result += name;
                    else if ('0'<=ch && ch <='9')
                    {
                        int len = 0;
                        while (i < style.Length
                               && '0'<=style[i] && style[i]<='9')
                        {
                            len *= 10;
                            len += style[i] - '0';
                            ++i;
                        }
                        
                        ch = style[i];
                        if (ch=='i' || ch=='I')
                        {
                            string format = "{0:D" + len + "}";
                            format = string.Format (format, ++index);
                            result += format;
                        }
                        else if (ch=='v' || ch=='V')
                        {
                            string format = "{0:D" + len + "}";
                            format = string.Format (format, volume);
                            result += format;
                        }
                        else if (ch==':')
                        {
                            int start = 0;
                            ++i;
                            while (i < style.Length
                                   && '0'<=style[i] && style[i]<='9')
                            {
                                start *= 10;
                                start += style[i] - '0';
                                ++i;
                            }
                            --start;
                            
                            ch = style[i];
                            if (ch=='i' || ch=='I')
                            {
                                string format = "{0:D" + len + "}";
                                format = string.Format
                                    (format, start + ++index);
                                result += format;
                            }
                            else if (ch=='v' || ch=='V')
                            {
                                string format = "{0:D" + len + "}";
                                format = string.Format (format, volume);
                                result += format;
                            }
                        }
                    }
                }
                else
                {
                    result += style[i];
                }
            }
            
            result += ext.ToLower ();
            //            Console.WriteLine ("Result: {0}", result);
            return result;
        }
#endregion
        
        public static void Main(string[] args)
        {
            Gtk.Application.Init ();
            new App ();
            Gtk.Application.Run ();
        }
    }
}