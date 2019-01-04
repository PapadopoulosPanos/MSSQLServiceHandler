using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.ServiceProcess;
using System.Security.AccessControl;
using System.ComponentModel;

namespace DbServiceHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceController service;
        public MainWindow()
        {
            service = new ServiceController("mssql$sqlexpress");
            InitializeComponent();
            try
            {
                SetStatus();
            }
            catch (Exception)
            {
                Service404();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            TimeSpan timeout = new TimeSpan(0, 0, 10);

            if (service.Status != ServiceControllerStatus.Running)
            {
                service.Start();
                SetStatus();
                service.WaitForStatus(ServiceControllerStatus.StartPending, timeout);
                SetStatus();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                SetStatus();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TimeSpan timeout = new TimeSpan(0, 0, 10);

            if (service.Status != ServiceControllerStatus.Stopped)
            {
                if (service.CanStop)
                {
                    service.Stop();
                    SetStatus();
                    service.WaitForStatus(ServiceControllerStatus.StopPending, timeout);
                    SetStatus();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    SetStatus();
                }
                else
                {
                    ServiceStatus.Text = "Service is unavailable";
                }
            }
        }

        private void SetStatus()
        {
            var bc = new BrushConverter();
            ServiceStatus.Text = "Unavailable";
            switch (service.Status)
            {
                case ServiceControllerStatus.Paused:
                    ServiceStatus.Background = (Brush)bc.ConvertFrom("Red");
                    StatusProgress.Value = 10;
                    ProgressPercent.Text = "10%";
                    ServiceStatus.Text = "Paused";
                    break;
                case ServiceControllerStatus.Running:
                    ServiceStatus.Background = (Brush)bc.ConvertFrom("Green");
                    StatusProgress.Value = 100;
                    ProgressPercent.Text = "100%";
                    ServiceStatus.Text = "Running";
                    break;
                case ServiceControllerStatus.StartPending:
                    ServiceStatus.Background = (Brush)bc.ConvertFrom("Green");
                    StatusProgress.Value = 50;
                    ProgressPercent.Text = "50%";
                    ServiceStatus.Text = "Start Pending";
                    break;
                case ServiceControllerStatus.Stopped:
                    ServiceStatus.Background = (Brush)bc.ConvertFrom("Red");
                    StatusProgress.Value = 100;
                    ProgressPercent.Text = "100%";
                    ServiceStatus.Text = "Stopped";
                    break;
                case ServiceControllerStatus.StopPending:
                    ServiceStatus.Background = (Brush)bc.ConvertFrom("Red");
                    StatusProgress.Value = 50;
                    ProgressPercent.Text = "50%";
                    ServiceStatus.Text = "Stop Pending";
                    break;
                default:
                    ServiceStatus.Background = (Brush)bc.ConvertFrom("Grey");
                    StatusProgress.Value = 50;
                    ProgressPercent.Text = "50%";
                    ServiceStatus.Text = "Pending";
                    break;
            }
        }

        private void Service404()
        {
            var bc = new BrushConverter();
            ServiceStatus.Text = "Unavailable";
            StatusProgress.Value = 100;
            ProgressPercent.Text = "100%";
            ServiceStatus.Background = (Brush)bc.ConvertFrom("Yellow");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
