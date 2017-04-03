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

public partial class Monitor_MonitorBudget : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }

        if(!IsPostBack)
        {
            SqlDataSource1.SelectCommand = "SELECT '$' + CONVERT(varchar, CAST(tbl_budget.amount AS money), 1) AS 'Amount', RTRIM(tbl_grower.firstname) + ' ' + RTRIM(tbl_grower.lastname) AS 'Grower', tbl_farms.farm_name AS 'Farm Name', tbl_job_mcat.catname AS 'Main Job Category', tbl_job_cat.catname AS 'Job Subcategory', tbl_budget.note AS 'Notes', tbl_budget.timestamp AS 'Timestamp' FROM[tbl_Budget] INNER JOIN tbl_farms ON tbl_budget.farmid = tbl_farms.farmid INNER JOIN tbl_job_mcat ON tbl_budget.mcatid = tbl_job_mcat.jobmcatid INNER JOIN tbl_job_cat ON tbl_budget.scatid = tbl_job_cat.jobcatid INNER JOIN tbl_grower ON tbl_budget.growerid = tbl_grower.growersid WHERE tbl_budget.monitorId = @id ORDER BY tbl_budget.timestamp DESC";
            SqlDataSource1.SelectParameters.Add("id", Session["Id"].ToString());
            

            ((Label)Master.FindControl("lbl_title")).Text = "Budget";
            // add farms
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
            con.Open();
            SqlCommand comFram = new SqlCommand("SELECT farm_name, farmId FROM tbl_farms WHERE growerId = @0", con);
            comFram.Parameters.AddWithValue("@0", Session["Id"]);
            SqlDataReader readerFarm = comFram.ExecuteReader();
            dl_Farm.Items.Clear();
            dl_Farm.Items.Add("Select Farms");

            if (readerFarm.HasRows)
            {

                while (readerFarm.Read())
                {
                    dl_Farm.Items.Add(new ListItem(readerFarm.GetValue(0).ToString(), readerFarm.GetValue(1).ToString()));

                }
            }
            comFram.Dispose();
            readerFarm.Close();

            // add Cat
            SqlCommand comMcat = new SqlCommand("Select JobmCatID,CatName from tbl_job_mcat", con);
            SqlDataReader readerMcat = comMcat.ExecuteReader();
            dl_Mcat.Items.Clear();
            dl_Mcat.Items.Add("Select  Main Category");
            if (readerMcat.HasRows)
            {

                while (readerMcat.Read())
                {
                    dl_Mcat.Items.Add(new ListItem(readerMcat.GetValue(1).ToString(), readerMcat.GetValue(0).ToString()));

                }
            }
            comMcat.Dispose();
            readerMcat.Close();
            //add sub cat
            dl_Cat.Items.Add("Select Sub Cat");

            // add contractor
            SqlCommand comCont = new SqlCommand("Select tbl_grower.growersId, tbl_grower.Firstname,tbl_grower.Lastname from tbl_grower inner join [dbo].[tbl_employees] on [dbo].[tbl_grower].[GrowersId] =[dbo].[tbl_employees].[workersid] where[dbo].[tbl_employees].[growersid] = @0", con);
            comCont.Parameters.AddWithValue("@0", Session["Id"]);
            SqlDataReader readerCont = comCont.ExecuteReader();
            dl_Contractor.Items.Clear();
            dl_Contractor.Items.Add("Select Contractor");
            if (readerCont.HasRows)
            {

                while (readerCont.Read())
                {
                    dl_Contractor.Items.Add(new ListItem(readerCont.GetValue(1).ToString() + readerCont.GetValue(2).ToString(), readerCont.GetValue(0).ToString()));

                }
            }
            comCont.Dispose();
            readerCont.Close();


        }




    }

    protected void dl_Mcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        SqlCommand comMcat = new SqlCommand("Select JobCatID,CatName from tbl_job_cat where mcatid=@0", con);
        con.Open();
        comMcat.Parameters.AddWithValue("@0", dl_Mcat.SelectedValue.ToString());
        SqlDataReader readerMcat = comMcat.ExecuteReader();
        dl_Cat.Items.Clear();
        dl_Cat.Items.Add("Select Sub Category");
        if (readerMcat.HasRows)
        {

            while (readerMcat.Read())
            {
                dl_Cat.Items.Add(new ListItem(readerMcat.GetValue(1).ToString(), readerMcat.GetValue(0).ToString()));

            }
        }
        else
        {
            dl_Cat.Items.Clear();
            dl_Cat.Items.Add("No Sub Category");

        }
        comMcat.Dispose();
        readerMcat.Close();
    }

    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        if (dl_Farm.SelectedIndex != 0)
        {
            if (dl_Mcat.SelectedIndex != 0)
            {
                if (dl_Cat.SelectedIndex != 0)
                {
                    if (dl_Contractor.SelectedIndex != 0)
                    {
                        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
                        SqlCommand comMcat = new SqlCommand("INSERT INTO [dbo].[tbl_Budget] ([MonitorID],[FarmID],[McatID],[ScatID],[GrowerID],[Note],[Amount],[TimeStamp])  VALUES (@0,@1,@2,@3,@4,@5,@6,@7) ", con);
                        con.Open();
                        comMcat.Parameters.AddWithValue("@0", Session["Id"].ToString());
                        comMcat.Parameters.AddWithValue("@1", dl_Farm.SelectedValue.ToString());
                        comMcat.Parameters.AddWithValue("@2", dl_Mcat.SelectedValue.ToString());
                        comMcat.Parameters.AddWithValue("@3", dl_Cat.SelectedValue.ToString());
                        comMcat.Parameters.AddWithValue("@4", dl_Contractor.SelectedValue.ToString());
                        comMcat.Parameters.AddWithValue("@5", txt_note.Text);
                        comMcat.Parameters.AddWithValue("@6", txt_amount.Text);
                        comMcat.Parameters.AddWithValue("@7", DateTime.UtcNow.Month.ToString() + "/" + DateTime.UtcNow.Day.ToString() + "/" + DateTime.UtcNow.Year.ToString() + " " + DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute.ToString() + ":" + DateTime.UtcNow.Second.ToString());
                        //comMcat.Parameters.AddWithValue("@7", DateTime.UtcNow.ToString()); // -- !!Double check this one!! --
                        comMcat.ExecuteReader();
                    }
                   
                     else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to select Contracor');", true);

                    }

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to select Sub Cat');", true);

                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to select Main Cat');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to select Farm');", true);

        }
    }
}