===================================
Dynamic Source Watcher

ver    : 0.2.0
Author : Tetsunari Sumiyoshi
===================================

■Dynamic Source Watcherとは

  cppの静的解析ツール「Cppcheck」を動的に実行するコンソールアプリケーションです。
  設定ファイルに指定したディレクトリ以下のcppファイルを変更すると、そのファイルに対して静的解析が実行されます。


■開発環境

  - Visual Studio 2019
  - .Net Framework 4.7.2


■実行ファイル

  Release_objectsフォルダにモジュール一式が入っています。
    - DynamicSourceWatcher.exe
	    DSWの実行ファイルです。

	- DynamicSourceWatcher.exe.config
	    DSWの設定ファイルです。

	- Cppcheck_installer
	    Cppcheckのインストーラが入っています。
		最新版が欲しい場合はCppcheckの公式サイトからDLしてください。


■DynamicSourceWatcher.exe.config

  設定ファイルから以下の内容を変更できます

  - workdir
      解析対象のフォルダを指定できます。
	  デフォルトは空なので最初に設定が必要です。
  
  - cppcheck_exe
      Cppcheckのインストール先フォルダを指定します。
	  インストール先が違う場合は変更してください。

  - checklevel
      Cppcheckのenableオプションを指定します。
	  デフォルトはwarningとstyleになっています。

  - format
      Cppcheckのtemplateオプションを指定します。
	  デフォルトはvs（Visual Studio形式）になっています。


■DSWの使い方

  1. Cppcheckをインストールします
      SourceForge(http://cppcheck.sourceforge.net/)またはCppcheck_installer内のインストーラを使用してください。

  2. DynamicSourceWatcher.exe.config の name="workdir" のvalue値に、解析対象のフォルダパスを絶対パスで指定します。

  3. DynamicSourceWatcher.exe を実行するとコンソールが立ち上がって監視が開始されます。


■その他（注意事項）

  ・Visual Studioの外部ツールに組み込めますが、qキーで終了できないです。。。
