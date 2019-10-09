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

        static void Main()
        {
            Watcher me = new Watcher();
            me.Run();
        }

        private void Run()
        {
            FileSystemWatcher fsw = new FileSystemWatcher();
            this.executed = false;

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
            fsw.NotifyFilter = System.IO.NotifyFilters.LastWrite;

            Console.WriteLine("=================================");
            Console.WriteLine("Dynamic Cpp Check");
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
            Process proc = new Process();

            // 
            if (this.executed)
            {
            	this.executed = false;
            	return;
            }

            // 更新のあったファイルに対してcppcheckを実行する
            Console.ForegroundColor = ConsoleColor.Cyan;
            proc.StartInfo.FileName = Properties.Settings.Default.cppcheck_exe;
            proc.StartInfo.Arguments = "--enable=warning,style --template=vs " + e.FullPath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            Console.ResetColor();

            Console.WriteLine("==========End==========");

            this.executed = true;
        }
    }
}
