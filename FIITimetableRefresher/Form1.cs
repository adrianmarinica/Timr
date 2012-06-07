using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using BusinessLogic;
using FIITimetableParser;
using FIITimetableParser.FiiObjects;
using Objects;
using WebsiteMonitor;

namespace FIITimetableRefresher
{
    public partial class Form1 : Form
    {
        private BackgroundWorker timetableWorker;
        private BackgroundWorker websiteWorker;

        public Form1()
        {
            InitializeComponent();
            label1.Text = String.Empty;
            label2.Text = String.Empty;
            refreshTimer.Interval = 60000;
            refreshTimer.Tick += OnRefreshTimerTick;

            timetableWorker = new BackgroundWorker();
            timetableWorker.DoWork += OnTimetableWorkerDoWork;
            timetableWorker.RunWorkerCompleted += OnWorkerCompleted;

            websiteWorker = new BackgroundWorker();
            websiteWorker.DoWork += OnWebsiteWorkerDoWork;
        }

        private void OnWebsiteWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var monitoredWebsitesBL = new MonitoredWebsitesBL();
            List<MonitoredWebsite> monitoredWebsites = monitoredWebsitesBL.GetAllWebsites();
            var modifiedWebsites = new List<MonitoredWebsite>();
            if (monitoredWebsites != null)
            {
                for (int i = 0; i < monitoredWebsites.Count; i++)
                {
                    MonitoredWebsite monitoredWebsite = monitoredWebsites[i];
                    string hashedContent = Monitor.GetMD5Hash(monitoredWebsite.Id);
                    if (hashedContent != monitoredWebsite.HashedContent)
                    {
                        monitoredWebsites[i].HashedContent = hashedContent;
                        monitoredWebsites[i].LastModified = DateTime.Now;
                        modifiedWebsites.Add(monitoredWebsites[i]);
                    }
                }
            }
            monitoredWebsitesBL.SaveMonitoredWebsites(modifiedWebsites);
        }

        private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnRefresh.Enabled = true;
        }

        private void OnTimetableWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            // btnRefresh.Enabled = false;
            // var usersBL = new UsersBL();
            // var groups = usersBL.GetGroupsByFaculty("info.uaic.ro");


            // var timetablesBL = new TimetablesBL();
            // var parser = new Parser();

            // var timetableForGroup = parser.GetTimetableForGroup(StudyYear.I1, HalfYear.A, "1");
            // timetablesBL.SaveTimetable(timetableForGroup);
        }

        private void OnRefreshTimerTick(object sender, EventArgs e)
        {
            TryRefresh();
        }

        private void RefreshTimetable()
        {
        }

        private void OnBtnStartClick(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            refreshTimer.Start();
        }

        private void OnBtnStopClick(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            refreshTimer.Stop();
        }

        private void OnBtnRefreshClick(object sender, EventArgs e)
        {
            TryRefresh();
        }

        private void TryRefresh()
        {
            if (!timetableWorker.IsBusy)
            {
                timetableWorker.RunWorkerAsync();
            }
            if (!websiteWorker.IsBusy)
            {
                websiteWorker.RunWorkerAsync();
            }
        }
    }
}