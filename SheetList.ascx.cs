using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Jenzabar.Portal.Framework.Web.UI;
using Jenzabar.Portal.Framework;		// get userID from ADAM
using CUS.Jenzabar.OdbcConnectionClass;
using Jenzabar.Common;
using System.Linq;
using System.Collections;

namespace CUSTimesheet
{
    public partial class SheetList : PortletViewBase
    {
        OdbcConnectionClass connection;
        DataTable dt = null;
        DataTable outtable = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            noTimesheets.Visible = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("datedue", typeof(String));
            dt.Columns.Add("payperiod", typeof(String));
            dt.Columns.Add("department", typeof(String));
            dt.Columns.Add("status", typeof(String));
            dt.Columns.Add("truetime", typeof(int));
            dt.Columns.Add("deptnum", typeof(string));

            DateTime time = DateTime.Now;
            string format = "yyyyMMdd";


            //select all of the timesheets for this person
            //then parse through all the pay periods since they have started and check if there is a paysheet for that period for each department they belong to
            //then check to see if the status is one of the ones that needs to be shown
            //june 13th, 2014 (first start payday to check for future timesheets)

            //first get their first active date
            Exception dbex = null;
            connection = new OdbcConnectionClass("EXConfig.xml");
            string activeQuery = "select * from tmseprd.dbo.svu_timesheet_status_change where id = '" + PortalUser.Current.HostID.ToString() + "'";
            DataTable activeTable = connection.ConnectToERP(activeQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //checking for users who slipped through the cracks
            if (activeTable.Rows.Count == 0)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = "You have not been set to an active employee yet.  Please advise your supervisor if you feel this is in error.";
                return;
            }

            string timesheetQuery = "select * from tmseprd.dbo.svu_timesheet_timesheet where id = '" + PortalUser.Current.HostID.ToString() + "'";
            DataTable timesheetTable = connection.ConnectToERP(timesheetQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //at this point we want to start parsing through each pay period (the sunday before the friday payday is the last day of the period
            //so lets identify each pay period based on its payday
            //start day is june 13th, 2014 == 20140613

            //the start date for all employees will always be the first thing grabbed from the change table
            //int employeestart = Convert.ToInt32(activeTable.Rows[0]["date"].ToString());

            //we need to get a list of their different departments
            ArrayList departmentList = new ArrayList();
            for (int i = 0; i < activeTable.Rows.Count; i++)
            {
                bool founddept = false;
                foreach (string s in departmentList)
                {
                    if (activeTable.Rows[i]["departments"].ToString().Equals(s))
                    {
                        founddept = true;
                    }
                }
                if (founddept == false)
                {
                    departmentList.Add(activeTable.Rows[i]["departments"].ToString());
                }
            }

            ArrayList groupsOfChanges = new ArrayList();

            //at this point we have a list of all the departments they have belonged to.
            foreach (string s in departmentList)
            {
                //each department will have a start date

                ArrayList temp = new ArrayList();
                for (int i = 0; i < activeTable.Rows.Count; i++)
                {
                    if (activeTable.Rows[i]["departments"].ToString().Equals(s))
                    {
                        StatusChange sc = new StatusChange(activeTable.Rows[i]["id"].ToString(), activeTable.Rows[i]["changedTo"].ToString(), activeTable.Rows[i]["departments"].ToString(), activeTable.Rows[i]["date"].ToString());
                        temp.Add(sc);
                    }
                }
                groupsOfChanges.Add(temp);
            }

            //now we have a group of lists for each department.  We can use this to parse everything
            ArrayList datestoadd = new ArrayList();
            foreach (ArrayList list in groupsOfChanges)
            {
                int pointer = 0;
                bool continueloop = true;
                while (continueloop == true)
                {
                    int employeeend = 0;
                    StatusChange start = (StatusChange)list[pointer];
                    int employeestart = Convert.ToInt32(start.date);
                    if (pointer + 1 == list.Count)
                    {
                        employeeend = Convert.ToInt32(time.ToString(format));
                        continueloop = false;
                    }
                    else
                    {
                        StatusChange end = (StatusChange)list[pointer + 1];
                        employeeend = Convert.ToInt32(end.date);
                    }
                    datestoadd.AddRange(CheckForTimesheets(employeestart, employeeend, timesheetTable, start.dept));
                    if (pointer + 2 > list.Count)
                    {
                        continueloop = false;
                    }
                    else
                    {
                        pointer = pointer + 2;
                    }
                }

            }

            dbex = null;
            connection = new OdbcConnectionClass("EXConfig.xml");
            string userQuery = "select * from svu_timesheet_departments";
            DataTable dt3 = connection.ConnectToERP(userQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }

            //so now we have a list of timesheets that are new.  we also need to parse through the timesheets that they've already filled out and check on the status
            //if the status is listed as "incomplete" or "needs review" then it should be displayed, otherwise we'll not show it



            //these are the new timesheets
            string visformat = "MM/dd/yyyy";
            foreach (DateDept dd in datestoadd)
            {
                DateTime timesheettime = new DateTime(Convert.ToInt32(dd.date.ToString().Substring(0, 4)), Convert.ToInt32(dd.date.ToString().Substring(4, 2)), Convert.ToInt32(dd.date.ToString().Substring(6, 2)));
                DateTime startofpayperiod = timesheettime.AddDays(-14);
                DateTime endofpayperiod = timesheettime.AddDays(-1);
                string departmentname = "";
                foreach (DataRow row in dt3.Rows)
                {
                    if (row["id"].ToString().Equals(dd.dept))
                    {
                        departmentname = row["departmentname"].ToString();
                    }
                }

                dt.Rows.Add(timesheettime.ToString(visformat), startofpayperiod.ToString(visformat) + " - " + endofpayperiod.ToString(visformat), departmentname, "Not filled out.", timesheettime.ToString(format), dd.dept);
            }

            /*
                 bool foundTimesheet = false;
                 DateTime timesheettime = new DateTime(Convert.ToInt32(dd.date.ToString().Substring(0, 4)), Convert.ToInt32(dd.date.ToString().Substring(4, 2)), Convert.ToInt32(dd.date.ToString().Substring(6, 2)));
                 int rowfound = -1;
                 for (int i = 0; i < timesheetTable.Rows.Count; i++)
                 {
                     errMsg.Visible = true;
                     errMsg.ErrorMessage += " timesheet: " + timesheetTable.Rows[i]["payPeriod"].ToString() + " format: " + timesheettime.ToString(visformat) + " timesheetdept: " + timesheetTable.Rows[i]["dept"].ToString() + " otherdept: " + dd.dept;
                    
                     if (timesheetTable.Rows[i]["payPeriod"].ToString().Equals(timesheettime.ToString(visformat)) && timesheetTable.Rows[i]["dept"].ToString().Equals(dd.dept))
                     {
                         foundTimesheet = true;
                         rowfound = i;
                     }
                 }
             */

            //status' for timesheets
            //1 = not filled out
            //2 = saved but not submitted
            //3 = submitted for approval by suppervisor
            //4 = approved by supervisor
            //5 = rejected by supervisor, needs to be fixed

            for (int i = 0; i < timesheetTable.Rows.Count; i++)
            {
                // errMsg.Visible = true;
                // errMsg.ErrorMessage += "found: " + timesheettime.ToString(visformat);
                DateTime timesheettime = new DateTime(Convert.ToInt32(timesheetTable.Rows[i]["payPeriod"].ToString().Substring(6, 4)), Convert.ToInt32(timesheetTable.Rows[i]["payPeriod"].ToString().Substring(0, 2)), Convert.ToInt32(timesheetTable.Rows[i]["payPeriod"].ToString().Substring(3, 2)));
                if (Convert.ToInt32(timesheetTable.Rows[i]["status"].ToString()) == 2 || Convert.ToInt32(timesheetTable.Rows[i]["status"].ToString()) == 5)
                {
                    DateTime startofpayperiod = timesheettime.AddDays(-14);
                    DateTime endofpayperiod = timesheettime.AddDays(-1);

                    string departmentname = "";
                    for (int j = 0; j < dt3.Rows.Count; j++)
                    {
                        if (dt3.Rows[j]["id"].ToString().Equals(timesheetTable.Rows[i]["dept"].ToString()))
                        {
                            departmentname = dt3.Rows[j]["departmentname"].ToString();
                        }
                    }
                    string statusdescrip = "";
                    if (Convert.ToInt32(timesheetTable.Rows[i]["status"].ToString()) == 2)
                    {
                        statusdescrip = "Saved, but not submitted.";
                    }

                    if (Convert.ToInt32(timesheetTable.Rows[i]["status"].ToString()) == 5)
                    {
                        statusdescrip = "Rejected by supervisor, please see notes inside.";
                    }

                    dt.Rows.Add(timesheetTable.Rows[i]["payPeriod"].ToString(), startofpayperiod.ToString(visformat) + " - " + endofpayperiod.ToString(visformat), departmentname, statusdescrip, timesheettime.ToString(format), timesheetTable.Rows[i]["dept"].ToString());
                }
            }



            dt.DefaultView.Sort = "truetime ASC";
            outtable = dt.DefaultView.ToTable();


            GridView1.DataSource = null;
            GridView1.DataSource = outtable;
            GridView1.DataBind();
            this.dt = dt;
        }

            //this section is to make it where the actual employee start and end dates actually take effect and allows the program to stop producing timesheets for that job.

            /*
            int employeestart = 20140620;
             int employeeend= 0;
            if (activeTable.Rows.Count >= 2)
            {
                 employeeend = Convert.ToInt32(activeTable.Rows[1]["date"].ToString());
            }
            else
            {
                 employeeend = Convert.ToInt32(time.ToString(format));
            }

            ArrayList datestoadd = new ArrayList();
            datestoadd.AddRange(CheckForTimesheets(employeestart, employeeend, timesheetTable));
            //at this point our arraylist should have a list of all of the new timesheets that need to be displayed
            
            errMsg.Visible = true;
            errMsg.ErrorMessage = "number of new timesheets: " + datestoadd.Count + " and their dates are: ";
            foreach (string s in datestoadd)
            {
                errMsg.ErrorMessage += s + " , ";
            }
           
           // *********** work on this part later **********///
            /*
            if (activeTable.Rows.Count > 2)
            {
                bool continueloop = true;
                int startint = 2;
                while(continueloop == true)
                {
                    int employeestartloop = Convert.ToInt32(activeTable.Rows[startint]["date"].ToString());
                    if (activeTable.Rows.Count >= startint + 2)
                    {
                        employeeend = Convert.ToInt32(activeTable.Rows[startint + 1]["date"].ToString());
                    }
                    else if (activeTable.Rows.Count == startint + 2)
                    {
                        employeeend = Convert.ToInt32(activeTable.Rows[startint + 1]["date"].ToString());
                        continueloop = false;
                    }
                    else
                    {
                        employeeend = Convert.ToInt32(time.ToString(format));
                        continueloop = false;
                    }
                    datestoadd.AddRange(CheckForTimesheets(employeestartloop, employeeend, timesheetTable));
                    startint = startint + 2;
             *
             if (activeTable.Rows.Count > 1)
             {
                bool continueloop = false;
                int startint = 1;
                while(continueloop == true)
                {
                    int employeeendloop = Convert.ToInt32(activeTable.Rows[endint]["date"].ToString());
                    if (activeTable.Rows.Count >= endint + 1)
                {
                    employeeend = Convert ToInt32(activeTable.Rows[startint + 1];
               }
                           
           ArrayList datestoadd = new ArrayList();
            datestoadd.AddRange(CheckForTimesheets(employeestart, employeeend, timesheetTable));
            //at this point our arraylist should have a list of all of the new timesheets that need to be displayed
            
            errMsg.Visible = true;
            errMsg.ErrorMessage = "number of new timesheets: " + datestoadd.Count + " and their dates are: ";
            foreach (string s in datestoadd)
            {
                errMsg.ErrorMessage += s + " , ";
            }
              }
              
             }

            }          
                        
             

         }}
        */
        override protected void OnInit(EventArgs e)
        {
            btnBack.Click += btnBack_Click;
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.ChangeScreenToDefaultView();
        }

        private ArrayList CheckForTimesheets(int startdate, int enddate, DataTable timesheets, string dept)
        {
            ArrayList temp = new ArrayList();
            DateTime time = DateTime.Now;
            string format = "yyyyMMdd";
            string visformat = "MM/dd/yyyy";

            DateTime enddatetime = new DateTime(Convert.ToInt32(enddate.ToString().Substring(0, 4)), Convert.ToInt32(enddate.ToString().Substring(4, 2)), Convert.ToInt32(enddate.ToString().Substring(6, 2)));
            enddatetime = enddatetime.AddDays(14); //we need to add a week to get the next timesheet, this should work for both people who are active, and those who are inactive.

            //now we need to find the first payday after they started.
            int firstpayday = FindFirstPayday(startdate);

            DateTime firstpaydaytime = new DateTime(Convert.ToInt32(firstpayday.ToString().Substring(0,4)),Convert.ToInt32(firstpayday.ToString().Substring(4,2)),Convert.ToInt32(firstpayday.ToString().Substring(6,2)));
            //errMsg.Visible = true;
            //errMsg.ErrorMessage += " first payday: " + firstpaydaytime.ToString(format) ;
            //errMsg.ErrorMessage += " first payday: " + firstpayday;
            //return null;
            
            while (Convert.ToInt32(firstpaydaytime.ToString(format)) <= Convert.ToInt32(enddatetime.ToString(format)))
            {
                bool foundTimesheet = false;
                for (int i = 0; i < timesheets.Rows.Count; i++)
                {
                    //errMsg.Visible = true;
                    //errMsg.ErrorMessage += " timesheet: " + timesheets.Rows[i]["payPeriod"].ToString() + " format: " + firstpaydaytime.ToString(visformat) + " timesheetdept: " + timesheets.Rows[i]["dept"].ToString() + " otherdept: " + dept;
                    if (timesheets.Rows[i]["payPeriod"].ToString().Equals(firstpaydaytime.ToString(visformat)) && timesheets.Rows[i]["dept"].ToString().Equals(dept))
                    {
                        foundTimesheet = true;
                    }
                }
                if (foundTimesheet == false)
                {
                    //we need to add a timesheet to fill out for the person
                    DateDept dd = new DateDept(firstpaydaytime.ToString(format), dept);
                    temp.Add(dd);
                }
                else
                {
                    //there is a timesheet, but it may not be filled out completely, we can check this later
                   
                }
                firstpaydaytime = firstpaydaytime.AddDays(14);
            }
            
            return temp;
            
        }

        //this seems to be working
        private int FindFirstPayday(int startdate)
        {
            //startdate = startdate + 5;  //we need to add 5 days since they could have started during a new pay period
            DateTime checkTime = new DateTime(2014, 6, 9);
            string format = "yyyyMMdd";
           // errMsg.ErrorMessage += " startdate = " + startdate + " checktimestart = " + checkTime.ToString(format) + " ";
            while (Convert.ToInt32(checkTime.ToString(format)) <= startdate)
            {
                checkTime = checkTime.AddDays(14);
            }

            return Convert.ToInt32(checkTime.ToString(format));  //this should return the payday after they started
        }

        public void CUSTimesheet()
        {
           
        }

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridView1.SelectedRow.RowIndex;
            //Session["datedue"] =  dt.Rows[index]["datedue"].ToString();
            Application["datedue"] = outtable.Rows[index]["datedue"].ToString();
            Application["payperiod"] = outtable.Rows[index]["payperiod"].ToString();
            Application["dept"] = outtable.Rows[index]["deptnum"].ToString();
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = "dept: " + dt.Rows[index]["deptnum"].ToString();
            this.ParentPortlet.NextScreen("FillTimesheet");
        }
    }
}