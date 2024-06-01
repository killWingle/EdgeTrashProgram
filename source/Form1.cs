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

            // �t�H�[���̃T�C�Y�ƌŒ�ݒ�
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // �Ǘ��Ҍ����̃`�F�b�N
            if (!IsAdministrator())
            {
                MessageBox.Show("���̃A�v���P�[�V���������s����ɂ͊Ǘ��Ҍ������K�v�ł��B", "�����G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                uninstallButton.Enabled = false;
            }

            // �e�L�X�g�t�@�C�����J��
            OpenTextFile();
        }

        private void OpenTextFile()
        {
            // .txt�t�@�C���̃p�X
            string textFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agreement.txt");

            // .txt�t�@�C�����J��
            if (File.Exists(textFilePath))
            {
                Process.Start(new ProcessStartInfo("notepad.exe", textFilePath) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("agreement.txt�t�@�C����������܂���B", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void agreeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // �m�F�`�F�b�N�{�b�N�X��WebView2�A���C���X�g�[���`�F�b�N�{�b�N�X�̗������`�F�b�N����Ă���ꍇ�̂݃A���C���X�g�[���{�^����L���ɂ���
            uninstallButton.Enabled = agreeCheckBox.Checked && webViewCheckBox.Checked;
        }

        private void webViewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // �m�F�`�F�b�N�{�b�N�X��WebView2�A���C���X�g�[���`�F�b�N�{�b�N�X�̗������`�F�b�N����Ă���ꍇ�̂݃A���C���X�g�[���{�^����L���ɂ���
            uninstallButton.Enabled = agreeCheckBox.Checked && webViewCheckBox.Checked;
        }

        private void uninstallButton_Click(object sender, EventArgs e)
        {
            // Microsoft Edge�̃v���Z�X���I��
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
                MessageBox.Show($"Microsoft Edge�̃v���Z�X���I�����ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Microsoft Edge�̊֘A�t�@�C�����폜
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

                // Edge�C���A�v���̍폜
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
                MessageBox.Show($"Microsoft Edge�̃t�@�C�����폜���ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // WebView2�̃A���C���X�g�[��
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

                    // WebView2�̃��W�X�g���L�[�̍폜
                    DeleteRegistryKey(@"SOFTWARE\Microsoft\EdgeWebView");
                    DeleteRegistryKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeWebView");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Microsoft Edge WebView2�̍폜���ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // ���W�X�g���L�[�̍폜
            try
            {
                DeleteRegistryKey(@"SOFTWARE\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate");
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Edge.Application");
                DeleteRegistryKey(@"HKEY_CURRENT_USER\Software\Microsoft\Edge");

                // �ݒ肩��Microsoft Edge�̕\�����폜
                DeleteRegistryKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Microsoft Edge");

                // �X�^�[�g�A�b�v����Microsoft Edge���폜
                DeleteStartupEntry("MicrosoftEdge");

                // �^�X�N�o�[����Microsoft Edge�̃s���~�߂��O��
                UnpinFromTaskbar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Microsoft Edge�̃��W�X�g���L�[���폜���ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // �������b�Z�[�W�ƍċN���{�^���̕\��
            this.BackColor = System.Drawing.Color.Red;
            this.completeLabel.Visible = true;
            this.uninstallButton.Visible = false; // �A���C���X�g�[���{�^�����\��
            this.restartButton.Visible = true; // �ċN���{�^����\��
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            // �V�X�e�����ċN��
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
                MessageBox.Show($"���W�X�g���L�[ {keyPath} ���폜���ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteStartupEntry(string appName)
        {
            try
            {
                // �X�^�[�g�A�b�v�t�H���_����폜
                string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string startupFilePath = Path.Combine(startupFolderPath, $"{appName}.lnk");
                if (File.Exists(startupFilePath))
                {
                    File.Delete(startupFilePath);
                }

                // ���W�X�g���̃X�^�[�g�A�b�v�L�[����폜
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
                MessageBox.Show($"�X�^�[�g�A�b�v����{appName}���폜���ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnpinFromTaskbar()
        {
            try
            {
                // VBScript�t�@�C���̃p�X
                string scriptPath = Path.Combine(Path.GetTempPath(), "UnpinEdge.vbs");

                // ���\�[�X����VBScript�t�@�C�����쐬
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EdgeUninstaller.Resources.UnpinEdge.vbs"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    File.WriteAllText(scriptPath, reader.ReadToEnd());
                }

                // VBScript�����s
                Process.Start(new ProcessStartInfo("cscript", $"//nologo \"{scriptPath}\"")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�^�X�N�o�[����Microsoft Edge�̃s���~�߂��O�����ɃG���[���������܂���: {ex.Message}", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
