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

public partial class Monitor_MonitorOrganiser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }

        ((Label)Master.FindControl("lbl_title")).Text = "Organiser";


        if(!IsPostBack)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("SELECT [dbo].[tbl_grower].[GrowersId] AS 'Id', RTRIM([dbo].[tbl_grower].[FirstName]) + ' ' + RTRIM([dbo].[tbl_grower].[LastName]) AS 'Contractor' FROM[dbo].[tbl_grower]  INNER JOIN[dbo].[tbl_employees] ON[dbo].[tbl_employees].[workersid] = [dbo].[tbl_grower].[GrowersId] WHERE[dbo].[tbl_employees].[growersid] = @id", con);
            com.Parameters.AddWithValue("@id", Session["Id"]);

            SqlDataReader reader = com.ExecuteReader();
            com.Dispose();

            cbo_contractor.Items.Clear();
            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    cbo_contractor.Items.Add(new ListItem(reader[1].ToString(), reader[0].ToString()));
                }
            }
            reader.Close();
            con.Dispose();

            sds_date.SelectCommand = "SELECT DISTINCT CONVERT(VARCHAR, [dbo].[tbl_Duty].[Day], 103) AS 'Date', [dbo].[tbl_Duty].[Day] FROM [dbo].[tbl_Duty] WHERE [GrowerID] = @id ORDER BY [dbo].[tbl_Duty].[Day] DESC";
            sds_date.SelectParameters.Add("id", cbo_contractor.SelectedValue.ToString());
            cbo_startDate.DataSourceID = null;
            cbo_startDate.DataSource = sds_date;
            cbo_startDate.DataBind();
            cbo_endDate.DataSourceID = null;
            cbo_endDate.DataSource = sds_date;
            cbo_endDate.DataBind();
        }
        else
        {
            while(cbo_endDate.SelectedIndex > cbo_startDate.SelectedIndex)
            {
                cbo_startDate.SelectedIndex ++;
            }
        }

        

        sds_contractorView.SelectCommand = "SELECT	RTRIM([dbo].[tbl_worker].[FirstName]) + ' ' + RTRIM([dbo].[tbl_worker].[LastName]) AS 'Workers' FROM[dbo].[tbl_worker]  INNER JOIN[dbo].[tbl_employees] ON[dbo].[tbl_worker].[WorkersId] = [dbo].[tbl_employees].[workersid] INNER JOIN[dbo].[tbl_grower] ON[dbo].[tbl_employees].[growersid] = [dbo].[tbl_grower].[GrowersId] WHERE[dbo].[tbl_employees].[growersid] = @id ORDER BY[dbo].[tbl_worker].[FirstName], [dbo].[tbl_worker].[LastName]";
        sds_contractorView.SelectParameters.Clear();
        sds_contractorView.SelectParameters.Add("id", cbo_contractor.SelectedValue.ToString());
        dgd_contractorView.DataSourceID = null;
        dgd_contractorView.DataSource = sds_contractorView;
        dgd_contractorView.DataBind();

        sds_farmView.SelectCommand = "SELECT CONVERT(VARCHAR, [dbo].[tbl_Duty].[Day], 103) AS 'Date', [dbo].[tbl_farms].[Farm_Name] AS 'Farm', RTRIM([dbo].[tbl_worker].[FirstName]) + ' ' + RTRIM([dbo].[tbl_worker].[LastName]) AS 'Worker', CONVERT(VARCHAR, [dbo].[tbl_Shift].[Shiftstarttime], 100) AS 'Start Time', CONVERT(VARCHAR, [dbo].[tbl_Shift].[ShiftendTime], 100) AS 'End Time', CONVERT(VARCHAR, [dbo].[tbl_Shift].[TotalTime], 108) AS 'Total Hours' FROM	[dbo].[tbl_worker]	INNER JOIN [dbo].[tbl_Duty] ON [dbo].[tbl_worker].[WorkersId] = [dbo].[tbl_Duty].[WorkerID] INNER JOIN [dbo].[tbl_Shift] ON [dbo].[tbl_Duty].[ShiftID] = [dbo].[tbl_Shift].[ShiftID] INNER JOIN [dbo].[tbl_farms] ON [dbo].[tbl_Shift].[farmId] = [dbo].[tbl_farms].[FarmId] INNER JOIN [dbo].[tbl_employees] ON [dbo].[tbl_Duty].[WorkerID] = [dbo].[tbl_employees].[workersid] WHERE [dbo].[tbl_Duty].[Day] >= @start AND [dbo].[tbl_Duty].[Day] <= @end AND [dbo].[tbl_employees].[growersid] = @id ORDER BY [dbo].[tbl_Duty].[Day] DESC, [dbo].[tbl_farms].[Farm_Name]";
        sds_farmView.SelectParameters.Clear();
        sds_farmView.SelectParameters.Add("id", cbo_contractor.SelectedValue.ToString());
        sds_farmView.SelectParameters.Add("start", Convert.ToDateTime(cbo_startDate.SelectedValue.ToString()).Month.ToString() + "/" + Convert.ToDateTime(cbo_startDate.SelectedValue.ToString()).Day.ToString() + "/" + Convert.ToDateTime(cbo_startDate.SelectedValue.ToString()).Year.ToString());
        sds_farmView.SelectParameters.Add("end", Convert.ToDateTime(cbo_endDate.SelectedValue.ToString()).Month.ToString() + "/" + Convert.ToDateTime(cbo_endDate.SelectedValue.ToString()).Day.ToString() + "/" + Convert.ToDateTime(cbo_endDate.SelectedValue.ToString()).Year.ToString());
        dgd_farmView.DataSourceID = null;
        dgd_farmView.DataSource = sds_farmView;
        dgd_farmView.DataBind();
    }
}