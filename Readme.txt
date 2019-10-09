===================================
Dynamic Source Checker

ver    : 0.1.0
Author : Tetsunari Sumiyoshi
===================================

■Dynamic Source Checkerとは

  cppの静的チェックツール「Cppcheck」を動的に実行するコンソールアプリケーションです。
  設定ファイルに指定したディレクトリ以下のcppファイルを変更すると、そのファイルに対して静的チェックが実行されます。
  現在はVisual Studioでのビルドを想定した設定になっています。（後に他のにも対応する予定です）

  検出できるチェック項目は、以下になります（括弧内はCppcheckでのチェック項目）
	・エラーとなるコード（error）
	・バグの原因となりうるコード（warning）
	・綺麗でないコードの書き方（style）


■開発環境

  ・Visual Studio 2019
  ・.Net Framework 4.7.2


■使い方

  1. Cppcheckをインストールします
      公式サイトまたはCppcheck_installer内のインストーラを使用してください

  2. DynamicSourceWatcher.exe.config の name="work" のvalue値に、チェック対象のフォルダパスを絶対パスで指定します。

  3. DynamicSourceWatcher.exe を実行するとコンソールが立ち上がって監視が開始されます。


■その他（注意事項）

  ・Visual Studio上で修正しても検知できない場合があります。。。（Visual Studio 2019 はダメそうです）

  ・Visual Studioの外部ツールに組み込めますが、qキーで終了できないです。。。
