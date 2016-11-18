using CUS.Jenzabar.OdbcConnectionClass;
using Jenzabar.Portal.Framework;
using Jenzabar.Portal.Framework.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jenzabar.Portal.Framework.Facade;
using Jenzabar.Common;

namespace CUSTimesheet
{
    public partial class Supervisor_FillTimesheet_View : PortletViewBase
    {

        OdbcConnectionClass connection;
        DataTable dt = null;
        DataTable dt2 = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsFirstLoad) //IsFirstLoad
            {
                Exception dbex = null;
                connection = new OdbcConnectionClass("EXConfig.xml");

                 

                string datedue = "";
                //Application["monpart11"] = "0";
                //Application["monpart21"] = "0";

                if (Application["datedue"] != null)
                {
                    datedue = Application["datedue"].ToString();
                }

                lbldatedue.Text = datedue;
                lbldatedue2.Text = datedue;

                DataTable dt = new DataTable();
                dt.Columns.Add("date", typeof(String));
                dt.Columns.Add("total", typeof(String));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("date", typeof(String));
                dt2.Columns.Add("total", typeof(String));

                //   07/21/2014
                string visformat = "ddd MM/dd/yyyy";

                DateTime dateduetime = new DateTime(Convert.ToInt32(datedue.ToString().Substring(6, 4)), Convert.ToInt32(datedue.ToString().Substring(0, 2)), Convert.ToInt32(datedue.ToString().Substring(3, 2)));
                DateTime firstweektime = dateduetime.AddDays(-14);
                DateTime secondweektime = dateduetime.AddDays(-7);

                for (int i = 0; i < 7; i++)
                {
                    dt.Rows.Add(firstweektime.ToString(visformat), "0");
                    firstweektime = firstweektime.AddDays(1);
                }

                for (int i = 0; i < 7; i++)
                {
                    dt2.Rows.Add(secondweektime.ToString(visformat), "0");
                    secondweektime = secondweektime.AddDays(1);
                }

               

                GridView1.DataSource = null;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                this.dt = dt;
                Application["dt"] = dt;

                GridView2.DataSource = null;
                GridView2.DataSource = dt2;
                GridView2.DataBind();
                this.dt2 = dt2;
                Application["dt2"] = dt2;
                ArrayList dropdownhours = new ArrayList();

                double time = 0;

                for (int i = 0; i < 161; i++)
                {
                    dropdownhours.Add(time.ToString());
                    time = time + 0.25;

                }
                ddlFMLA1.DataSource = dropdownhours;
                ddlFMLA1.DataBind();

                ddlHB1.DataSource = dropdownhours;
                ddlHB1.DataBind();

                ddlOT1.DataSource = dropdownhours;
                ddlOT1.DataBind();

                ddlPER1.DataSource = dropdownhours;
                ddlPER1.DataBind();

                ddlSHIF1.DataSource = dropdownhours;
                ddlSHIF1.DataBind();

                ddlSICK1.DataSource = dropdownhours;
                ddlSICK1.DataBind();

                ddlVAC1.DataSource = dropdownhours;
                ddlVAC1.DataBind();

                ddlFMLA2.DataSource = dropdownhours;
                ddlFMLA2.DataBind();

                ddlHB2.DataSource = dropdownhours;
                ddlHB2.DataBind();

                ddlOT2.DataSource = dropdownhours;
                ddlOT2.DataBind();

                ddlPER2.DataSource = dropdownhours;
                ddlPER2.DataBind();

                ddlSHIF2.DataSource = dropdownhours;
                ddlSHIF2.DataBind();

                ddlSICK2.DataSource = dropdownhours;
                ddlSICK2.DataBind();

                ddlVAC2.DataSource = dropdownhours;
                ddlVAC2.DataBind();

                //check to see if this is already filled out
                string checkQuery = "select * from Tmseprd.dbo.svu_timesheet_timesheet where id = '" + Application["employeeid"].ToString() + "' and dept = '" + Application["dept"].ToString() + "' and payPeriod = '" + Application["datedue"].ToString() + "'";
                DataTable checkTable = connection.ConnectToERP(checkQuery, ref dbex);
                if (dbex != null)
                {
                    errMsg.Visible = true;
                    errMsg.ErrorMessage = dbex.Message.ToString();
                    return;
                }

                Application["checktable"] = checkTable;
                // errMsg.Visible = true;
                //errMsg.ErrorMessage = checkQuery;
                //return;

                if (checkTable.Rows.Count != 0)
                {


                    lblTotalWeek1.Text = checkTable.Rows[0]["totalweek1"].ToString();
                    lblTotalWeek2.Text = checkTable.Rows[0]["totalweek2"].ToString();
                    lblTotalBothWeeks.Text = checkTable.Rows[0]["totalbothweeks"].ToString();

                    txtSupervisorNotes.Value = checkTable.Rows[0]["supervisorcomments"].ToString();
                    txtUserNotes.Value = checkTable.Rows[0]["usercomments"].ToString();

                    ddlOT1.SelectedValue = checkTable.Rows[0]["ot1"].ToString();
                    ddlOT2.SelectedValue = checkTable.Rows[0]["ot2"].ToString();
                    ddlSICK1.SelectedValue = checkTable.Rows[0]["sick1"].ToString();
                    ddlSICK2.SelectedValue = checkTable.Rows[0]["sick2"].ToString();
                    ddlVAC1.SelectedValue = checkTable.Rows[0]["vac1"].ToString();
                    ddlVAC2.SelectedValue = checkTable.Rows[0]["vac2"].ToString();
                    ddlPER1.SelectedValue = checkTable.Rows[0]["per1"].ToString();
                    ddlPER2.SelectedValue = checkTable.Rows[0]["per2"].ToString();
                    ddlHB1.SelectedValue = checkTable.Rows[0]["hb1"].ToString();
                    ddlHB2.SelectedValue = checkTable.Rows[0]["hb2"].ToString();
                    ddlFMLA1.SelectedValue = checkTable.Rows[0]["fmla1"].ToString();
                    ddlFMLA2.SelectedValue = checkTable.Rows[0]["fmla2"].ToString();
                    ddlSHIF1.SelectedValue = checkTable.Rows[0]["shift1"].ToString();
                    ddlSHIF2.SelectedValue = checkTable.Rows[0]["shift2"].ToString();

                    //monday1

                    ((DropDownList)GridView1.Rows[0].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["m1in1"].ToString();
                    ((DropDownList)GridView1.Rows[0].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["m1out1"].ToString();
                    ((DropDownList)GridView1.Rows[0].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["m1in2"].ToString();
                    ((DropDownList)GridView1.Rows[0].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["m1out2"].ToString();
                    ((Label)GridView1.Rows[0].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["m1"].ToString();
                    GridViewRow mon1row = (GridViewRow)GridView1.Rows[0];
                    HiddenField mon1part1 = (HiddenField)mon1row.FindControl("part1total");
                    HiddenField mon1part2 = (HiddenField)mon1row.FindControl("part2total");
                    double mon1part1num = GetDifference(checkTable.Rows[0]["m1in1"].ToString(), checkTable.Rows[0]["m1out1"].ToString());
                    if (mon1part1num != -1)
                    {
                        mon1part1.Value = mon1part1num.ToString();
                    }
                    double mon1part2num = GetDifference(checkTable.Rows[0]["m1in2"].ToString(), checkTable.Rows[0]["m1out2"].ToString());
                    if (mon1part2num != -1)
                    {
                        mon1part2.Value = mon1part2num.ToString();
                    }

                    //tuesday1
                    ((DropDownList)GridView1.Rows[1].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["t1in1"].ToString();
                    ((DropDownList)GridView1.Rows[1].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["t1out1"].ToString();
                    ((DropDownList)GridView1.Rows[1].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["t1in2"].ToString();
                    ((DropDownList)GridView1.Rows[1].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["t1out2"].ToString();
                    ((Label)GridView1.Rows[1].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["t1"].ToString();
                    GridViewRow tue1row = (GridViewRow)GridView1.Rows[1];
                    HiddenField tue1part1 = (HiddenField)tue1row.FindControl("part1total");
                    HiddenField tue1part2 = (HiddenField)tue1row.FindControl("part2total");
                    double tue1part1num = GetDifference(checkTable.Rows[0]["t1in1"].ToString(), checkTable.Rows[0]["t1out1"].ToString());
                    if (tue1part1num != -1)
                    {
                        tue1part1.Value = tue1part1num.ToString();
                    }
                    double tue1part2num = GetDifference(checkTable.Rows[0]["t1in2"].ToString(), checkTable.Rows[0]["t1out2"].ToString());
                    if (tue1part2num != -1)
                    {
                        tue1part2.Value = tue1part2num.ToString();
                    }

                    //wednesday1
                    ((DropDownList)GridView1.Rows[2].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["w1in1"].ToString();
                    ((DropDownList)GridView1.Rows[2].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["w1out1"].ToString();
                    ((DropDownList)GridView1.Rows[2].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["w1in2"].ToString();
                    ((DropDownList)GridView1.Rows[2].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["w1out2"].ToString();
                    ((Label)GridView1.Rows[2].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["w1"].ToString();
                    GridViewRow wed1row = (GridViewRow)GridView1.Rows[2];
                    HiddenField wed1part1 = (HiddenField)tue1row.FindControl("part1total");
                    HiddenField wed1part2 = (HiddenField)tue1row.FindControl("part2total");
                    double wed1part1num = GetDifference(checkTable.Rows[0]["w1in1"].ToString(), checkTable.Rows[0]["w1out1"].ToString());
                    if (wed1part1num != -1)
                    {
                        wed1part1.Value = wed1part1num.ToString();
                    }
                    double wed1part2num = GetDifference(checkTable.Rows[0]["w1in2"].ToString(), checkTable.Rows[0]["w1out2"].ToString());
                    if (wed1part2num != -1)
                    {
                        wed1part2.Value = wed1part2num.ToString();
                    }

                    //thursday1
                    ((DropDownList)GridView1.Rows[3].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["th1in1"].ToString();
                    ((DropDownList)GridView1.Rows[3].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["th1out1"].ToString();
                    ((DropDownList)GridView1.Rows[3].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["th1in2"].ToString();
                    ((DropDownList)GridView1.Rows[3].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["th1out2"].ToString();
                    ((Label)GridView1.Rows[3].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["th1"].ToString();
                    GridViewRow thur1row = (GridViewRow)GridView1.Rows[3];
                    HiddenField thur1part1 = (HiddenField)thur1row.FindControl("part1total");
                    HiddenField thur1part2 = (HiddenField)thur1row.FindControl("part2total");
                    double thur1part1num = GetDifference(checkTable.Rows[0]["th1in1"].ToString(), checkTable.Rows[0]["th1out1"].ToString());
                    if (thur1part1num != -1)
                    {
                        thur1part1.Value = thur1part1num.ToString();
                    }
                    double thur1part2num = GetDifference(checkTable.Rows[0]["th1in2"].ToString(), checkTable.Rows[0]["th1out2"].ToString());
                    if (thur1part2num != -1)
                    {
                        thur1part2.Value = thur1part2num.ToString();
                    }

                    //friday1
                    ((DropDownList)GridView1.Rows[4].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["f1in1"].ToString();
                    ((DropDownList)GridView1.Rows[4].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["f1out1"].ToString();
                    ((DropDownList)GridView1.Rows[4].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["f1in2"].ToString();
                    ((DropDownList)GridView1.Rows[4].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["f1out2"].ToString();
                    ((Label)GridView1.Rows[4].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["f1"].ToString();
                    GridViewRow fri1row = (GridViewRow)GridView1.Rows[4];
                    HiddenField fri1part1 = (HiddenField)fri1row.FindControl("part1total");
                    HiddenField fri1part2 = (HiddenField)fri1row.FindControl("part2total");
                    double fri1part1num = GetDifference(checkTable.Rows[0]["f1in1"].ToString(), checkTable.Rows[0]["f1out1"].ToString());
                    if (fri1part1num != -1)
                    {
                        fri1part1.Value = fri1part1num.ToString();
                    }
                    double fri1part2num = GetDifference(checkTable.Rows[0]["f1in2"].ToString(), checkTable.Rows[0]["f1out2"].ToString());
                    if (fri1part2num != -1)
                    {
                        fri1part2.Value = fri1part2num.ToString();
                    }

                    //sat1
                    ((DropDownList)GridView1.Rows[5].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["sat1in1"].ToString();
                    ((DropDownList)GridView1.Rows[5].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["sat1out1"].ToString();
                    ((DropDownList)GridView1.Rows[5].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["sat1in2"].ToString();
                    ((DropDownList)GridView1.Rows[5].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["sat1out2"].ToString();
                    ((Label)GridView1.Rows[5].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["sat1"].ToString();
                    GridViewRow sat1row = (GridViewRow)GridView1.Rows[5];
                    HiddenField sat1part1 = (HiddenField)sat1row.FindControl("part1total");
                    HiddenField sat1part2 = (HiddenField)sat1row.FindControl("part2total");
                    double sat1part1num = GetDifference(checkTable.Rows[0]["sat1in1"].ToString(), checkTable.Rows[0]["sat1out1"].ToString());
                    if (sat1part1num != -1)
                    {
                        sat1part1.Value = sat1part1num.ToString();
                    }
                    double sat1part2num = GetDifference(checkTable.Rows[0]["sat1in2"].ToString(), checkTable.Rows[0]["sat1out2"].ToString());
                    if (sat1part2num != -1)
                    {
                        sat1part2.Value = sat1part2num.ToString();
                    }

                    //sun1
                    ((DropDownList)GridView1.Rows[6].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["sun1in1"].ToString();
                    ((DropDownList)GridView1.Rows[6].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["sun1out1"].ToString();
                    ((DropDownList)GridView1.Rows[6].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["sun1in2"].ToString();
                    ((DropDownList)GridView1.Rows[6].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["sun1out2"].ToString();
                    ((Label)GridView1.Rows[6].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["sun1"].ToString();
                    GridViewRow sun1row = (GridViewRow)GridView1.Rows[6];
                    HiddenField sun1part1 = (HiddenField)sun1row.FindControl("part1total");
                    HiddenField sun1part2 = (HiddenField)sun1row.FindControl("part2total");
                    double sun1part1num = GetDifference(checkTable.Rows[0]["sun1in1"].ToString(), checkTable.Rows[0]["sun1out1"].ToString());
                    if (sun1part1num != -1)
                    {
                        sun1part1.Value = sun1part1num.ToString();
                    }
                    double sun1part2num = GetDifference(checkTable.Rows[0]["sun1in2"].ToString(), checkTable.Rows[0]["sun1out2"].ToString());
                    if (sun1part2num != -1)
                    {
                        sun1part2.Value = sun1part2num.ToString();
                    }

                    /////////////////**********week 2*****************////////////////

                    //monday2
                    ((DropDownList)GridView2.Rows[0].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["m2in1"].ToString();
                    ((DropDownList)GridView2.Rows[0].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["m2out1"].ToString();
                    ((DropDownList)GridView2.Rows[0].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["m2in2"].ToString();
                    ((DropDownList)GridView2.Rows[0].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["m2out2"].ToString();
                    ((Label)GridView2.Rows[0].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["m2"].ToString();
                    GridViewRow mon2row = (GridViewRow)GridView2.Rows[0];
                    HiddenField mon2part1 = (HiddenField)mon2row.FindControl("part1total");
                    HiddenField mon2part2 = (HiddenField)mon2row.FindControl("part2total");
                    double mon2part1num = GetDifference(checkTable.Rows[0]["m2in1"].ToString(), checkTable.Rows[0]["m2out1"].ToString());
                    if (mon2part1num != -1)
                    {
                        mon2part1.Value = mon2part1num.ToString();
                    }
                    double mon2part2num = GetDifference(checkTable.Rows[0]["m2in2"].ToString(), checkTable.Rows[0]["m2out2"].ToString());
                    if (mon2part2num != -1)
                    {
                        mon2part2.Value = mon2part2num.ToString();
                    }

                    //tuesday2
                    ((DropDownList)GridView2.Rows[1].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["t2in1"].ToString();
                    ((DropDownList)GridView2.Rows[1].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["t2out1"].ToString();
                    ((DropDownList)GridView2.Rows[1].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["t2in2"].ToString();
                    ((DropDownList)GridView2.Rows[1].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["t2out2"].ToString();
                    ((Label)GridView2.Rows[1].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["t2"].ToString();
                    GridViewRow tue2row = (GridViewRow)GridView2.Rows[1];
                    HiddenField tue2part1 = (HiddenField)tue2row.FindControl("part1total");
                    HiddenField tue2part2 = (HiddenField)tue2row.FindControl("part2total");
                    double tue2part1num = GetDifference(checkTable.Rows[0]["t2in1"].ToString(), checkTable.Rows[0]["t2out1"].ToString());
                    if (tue2part1num != -1)
                    {
                        tue2part1.Value = tue2part1num.ToString();
                    }
                    double tue2part2num = GetDifference(checkTable.Rows[0]["t2in2"].ToString(), checkTable.Rows[0]["t2out2"].ToString());
                    if (tue2part2num != -1)
                    {
                        tue2part2.Value = tue2part2num.ToString();
                    }

                    //wednesday2
                    ((DropDownList)GridView2.Rows[2].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["w2in1"].ToString();
                    ((DropDownList)GridView2.Rows[2].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["w2out1"].ToString();
                    ((DropDownList)GridView2.Rows[2].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["w2in2"].ToString();
                    ((DropDownList)GridView2.Rows[2].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["w2out2"].ToString();
                    ((Label)GridView2.Rows[2].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["w2"].ToString();
                    GridViewRow wed2row = (GridViewRow)GridView2.Rows[2];
                    HiddenField wed2part1 = (HiddenField)wed2row.FindControl("part1total");
                    HiddenField wed2part2 = (HiddenField)wed2row.FindControl("part2total");
                    double wed2part1num = GetDifference(checkTable.Rows[0]["w2in1"].ToString(), checkTable.Rows[0]["w2out1"].ToString());
                    if (wed2part1num != -1)
                    {
                        wed2part1.Value = wed2part1num.ToString();
                    }
                    double wed2part2num = GetDifference(checkTable.Rows[0]["w2in2"].ToString(), checkTable.Rows[0]["w2out2"].ToString());
                    if (wed2part2num != -1)
                    {
                        wed2part2.Value = wed2part2num.ToString();
                    }

                    //thursday2
                    ((DropDownList)GridView2.Rows[3].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["th2in1"].ToString();
                    ((DropDownList)GridView2.Rows[3].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["th2out1"].ToString();
                    ((DropDownList)GridView2.Rows[3].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["th2in2"].ToString();
                    ((DropDownList)GridView2.Rows[3].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["th2out2"].ToString();
                    ((Label)GridView2.Rows[3].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["th2"].ToString();
                    GridViewRow thur2row = (GridViewRow)GridView2.Rows[3];
                    HiddenField thur2part1 = (HiddenField)thur2row.FindControl("part1total");
                    HiddenField thur2part2 = (HiddenField)thur2row.FindControl("part2total");
                    double thur2part1num = GetDifference(checkTable.Rows[0]["th2in1"].ToString(), checkTable.Rows[0]["th2out1"].ToString());
                    if (thur2part1num != -1)
                    {
                        thur2part1.Value = thur2part1num.ToString();
                    }
                    double thur2part2num = GetDifference(checkTable.Rows[0]["th2in2"].ToString(), checkTable.Rows[0]["th2out2"].ToString());
                    if (thur2part2num != -1)
                    {
                        thur2part2.Value = thur2part2num.ToString();
                    }

                    //friday2
                    ((DropDownList)GridView2.Rows[4].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["f2in1"].ToString();
                    ((DropDownList)GridView2.Rows[4].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["f2out1"].ToString();
                    ((DropDownList)GridView2.Rows[4].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["f2in2"].ToString();
                    ((DropDownList)GridView2.Rows[4].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["f2out2"].ToString();
                    ((Label)GridView2.Rows[4].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["f2"].ToString();
                    GridViewRow fri2row = (GridViewRow)GridView2.Rows[4];
                    HiddenField fri2part1 = (HiddenField)fri2row.FindControl("part1total");
                    HiddenField fri2part2 = (HiddenField)fri2row.FindControl("part2total");
                    double fri2part1num = GetDifference(checkTable.Rows[0]["f2in1"].ToString(), checkTable.Rows[0]["f2out1"].ToString());
                    if (fri2part1num != -1)
                    {
                        fri2part1.Value = fri2part1num.ToString();
                    }
                    double fri2part2num = GetDifference(checkTable.Rows[0]["f2in2"].ToString(), checkTable.Rows[0]["f2out2"].ToString());
                    if (fri2part2num != -1)
                    {
                        fri2part2.Value = fri2part2num.ToString();
                    }

                    //sat2
                    ((DropDownList)GridView2.Rows[5].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["sat2in1"].ToString();
                    ((DropDownList)GridView2.Rows[5].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["sat2out1"].ToString();
                    ((DropDownList)GridView2.Rows[5].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["sat2in2"].ToString();
                    ((DropDownList)GridView2.Rows[5].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["sat2out2"].ToString();
                    ((Label)GridView2.Rows[5].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["sat2"].ToString();
                    GridViewRow sat2row = (GridViewRow)GridView2.Rows[5];
                    HiddenField sat2part1 = (HiddenField)sat2row.FindControl("part1total");
                    HiddenField sat2part2 = (HiddenField)sat2row.FindControl("part2total");
                    double sat2part1num = GetDifference(checkTable.Rows[0]["sat2in1"].ToString(), checkTable.Rows[0]["sat2out1"].ToString());
                    if (sat2part1num != -1)
                    {
                        sat2part1.Value = sat2part1num.ToString();
                    }
                    double sat2part2num = GetDifference(checkTable.Rows[0]["sat2in2"].ToString(), checkTable.Rows[0]["sat2out2"].ToString());
                    if (sat2part2num != -1)
                    {
                        sat2part2.Value = sat2part2num.ToString();
                    }

                    //sun2
                    ((DropDownList)GridView2.Rows[6].Cells[1].FindControl("ddlin1")).SelectedValue = checkTable.Rows[0]["sun2in1"].ToString();
                    ((DropDownList)GridView2.Rows[6].Cells[2].FindControl("ddlout1")).SelectedValue = checkTable.Rows[0]["sun2out1"].ToString();
                    ((DropDownList)GridView2.Rows[6].Cells[3].FindControl("ddlin2")).SelectedValue = checkTable.Rows[0]["sun2in2"].ToString();
                    ((DropDownList)GridView2.Rows[6].Cells[4].FindControl("ddlout2")).SelectedValue = checkTable.Rows[0]["sun2out2"].ToString();
                    ((Label)GridView2.Rows[6].Cells[5].FindControl("hoursworked")).Text = checkTable.Rows[0]["sun2"].ToString();
                    GridViewRow sun2row = (GridViewRow)GridView2.Rows[6];
                    HiddenField sun2part1 = (HiddenField)sun2row.FindControl("part1total");
                    HiddenField sun2part2 = (HiddenField)sun2row.FindControl("part2total");
                    double sun2part1num = GetDifference(checkTable.Rows[0]["sun2in1"].ToString(), checkTable.Rows[0]["sun2out1"].ToString());
                    if (sun2part1num != -1)
                    {
                        sun2part1.Value = sun2part1num.ToString();
                    }
                    double sun2part2num = GetDifference(checkTable.Rows[0]["sun2in2"].ToString(), checkTable.Rows[0]["sun2out2"].ToString());
                    if (sun2part2num != -1)
                    {
                        sun2part2.Value = sun2part2num.ToString();
                    }


                    DateTime timesheettime = new DateTime(Convert.ToInt32(checkTable.Rows[0]["payPeriod"].ToString().Substring(6, 4)), Convert.ToInt32(checkTable.Rows[0]["payPeriod"].ToString().Substring(0, 2)), Convert.ToInt32(checkTable.Rows[0]["payPeriod"].ToString().Substring(3, 2)));
                    DateTime startofpayperiod = timesheettime.AddDays(-14);
                    DateTime endofpayperiod = timesheettime.AddDays(-1);


                    //get users name
                    string getNameQuery = "SELECT * FROM TmsEPrd.dbo.NAME_MASTER  where ID_NUM = '" + Application["employeeid"].ToString() + "'";
                    DataTable nameTable = connection.ConnectToERP(getNameQuery, ref dbex);
                    if (dbex != null)
                    {
                        errMsg.Visible = true;
                        errMsg.ErrorMessage = dbex.Message.ToString();
                        return;
                    }

                   
                    //return;

                    if (nameTable.Rows.Count != 0)
                    {
                        lblPersonName.InnerText = nameTable.Rows[0]["FIRST_NAME"].ToString() + " " + nameTable.Rows[0]["LAST_NAME"].ToString() + " " + startofpayperiod.ToString(visformat) + " - " + endofpayperiod.ToString(visformat);
                    }
                    // end get user name




                    ArrayList nameList = new ArrayList();
                    nameList.Add("ddlin1");
                    nameList.Add("ddlout1");
                    nameList.Add("ddlin2");
                    nameList.Add("ddlout2");

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        foreach (string s in nameList)
                        {
                            DropDownList temp = (DropDownList)(GridView1.Rows[i].FindControl(s));
                            if (temp.SelectedValue.Equals("Select Time"))
                            {

                                temp.ForeColor = System.Drawing.Color.Gray;
                            }
                            else
                            {
                                temp.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                    }

                    for (int i = 0; i < GridView2.Rows.Count; i++)
                    {
                        foreach (string s in nameList)
                        {
                            DropDownList temp = (DropDownList)(GridView2.Rows[i].FindControl(s));
                            if (temp.SelectedValue.Equals("Select Time"))
                            {

                                temp.ForeColor = System.Drawing.Color.Gray;
                            }
                            else
                            {
                                temp.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                    }


                }
            }
        }

        override protected void OnInit(EventArgs e)
        {
            btnApprove.Click += btnApprove_Click;
            btnBack.Click += btnBack_Click;
            btnReject.Click += btnReject_Click;
        }

        private double GetDifference(string intimestr, string outtimestr)
        {
            if (!intimestr.Equals("Select Time") && !outtimestr.Equals("Select Time"))
            {
                var dateNow = DateTime.Now;
                //12:30 AM
                int inhour = Convert.ToInt32(intimestr.Substring(0, 2));
                if (inhour == 12 && !intimestr.Substring(6, 2).Equals("PM"))
                {
                    inhour = 0;
                }
                int inmin = Convert.ToInt32(intimestr.Substring(3, 2));
                if (intimestr.Substring(6, 2).Equals("PM") && inhour != 12)
                {
                    inhour = inhour + 12;
                }
                DateTime intime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, inhour, inmin, 0);

                int outhour = Convert.ToInt32(outtimestr.Substring(0, 2));
                if (outhour == 12 && !outtimestr.Substring(6, 2).Equals("PM"))
                {
                    outhour = 0;
                }
                int outmin = Convert.ToInt32(outtimestr.Substring(3, 2));
                if (outtimestr.Substring(6, 2).Equals("PM") && outhour != 12)
                {
                    outhour = outhour + 12;
                }
                DateTime outtime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, outhour, outmin, 0);
                if (outtime < intime)
                {
                    outtime = outtime.AddDays(1);//need to add a day here if time is less
                }

                int hours = (outtime - intime).Hours;

                double minssworked = (outtime - intime).Minutes;
                double minsdivided = minssworked / 60;



                string totalstring = hours.ToString() + minsdivided.ToString().Substring(1, minsdivided.ToString().Length - 1);
                //Application["monpart11"] = Convert.ToDouble(totalstring);

                return (Convert.ToDouble(totalstring));
            }
            return -1;
        }

        void btnReject_Click(object sender, EventArgs e)
        {
           

            Exception dbex = null;
            connection = new OdbcConnectionClass("EXConfig.xml");
            string updateQuery = "update Tmseprd.dbo.svu_timesheet_timesheet set status = '5', supervisorcomments = '" + txtSupervisorNotes.Value + "' where id = '" + Application["employeeid"].ToString() + "' and dept = '" + Application["dept"].ToString() + "' and payPeriod = '" + Application["datedue"].ToString() + "'";
            dt = connection.ConnectToERP(updateQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }

            //get this person's email address
            string getSupervisorIDQuery = "SELECT EMAIL_ADDRESS FROM TmsEPrd.dbo.NAME_MASTER where ID_NUM = '" + PortalUser.Current.HostID.ToString() + "'";
            DataTable superTable = connection.ConnectToERP(getSupervisorIDQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = checkQuery;
            //return;
            string superEmail = "";
            if (superTable.Rows.Count != 0)
            {
                 superEmail = superTable.Rows[0]["EMAIL_ADDRESS"].ToString();
               
            }



            //from,to,subject,message
            string getEmployeeIDQuery = "SELECT EMAIL_ADDRESS FROM TmsEPrd.dbo.NAME_MASTER where ID_NUM = '" + Application["employeeid"].ToString() + "'";
            DataTable checkTable = connection.ConnectToERP(getEmployeeIDQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = checkTable.Rows[0]["EMAIL_ADDRESS"].ToString();
            //return;

            if (checkTable.Rows.Count != 0)
            {
                string userEmail = checkTable.Rows[0]["EMAIL_ADDRESS"].ToString();
                Jenzabar.Common.Mail.Email.CreateAndSendMailMessage(superEmail.Trim(), userEmail.Trim(), "Timesheet rejected", "A timesheet you submitted has been sent back by your supervisor to fix problems.  Please check the supervisor comments to see why the supervisor rejected the timesheet.");
            }


            
            
            
            


            this.ParentPortlet.ChangeScreenToDefaultView();
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.ChangeScreenToDefaultView();
        }

        void btnApprove_Click(object sender, EventArgs e)
        {
            DataTable originalTable = (DataTable)Application["checktable"];

            DateTime time = DateTime.Now;
            string format = "yyyyMMddHHmmss";
            connection = new OdbcConnectionClass("EXConfig.xml");
            Exception dbex = null;
            //we need to check if it's already been saved
            string checkQuery = "select * from Tmseprd.dbo.svu_timesheet_timesheet where id = '" + Application["employeeid"].ToString() + "' and dept = '" + Application["dept"].ToString() + "' and payPeriod = '" + Application["datedue"].ToString() + "'";
            DataTable checkTable = connection.ConnectToERP(checkQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = checkQuery;
            //return;

            if (checkTable.Rows.Count != 0)
            {
                //it's already been saved, we need to update it instead
                dbex = null;
                string deleteoldone = "delete from Tmseprd.dbo.svu_timesheet_timesheet where id = '" + Application["employeeid"].ToString() + "' and dept = '" + Application["dept"].ToString() + "' and payPeriod = '" + Application["datedue"].ToString() + "'";
                connection.ConnectToERP(deleteoldone, ref dbex);
                if (dbex != null)
                {
                    errMsg.Visible = true;
                    errMsg.ErrorMessage = dbex.Message.ToString();
                    return;
                }
            }

            //timesheet not found, we need to insert it  //28
            //87 columns

            double totalot = Convert.ToDouble(ddlOT1.SelectedValue.ToString()) + Convert.ToDouble(ddlOT2.SelectedValue.ToString());
            double totalsick = Convert.ToDouble(ddlSICK1.SelectedValue.ToString()) + Convert.ToDouble(ddlSICK2.SelectedValue.ToString());
            double totalvac = Convert.ToDouble(ddlVAC1.SelectedValue.ToString()) + Convert.ToDouble(ddlVAC2.SelectedValue.ToString());
            double totalper = Convert.ToDouble(ddlPER1.SelectedValue.ToString()) + Convert.ToDouble(ddlPER2.SelectedValue.ToString());
            double totalhb = Convert.ToDouble(ddlHB1.SelectedValue.ToString()) + Convert.ToDouble(ddlHB2.SelectedValue.ToString());
            double totalshift = Convert.ToDouble(ddlSHIF1.SelectedValue.ToString()) + Convert.ToDouble(ddlSHIF2.SelectedValue.ToString());
            double totalfmla = Convert.ToDouble(ddlFMLA1.SelectedValue.ToString()) + Convert.ToDouble(ddlFMLA2.SelectedValue.ToString());
            DateTime endofpayperiod = time.AddDays(-1);





            dbex = null;
            string userQuery = "insert into Tmseprd.dbo.svu_timesheet_timesheet VALUES ('" + Application["employeeid"].ToString() + "', '" + Application["dept"].ToString() + "', '" +
                    Application["datedue"].ToString() + "', '" + originalTable.Rows[0]["dateSubmitted"].ToString() + "', '" + "4" + "', '" + ((Label)GridView1.Rows[0].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[1].Cells[5].FindControl("hoursworked")).Text +
                    "', '" + ((Label)GridView1.Rows[2].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[3].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[4].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[5].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[6].Cells[5].FindControl("hoursworked")).Text + "', '" +
                    ((Label)GridView2.Rows[0].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[1].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[2].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[3].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[4].Cells[5].FindControl("hoursworked")).Text + "', '" +
                   ((Label)GridView2.Rows[5].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[6].Cells[5].FindControl("hoursworked")).Text + "', '" + ddlOT1.SelectedValue.ToString() + "', '" + ddlOT2.SelectedValue.ToString() +
                     "', '" + ddlSICK1.SelectedValue.ToString() + "', '" + ddlSICK2.SelectedValue.ToString() + "', '" + ddlVAC1.SelectedValue.ToString() + "', '" + ddlVAC2.SelectedValue.ToString() +
                      "', '" + ddlPER1.SelectedValue.ToString() + "', '" + ddlPER2.SelectedValue.ToString() + "', '" + ddlHB1.SelectedValue.ToString() + "', '" + ddlHB2.SelectedValue.ToString() + "', '" + ddlFMLA1.SelectedValue.ToString() + "', '" + ddlFMLA2.SelectedValue.ToString() +
                       "', '" + ddlSHIF1.SelectedValue.ToString() + "', '" + ddlSHIF2.SelectedValue.ToString() +
                       "', '" + ((DropDownList)GridView1.Rows[0].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[0].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[0].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[0].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[0].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[0].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[0].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[0].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView1.Rows[1].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[1].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[1].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[1].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[1].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[1].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[1].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[1].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView1.Rows[2].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[2].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[2].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[2].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[2].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[2].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[2].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[2].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView1.Rows[3].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[3].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[3].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[3].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[3].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[3].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[3].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[3].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView1.Rows[4].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[4].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[4].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[4].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[4].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[4].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[4].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[4].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView1.Rows[5].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[5].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[5].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[5].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[5].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[5].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[5].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[5].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView1.Rows[6].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[6].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[6].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[6].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '" + ((DropDownList)GridView2.Rows[6].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[6].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[6].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[6].Cells[4].FindControl("ddlout2")).SelectedValue +
                        "', '" + txtUserNotes.Value + "', '" + txtSupervisorNotes.Value + "', '" + lblTotalWeek1.Text + "', '" + lblTotalWeek2.Text + "', '" + lblTotalBothWeeks.Text +
                         "', '" + totalot.ToString() + "', '" + totalsick.ToString() + "', '" + totalvac.ToString() + "', '" + totalper.ToString() +
                          "', '" + totalhb.ToString() + "', '" + totalshift.ToString() + "', '" + totalfmla.ToString() + "', '" + endofpayperiod.ToString(format) + "', '" + PortalUser.Current.HostID.ToString() + "', '" + time.ToString(format) + "')";
            connection.ConnectToERP(userQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                errMsg.ErrorMessage += " size: " + ddlOT1.SelectedValue.ToString().Length + " = " + ddlOT1.SelectedValue.ToString();
                return;
            }
            dbex = null;
            //original table
            string changeQuery = "insert into Tmseprd.dbo.svu_timesheet_timesheet_changes VALUES ('" + Application["employeeid"].ToString() + "', '" + Application["dept"].ToString() + "', '" +
                   Application["datedue"].ToString() + "', '" + originalTable.Rows[0]["dateSubmitted"].ToString() + "', '" + "4" + "', '" + originalTable.Rows[0]["m1"].ToString() + "', '" + originalTable.Rows[0]["t1"].ToString() +
                   "', '" + originalTable.Rows[0]["w1"].ToString() + "', '" + originalTable.Rows[0]["th1"].ToString() + "', '" + originalTable.Rows[0]["f1"].ToString() + "', '" + originalTable.Rows[0]["sat1"].ToString() + "', '" + originalTable.Rows[0]["sun1"].ToString() + "', '" +
                  originalTable.Rows[0]["m2"].ToString() + "', '" + originalTable.Rows[0]["t2"].ToString() + "', '" + originalTable.Rows[0]["w2"].ToString() + "', '" + originalTable.Rows[0]["th2"].ToString() + "', '" + originalTable.Rows[0]["f2"].ToString() + "', '" +
                 originalTable.Rows[0]["sat2"].ToString() + "', '" + originalTable.Rows[0]["sun2"].ToString() + "', '" + originalTable.Rows[0]["ot1"].ToString() + "', '" + originalTable.Rows[0]["ot2"].ToString() +
                    "', '" + originalTable.Rows[0]["sick1"].ToString() + "', '" + originalTable.Rows[0]["sick2"].ToString() + "', '" + originalTable.Rows[0]["vac1"].ToString() + "', '" + originalTable.Rows[0]["vac2"].ToString() +
                     "', '" + originalTable.Rows[0]["per1"].ToString() + "', '" + originalTable.Rows[0]["per2"].ToString() + "', '" + originalTable.Rows[0]["hb1"].ToString() + "', '" + originalTable.Rows[0]["hb2"].ToString() + "', '" + originalTable.Rows[0]["fmla1"].ToString() + "', '" + originalTable.Rows[0]["fmla2"].ToString() +
                      "', '" + originalTable.Rows[0]["shift1"].ToString() + "', '" + originalTable.Rows[0]["shift2"].ToString() +

                      "', '" + originalTable.Rows[0]["m1in1"].ToString() + "', '" + originalTable.Rows[0]["m1in2"].ToString() + "', '" + originalTable.Rows[0]["m1out1"].ToString() + "', '" + originalTable.Rows[0]["m1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["m2in1"].ToString() + "', '" + originalTable.Rows[0]["m2in2"].ToString() + "', '" + originalTable.Rows[0]["m2out1"].ToString() + "', '" + originalTable.Rows[0]["m2out2"].ToString() +
                      "', '" + originalTable.Rows[0]["t1in1"].ToString() + "', '" + originalTable.Rows[0]["t1in2"].ToString() + "', '" + originalTable.Rows[0]["t1out1"].ToString() + "', '" + originalTable.Rows[0]["t1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["t2in1"].ToString() + "', '" + originalTable.Rows[0]["t2in2"].ToString() + "', '" + originalTable.Rows[0]["t2out1"].ToString() + "', '" + originalTable.Rows[0]["t2out2"].ToString() +
                      "', '" + originalTable.Rows[0]["w1in1"].ToString() + "', '" + originalTable.Rows[0]["w1in2"].ToString() + "', '" + originalTable.Rows[0]["w1out1"].ToString() + "', '" + originalTable.Rows[0]["w1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["w2in1"].ToString() + "', '" + originalTable.Rows[0]["w2in2"].ToString() + "', '" + originalTable.Rows[0]["w2out1"].ToString() + "', '" + originalTable.Rows[0]["w2out2"].ToString() +
                      "', '" + originalTable.Rows[0]["th1in1"].ToString() + "', '" + originalTable.Rows[0]["th1in2"].ToString() + "', '" + originalTable.Rows[0]["th1out1"].ToString() + "', '" + originalTable.Rows[0]["th1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["th2in1"].ToString() + "', '" + originalTable.Rows[0]["th2in2"].ToString() + "', '" + originalTable.Rows[0]["th2out1"].ToString() + "', '" + originalTable.Rows[0]["th2out2"].ToString() +
                      "', '" + originalTable.Rows[0]["f1in1"].ToString() + "', '" + originalTable.Rows[0]["f1in2"].ToString() + "', '" + originalTable.Rows[0]["f1out1"].ToString() + "', '" + originalTable.Rows[0]["f1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["f2in1"].ToString() + "', '" + originalTable.Rows[0]["f2in2"].ToString() + "', '" + originalTable.Rows[0]["f2out1"].ToString() + "', '" + originalTable.Rows[0]["f2out2"].ToString() +
                      "', '" + originalTable.Rows[0]["sat1in1"].ToString() + "', '" + originalTable.Rows[0]["sat1in2"].ToString() + "', '" + originalTable.Rows[0]["sat1out1"].ToString() + "', '" + originalTable.Rows[0]["sat1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["sat2in1"].ToString() + "', '" + originalTable.Rows[0]["sat2in2"].ToString() + "', '" + originalTable.Rows[0]["sat2out1"].ToString() + "', '" + originalTable.Rows[0]["sat2out2"].ToString() +
                      "', '" + originalTable.Rows[0]["sun1in1"].ToString() + "', '" + originalTable.Rows[0]["sun1in2"].ToString() + "', '" + originalTable.Rows[0]["sun1out1"].ToString() + "', '" + originalTable.Rows[0]["sun1out2"].ToString() +
                      "', '" + originalTable.Rows[0]["sun2in1"].ToString() + "', '" + originalTable.Rows[0]["sun2in2"].ToString() + "', '" + originalTable.Rows[0]["sun2out1"].ToString() + "', '" + originalTable.Rows[0]["sun2out2"].ToString() +

                       "', '" + originalTable.Rows[0]["usercomments"].ToString() + "', '" + originalTable.Rows[0]["supervisorcomments"].ToString() + "', '" + originalTable.Rows[0]["totalweek1"].ToString() + "', '" + originalTable.Rows[0]["totalweek2"].ToString() + "', '" + originalTable.Rows[0]["totalbothweeks"].ToString() +
                        "', '" + originalTable.Rows[0]["totalot"].ToString() + "', '" + originalTable.Rows[0]["totalsick"].ToString() + "', '" + originalTable.Rows[0]["totalvac"].ToString() + "', '" + originalTable.Rows[0]["totalper"].ToString() +
                         "', '" + originalTable.Rows[0]["totalhb"].ToString() + "', '" + originalTable.Rows[0]["totalshift"].ToString() + "', '" + originalTable.Rows[0]["totalfmla"].ToString() + "', '" + originalTable.Rows[0]["endofpayperiod"].ToString() + "', '" + PortalUser.Current.HostID.ToString() + "', '" + time.ToString(format) +
                          "', '" + ((Label)GridView1.Rows[0].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[1].Cells[5].FindControl("hoursworked")).Text +
                   "', '" + ((Label)GridView1.Rows[2].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[3].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[4].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[5].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView1.Rows[6].Cells[5].FindControl("hoursworked")).Text + "', '" +
                   ((Label)GridView2.Rows[0].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[1].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[2].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[3].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[4].Cells[5].FindControl("hoursworked")).Text + "', '" +
                  ((Label)GridView2.Rows[5].Cells[5].FindControl("hoursworked")).Text + "', '" + ((Label)GridView2.Rows[6].Cells[5].FindControl("hoursworked")).Text + "', '" + ddlOT1.SelectedValue.ToString() + "', '" + ddlOT2.SelectedValue.ToString() +
                    "', '" + ddlSICK1.SelectedValue.ToString() + "', '" + ddlSICK2.SelectedValue.ToString() + "', '" + ddlVAC1.SelectedValue.ToString() + "', '" + ddlVAC2.SelectedValue.ToString() +
                     "', '" + ddlPER1.SelectedValue.ToString() + "', '" + ddlPER2.SelectedValue.ToString() + "', '" + ddlHB1.SelectedValue.ToString() + "', '" + ddlHB2.SelectedValue.ToString() + "', '" + ddlFMLA1.SelectedValue.ToString() + "', '" + ddlFMLA2.SelectedValue.ToString() +
                      "', '" + ddlSHIF1.SelectedValue.ToString() + "', '" + ddlSHIF2.SelectedValue.ToString() +
                      "', '" + ((DropDownList)GridView1.Rows[0].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[0].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[0].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[0].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[0].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[0].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[0].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[0].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView1.Rows[1].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[1].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[1].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[1].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[1].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[1].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[1].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[1].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView1.Rows[2].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[2].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[2].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[2].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[2].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[2].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[2].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[2].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView1.Rows[3].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[3].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[3].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[3].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[3].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[3].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[3].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[3].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView1.Rows[4].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[4].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[4].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[4].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[4].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[4].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[4].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[4].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView1.Rows[5].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[5].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[5].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[5].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[5].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[5].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[5].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[5].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView1.Rows[6].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[6].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[6].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView1.Rows[6].Cells[4].FindControl("ddlout2")).SelectedValue +
                      "', '" + ((DropDownList)GridView2.Rows[6].Cells[1].FindControl("ddlin1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[6].Cells[3].FindControl("ddlin2")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[6].Cells[2].FindControl("ddlout1")).SelectedValue + "', '" + ((DropDownList)GridView2.Rows[6].Cells[4].FindControl("ddlout2")).SelectedValue +
                       "', '"  + lblTotalWeek1.Text + "', '" + lblTotalWeek2.Text + "', '" + lblTotalBothWeeks.Text +
                        "', '" + totalot.ToString() + "', '" + totalsick.ToString() + "', '" + totalvac.ToString() + "', '" + totalper.ToString() +
                         "', '" + totalhb.ToString() + "', '" + totalshift.ToString() + "', '" + totalfmla.ToString() + "')";
            connection.ConnectToERP(changeQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                errMsg.ErrorMessage += " size: " + ddlOT1.SelectedValue.ToString().Length + " = " + ddlOT1.SelectedValue.ToString();
                return;
            }

            //get this person's email address
            string getSupervisorIDQuery = "SELECT EMAIL_ADDRESS FROM TmsEPrd.dbo.NAME_MASTER where ID_NUM = '" + PortalUser.Current.HostID.ToString() + "'";
            DataTable superTable = connection.ConnectToERP(getSupervisorIDQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = checkQuery;
            //return;
            string superEmail = "";
            if (superTable.Rows.Count != 0)
            {
                superEmail = superTable.Rows[0]["EMAIL_ADDRESS"].ToString();

            }

            string getEmployeeIDQuery = "SELECT EMAIL_ADDRESS FROM TmsEPrd.dbo.NAME_MASTER where ID_NUM = '" + Application["employeeid"].ToString() + "'";
            DataTable employeeTable = connection.ConnectToERP(getEmployeeIDQuery, ref dbex);
            if (dbex != null)
            {
                errMsg.Visible = true;
                errMsg.ErrorMessage = dbex.Message.ToString();
                return;
            }
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = checkQuery;
            //return;

            if (employeeTable.Rows.Count != 0)
            {
                string userEmail = employeeTable.Rows[0]["EMAIL_ADDRESS"].ToString();
                Jenzabar.Common.Mail.Email.CreateAndSendMailMessage(superEmail.Trim(), userEmail.Trim(), "Timesheet approved", "Your timesheet that was due on " + Application["datedue"].ToString() + " has been approved!");
            }


            this.ParentPortlet.ChangeScreenToDefaultView();
        }

        private void UpdateWeeklyTotals()
        {
            double week1total = 0;
            foreach (GridViewRow row in GridView1.Rows)
            {
                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                week1total = week1total + Convert.ToDouble(hoursworkedlabel.Text);
            }
            lblTotalWeek1.Text = week1total.ToString();

            double week2total = 0;
            foreach (GridViewRow row in GridView2.Rows)
            {
                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                week2total = week2total + Convert.ToDouble(hoursworkedlabel.Text);
            }
            lblTotalWeek2.Text = week2total.ToString();
            lblTotalBothWeeks.Text = (week1total + week2total).ToString();


        }

        protected void ddlIn1Change(object sender, EventArgs e)
        {
            DropDownList ddlin = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlin.NamingContainer;
            int rowindex = row.RowIndex;
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = rowindex.ToString();
            DropDownList ddlout = (DropDownList)row.FindControl("ddlout1");

            if (ddlin.SelectedValue.Equals("Select Time"))
            {

                ddlin.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                ddlin.ForeColor = System.Drawing.Color.Black;
            }

            if (!ddlout.SelectedValue.Equals("Select Time") && !ddlin.SelectedValue.Equals("Select Time"))
            {
                //both are times, now we should see convert them to datetimes
                var dateNow = DateTime.Now;
                //12:30 AM
                int inhour = Convert.ToInt32(ddlin.SelectedValue.Substring(0, 2));
                if (inhour == 12 && !ddlin.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    inhour = 0;
                }
                int inmin = Convert.ToInt32(ddlin.SelectedValue.Substring(3, 2));
                if (ddlin.SelectedValue.Substring(6, 2).Equals("PM") && inhour != 12)
                {
                    inhour = inhour + 12;
                }
                DateTime intime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, inhour, inmin, 0);

                int outhour = Convert.ToInt32(ddlout.SelectedValue.Substring(0, 2));
                if (outhour == 12 && !ddlout.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    outhour = 0;
                }
                int outmin = Convert.ToInt32(ddlout.SelectedValue.Substring(3, 2));
                if (ddlout.SelectedValue.Substring(6, 2).Equals("PM") && outhour != 12)
                {
                    outhour = outhour + 12;
                }
                DateTime outtime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, outhour, outmin, 0);
                if (outtime < intime)
                {
                    outtime = outtime.AddDays(1);//need to add a day here if time is less
                }

                int hours = (outtime - intime).Hours;

                double minssworked = (outtime - intime).Minutes;
                double minsdivided = minssworked / 60;


                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                string totalstring = hours.ToString() + minsdivided.ToString().Substring(1, minsdivided.ToString().Length - 1);
                //Application["monpart11"] = Convert.ToDouble(totalstring);
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part1.Value = Convert.ToDouble(totalstring).ToString();
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
                //errMsg.Visible = true;
                //errMsg.ErrorMessage = "mins: " + minssworked + " hours: " + hours.ToString() + " divided: " + minsdivided.ToString();

            }
            else
            {
                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part1.Value = "0";
                //Application["monpart11"] = "0";
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
            }
            UpdateWeeklyTotals();

        }

        protected void ddlOut1Change(object sender, EventArgs e)
        {
            DropDownList ddlout2 = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlout2.NamingContainer;
            int rowindex = row.RowIndex;
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = rowindex.ToString();
            DropDownList ddlin2 = (DropDownList)row.FindControl("ddlin1");

            if (ddlout2.SelectedValue.Equals("Select Time"))
            {

                ddlout2.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                ddlout2.ForeColor = System.Drawing.Color.Black;
            }
            if (!ddlout2.SelectedValue.Equals("Select Time") && !ddlin2.SelectedValue.Equals("Select Time"))
            {
                //both are times, now we should see convert them to datetimes
                var dateNow = DateTime.Now;
                //12:30 AM
                int inhour = Convert.ToInt32(ddlin2.SelectedValue.Substring(0, 2));
                if (inhour == 12 && !ddlin2.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    inhour = 0;
                }
                int inmin = Convert.ToInt32(ddlin2.SelectedValue.Substring(3, 2));
                if (ddlin2.SelectedValue.Substring(6, 2).Equals("PM") && inhour != 12)
                {
                    inhour = inhour + 12;
                }
                DateTime intime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, inhour, inmin, 0);

                int outhour = Convert.ToInt32(ddlout2.SelectedValue.Substring(0, 2));
                if (outhour == 12 && !ddlout2.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    outhour = 0;
                }
                int outmin = Convert.ToInt32(ddlout2.SelectedValue.Substring(3, 2));
                if (ddlout2.SelectedValue.Substring(6, 2).Equals("PM") && outhour != 12)
                {
                    outhour = outhour + 12;
                }
                DateTime outtime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, outhour, outmin, 0);
                if (outtime < intime)
                {
                    outtime = outtime.AddDays(1);//need to add a day here if time is less
                }

                int hours = (outtime - intime).Hours;

                double minssworked = (outtime - intime).Minutes;
                double minsdivided = minssworked / 60;


                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                string totalstring = hours.ToString() + minsdivided.ToString().Substring(1, minsdivided.ToString().Length - 1);
                //Application["monpart11"] = Convert.ToDouble(totalstring);
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part1.Value = Convert.ToDouble(totalstring).ToString();
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
                //errMsg.Visible = true;
                //errMsg.ErrorMessage = "mins: " + minssworked + " hours: " + hours.ToString() + " divided: " + minsdivided.ToString();

            }
            else
            {
                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part1.Value = "0";
                //Application["monpart11"] = "0";
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
            }
            UpdateWeeklyTotals();
        }

        protected void ddlIn2Change(object sender, EventArgs e)
        {
            DropDownList ddlin = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlin.NamingContainer;
            int rowindex = row.RowIndex;
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = rowindex.ToString();
            DropDownList ddlout = (DropDownList)row.FindControl("ddlout2");

            if (ddlin.SelectedValue.Equals("Select Time"))
            {

                ddlin.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                ddlin.ForeColor = System.Drawing.Color.Black;
            }
            if (!ddlout.SelectedValue.Equals("Select Time") && !ddlin.SelectedValue.Equals("Select Time"))
            {
                //both are times, now we should see convert them to datetimes
                var dateNow = DateTime.Now;
                //12:30 AM
                int inhour = Convert.ToInt32(ddlin.SelectedValue.Substring(0, 2));
                if (inhour == 12 && !ddlin.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    inhour = 0;
                }
                int inmin = Convert.ToInt32(ddlin.SelectedValue.Substring(3, 2));
                if (ddlin.SelectedValue.Substring(6, 2).Equals("PM") && inhour != 12)
                {
                    inhour = inhour + 12;
                }
                DateTime intime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, inhour, inmin, 0);

                int outhour = Convert.ToInt32(ddlout.SelectedValue.Substring(0, 2));
                if (outhour == 12 && !ddlout.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    outhour = 0;
                }
                int outmin = Convert.ToInt32(ddlout.SelectedValue.Substring(3, 2));
                if (ddlout.SelectedValue.Substring(6, 2).Equals("PM") && outhour != 12)
                {
                    outhour = outhour + 12;
                }
                DateTime outtime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, outhour, outmin, 0);
                if (outtime < intime)
                {
                    outtime = outtime.AddDays(1);//need to add a day here if time is less
                }

                int hours = (outtime - intime).Hours;

                double minssworked = (outtime - intime).Minutes;
                double minsdivided = minssworked / 60;


                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                string totalstring = hours.ToString() + minsdivided.ToString().Substring(1, minsdivided.ToString().Length - 1);
                //Application["monpart11"] = Convert.ToDouble(totalstring);
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part2.Value = Convert.ToDouble(totalstring).ToString();
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
                //errMsg.Visible = true;
                //errMsg.ErrorMessage = "mins: " + minssworked + " hours: " + hours.ToString() + " divided: " + minsdivided.ToString();

            }
            else
            {
                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part2.Value = "0";
                //Application["monpart11"] = "0";
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
            }
            UpdateWeeklyTotals();
        }

        protected void ddlOut2Change(object sender, EventArgs e)
        {
            DropDownList ddlout2 = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlout2.NamingContainer;
            int rowindex = row.RowIndex;
            //errMsg.Visible = true;
            //errMsg.ErrorMessage = rowindex.ToString();
            DropDownList ddlin2 = (DropDownList)row.FindControl("ddlin2");

            if (ddlout2.SelectedValue.Equals("Select Time"))
            {

                ddlout2.ForeColor = System.Drawing.Color.Gray;
            }
            else
            {
                ddlout2.ForeColor = System.Drawing.Color.Black;
            }
            if (!ddlout2.SelectedValue.Equals("Select Time") && !ddlin2.SelectedValue.Equals("Select Time"))
            {
                //both are times, now we should see convert them to datetimes
                var dateNow = DateTime.Now;
                //12:30 AM
                int inhour = Convert.ToInt32(ddlin2.SelectedValue.Substring(0, 2));
                if (inhour == 12 && !ddlin2.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    inhour = 0;
                }
                int inmin = Convert.ToInt32(ddlin2.SelectedValue.Substring(3, 2));
                if (ddlin2.SelectedValue.Substring(6, 2).Equals("PM") && inhour != 12)
                {
                    inhour = inhour + 12;
                }
                DateTime intime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, inhour, inmin, 0);

                int outhour = Convert.ToInt32(ddlout2.SelectedValue.Substring(0, 2));
                if (outhour == 12 && !ddlout2.SelectedValue.Substring(6, 2).Equals("PM"))
                {
                    outhour = 0;
                }
                int outmin = Convert.ToInt32(ddlout2.SelectedValue.Substring(3, 2));
                if (ddlout2.SelectedValue.Substring(6, 2).Equals("PM") && outhour != 12)
                {
                    outhour = outhour + 12;
                }
                DateTime outtime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, outhour, outmin, 0);
                if (outtime < intime)
                {
                    outtime = outtime.AddDays(1);//need to add a day here if time is less
                }

                int hours = (outtime - intime).Hours;

                double minssworked = (outtime - intime).Minutes;
                double minsdivided = minssworked / 60;


                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                string totalstring = hours.ToString() + minsdivided.ToString().Substring(1, minsdivided.ToString().Length - 1);
                //Application["monpart11"] = Convert.ToDouble(totalstring);
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part2.Value = Convert.ToDouble(totalstring).ToString();
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
                //errMsg.Visible = true;
                //errMsg.ErrorMessage = "mins: " + minssworked + " hours: " + hours.ToString() + " divided: " + minsdivided.ToString();

            }
            else
            {
                Label hoursworkedlabel = (Label)row.FindControl("hoursworked");
                HiddenField part1 = (HiddenField)row.FindControl("part1total");
                HiddenField part2 = (HiddenField)row.FindControl("part2total");
                part2.Value = "0";
                //Application["monpart11"] = "0";
                hoursworkedlabel.Text = (Convert.ToDouble(part1.Value) + (Convert.ToDouble(part2.Value))).ToString();
            }
            UpdateWeeklyTotals();
        }

        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].CssClass = "hiddencol";
                e.Row.Cells[7].CssClass = "hiddencol";
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[6].CssClass = "hiddencol";
                e.Row.Cells[7].CssClass = "hiddencol";
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {



                DropDownList ddlin1 = (e.Row.FindControl("ddlin1") as DropDownList);
                DropDownList ddlout1 = (e.Row.FindControl("ddlout1") as DropDownList);
                DropDownList ddlin2 = (e.Row.FindControl("ddlin2") as DropDownList);
                DropDownList ddlout2 = (e.Row.FindControl("ddlout2") as DropDownList);

                ddlin1.ForeColor = System.Drawing.Color.Gray;
                ddlout1.ForeColor = System.Drawing.Color.Gray;
                ddlin2.ForeColor = System.Drawing.Color.Gray;
                ddlout2.ForeColor = System.Drawing.Color.Gray;

                ArrayList times = new ArrayList();
                var dateNow = DateTime.Now;
                DateTime time = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 0, 0, 0);
                string format = "hh:mm tt";

                DateTime end = time.AddDays(1);

                times.Add("Select Time");
                while (time < end)
                {
                    times.Add(time.ToString(format));
                    time = time.AddMinutes(15);
                }
                ddlin1.DataSource = times;
                ddlin1.DataBind();
                //ddlin1.SelectedIndex = 32;

                ddlout1.DataSource = times;
                ddlout1.DataBind();
                // ddlout1.SelectedIndex = 48;

                ddlin2.DataSource = times;
                ddlin2.DataBind();
                // ddlin2.SelectedIndex = 52;

                ddlout2.DataSource = times;
                ddlout2.DataBind();
                //ddlout2.SelectedIndex = 68;

            }
        }


    }
}