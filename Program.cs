using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;


namespace DynamicSourceWatcher
{
    class Watcher
    {
        bool executed;
        String deletedFile;

        static void Main()
        {
            Watcher me = new Watcher();
            me.Run();
        }

        private void Run()
        {
            FileSystemWatcher fsw = new FileSystemWatcher();
            this.executed = false;
            this.deletedFile = "";

            // 監視対象のフォルダが存在しているか
            if (!Directory.Exists(Properties.Settings.Default.workdir))
            {
                Console.WriteLine(Properties.Settings.Default.workdir + " が見つかりませんでした");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("DynamicSourceWatcher.exe.config");
                Console.ResetColor();
                Console.WriteLine("の設定を見直してください。");
                Console.WriteLine("Enterを押してください。");
                while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                return;
            }

            // ファイル監視の設定
            fsw.Path = Properties.Settings.Default.workdir;
            fsw.Filter = "*.cpp";
            fsw.IncludeSubdirectories = true;
            fsw.EnableRaisingEvents = true;
            fsw.Changed += OnChanged;
            fsw.Deleted += OnDeleted;
            fsw.Renamed += OnRenamed;
            fsw.NotifyFilter = NotifyFilters.LastWrite
                               | NotifyFilters.FileName;

            Console.WriteLine("=================================");
            Console.WriteLine("Dynamic Source Watcher");
            Console.WriteLine("詳細はReadme.txtを見てください。");
            Console.WriteLine("");
            Console.WriteLine("監視対象のフォルダ：");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Properties.Settings.Default.workdir);
            Console.ResetColor();
            Console.WriteLine("qで終了します。");
            Console.WriteLine("=================================");

            // qが入力されたら終了する
            while (Console.Read() != 'q') ;
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {

            // Changedメソッドが2回発行されてしまうので1回だけ実行するようにフラグで管理する
            if (this.executed)
            {
                this.executed = false;
                return;
            }

            StartCheck(e.FullPath);

            this.executed = true;
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            this.deletedFile = e.FullPath;
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            // Visual Studio2015ではDelete -> Renameの順でイベントが起きる
            // DeleteとRenameのファイル名が一緒ならばCppcheckを実行する
            if (this.deletedFile != "" && this.deletedFile != e.FullPath)
            {
                this.deletedFile = "";
                return;
            }

            // Visual Studio2019ではTMPファイルへのRename -> cppへのRename が発生する
            // cppファイルへのRename発生時のみCppcheckを実行する
            if (Path.GetExtension(e.Name).ToLower() != ".cpp")
            {
                return;
            }

            StartCheck(e.FullPath);

            this.deletedFile = "";
        }

        private void StartCheck(string fullPath)
        {
            Process proc = new Process();
            String checkLevel = "--enable=" + Properties.Settings.Default.checklevel;
            String format     = "--template=" + Properties.Settings.Default.format;

            // 更新のあったファイルに対してcppcheckを実行する
            Console.ForegroundColor = ConsoleColor.Cyan;
            proc.StartInfo.FileName = Properties.Settings.Default.cppcheck_exe;
            proc.StartInfo.Arguments = checkLevel + " " + format + " " + fullPath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            Console.ResetColor();

            Console.WriteLine("==========End==========");
        }

    }
}
