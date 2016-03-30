using System;

namespace HandyComicNamer
{
    public class StyleDialog: Gtk.Dialog
    {
        Gtk.Entry entry;

        public static string style = ":o";
        new public static string Style {
            get { return style; }
            set { style = value; }
        }

        public StyleDialog (string title, Gtk.Window parent,
            Gtk.DialogFlags flags, params object[] button_data)
                : base (title, parent, flags, button_data)
        {
            this.BorderWidth = 4;
            this.SetDefaultSize (250, -1);
            this.SetPosition (Gtk.WindowPosition.Center);
            this.Destroyed += OnClosed;

            Gtk.VBox vbox = new Gtk.VBox (false, 0);
            Gtk.Alignment align = new Gtk.Alignment (0, 0, 0, 0);
            vbox.Add (align);

            Gtk.Label lblExplain = new Gtk.Label
                (@"
:o - 원본 파일 이름
:d - 상위 디렉터리 이름
:1v - 권 번호를 1개의 자릿수로 출력
:2:5v - 권 번호를 2개의 자릿수로 5부터 출력
:3i - 인덱스를 3개의 자릿수로 출력
:4:20i - 인덱스를 4개의 자릿수로 20부터 출력
");
            vbox.PackStart (lblExplain, false, false, 0);

            this.entry = new Gtk.Entry ();
            vbox.PackStart (entry, false, false, 0);
            vbox.ShowAll ();
            this.VBox.Add (vbox);
        }

        void OnClosed (object sender, EventArgs args)
        {
            if (IsValid (this.entry.Text)) {
                Console.WriteLine ("Valid");
                StyleDialog.Style = this.entry.Text;
            } else {
                Console.WriteLine ("Not valid");
                StyleDialog.Style = ":o";
            }
        }

        public static bool IsValid (string expr)
        {
            if (expr=="")
                return false;

            char[] arr = expr.ToCharArray ();
            for (int i=0; i<arr.Length; ++i) {
                if (arr[i]==':')
                {
                    ++i;
                    if (arr.Length<=i)
                        return false;

                    char ch = arr[i];

                    if (ch=='o' || ch=='O')
                    {
                        ++i;
                    }
                    else if (ch=='d' || ch=='D')
                    {
                        ++i;
                    }
                    else if (ch=='n' || ch=='N')
                    {
                        ++i;
                    }
                    else if ('0'<=ch || ch<='9')
                    {
                        while (i < arr.Length
                               && '0'<=arr[i] && arr[i]<='9')
                            ++i;

                        if (arr.Length<=i)
                            return false;
                        else if (arr[i]=='i' || arr[i]=='I')
                            ++i;
                        else if (arr[i]=='v' || arr[i]=='V')
                            ++i;
                        else if (arr[i]==':')
                        {
                            ++i;
                            while (i < arr.Length
                                   && '0'<=arr[i] && arr[i]<='9')
                                ++i;

                            if (arr.Length<=i)
                                return false;
                            else if (arr[i]=='i' || arr[i]=='I')
                                ++i;
                            else if (arr[i]=='v' || arr[i]=='V')
                                ++i;
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}