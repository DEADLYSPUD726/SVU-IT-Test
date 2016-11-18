using CUS.Jenzabar.OdbcConnectionClass;
using Jenzabar.Portal.Framework.Web.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CUSTimesheet
{
    public partial class Departments_View : PortletViewBase
    {

        OdbcConnectionClass connection;
        DataTable dt = null;

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        override protected void OnInit(EventArgs e)
        {
            btnAddDepartment.Click += btnAddDepartment_Click;
            btnUpdateDepartment.Click += btnUpdateDepartment_Click;
            btnBack.Click += btnBack_Click;
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.ChangeScreenToDefaultView();
        }

        void btnUpdateDepartment_Click(object sender, EventArgs e)
        {
            int index = GridView1.SelectedRow.RowIndex;

            Department olddept = new Department();
            olddept.id = dt.Rows[index]["id"].ToString();

            Department newdept = new Department(txtID.Text, txtDeptName.Text, txtPrimary.Text, txtSecondary.Text, txtTertiary.Text, txtQuaternary.Text);

            Exception dbex = null;
            string updateQuery = "update Tmseprd.dbo.svu_timesheet_departments set id = '" + newdept.id + "', departmentname = '" + newdept.name + "', supervisor1 = '" +
                newdept.primary + "', supervisor2 = '" + newdept.secondary + "', supervisor3 = '" + newdept.tertiary + "', supervisor4 = '" + newdept.quaternary + "' where id = '" +
                olddept.id + "'";
            dt = connection.ConnectToERP(updateQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            RefreshData();
        }

        void btnAddDepartment_Click(object sender, EventArgs e)
        {
            Department dept = new Department(txtID.Text, txtDeptName.Text, txtPrimary.Text, txtSecondary.Text, txtTertiary.Text, txtQuaternary.Text);

            Exception dbex = null;
            string deptInsert = "insert into Tmseprd.dbo.svu_timesheet_departments VALUES ('" + dept.id + "', '" + dept.name + "','" + dept.primary + "','" + dept.secondary +
                "','" + dept.tertiary + "','" + dept.quaternary + "')";
            dt = connection.ConnectToERP(deptInsert, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }

            RefreshData();
        }

        
        public Departments_View()
        {
            this.Load += Departments_View_Load;
        }

        void Departments_View_Load(object sender, EventArgs e)
        {
            RefreshData();

            Exception dbex = null;
            connection = new OdbcConnectionClass("EXConfig.xml");
            string userQuery = "select * from svu_timesheet_user";
            DataTable dt2 = connection.ConnectToERP(userQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }


            GridView2.DataSource = null;
            GridView2.DataSource = dt2;
            GridView2.DataBind();
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridView1.SelectedRow.RowIndex;
            
            txtID.Text = dt.Rows[index]["id"].ToString();
            txtDeptName.Text = dt.Rows[index]["departmentname"].ToString();
            txtPrimary.Text = dt.Rows[index]["supervisor1"].ToString();
            txtSecondary.Text = dt.Rows[index]["supervisor2"].ToString();
            txtTertiary.Text = dt.Rows[index]["supervisor3"].ToString();
            txtQuaternary.Text = dt.Rows[index]["supervisor4"].ToString();
            
           
        }

        private void RefreshData()
        {
            Exception dbex = null;
            connection = new OdbcConnectionClass("EXConfig.xml");
            string userQuery = "select * from svu_timesheet_departments";
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

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            GridView view = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(view, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void OnDataBound(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            for (int i = 0; i < view.Columns.Count; i++)
            {
                TableHeaderCell cell = new TableHeaderCell();
                TextBox txtSearch = new TextBox();
                txtSearch.Attributes["placeholder"] = view.Columns[i].HeaderText;
                txtSearch.CssClass = "search_textbox";
                cell.Controls.Add(txtSearch);
                row.Controls.Add(cell);
            }
            view.HeaderRow.Parent.Controls.AddAt(1, row);
        }

        protected void OnDataBound2(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            for (int i = 0; i < view.Columns.Count; i++)
            {
                TableHeaderCell cell = new TableHeaderCell();
                TextBox txtSearch = new TextBox();
                txtSearch.Attributes["placeholder"] = view.Columns[i].HeaderText;
                txtSearch.CssClass = "search_textbox2";
                cell.Controls.Add(txtSearch);
                row.Controls.Add(cell);
            }
            view.HeaderRow.Parent.Controls.AddAt(1, row);
        }
    }
}