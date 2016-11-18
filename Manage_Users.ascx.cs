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
using System.Text.RegularExpressions;

namespace CUSTimesheet
{
    public partial class Manage_Users : PortletViewBase
    {

        OdbcConnectionClass connection;
        DataTable dt = null;

        override protected void OnInit(EventArgs e)
        {
            btnAddUser.Click += btnAddUser_Click;
            btnUpdateUser.Click += btnUpdateUser_Click;
            btnBack.Click += btnBack_Click;
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.ChangeScreenToDefaultView();
        }

        void btnUpdateUser_Click(object sender, EventArgs e)
        {
            int index = GridView1.SelectedRow.RowIndex;

            User olduser = new User();

            olduser.id = dt.Rows[index]["id"].ToString();
            //olduser.fname = dt.Rows[index]["fname"].ToString();
            //olduser.lname = dt.Rows[index]["lname"].ToString();
            //olduser.departments = dt.Rows[index]["departments"].ToString();
            //olduser.supervisors = dt.Rows[index]["supervisors"].ToString();
            //olduser.role = dt.Rows[index]["userLevel"].ToString();
            
            olduser.department1 = dt.Rows[index]["department1"].ToString();
            olduser.department2 = dt.Rows[index]["department2"].ToString();
            olduser.department3 = dt.Rows[index]["department3"].ToString();
            olduser.department4 = dt.Rows[index]["department4"].ToString();
            
            olduser.department1status = dt.Rows[index]["department1status"].ToString();
            olduser.department2status = dt.Rows[index]["department2status"].ToString();
            olduser.department3status = dt.Rows[index]["department3status"].ToString();
            olduser.department4status = dt.Rows[index]["department4status"].ToString();

            User updateduser = new User();

            updateduser.id = txtID.Text;
            updateduser.fname = txtfname.Text;
            updateduser.lname = txtlname.Text;

            if (ddlDepartment1choice.SelectedValue.Length > 2)
                {
                    updateduser.department1 = Regex.Match(ddlDepartment1choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
                }
                else
                {
                    updateduser.department1 = "";
                }

            if (ddlDepartment2choice.SelectedValue.Length > 2)
            {
                updateduser.department2 = Regex.Match(ddlDepartment2choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department2 = "";
            }

            if (ddlDepartment3choice.SelectedValue.Length > 2)
            {
                updateduser.department3 = Regex.Match(ddlDepartment3choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department3 = "";
            }

            if (ddlDepartment4choice.SelectedValue.Length > 2)
            {
                updateduser.department4 = Regex.Match(ddlDepartment4choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department4 = "";
            }
           






           
            updateduser.department1status = ddlDepartment1.SelectedValue;
            updateduser.department2status = ddlDepartment2.SelectedValue;
            updateduser.department3status = ddlDepartment3.SelectedValue;
            updateduser.department4status = ddlDepartment4.SelectedValue;
            updateduser.supervisors = txtSupervisors.Text;
            updateduser.role = ddlRole.SelectedValue;
            updateduser.status = "";

            Exception dbex = null;
            string userQuery = "update svu_timesheet_user set fname = '" + updateduser.fname + "', lname = '" + updateduser.lname + "', supervisors = '"+
                updateduser.supervisors + "', userLevel = '" + updateduser.role + "', department1 = '" + updateduser.department1 + "', department2 = '" + updateduser.department2 +
                 "', department3 = '" + updateduser.department3 + "', department4 = '" + updateduser.department4 +
                 "', department1status = '" + updateduser.department1status + "', department2status = '" + updateduser.department2status +
                 "', department3status = '" + updateduser.department3status + "', department4status = '" + updateduser.department4status + "' where id = '" +
                olduser.id + "'";
            dt = connection.ConnectToERP(userQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            if (!olduser.department1status.Equals(updateduser.department1status))
            {
                DateTime time = DateTime.Now;
                string format = "yyyyMMdd";
                //add change notice
                string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department1status + "', '" + time.ToString(format) + "', '" + updateduser.department1 + "')";
                connection.ConnectToERP(changeQuery, ref dbex);
            }

            if (!olduser.department2status.Equals(updateduser.department2status))
            {
                DateTime time = DateTime.Now;
                string format = "yyyyMMdd";
                //add change notice
                string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department2status + "', '" + time.ToString(format) + "', '" + updateduser.department2 + "')";
                connection.ConnectToERP(changeQuery, ref dbex);
            }

            if (!olduser.department3status.Equals(updateduser.department3status))
            {
                DateTime time = DateTime.Now;
                string format = "yyyyMMdd";
                //add change notice
                string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department3status + "', '" + time.ToString(format) + "', '" + updateduser.department3 + "')";
                connection.ConnectToERP(changeQuery, ref dbex);
            }

            if (!olduser.department4status.Equals(updateduser.department4status))
            {
                DateTime time = DateTime.Now;
                string format = "yyyyMMdd";
                //add change notice
                string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department4status + "', '" + time.ToString(format) + "', '" + updateduser.department4 + "')";
                connection.ConnectToERP(changeQuery, ref dbex);
            }

            RefreshData();
          }

        void btnAddUser_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            string format = "yyyyMMddHHmmss";


            User updateduser = new User();

            updateduser.id = txtID.Text;
            updateduser.fname = txtfname.Text;
            updateduser.lname = txtlname.Text;
            if (ddlDepartment1choice.SelectedValue.Length > 2)
            {
                updateduser.department1 = Regex.Match(ddlDepartment1choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department1 = "";
            }

            if (ddlDepartment2choice.SelectedValue.Length > 2)
            {
                updateduser.department2 = Regex.Match(ddlDepartment2choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department2 = "";
            }

            if (ddlDepartment3choice.SelectedValue.Length > 2)
            {
                updateduser.department3 = Regex.Match(ddlDepartment3choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department3 = "";
            }

            if (ddlDepartment4choice.SelectedValue.Length > 2)
            {
                updateduser.department4 = Regex.Match(ddlDepartment4choice.SelectedValue, @"\(([^)]*)\)").Groups[1].Value;
            }
            else
            {
                updateduser.department4 = "";
            }

            //errMsg.Visible = true;
           // errMsg.ErrorMessage = "output: " + updateduser.department1 + " original: " + ddlDepartment1choice.SelectedValue + " length: " + ddlDepartment1choice.SelectedValue.Length;

            updateduser.department1status = ddlDepartment1.SelectedValue;
            updateduser.department2status = ddlDepartment2.SelectedValue;
            updateduser.department3status = ddlDepartment3.SelectedValue;
            updateduser.department4status = ddlDepartment4.SelectedValue;
            updateduser.supervisors = txtSupervisors.Text;
            updateduser.role = ddlRole.SelectedValue;
            updateduser.status = "";

           
            Exception dbex = null;
            string userQuery = "insert into Tmseprd.dbo.svu_timesheet_user VALUES (" + updateduser.id + ", '" + updateduser.role + "'," + PortalUser.Current.HostID.ToString() + "," + time.ToString(format) +
                ",'" + updateduser.supervisors + "','" + "na" + "','" + "na" + "','" + updateduser.fname + "','" + updateduser.lname + "','" + updateduser.department1 +
               "','" + updateduser.department2 + "','" + updateduser.department3 + "','" + updateduser.department4 + "','" + updateduser.department1status + "','" + updateduser.department2status +
              "','" + updateduser.department3status + "','" + updateduser.department4status + "')";
            dt = connection.ConnectToERP(userQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            if (dbex == null)
            {
                string myfunction = "$('#noticeadded').show(); setTimeout(function() { $('#noticeadded').fadeOut(); }, 5000);"; //TODO: fix this, not showing long enough; input 7000 instead of 5000
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", myfunction, true);

                
                DateTime time2 = DateTime.Now;
                string format2 = "yyyyMMdd";

                if (!updateduser.department1.Equals(""))
                {
                    string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department1status + "', '" + time2.ToString(format2) + "', '" + updateduser.department1 + "')";
                    connection.ConnectToERP(changeQuery, ref dbex);

                    if (dbex != null)
                    {
                        errMsg.Visible = true;
                        errMsg.ErrorMessage = dbex.Message.ToString();
                        return;
                    }
                }

                if (!updateduser.department2.Equals(""))
                {
                    string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department2status + "', '" + time2.ToString(format2) + "', '" + updateduser.department2 + "')";
                    connection.ConnectToERP(changeQuery, ref dbex);

                    if (dbex != null)
                    {
                        errMsg.Visible = true;
                        errMsg.ErrorMessage = dbex.Message.ToString();
                        return;
                    }
                }

                if (!updateduser.department3.Equals(""))
                {
                    string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department3status + "', '" + time2.ToString(format2) + "', '" + updateduser.department3 + "')";
                    connection.ConnectToERP(changeQuery, ref dbex);

                    if (dbex != null)
                    {
                        errMsg.Visible = true;
                        errMsg.ErrorMessage = dbex.Message.ToString();
                        return;
                    }
                }

                if (!updateduser.department4.Equals(""))
                {
                    string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_status_change VALUES ('" + updateduser.id + "', '" + PortalUser.Current.HostID.ToString() + "', '" +
                    updateduser.department4status + "', '" + time2.ToString(format2) + "', '" + updateduser.department4 + "')";
                    connection.ConnectToERP(changeQuery, ref dbex);

                    if (dbex != null)
                    {
                        errMsg.Visible = true;
                        errMsg.ErrorMessage = dbex.Message.ToString();
                        return;
                    }
                }

                
            }

            RefreshData();

        }

        // New button for admin with the ability to edit user timesheets
        // Not sure if the edit button should be implimented here or not?
        /*
         public Form1()
        {
        InitializeComponent ();
         for (int i = 0; i < 5; i++)
          {
              Button edit = new Button();
              button.Location = new Point (50, 60 * i + 10);
              switch(i)
             {
                case 0:
                  button.Click += new EventHandler(ButtonClick);
                  break;
               case 1:
                  button.Click += new EventHandler(ButtonClick2);
                  break;
                  //...
              }
              this.Controls.add(button);
          }
          for (int i = 0; i < 5; i++)
           {
              Button edit = new Button();
              button.Location = new Point (50, 60 * i + 10);
              button.Click += new EventHandler(ButtonClickOneEvent);
              button.Tag = i;
              this.Controls.Add(button);
           }
         }
        void ButtonClick(object sender, EventArgs e)
        {
            //button clicked
        }
        
        void ButtonClickOneEvent(object sender, EventArgs e)
        {
            Button edit = sender as Button;
            if (button != null)
            {
                //button was clicked
                switch ((int)button.Tag)
                {
                case 0:
                    //first button clicked
                    break;
                }
              }
            }         
        */

        public Manage_Users()
        {
            this.Load += Manage_Users_Load;
            
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridView1.SelectedRow.RowIndex;
            //string name = GridView1.SelectedRow.Cells[4].Text;
            //string country = dt.Rows[index][6].ToString();
            //string message = " Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = "Message: " + dt.Rows[index]["id"].ToString() + " " + dt.Rows[index]["departments"].ToString() + " " + dt.Rows[index]["status"].ToString();

            
            txtID.Text = dt.Rows[index]["id"].ToString();
            txtfname.Text = dt.Rows[index]["fname"].ToString();
            txtlname.Text= dt.Rows[index]["lname"].ToString();
            //TODO: go through list of departments and find the one that matches with the id, then display it
            foreach (ListItem s in ddlDepartment1choice.Items)
            {
                if(Regex.Match(s.Value, @"\(([^)]*)\)").Groups[1].Value.Equals(dt.Rows[index]["department1"].ToString()))
                {
                    ddlDepartment1choice.SelectedValue = s.Value;
                }
                if (Regex.Match(s.Value, @"\(([^)]*)\)").Groups[1].Value.Equals(dt.Rows[index]["department2"].ToString()))
                {
                    ddlDepartment2choice.SelectedValue = s.Value;
                }
                if (Regex.Match(s.Value, @"\(([^)]*)\)").Groups[1].Value.Equals(dt.Rows[index]["department3"].ToString()))
                {
                    ddlDepartment3choice.SelectedValue = s.Value;
                }
                if (Regex.Match(s.Value, @"\(([^)]*)\)").Groups[1].Value.Equals(dt.Rows[index]["department4"].ToString()))
                {
                    ddlDepartment4choice.SelectedValue = s.Value;
                }
            }

           
            //txtSupervisors.Text = dt.Rows[index]["supervisors"].ToString();
            ddlRole.SelectedValue = dt.Rows[index]["userLevel"].ToString();
            ddlDepartment1.SelectedValue = dt.Rows[index]["department1status"].ToString();
            ddlDepartment2.SelectedValue = dt.Rows[index]["department2status"].ToString();
            ddlDepartment3.SelectedValue = dt.Rows[index]["department3status"].ToString();
            ddlDepartment4.SelectedValue = dt.Rows[index]["department4status"].ToString();
            //ddlStatus.SelectedValue = dt.Rows[index]["status"].ToString();
         
        }

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        private void RefreshData()
        {
            Exception dbex = null;
            connection = new OdbcConnectionClass("EXConfig.xml");
            string userQuery = "select * from svu_timesheet_user";
            //if we want to only get admins and supervisors select * from Tmseprd.dbo.svu_timesheet_user where userLevel = 'supervisor' or userLevel = 'admin'
            dt = connection.ConnectToERP(userQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }

            
            GridView1.DataSource = null;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
       

        void Manage_Users_Load(object sender, EventArgs e)
        {
            RefreshData();
            if (IsFirstLoad) //IsFirstLoad
            {
                Exception dbex = null;
                connection = new OdbcConnectionClass("EXConfig.xml");
                string userQuery = "select * from svu_timesheet_departments";
                DataTable dt2 = connection.ConnectToERP(userQuery, ref dbex);
                if (dbex != null)
                {
                    errMsg.Visible = true;
                    errMsg.ErrorMessage = dbex.Message.ToString();
                    return;
                }

                ArrayList ddlchoices = new ArrayList();
                ddlchoices.Add("");
                foreach (DataRow row in dt2.Rows)
                {
                    string choice = row["departmentname"].ToString() + " (" + row["id"].ToString() + ")";
                    ddlchoices.Add(choice);
                }

                ddlDepartment1choice.DataSource = ddlchoices;
                ddlDepartment1choice.DataBind();
                ddlDepartment2choice.DataSource = ddlchoices;
                ddlDepartment2choice.DataBind();
                ddlDepartment3choice.DataSource = ddlchoices;
                ddlDepartment3choice.DataBind();
                ddlDepartment4choice.DataSource = ddlchoices;
                ddlDepartment4choice.DataBind();
            }
        }

       

        protected void OnDataBound(object sender, EventArgs e)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            for (int i = 0; i < GridView1.Columns.Count; i++)
            {
                TableHeaderCell cell = new TableHeaderCell();
                TextBox txtSearch = new TextBox();
                txtSearch.Attributes["placeholder"] = GridView1.Columns[i].HeaderText;
                txtSearch.CssClass = "search_textbox";
                cell.Controls.Add(txtSearch);
                row.Controls.Add(cell);
            }
            GridView1.HeaderRow.Parent.Controls.AddAt(1, row);
        }
    }
}