using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Configuration;

public partial class Contractor_ContractorReport : System.Web.UI.Page
{
    protected void Page_Load(object moveIcon, EventArgs e)
    {
        btn_excel.Visible = false;

        int cbo_day_index = cbo_day.SelectedIndex;
        int cbo_week_index = cbo_week.SelectedIndex;
        int cbo_farm_index = cbo_farm.SelectedIndex;


        ((Label)Master.FindControl("lbl_title")).Text = "Report";

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        dateConverter d = new dateConverter();


        SqlCommand comMin = new SqlCommand("SELECT min(Day) FROM tbl_Duty", con);
        SqlCommand comMax = new SqlCommand("SELECT max(Day) FROM tbl_Duty", con);

        SqlDataReader readerMin = comMin.ExecuteReader();

        readerMin.Read();
        if (readerMin.HasRows == true && String.Compare(readerMin.GetValue(0).ToString(), null) != 0)
        {
            btn_submit.Enabled = true;
            DateTime dayMin = d.convertUTCtoNZT(readerMin.GetDateTime(0));
            DateTime dayMax = d.convertUTCtoNZT((DateTime)comMax.ExecuteScalar());
            cbo_day.Items.Clear();
            cbo_farm.Items.Clear();
            cbo_day.Items.Add("Select a day");

            for (var count = dayMax; count >= dayMin; count = count.AddDays(-1))
            {
                cbo_day.Items.Add(new ListItem(count.Date.ToString("d/M/yyyy") + " - " + count.DayOfWeek.ToString(), d.convertNZTtoUTC(count.Date).ToString()));
            }

            var firstRun = true;

            cbo_week.Items.Clear();

            cbo_week.Items.Add("Select a week");

            //for (var count = dayMin; count <= dayMax; dayMin = dayMin.AddDays(1), count = count.AddDays(1))
            //{
            //    while (dayMin.DayOfWeek.ToString() != "Monday")
            //    {
            //        if (firstRun)
            //        {
            //            dayMin = dayMin.AddDays(-1);
            //        }
            //        else
            //        {
            //            dayMin = dayMin.AddDays(1);
            //            count = count.AddDays(1);
            //        }
            //    }
            //    firstRun = false;
            //    cbo_week.Items.Add(new ListItem(dayMin.Day.ToString() + "/" + dayMin.Month.ToString() + "/" + dayMin.Year.ToString(), d.convertNZTtoUTC(dayMin).ToString()));

            //}

            for (var count = dayMax; count >= dayMin; dayMax = dayMax.AddDays(-1), count = count.AddDays(-1))
            {
                while (dayMax.DayOfWeek.ToString() != "Monday")
                {
                    if (firstRun)
                    {
                        dayMax = dayMax.AddDays(-1);
                    }
                    else
                    {
                        dayMax = dayMax.AddDays(-1);
                        count = count.AddDays(-1);
                    }
                }
                firstRun = false;
                cbo_week.Items.Add(new ListItem(dayMax.Day.ToString() + "/" + dayMax.Month.ToString() + "/" + dayMax.Year.ToString(), d.convertNZTtoUTC(dayMax).ToString()));

            }
        }
        else
        {
            btn_submit.Enabled = false;
        }

        SqlCommand comFarm = new SqlCommand("SELECT Farm_Name, farmid FROM tbl_farms WHERE GrowerID = @0", con);
        comFarm.Parameters.AddWithValue("@0", Session["Id"].ToString());


        SqlDataReader reader = comFarm.ExecuteReader();

        if(reader.HasRows)
        {
            cbo_farm.Items.Add("Select a farm");
            while(reader.Read())
            {
                cbo_farm.Items.Add(new ListItem(reader.GetString(0), reader.GetString(1)));
            }
        }

        cbo_day.SelectedIndex = cbo_day_index;
        cbo_week.SelectedIndex = cbo_week_index;
        cbo_farm.SelectedIndex = cbo_farm_index;
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        
        if (cbo_farm.SelectedIndex != 0 && (cbo_week.SelectedIndex != 0 && Convert.ToBoolean(Session["day"]) == false) || (cbo_day.SelectedIndex != 0 && Convert.ToBoolean(Session["day"]) == true))
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "WORKING HOURS FOR " + cbo_farm.SelectedItem.Text + ".xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dgd_workers.AllowPaging = false;
            dateConverter d = new dateConverter();
            SqlDataSource1.SelectParameters.Clear();
          //  SqlDataSource1.SelectCommand = "SELECT [dbo].[tbl_worker].[FirstName] as Fname ,[dbo].[tbl_worker].[LastName] as lname ,[dbo].[tbl_Attendance].[Start_time],[dbo].[tbl_Attendance].[End_time],[dbo].[tbl_Attendance].[breaktime] as [Break Time],[dbo].[tbl_Attendance].[paid_break] as [Paid Break],[dbo].[tbl_Attendance].[Total_hours],[dbo].[tbl_Attendance].[note] As [] FROM [dbo].[tbl_Attendance] INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid  where tbl_duty.day = @day and tbl_duty.growerid = @grower and farmid = @farm and tbl_attendance.end_time is not null";

            SqlDataSource1.SelectCommand = "SELECT ([dbo].[tbl_worker].[FirstName] + [dbo].[tbl_worker].[LastName]) as Name ,[dbo].[tbl_Attendance].[Start_time] As [Start Time],[dbo].[tbl_Attendance].[End_time] as  [End Time],[dbo].[tbl_Attendance].[breaktime] as [Break Time],[dbo].[tbl_Attendance].[paid_break] as [Paid Break],cast(round([dbo].[tbl_Attendance].[Total_hours]/60,2) as numeric(36,2)) As [Total Hours] ,[dbo].[tbl_Attendance].[note] as [Note] FROM [dbo].[tbl_Attendance] INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid  where tbl_duty.day = @day and tbl_duty.growerid = @grower and farmid = @farm ";
            //SqlDataSource1.SelectParameters.Add("supervisor", cbo_supervisor.ToString());
            if (Convert.ToBoolean(Session["day"]))
            {
                //SqlDataSource1.SelectParameters.Add("day", Convert.ToDateTime(cbo_day.SelectedValue).Date.ToString());
                //SqlDataSource1.SelectParameters.Add("day", "12/05/2016");
                SqlDataSource1.SelectParameters.Add("day", d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Year.ToString());
            }
            else
            {
                SqlDataSource1.SelectParameters.Clear();
                //  SqlDataSource1.SelectCommand = "SELECT tbl_worker.firstname AS 'First Name', tbl_worker.lastname AS 'Last Name', sum(tbl_attendance.total_hours)/60 AS 'Total Hours' FROM tbl_attendance INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid where tbl_duty.day BETWEEN @0 AND @1  and tbl_duty.growerid = @grower and farmid = @farm and tbl_attendance.end_time is not null group by tbl_worker.firstname,tbl_worker.lastname";
                SqlDataSource1.SelectCommand = "SELECT tbl_worker.firstname AS 'First Name', tbl_worker.lastname AS 'Last Name',cast(round(sum(tbl_attendance.total_hours) / 60 , 2) as numeric(36,2))  AS 'Total Hours' FROM tbl_attendance INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid where tbl_duty.day BETWEEN @0 AND @1  and tbl_duty.growerid = @grower and farmid = @farm and tbl_attendance.end_time is not null group by tbl_worker.firstname,tbl_worker.lastname";
                SqlDataSource1.SelectParameters.Add("0", d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).Year.ToString());
                SqlDataSource1.SelectParameters.Add("1", d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).AddDays(7).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).AddDays(7).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).AddDays(7).Year.ToString());
            }
            SqlDataSource1.SelectParameters.Add("farm", cbo_farm.SelectedValue.ToString());
            SqlDataSource1.SelectParameters.Add("grower", Session["Id"].ToString());
            SqlDataSource1.DataBind();
            //Change the Header Row back to white color
            dgd_workers.HeaderRow.Style.Add("background-color", "#FFFFFF");
            //Applying stlye to gridview header cells
            for (int i = 0; i < dgd_workers.HeaderRow.Cells.Count; i++)
            {
                dgd_workers.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
            }
            dgd_workers.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
       
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        dateConverter d = new dateConverter();


        if (cbo_farm.SelectedIndex != 0 && (cbo_week.SelectedIndex != 0 && Convert.ToBoolean(Session["day"]) == false) || (cbo_day.SelectedIndex != 0 && Convert.ToBoolean(Session["day"]) == true))
        {
            SqlDataSource1.SelectParameters.Clear();
            // SqlDataSource1.SelectCommand = "SELECT tbl_worker.firstname AS 'Fisrt Name', tbl_worker.lastname AS 'Last Name', CONVERT(DECIMAL(10,2), tbl_attendance.total_hours/60) AS 'Total Hours' FROM tbl_attendance INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid where tbl_duty.day = @day and tbl_duty.growerid = @grower and farmid = @farm and tbl_attendance.end_time is not null";

            if (chkphase.Checked)
            {
                SqlDataSource1.SelectCommand= "SELECT ([dbo].[tbl_worker].[FirstName] + [dbo].[tbl_worker].[LastName]) as Name ,[dbo].[temp_attendance].[Start_time] As [Start Time],[dbo].[temp_attendance].[End_time] as  [End Time], Phase,[dbo].[temp_attendance].[pay] FROM [dbo].[temp_attendance] inner join [dbo].[tbl_Attendance] on [dbo].[tbl_Attendance].[RosterID]=[dbo].[temp_attendance].[RosterID] INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid  where tbl_duty.day = @day and tbl_duty.growerid = @grower and farmid = @farm and  [dbo].[temp_attendance].[End_time] is not null and [dbo].[temp_attendance].[pay] is not null and [dbo].[temp_attendance].[pay] <> 0";
            }
            else
            {
                SqlDataSource1.SelectCommand = "SELECT ([dbo].[tbl_worker].[FirstName] + [dbo].[tbl_worker].[LastName]) as Name ,[dbo].[tbl_Attendance].[Start_time] As [Start Time],[dbo].[tbl_Attendance].[End_time] as  [End Time],[dbo].[tbl_Attendance].[breaktime] as [Break Time],[dbo].[tbl_Attendance].[paid_break] as [Paid Break],cast(round([dbo].[tbl_Attendance].[Total_hours]/60,2) as numeric(36,2)) As [Total Hours] ,[dbo].[tbl_Attendance].[note] as [Note] FROM [dbo].[tbl_Attendance] INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid  where tbl_duty.day = @day and tbl_duty.growerid = @grower and farmid = @farm ";
            }
            //SqlDataSource1.SelectParameters.Add("supervisor", cbo_supervisor.ToString());
            if (Convert.ToBoolean(Session["day"]))
            {
                //SqlDataSource1.SelectParameters.Add("day", Convert.ToDateTime(cbo_day.SelectedValue).Date.ToString());
                //SqlDataSource1.SelectParameters.Add("day", "12/05/2016");
                SqlDataSource1.SelectParameters.Add("day", d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Year.ToString());
            }
            else
            {
                SqlDataSource1.SelectParameters.Clear();
                SqlDataSource1.SelectCommand = "SELECT tbl_worker.firstname AS 'First Name', tbl_worker.lastname AS 'Last Name',cast(round(sum(tbl_attendance.total_hours) / 60 , 2) as numeric(36,2))  AS 'Total Hours' FROM tbl_attendance INNER JOIN tbl_duty on tbl_attendance.rosterid = tbl_duty.rosterid INNER JOIN tbl_worker on tbl_worker.workersid = tbl_duty.workerid INNER JOIN tbl_shift on tbl_duty.shiftid = tbl_shift.shiftid where tbl_duty.day BETWEEN @0 AND @1  and tbl_duty.growerid = @grower and farmid = @farm and tbl_attendance.end_time is not null group by tbl_worker.firstname,tbl_worker.lastname";
                SqlDataSource1.SelectParameters.Add("0", d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).Year.ToString());

                SqlDataSource1.SelectParameters.Add("1", d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).AddDays(7).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).AddDays(7).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_week.SelectedValue)).AddDays(7).Year.ToString());

            }
            SqlDataSource1.SelectParameters.Add("farm", cbo_farm.SelectedValue.ToString());
            SqlDataSource1.SelectParameters.Add("grower", Session["Id"].ToString());
            SqlDataSource1.DataBind();
            btn_excel.Visible = true;
        }
    }

    protected void cbo_day_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbo_day.Style.Add("background-color", "#fff");
        cbo_day.Style.Add("border-color", "#89B53D");
        cbo_day.Style.Add("color", "#444");
        cbo_week.Style.Add("background-color", "rgba(200, 0, 0, 255)");
        cbo_week.Style.Add("border-color", "rgba(255, 0, 0, 200)");
        cbo_week.Style.Add("opacitiy", "0.8");
        cbo_week.Style.Add("color", "#fff");
        Session["day"] = true;
    }

    protected void cbo_week_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbo_week.Style.Add("background-color", "#fff");
        cbo_week.Style.Add("border-color", "#89B53D");
        cbo_week.Style.Add("color", "#444");
        cbo_day.Style.Add("background-color", "rgba(200, 0, 0, 255)");
        cbo_day.Style.Add("border-color", "rgba(255, 0, 0, 200)");
        cbo_day.Style.Add("opacitiy", "0.8");
        cbo_day.Style.Add("color", "#fff");
        Session["day"] = false;
    }
}