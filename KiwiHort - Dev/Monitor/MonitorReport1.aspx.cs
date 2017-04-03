using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;

public partial class Monitor_MonitorReport1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lbl_title")).Text = "Report";
            BindContractorDDl();
            BindSupervisorDDl();
           // GVBind();
            BindBlock();
            String binddlfarm = "Select Distinct Farm_Name from tbl_farms where GrowerID='" + Session["Id"].ToString() + "'";
            DataTable dt = GetData(binddlfarm);
            ddlselectfarm.DataSource = dt;
            ddlselectfarm.DataTextField = "Farm_Name";
            ddlselectfarm.DataValueField = "Farm_Name";
            ddlselectfarm.DataBind();
            ddlselectfarm.Items.Insert(0, new ListItem("Select Farm", ""));
        }
    }

    protected void BindBlock()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        string BindCon = "Select Distinct tbl_blocks.Block_Name as Name  from tbl_blocks";
        SqlCommand cmd = new SqlCommand(BindCon, con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        ddlselectblock.DataSource = ds;
        ddlselectblock.DataTextField = "Name";
        ddlselectblock.DataValueField = "Name";
        ddlselectblock.DataBind();
        ddlselectblock.Items.Insert(0, new ListItem("Select Block ", "0"));
        con.Close();
    }



    protected void BindContractorDDl()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        string BindCon = "select(rTRIM([dbo].[tbl_grower].[FirstName]) +[dbo].[tbl_grower].[LastName]) as Name from[dbo].[tbl_grower] inner join [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]=[dbo].[tbl_grower].[GrowersId] where [dbo].[tbl_employees].[growersid]='" + Session["Id"].ToString() + "'";
        SqlCommand cmd = new SqlCommand(BindCon, con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        ddlcontractor.DataSource = ds;
        ddlcontractor.DataTextField = "Name";
        ddlcontractor.DataValueField = "Name";
        ddlcontractor.DataBind();
        ddlcontractor.Items.Insert(0, new ListItem("Select Contractor ", "0"));
        con.Close();

    }

    protected void BindSupervisorDDl()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        string BindCon = "select (RTRIM(tbl_worker.FirstName)+RTRIM(tbl_worker.LastName)) as Name from tbl_login INNER JOIN tbl_worker ON tbl_login.Id=tbl_worker.WorkersId  where tbl_login.type='Supervisor'";
        SqlCommand cmd = new SqlCommand(BindCon, con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        ddlsupervisor.DataSource = ds;
        ddlsupervisor.DataTextField = "Name";
        ddlsupervisor.DataValueField = "Name";
        ddlsupervisor.DataBind();
        ddlsupervisor.Items.Insert(0, new ListItem("Select Supervisor ", "0"));
        con.Close();

    }
    //protected void GVBind()
    //{
    //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
    //    con.Open();
    //    string BindCon = "select tbl_farms.Farm_Name as FarmName, tbl_worker.FirstName as WorkerName  from tbl_Attendance INNER JOIN tbl_Duty ON tbl_Attendance.RosterID=tbl_Duty.RosterID INNER JOIN tbl_worker ON tbl_Duty.WorkerID=tbl_worker.WorkersId INNER JOIN tbl_blocks ON tbl_Attendance.blockid=tbl_blocks.BlockId INNER JOIN tbl_farms ON tbl_blocks.FarmId=tbl_farms.FarmId where tbl_farms.Farm_Name='Shlok''s Farm'";
    //    SqlCommand cmd = new SqlCommand(BindCon, con);
    //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
    //    DataSet ds = new DataSet();
    //    sda.Fill(ds);
    //    gvdatadisply.DataSource = ds.Tables[0];
    //    gvdatadisply.DataBind();
    //    con.Close();

    //}

    private static DataTable GetData(string binddlfarm)
    {
        DataTable dt = new DataTable();
        string constr = WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(binddlfarm))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                }
            }
            return dt;

        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Mv1.ActiveViewIndex = 0;

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Mv1.ActiveViewIndex = 1;



    }


    protected void btn_break_Click(object sender, EventArgs e)
    {
        Mv1.ActiveViewIndex = 2;
        //Session["shift"] = "BREAK";


    }

    protected void MultiView1_ActiveViewChanged(object sender, EventArgs e)
    {
        if (Mv1.ActiveViewIndex == 0) ///Farm Summary
        {
            Session["Farm Summary"] = null;
        }
        else if (Mv1.ActiveViewIndex == 1) //Work Summary
        {
            Session["Work Summary"] = "WorkS";
        }
        else
        {
            Session["Contractor Summary"] = "ContractorS"; //Contractor Summary
        }
    }

    string Bindfarm = "Select Farm_Name from tbl_farms ";

}