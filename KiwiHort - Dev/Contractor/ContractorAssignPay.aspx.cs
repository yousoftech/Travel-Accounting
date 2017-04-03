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
using System.Threading;
using System.Text;
public partial class Contractor_ContractorAssignPay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

       
        int cbo_day_index = cbo_day.SelectedIndex;
        int cbo_farm_index = cbo_farm.SelectedIndex;

        GridView1.Visible = false;

        SqlConnection con2 = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con2.Open();

        dateConverter d = new dateConverter();


        SqlCommand comMin = new SqlCommand("SELECT min(Day) FROM tbl_Duty", con2);
        SqlCommand comMax = new SqlCommand("SELECT max(Day) FROM tbl_Duty", con2);

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


        }
        else
        {
           
        }

        SqlCommand comFarm = new SqlCommand("SELECT Farm_Name, farmid FROM tbl_farms WHERE GrowerID = @0", con2);
        comFarm.Parameters.AddWithValue("@0", Session["Id"].ToString());


        SqlDataReader reader1 = comFarm.ExecuteReader();

        if (reader1.HasRows)
        {
            cbo_farm.Items.Add("Select a farm");
            while (reader1.Read())
            {
                cbo_farm.Items.Add(new ListItem(reader1.GetString(0), reader1.GetString(1)));
            }
        }

        cbo_day.SelectedIndex = cbo_day_index;
        cbo_farm.SelectedIndex = cbo_farm_index;





        SqlDataSource1.SelectCommand = "SELECT RTRIM(firstname) + ' ' + RTRIM(lastname) AS name, '$' + CAST(CAST(tbl_pay.pay AS MONEY) AS VARCHAR(MAX)) AS payrate FROM tbl_worker INNER JOIN tbl_pay ON tbl_worker.payrate = tbl_pay.payID INNER JOIN[dbo].[tbl_employees] on[dbo].[tbl_employees].[workersid]= [tbl_worker].[WorkersId] where[dbo].[tbl_employees].[growersid]=@0";
        SqlDataSource1.SelectParameters.Clear();
        SqlDataSource1.SelectParameters.Add("0", Session["Id"].ToString());

        SqlDataSource1.DataBind();


        if (Session["Id"] == null)
        {

            Response.Redirect("~/login.aspx");
        }

        ((Label)Master.FindControl("lbl_title")).Text = "Worker Pay";
        // RequiredFieldValidator2.ControlToValidate = txtsubject;
        con2.Close();
        if (!IsPostBack)
        {

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

            con.Open();

            SqlCommand com = new SqlCommand("SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] INNER JOIN [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]= [tbl_worker].[WorkersId] where [dbo].[tbl_employees].[growersid]=@0 ORDER BY [tbl_worker].[FirstName]", con);
            com.Parameters.AddWithValue("@0", Session["Id"]);

            SqlDataReader reader = com.ExecuteReader();



            while (reader.Read())

            {
                if (reader.HasRows == true && String.Compare(reader.GetValue(0).ToString(), null) != 0)
                {


                    cbo_worker.Items.Add(new ListItem(Convert.ToString(reader["FirstName"]) + " " + Convert.ToString(reader["LastName"]), Convert.ToString(reader["workersId"])));
                }
            }


            cbo_worker_SelectedIndexChanged(null, null);



        }

    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {


        //Response.Redirect("ContractorAssignPay.aspx");

        DateTime time = DateTime.UtcNow;

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();

        SqlCommand comSelectId = new SqlCommand("SELECT CAST(payrate AS nvarchar(10)) FROM tbl_worker WHERE workersId = @0", con);
        comSelectId.Parameters.AddWithValue("@0", cbo_worker.SelectedValue.ToString());

        string payID = (string)comSelectId.ExecuteScalar();
        comSelectId.Dispose();


        SqlCommand comSelectPay = new SqlCommand("SELECT CAST(pay AS nvarchar(10)) FROM tbl_pay WHERE payID = @0", con);
        comSelectPay.Parameters.AddWithValue("@0", payID);

        string payAmount = (string)comSelectPay.ExecuteScalar();
        comSelectPay.Dispose();


        SqlCommand comInsert = new SqlCommand("INSERT INTO tbl_history (payID, lastUpdate, pay) VALUES (@0, @1, @2)", con);
        comInsert.Parameters.AddWithValue("@0", payID);
        comInsert.Parameters.AddWithValue("@1", time);
        comInsert.Parameters.AddWithValue("@2", payAmount);

        comInsert.ExecuteNonQuery();

        SqlCommand comInsertCurrent = new SqlCommand("UPDATE tbl_pay SET pay = @1, lastUpdate = @2 WHERE payID = @0", con);
        comInsertCurrent.Parameters.AddWithValue("@0", payID);
        comInsertCurrent.Parameters.AddWithValue("@1", txt_pay.Text);
        comInsertCurrent.Parameters.AddWithValue("@2", time);

        comInsertCurrent.ExecuteNonQuery();

        comInsert.Dispose();
        comInsertCurrent.Dispose();

        GridView1.DataBind();
    }

    protected void cbo_worker_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        SqlCommand com = new SqlCommand("SELECT CAST(tbl_pay.pay AS decimal(18,2)) FROM tbl_pay INNER JOIN tbl_worker ON tbl_pay.payID = tbl_worker.payrate WHERE tbl_worker.workersID = @0", con);
        com.Parameters.AddWithValue("@0", cbo_worker.SelectedValue.ToString());


        txt_pay.Text = com.ExecuteScalar().ToString();

        //if (!txt_pay.Text.ToString().Contains('.'))
        //{
        //    txt_pay.Text = txt_pay.Text.ToString() + ".00";
        //}
        //else if (txt_pay.Text.ToString().Length - txt_pay.Text.ToString().IndexOf(".") == 2)
        //{
        //    txt_pay.Text = txt_pay.Text.ToString() + "0";
        //}
        //else
        //{
        //    txt_pay.Text = txt_pay.Text.ToString();
        //}
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        dateConverter d = new dateConverter();
        if (cbo_farm.SelectedIndex != 0 && cbo_day.SelectedIndex != 0)
        {
           
            SqlDataSource2.SelectParameters.Clear();
            SqlDataSource2.SelectParameters.Add("1", d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Month.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Day.ToString() + "/" + d.convertUTCtoNZT(Convert.ToDateTime(cbo_day.SelectedValue)).Year.ToString());

            SqlDataSource2.SelectParameters.Add("2", Session["Id"].ToString());
            SqlDataSource2.SelectParameters.Add("3", cbo_farm.SelectedValue.ToString());
            SqlDataSource2.DataBind();

            btn_excel.Visible = true;
        }
     //download();
    }
    protected void download()
    {
        dateConverter d = new dateConverter();
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "WORKING HOURS FOR " + cbo_farm.SelectedItem.Text + ".csv"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        dgd_workers.AllowPaging = false;
 

        //int j = dgd_workers.HeaderRow.Cells.Count;
        //for (int i = 0; i < j; i++)
        //{
        //    dgd_workers.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
        //}
        dgd_workers.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        dateConverter d = new dateConverter();
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "WORKING HOURS FOR " + cbo_farm.SelectedItem.Text + ".csv"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        Response.Charset = "";
        dgd_workers.AllowPaging = false;
        StringBuilder columnbind = new StringBuilder();
        for (int k = 0; k < dgd_workers.Columns.Count; k++)
        {

            columnbind.Append(dgd_workers.Columns[k].HeaderText + ',');
        }

        columnbind.Append("\r\n");
        for (int i = 0; i < dgd_workers.Rows.Count; i++)
        {
            for (int k = 0; k < dgd_workers.Columns.Count; k++)
            {

                columnbind.Append(dgd_workers.Rows[i].Cells[k].Text + ',');
            }

            columnbind.Append("\r\n");
        }
        Response.Output.Write(columnbind.ToString());
        Response.Flush();
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
}