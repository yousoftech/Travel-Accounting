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

public partial class Monitor_MonitorReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lbl_title")).Text = "Report";
            BindContractorDDl();
            BindSupervisorDDl();
            GVBind();
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
    //protected void ddlselectfarm_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
    //    SqlCommand comfarmname = new SqlCommand("Select Distinct tbl_blocks.BlockId, tbl_blocks.Block_Name  from tbl_blocks INNER JOIN tbl_farms ON tbl_blocks.FarmId=tbl_farms.FarmId where tbl_farms.Farm_Name=@0", con);
    //    con.Open();
    //    comfarmname.Parameters.AddWithValue("@0", ddlselectfarm.SelectedValue.ToString());
    //    SqlDataReader readerfarmname = comfarmname.ExecuteReader();
    //    ddlselectblock.Items.Clear();
    //    ddlselectblock.Items.Add("Select Block");
    //    if (readerfarmname.HasRows)
    //    {

    //        while (readerfarmname.Read())
    //        {
    //            ddlselectblock.Items.Add(new ListItem(readerfarmname.GetValue(1).ToString(), readerfarmname.GetValue(0).ToString()));

    //        }
    //    }
    //    else
    //    {
    //        ddlselectblock.Items.Clear();
    //        ddlselectblock.Items.Add("No blocks available");

    //    }
    //    comfarmname.Dispose();
    //    readerfarmname.Close();
    //    con.Close();
    //    con.Dispose();
    //}


    protected void BindBlock()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        string BindCon = "Select Distinct tbl_blocks.Block_Name as Name ,tbl_blocks.BlockId as ID from tbl_blocks";
        SqlCommand cmd = new SqlCommand(BindCon, con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        ddlselectblock.DataSource = ds;
        ddlselectblock.DataTextField = "Name";
        ddlselectblock.DataValueField = "Id";
        ddlselectblock.DataBind();
        ddlselectblock.Items.Insert(0, new ListItem("Select Block ", ""));
        con.Close();
    }



    protected void BindContractorDDl()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        string BindCon = "select(rTRIM([dbo].[tbl_grower].[FirstName]) +[dbo].[tbl_grower].[LastName]) as Name from[dbo].[tbl_grower] inner join [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]=[dbo].[tbl_grower].[GrowersId] where [dbo].[tbl_employees].[growersid]='"+Session["Id"].ToString()+"'";
        SqlCommand cmd = new SqlCommand(BindCon,con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        ddlcontractor.DataSource = ds;
        ddlcontractor.DataTextField = "Name";
        ddlcontractor.DataValueField = "Name";
        ddlcontractor.DataBind();
        ddlcontractor.Items.Insert(0, new ListItem("Select Contractor ",""));
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
        ddlsupervisor.Items.Insert(0, new ListItem("Select Supervisor ", ""));
        con.Close();

    }
    protected void GVBind()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        string BindCon = "select tbl_farms.Farm_Name as FarmName, tbl_worker.FirstName as WorkerName  from tbl_Attendance INNER JOIN tbl_Duty ON tbl_Attendance.RosterID=tbl_Duty.RosterID INNER JOIN tbl_worker ON tbl_Duty.WorkerID=tbl_worker.WorkersId INNER JOIN tbl_blocks ON tbl_Attendance.blockid=tbl_blocks.BlockId INNER JOIN tbl_farms ON tbl_blocks.FarmId=tbl_farms.FarmId where tbl_farms.Farm_Name='Shlok''s Farm'";
        SqlCommand cmd = new SqlCommand(BindCon, con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        sda.Fill(ds);
        gvdatadisply.DataSource = ds.Tables[0];
        gvdatadisply.DataBind();
        con.Close();

    }

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

    protected void btn_search_Click(object sender, EventArgs e)
    {
        string strFarm = ddlselectfarm.SelectedItem.Value.ToString();
        string strBlock = ddlselectblock.SelectedItem.Value.ToString();
        string strContractor = ddlcontractor.SelectedValue;
        string strSupervisor = ddlsupervisor.SelectedValue;

        if(strFarm == "" && strBlock !="")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please select the farm');", true);

        }
        else if (strFarm !="" && strBlock =="")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please select the block');", true);
        }
        else if (strFarm == "" && strBlock =="")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please select the farm and block');", true);
        }
        else
        {
             
        }
    }

}