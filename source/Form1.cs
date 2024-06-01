using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace EdgeUninstaller
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // フォームのサイズと固定設定
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 管理者権限のチェック
            if (!IsAdministrator())
            {
                MessageBox.Show("このアプリケーションを実行するには管理者権限が必要です。", "権限エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                uninstallButton.Enabled = false;
            }

            // テキストファイルを開く
            OpenTextFile();
        }

        private void OpenTextFile()
        {
            // .txtファイルのパス
            string textFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agreement.txt");

            // .txtファイルを開く
            if (File.Exists(textFilePath))
            {
                Process.Start(new ProcessStartInfo("notepad.exe", textFilePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("agreement.txtファイルが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void agreeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // 確認チェックボックスとWebView2アンインストールチェックボックスの両方がチェックされている場合のみアンインストールボタンを有効にする
            uninstallButton.Enabled = agreeCheckBox.Checked && webViewCheckBox.Checked;
        }

        private void webViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // 確認チェックボックスとWebView2アンインストールチェックボックスの両方がチェックされている場合のみアンインストールボタンを有効にする
            uninstallButton.Enabled = agreeCheckBox.Checked && webViewCheckBox.Checked;
        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            // Microsoft Edgeのプロセスを終了
            try
            {
                foreach (var process in Process.GetProcessesByName("msedge"))
                {
                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Microsoft Edgeのプロセスを終了中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Microsoft Edgeの関連ファイルを削除
            try
            {
                string[] edgeDirectories = {
                    @"C:\Program Files (x86)\Microsoft\Edge",
                    @"C:\Program Files\Microsoft\Edge",
                    @"C:\Users\" + Environment.UserName + @"\AppData\Local\Microsoft\Edge"
                };

                foreach (var directory in edgeDirectories)
                {
                    if (Directory.Exists(directory))
                    {
                        DeleteDirectory(directory);
                    }
                }

                // Edge修復アプリの削除
                string edgeRepairPath = @"C:\Program Files (x86)\Microsoft\EdgeUpdate";
                if (Directory.Exists(edgeRepairPath))
                {
                    DeleteDirectory(edgeRepairPath);
                }

                edgeRepairPath = @"C:\Program Files\Microsoft\EdgeUpdate";
                if (Directory.Exists(edgeRepairPath))
                {
                    DeleteDirectory(edgeRepairPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Microsoft Edgeのファイルを削除中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // WebView2のアンインストール
            if (webViewCheckBox.Checked)
            {
                try
                {
                    string[] webView2Directories = {
                        @"C:\Program Files (x86)\Microsoft\EdgeWebView",
                        @"C:\Program Files\Microsoft\EdgeWebView"
                    };

                    foreach (var directory in webView2Directories)
                    {
                        if (Directory.Exists(directory))
                        {
                            DeleteDirectory(directory);
                        }
                    }

                    // WebView2のレジストリキーの削除
                    DeleteRegistryKey(@"SOFTWARE\Microsoft\EdgeWebView");
                    DeleteRegistryKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeWebView");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Microsoft Edge WebView2の削除中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // レジストリキーの削除
            try
            {
                DeleteRegistryKey(@"SOFTWARE\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Edge.Application");
                DeleteRegistryKey(@"HKEY_CURRENT_USER\Software\Microsoft\Edge");

                // 設定からMicrosoft Edgeの表示を削除
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge");

                // スタートアップからMicrosoft Edgeを削除
                DeleteStartupEntry("MicrosoftEdge");

                // タスクバーからMicrosoft Edgeのピン止めを外す
                UnpinFromTaskbar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Microsoft Edgeのレジストリキーを削除中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 完了メッセージと再起動ボタンの表示
            this.BackColor = System.Drawing.Color.Red;
            this.completeLabel.Visible = true;
            this.uninstallButton.Visible = false; // アンインストールボタンを非表示
            this.restartButton.Visible = true; // 再起動ボタンを表示
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            // システムを再起動
            Process.Start(new ProcessStartInfo("shutdown", "/r /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }

        private void DeleteDirectory(string targetDir)
        {
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        private void DeleteRegistryKey(string keyPath)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath, true);
                if (key != null)
                {
                    key.DeleteSubKeyTree("");
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"レジストリキー {keyPath} を削除中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteStartupEntry(string appName)
        {
            try
            {
                // スタートアップフォルダから削除
                string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string startupFilePath = Path.Combine(startupFolderPath, $"{appName}.lnk");
                if (File.Exists(startupFilePath))
                {
                    File.Delete(startupFilePath);
                }

                // レジストリのスタートアップキーから削除
                string registryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath, true))
                {
                    if (key != null && key.GetValue(appName) != null)
                    {
                        key.DeleteValue(appName);
                    }
                }

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath, true))
                {
                    if (key != null && key.GetValue(appName) != null)
                    {
                        key.DeleteValue(appName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"スタートアップから{appName}を削除中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnpinFromTaskbar()
        {
            try
            {
                // VBScriptファイルのパス
                string scriptPath = Path.Combine(Path.GetTempPath(), "UnpinEdge.vbs");

                // リソースからVBScriptファイルを作成
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EdgeUninstaller.Resources.UnpinEdge.vbs"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    File.WriteAllText(scriptPath, reader.ReadToEnd());
                }

                // VBScriptを実行
                Process.Start(new ProcessStartInfo("cscript", $"//nologo \"{scriptPath}\"")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスクバーからMicrosoft Edgeのピン止めを外す中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
