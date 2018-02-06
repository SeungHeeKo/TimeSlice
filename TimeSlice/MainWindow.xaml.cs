using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimeSlice
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindowInstance;

        PopupWindow popupWindow;

        const string filePath = @"C:\TimeSlice_File\";
        const string formatFile = "format.txt";
        const string shareFolderFile = "shareFolder.txt";
        string[] textValue, shareTextValue;
        string fileName = "";
        public string fileExt = ".mp4";
        string sendFile = "1.mp4";

        int totalFile = 0;

        Thread _sendMailThread;

        private bool isFirst = true;
        private bool isLoading = false;



        SharedAPI sharedAPI;
        //const string serverURL = @"\\SHKO\sh_공유폴더";
        //const string serverID = "SEUNGHEEKO";
        //const string serverPW = "sh";

        string serverURL = "";
        string serverID = "";
        string serverPW = "";

        bool isShareFolder = false;

        public MainWindow()
        {
            mainWindowInstance = this;

            InitializeComponent();
            InitializeVariable();
            this.WindowState = WindowState.Maximized;
        }

        private void InitializeVariable()
        {
            serverURL = @"\\SHKO\sh_공유폴더";
            serverID = "SEUNGHEEKO";
            serverPW = "sh";
            ReadTextFile(formatFile);
            fileExt = textValue[0];

            ReadTextFile(shareFolderFile);
            GetShareFolderTextValue();
            
            // 전체 파일 갯수 가져오기
            //GetTotalFileNum();
            //GetFullFileName();
        }
        
        private void ReadTextFile(string file)
        {
            try
            {
                String[] lines = System.IO.File.ReadAllLines(filePath + file, Encoding.Default);
                int num = 0;

                textValue = new string[lines.Length];
                if (lines.Length > 0)
                {
                    foreach (string line in lines)
                    {
                        textValue[num++] = line;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetShareFolderTextValue()
        {
            for (int i = 0; i < textValue.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        serverURL = textValue[i];
                        break;
                    case 1:
                        serverID = textValue[i];
                        break;
                    case 2:
                        serverPW = textValue[i];
                        break;
                }

            }
        }

        private void GetTotalFileNum()
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(serverURL);
            System.IO.FileInfo[] fi;

            //if (textValue[1] != null)
            //   fi = di.GetFiles("*" + textValue[1]);
            //else
            //    fi = di.GetFiles("*" + fileExt);

            fi = di.GetFiles("*");

            if (fi.Length == 0) MessageBox.Show("없음");
            
            else
            {
                string s = "";
                
                for (int i = 0; i < fi.Length; i++)
                    s += fi[i].Name.ToString() + Environment.NewLine;
                
            }
        }
        private void GetFullFileName()
        {
            for(int i=0; i<textValue.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        fileName = textValue[i];
                        break;
                    case 1:
                        fileName += totalFile.ToString();
                        break;
                    case 2:
                        fileName += textValue[i];
                        break;
                }

            }

            foreach (string line in textValue)
            {
            }
        }

        private void showMailButton_Click(object sender, RoutedEventArgs e)
        {
            // 전체 화면 어둡게
            imgComposite.Opacity = 0.5;

            // 메일 입력칸, 가상키보드 띄우기
            emailTextBox.Visibility = Visibility.Visible;
            //emailTextBox.Focus();

            emailSendButton.Visibility = Visibility.Visible;
        }

        public void ResetWindow()
        {
            // 전체 화면 원상복귀
            imgComposite.Opacity = 1;

            SetLoadingBar(false);
            // 메일 입력칸, 가상키보드 숨기기
            emailTextBox.Visibility = Visibility.Hidden;
            //emailTextBox.Focus();
            emailTextBox.Text = "";
            emailSendButton.Visibility = Visibility.Hidden;
            emailSendButton.IsEnabled = true;
        }

        private void emailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        public void ShowPopupMessage(bool isSuccess)
        {
            popupWindow = new PopupWindow(isSuccess);
            popupWindow.Closed += PopupWindow_Closed;
            popupWindow.Show();
            isLoading = false;
            //SetLoadingBar();
            SetLoadingBar(isLoading);
        }
        private void PopupWindow_Closed(object sender, EventArgs e)
        {
            //popupWindow.Dispatcher.InvokeShutdown();
            try
            {
                ResetWindow();
                //Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                //{
                //    ResetWindow();
                //}));
            }
            catch (Exception ex)
            {

            }
        }


        public void SetLoadingBar()  //async
        {
            //var progress = new Progress<Int>
            if (isLoading)
            {
                if (spinnerBar.Visibility == Visibility.Hidden)
                    this.Dispatcher.Invoke(delegate
                    {
                        spinnerBar.Visibility = Visibility.Visible;
                    });
            }
            else
            {
                if (spinnerBar.Visibility == Visibility.Visible)
                    this.Dispatcher.Invoke(delegate
                    {
                        spinnerBar.Visibility = Visibility.Hidden;
                    });
            }
        }

        public void SetLoadingBar(bool isOn)
        {
            //var progress = new Progress<Int>
            if (isOn)
            {
                if (spinnerBar.Visibility == Visibility.Hidden)
                    spinnerBar.Visibility = Visibility.Visible;
            }
            else
            {
                if (spinnerBar.Visibility == Visibility.Visible)
                    spinnerBar.Visibility = Visibility.Hidden;
            }
        }

        private void emailTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnableKeyBoard();
            emailTextBox.Focus();
        }

        private void EnableKeyBoard()
        {
            VirtualKeyboard.Open();
        }

        public void DisableKeyBoard()
        {
            VirtualKeyboard.Close();
        }

        public string GetShareFolderFile(string shareFolder, string resultFolder)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(shareFolder);

            FileInfo[] files = dirInfo.GetFiles();

            string fileName = "";

            int maxNum = 0;
            MainWindow mainWindow = MainWindow.mainWindowInstance;
            Char[] fileExt = mainWindow.fileExt.ToCharArray();
            string[] fileName_Ext = new string[2];
            string strTmp = "";
            FileInfo fileToSend = files[files.Length - 1];

            if (files != null)
            {
                string strCurrentDirectory = System.IO.Directory.GetCurrentDirectory();
                if (!System.IO.Directory.Exists(resultFolder))
                {
                    System.IO.Directory.CreateDirectory(resultFolder);
                }

                foreach (FileInfo file in files)
                {
                    fileName_Ext = file.Name.Split(fileExt);
                    strTmp = Regex.Replace(fileName_Ext[1], @"\D", "");
                    if (!string.IsNullOrEmpty(strTmp))
                    {
                        if (int.Parse(strTmp) > maxNum)
                        {
                            maxNum = int.Parse(strTmp);
                            fileToSend = file;
                        }
                    }



                    //if (file.Name.Contains((files.Length).ToString()))
                    //{
                    //}
                }


                System.Diagnostics.Debug.Write(fileToSend.FullName);
                string strDest = resultFolder + fileToSend.Name;
                //MessageBox.Show(file.FullName);
                //System.IO.File.Copy(fileToSend.FullName, @strDest, true);
                //file.Attributes = FileAttributes.Normal;
                fileName = fileToSend.Name;

                string mailTo = emailTextBox.Text;
                EmailService emailService = new EmailService();

                emailService.SendEmail(mailTo, resultFolder   + fileName);
            }

            return fileName;
        }


        private void emailSendButton_Click(object sender, RoutedEventArgs e)
        {
            emailSendButton.IsEnabled = false;
            // 프로그래스바 보이기
            isLoading = true;
            SetLoadingBar(true);


            if (isShareFolder)
                connectShareFolder();
            else
                SendMail();
            //_sendMailThread = null;
            //_sendMailThread = new Thread(new ThreadStart(() =>
            //{
            //    EmailService emailService = new EmailService();


            //    if (isShareFolder)
            //    {
            //        if (string.IsNullOrEmpty(sendFile))
            //            mailSuccess = false;
            //        else
            //        {
            //            if (System.IO.File.Exists(filePath + sendFile))
            //            {
            //                try
            //                {
            //                    //mailSuccess = emailService.SendEmail(mailTo, filePath + sendFile);
            //                    emailService.SendEmail(mailTo, filePath + sendFile);

            //                }

            //                catch (System.IO.IOException ex)
            //                {
            //                    MessageBox.Show(ex.Message);
            //                    ResetWindow();
            //                }
            //            }
            //            else
            //                mailSuccess = false;
            //        }
            //    }
            //    else
            //    {
            //        //mailSuccess = emailService.SendEmail(mailTo, filePath + sendFile);
            //        emailService.SendEmail(mailTo, filePath + sendFile);
            //    }

            //    //ShowPopupMessage(mailSuccess);

            //    System.Windows.Threading.Dispatcher.Run();
            //}));
            //_sendMailThread.SetApartmentState(ApartmentState.STA);
            //_sendMailThread.IsBackground = true;


            //else
            //    _sendMailThread.Start();
        }

        public void SendMail()
        {
            bool mailSuccess = false;
            string mailTo = emailTextBox.Text;

            EmailService emailService = new EmailService();


            if (isShareFolder)
            {
                if (string.IsNullOrEmpty(sendFile))
                    mailSuccess = false;
                else
                {
                    if (System.IO.File.Exists(filePath + sendFile))
                    {
                        try
                        {
                            //mailSuccess = emailService.SendEmail(mailTo, filePath + sendFile);
                            emailService.SendEmail(mailTo, filePath + sendFile);

                        }

                        catch (System.IO.IOException ex)
                        {
                            MessageBox.Show(ex.Message);
                            ResetWindow();
                        }
                    }
                    else
                        mailSuccess = false;
                }
            }
            else
            {
                GetShareFolderFile(filePath, filePath);
                //mailSuccess = emailService.SendEmail(mailTo, filePath + sendFile);
                
            }
        }

        public void MailResult(bool isSuccess)
        {
            ShowPopupMessage(isSuccess);
            
        }

        private void mainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            DisableKeyBoard();
        }

        public void SendAttachedMail()
        {
            //if(_sendMailThread != null)
            //    _sendMailThread.Start();
            SendMail();
        }

        // 공유폴더에 접근해 최신파일 가져와 내부 폴더에 저장
        private void connectShareFolder()
        {
            try
            {
                sharedAPI = new SharedAPI();
                int result = sharedAPI.ConnectRemoteServer(serverURL, serverID, serverPW);
                //MessageBox.Show("result  " + result);=
                sendFile = sharedAPI.GetShareFolderFile(serverURL, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ResetWindow();
            }
        }

    }
}
