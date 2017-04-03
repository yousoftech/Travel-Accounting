using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization;
using System.Web.Configuration;

public partial class Monitor_MonitorHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         
        

        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }
        Bindchart();

        ((Label)Master.FindControl("lbl_title")).Text = "Dashboard";

        if (!IsPostBack)
        {
            String bindddl = "Select Distinct Farm_Name from tbl_farms where GrowerID='" + Session["Id"].ToString() + "'";
            DataTable dt = GetData(bindddl);
            ddlSelectFarmForBudgetPie.DataSource = dt;
            ddlSelectFarmForBudgetPie.DataTextField = "Farm_Name";
            ddlSelectFarmForBudgetPie.DataValueField = "Farm_Name";
            ddlSelectFarmForBudgetPie.DataBind();
            ddlSelectFarmForBudgetPie.Items.Insert(0, new ListItem("Select Farm", ""));
        }
        //rajan code start


        if (!IsPostBack)
        {
            ddlSelectFarmForBudgetPie.SelectedIndex = 1;
            ddlSelectFarmForBudgetPie_SelectedIndexChanged(null, null);
        }

        // end

    }



    //Farm details Pie chart code starts Here
    private void Bindchart()
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        //  con.Open();

        String Query2 = "select  tbl_Budget.GrowerID  as NoOfContractor, COUNT(tbl_farms.farm_name) as TotalFarms from tbl_Budget INNER JOIN tbl_farms ON tbl_Budget.FarmID=tbl_farms.FarmID where tbl_Budget.MonitorID='" + Session["Id"].ToString() + "' GROUP BY tbl_Budget.GrowerID, tbl_farms.farm_name ";

        DataTable dt = GetData2(Query2);

        //Loop and add each datatable row to the Pie Chart Values
        foreach (DataRow row in dt.Rows)
        {

            PieChart2.PieChartValues.Add(new AjaxControlToolkit.PieChartValue
            {
                Category = row["NoOfContractor"].ToString(),

                Data = Convert.ToDecimal(row["TotalFarms"])

                
                 
            });
        }
    }

    private static DataTable GetData2(string Query2)
    {
        DataTable dt = new DataTable();
        string constr = WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(Query2))
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
    //Farm details  Pie chart code Ends here

    // Budget Pie chart starts here



    protected void ddlSelectFarmForBudgetPie_SelectedIndexChanged(object sender, EventArgs e)
    {
        

        string Query = string.Format("select tbl_Budget.Amount as AssignedBudget, (tbl_Budget.Amount-tbl_Attendance.pay) as RemainingBudget from tbl_farms INNER JOIN tbl_Budget ON tbl_farms.FarmId=tbl_Budget.FarmID INNER JOIN tbl_blocks ON tbl_farms.FarmId=tbl_blocks.FarmId INNER JOIN tbl_Attendance ON tbl_blocks.BlockId=tbl_Attendance.blockid where tbl_farms.Farm_Name=@name" );
        DataTable dt2 = GetData1(Query,ddlSelectFarmForBudgetPie.SelectedValue.ToString());

        //Loop and add each datatable row to the Pie Chart Values
        foreach (DataRow row in dt2.Rows)
        {
            PieChart1.PieChartValues.Add(new AjaxControlToolkit.PieChartValue
            {
                Category = row["AssignedBudget"].ToString(),

                Data = Convert.ToDecimal(row["RemainingBudget"])
            });
        }
        PieChart1.Visible = ddlSelectFarmForBudgetPie.SelectedItem.Value != "";
    }


    private static DataTable GetData(string bindddl)
    {
        DataTable dt1 = new DataTable();
        string constr1 = WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;

        using (SqlConnection con = new SqlConnection(constr1))
        {
            using (SqlCommand cmd = new SqlCommand(bindddl))
            {
                using (SqlDataAdapter sda1 = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    sda1.SelectCommand = cmd;
                    sda1.Fill(dt1);
                }
            }
            return dt1;
        }
    }
    private  static DataTable GetData1(string Query,string farmname)
    {
        DataTable dt2 = new DataTable();
        string constr = WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(Query))
            {
                using (SqlDataAdapter sda3 = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    sda3.SelectCommand = cmd;
                    sda3.SelectCommand.Parameters.AddWithValue("name", farmname);
                    sda3.Fill(dt2);
                }
            }
            return dt2;
        }
    }

    // Budget Pie chart ends here
}