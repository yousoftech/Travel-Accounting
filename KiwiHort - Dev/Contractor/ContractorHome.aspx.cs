using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;

public partial class ContractorHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
            if (Session["Id"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            Session["redirect"] = "load";
            ((Label)Master.FindControl("lbl_title")).Text = "Dashboard";

            Session["flag"] = true;
            
            dateConverter dc = new dateConverter();
            DateTime dt = dc.convertUTCtoNZT(DateTime.UtcNow);

            //SqlDataSource2.SelectCommand = "DECLARE @count INT = 1 DECLARE @qry VARCHAR(MAX) = '' WHILE(@count <= (SELECT COUNT(*) FROM tbl_budget)) BEGIN IF (SELECT MONTH((SELECT TimeStamp FROM (SELECT ROW_NUMBER() OVER (ORDER BY TimeStamp ASC) AS rownumber, * FROM tbl_budget) AS looper WHERE rownumber = @count))) = @month BEGIN SET @qry = @qry + 'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY TimeStamp ASC) AS rownumber, * FROM tbl_budget) AS looper WHERE rownumber = ' + CAST(@count AS VARCHAR(MAX)) + ' UNION ALL ' END SET @count = @count + 1 END IF LEN(@qry) > 10 SET @qry = LEFT(@qry, LEN(@qry) - 10) EXEC(@qry)";
            SqlDataSource2.SelectCommand = "SELECT SUM((DATEPART(HOUR, tbl_Shift.totaltime) * tbl_worker.payrate))AS 'S', 'Assigned Worker Pay' AS 'grouper' FROM tbl_shift INNER JOIN tbl_Duty ON tbl_shift.ShiftID = tbl_Duty.ShiftID INNER JOIN tbl_worker ON tbl_worker.workersid = tbl_duty.workerid WHERE tbl_Duty.Day >= @monthStart AND tbl_Duty.Day < @monthEnd AND tbl_Duty.GrowerID = @growerId UNION SELECT( SELECT SUM(Amount) AS 'S' FROM tbl_budget WHERE GrowerID = @growerId AND TimeStamp >= @monthStart AND TimeStamp < @monthEnd) - (SELECT SUM((DATEPART(HOUR, tbl_Shift.totaltime) * tbl_worker.payrate)) AS 'S' FROM tbl_shift INNER JOIN tbl_Duty ON tbl_shift.ShiftID = tbl_Duty.ShiftID INNER JOIN tbl_worker ON tbl_worker.workersid = tbl_duty.workerid WHERE tbl_Duty.Day >= @monthStart AND tbl_Duty.Day < @monthEnd AND tbl_Duty.GrowerID = @growerId) AS 'S', 'Remaining Budget' AS 'grouper' ORDER BY 'grouper'";
        //SqlDataSource2.SelectParameters.Add("month", Convert.ToInt32(DateTime.UtcNow.Month).ToString());
        //SqlDataSource2.SelectParameters.Add("monthStart", DateTime.UtcNow.Month.ToString() + "/" + DateTime.UtcNow.Day.ToString() + "/" + DateTime.UtcNow.Year.ToString());
        //SqlDataSource2.SelectParameters.Add("monthEnd", (DateTime.UtcNow.Month + 1).ToString() + "/" + DateTime.UtcNow.Day.ToString() + "/" + DateTime.UtcNow.Year.ToString());

        if (!IsPostBack)
        {
            SqlDataSource2.SelectParameters.Add("monthStart", dt.Month.ToString() + "/1/" + dt.Year.ToString());
            SqlDataSource2.SelectParameters.Add("monthEnd", (dt.Month + 1).ToString() + "/1/" + dt.Year.ToString());
            SqlDataSource2.SelectParameters.Add("growerId", Session["Id"].ToString());
        }

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);


            con.Open();

            try
            {
            //SqlCommand comChart = new SqlCommand();
            //comChart.Connection = con;
            //comChart.CommandType = System.Data.CommandType.StoredProcedure;
            //comChart.CommandText = "ViewBudgetAndWorkerPay";
            ////comChart.Parameters.AddWithValue("@loops", 20);
            ////comChart.Parameters.AddWithValue("@growerId", Session["Id"].ToString());
            ////comChart.Parameters.AddWithValue("@startDate", DateTime.UtcNow.Month.ToString() + "/1/" + DateTime.UtcNow.Year.ToString());
            ////comChart.Parameters.AddWithValue("@endDate", (DateTime.UtcNow.Month + 1).ToString() + "/1/" + DateTime.UtcNow.Year.ToString());
            //comChart.Parameters.AddWithValue("@loops", 20);
            //comChart.Parameters.AddWithValue("@growerId", "grower4");
            //comChart.Parameters.AddWithValue("@startDate", "1-1-2017");
            //comChart.Parameters.AddWithValue("@endDate", "2-1-2017");

            //Chart1.DataSourceID = "";
            //Chart1.DataSource = comChart.ExecuteReader();
            //Chart1.Series[0].XValueMember = "month";
            //Chart1.Series[0].YValueMembers = "Workers Paid";
            //Chart1.DataBind();

            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("loops", Convert.ToString(6));
            SqlDataSource1.SelectParameters.Add("growerId", Session["Id"].ToString());
            SqlDataSource1.SelectParameters.Add("startDate", dt.Month.ToString() + "/1/" + dt.Year.ToString());
            SqlDataSource1.SelectParameters.Add("endDate", (dt.Month + 1).ToString() + "/1/" + dt.Year.ToString());
        }
            catch (Exception err6)
            {
                Session["error"] = err6.ToString();
                Response.Redirect("~/Debug.aspx");
            }

        try
        {
            SqlCommand comWorkerCheck = new SqlCommand("SELECT COUNT(DISTINCT WorkerID) FROM tbl_Duty WHERE Day >= @start AND Day < @end ", con);
            //comWorkerCheck.Parameters.AddWithValue("@0", Session["Id"].ToString());
            //AND GrowerId = @0"

            comWorkerCheck.Parameters.AddWithValue("@start", dt.AddMonths(-3).Month.ToString() + "/1/" + dt.AddMonths(-3).Year.ToString());
            comWorkerCheck.Parameters.AddWithValue("@end", dt.AddMonths(-2).Month.ToString() + "/1/" + dt.AddMonths(-2).Year.ToString());
            lbl_monthVal1.Text = comWorkerCheck.ExecuteScalar().ToString();
            comWorkerCheck.Parameters.Clear();
            lbl_month1.Text = dt.AddMonths(-3).ToString("MMM");
            comWorkerCheck.Parameters.AddWithValue("@start", dt.AddMonths(-2).Month.ToString() + "/1/" + dt.AddMonths(-2).Year.ToString());
            comWorkerCheck.Parameters.AddWithValue("@end", dt.AddMonths(-1).Month.ToString() + "/1/" + dt.AddMonths(-1).Year.ToString());
            lbl_monthVal2.Text = comWorkerCheck.ExecuteScalar().ToString();
            comWorkerCheck.Parameters.Clear();
            lbl_month2.Text = dt.AddMonths(-2).ToString("MMM");
            comWorkerCheck.Parameters.AddWithValue("@start", dt.AddMonths(-1).Month.ToString() + "/1/" + dt.AddMonths(-1).Year.ToString());
            comWorkerCheck.Parameters.AddWithValue("@end", dt.Month.ToString() + "/1/" + dt.Year.ToString());
            lbl_monthVal3.Text = comWorkerCheck.ExecuteScalar().ToString();
            comWorkerCheck.Parameters.Clear();
            lbl_month3.Text = dt.AddMonths(-1).ToString("MMM");
            comWorkerCheck.Parameters.AddWithValue("@start", dt.Month.ToString() + "/1/" + dt.Year.ToString());
            comWorkerCheck.Parameters.AddWithValue("@end", dt.AddMonths(1).Month.ToString() + "/1/" + dt.AddMonths(1).Year.ToString());
            lbl_monthVal4.Text = comWorkerCheck.ExecuteScalar().ToString();

            comWorkerCheck.Dispose();
        }
        catch (Exception err5)
        {
            Session["error"] = err5.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        try
        {
            //SqlCommand comTotalAmount = new SqlCommand("SELECT SUM(Amount) FROM tbl_budget WHERE GrowerID = @0 AND TimeStamp >= @monthStart AND TimeStamp < @monthEnd", con);
            SqlCommand comTotalAmount = new SqlCommand("SELECT SUM(Amount) FROM tbl_budget WHERE GrowerID = @0 AND TimeStamp >= @monthStart AND TimeStamp < @monthEnd", con);
            comTotalAmount.Parameters.AddWithValue("@0", Session["Id"].ToString());
            comTotalAmount.Parameters.AddWithValue("@monthStart", dt.Month.ToString() + "/1/" + dt.Year.ToString());
            comTotalAmount.Parameters.AddWithValue("@monthEnd", dt.AddMonths(1).Month.ToString() + "/1/" + dt.AddMonths(1).Year.ToString());

            SqlCommand comRemainingAmount = new SqlCommand("SELECT( SELECT SUM(Amount) AS 'S' FROM tbl_budget WHERE GrowerID = @growerId AND TimeStamp >= @monthStart AND TimeStamp < @monthEnd) - ( SELECT SUM((DATEPART(HOUR, tbl_Shift.totaltime) * tbl_worker.payrate)) AS 'S' FROM tbl_shift INNER JOIN tbl_Duty ON tbl_shift.ShiftID = tbl_Duty.ShiftID INNER JOIN tbl_worker ON tbl_worker.workersid = tbl_duty.workerid WHERE tbl_Duty.Day >= @monthStart AND tbl_Duty.Day < @monthEnd AND tbl_Duty.GrowerID = @growerId) AS 'S'", con);
            comRemainingAmount.Parameters.AddWithValue("@growerId", Session["Id"].ToString());
            comRemainingAmount.Parameters.AddWithValue("@monthStart", dt.Month.ToString() + "/1/" + dt.Year.ToString());
            comRemainingAmount.Parameters.AddWithValue("@monthEnd", dt.AddMonths(1).Month.ToString() + "/1/" + dt.AddMonths(1).Year.ToString());


            SqlDataReader readerTotalAmount = comTotalAmount.ExecuteReader();
            SqlDataReader readerRemainingAmount = comRemainingAmount.ExecuteReader();

            if (readerTotalAmount.HasRows && readerRemainingAmount.HasRows)
            {
                if(readerTotalAmount.Read())
                {
                    if (readerRemainingAmount.Read())
                    {
                        try
                        {
                            lbl_percentage2.Text = (Convert.ToDecimal(readerRemainingAmount[0]) * (100 / Convert.ToDecimal(readerTotalAmount[0]))).ToString("#,##0.00") + "%";
                        }
                        catch
                        {
                            //Should this display something to tell the user some data was missing?
                        }

                        //string amountVar = ;

                        //if (!amountVar.Contains('.'))
                        //{
                        //    amountVar = amountVar + ".00";
                        //}
                        //else if (amountVar.Length - amountVar.IndexOf(".") == 2)
                        //{
                        //    amountVar = amountVar + "0";
                        //}

                        //lbl_amount2.Text = ;

                        //if (!lbl_amount2.Text.Contains('.'))
                        //{
                        //    lbl_amount2.Text = lbl_amount2.Text + ".00";
                        //}
                        //else if (lbl_amount2.Text.Length - lbl_amount2.Text.IndexOf(".") == 2)
                        //{
                        //    lbl_amount2.Text = lbl_amount2.Text + "0";
                        //}
                        try
                        {
                            lbl_amount2.Text = "$" + Convert.ToDecimal(readerRemainingAmount[0].ToString()).ToString("#,##0.00") + "/$" + Convert.ToDecimal(readerTotalAmount[0].ToString()).ToString("#,##0.00");
                        }
                        catch
                        {
                            //Should this display something to tell the user some data was missing?
                        }
                    }
                }
            }

            readerTotalAmount.Close();
            readerRemainingAmount.Close();


            comRemainingAmount.Dispose();
            comTotalAmount.Dispose();
        }
        catch (Exception err4)
        {
            Session["error"] = err4.ToString();
            Response.Redirect("~/Debug.aspx");
        }


        try
        {


            SqlCommand com = new SqlCommand("SELECT count(*) FROM tbl_address WHERE AddressId = @0", con);
            com.Parameters.AddWithValue("@0", Session["Id"]);


            if (Convert.ToInt32(com.ExecuteScalar()) == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to fill in your address details'); window.location='" + Request.ApplicationPath + "Contractor/ContractorProfile.aspx';", true);
            }

            com.Dispose();
        }
        catch(Exception err1)
        {
            Session["error"] = err1.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        try
        {
            SqlCommand comAddress = new SqlCommand("SELECT Region FROM tbl_address WHERE AddressId = @0", con);
            comAddress.Parameters.AddWithValue("@0", Session["Id"].ToString());

            if (comAddress.ExecuteScalar() != DBNull.Value)
            {
                SqlCommand comAddress2 = new SqlCommand("SELECT AddressId FROM tbl_address WHERE Region = @0", con);
                comAddress2.Parameters.AddWithValue("@0", Convert.ToString(comAddress.ExecuteScalar()));

                SqlDataReader readerAddress2 = comAddress2.ExecuteReader();

                var count = 0;

                if (readerAddress2.HasRows)
                {


                    while (readerAddress2.Read())
                    {
                        SqlCommand comWorker = new SqlCommand("SELECT * FROM tbl_worker WHERE WorkersID = @0", con);
                        comWorker.Parameters.AddWithValue("@0", readerAddress2["AddressId"].ToString());

                        SqlDataReader readerWorker = comWorker.ExecuteReader();

                        if (readerWorker.HasRows)
                        {
                            count++;
                        }

                        comWorker.Dispose();
                        readerWorker.Close();
                    }
                }

                //lbl_workerRegion.Text = Convert.ToString(count);

                comAddress2.Dispose();
                readerAddress2.Close();
            }
            else
            {
                //lbl_workerRegion.Text = "?";

            }

            comAddress.Dispose();
        }
        catch(Exception err2)
        {
            Session["error"] = err2.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        try
        {
            SqlCommand comDuty = new SqlCommand("SELECT * FROM tbl_Duty WHERE GrowerID = @0", con);
            comDuty.Parameters.AddWithValue("@0", Session["Id"]);

            SqlDataReader readerDuty = comDuty.ExecuteReader();

            comDuty.Dispose();

            lbl_workerTodayContent.Text = "";
            lbl_workerTomorrowContent.Text = "";

            if (readerDuty.HasRows)
            {
                while (readerDuty.Read())
                {
                    SqlCommand comWorker2 = new SqlCommand("SELECT * FROM tbl_worker WHERE WorkersID = @0", con);
                    comWorker2.Parameters.AddWithValue("@0", readerDuty["WorkerID"]);

                    SqlDataReader readerWorker = comWorker2.ExecuteReader();

                    if (readerWorker.Read())
                    {
                       
                        
                        dc = new dateConverter();
                        DateTime date = Convert.ToDateTime(readerDuty["Day"]);
                        DateTime dtc = dc.convertUTCtoNZT(DateTime.UtcNow);



                        string img = "../img/14456900_1036563233107787_1965655255_o.jpg";

                        if (readerWorker["Picture"] != DBNull.Value)
                        {
                            img = readerWorker["Picture"].ToString().TrimStart('~');
                        }

                        if (DateTime.Compare(date.Date,dtc.Date)==0)
                        {
                            lbl_workerTodayContent.Text += "<div class=\"workingSingleItem\">" +
                            "<img src = \"" + img + "\" class=\"workingTodayBoxImage\" />" +
                            "<h3>" + readerWorker["FirstName"] + " " + readerWorker["LastName"] + "</h3>" +
                            "</div>";
                        }
                        DateTime dtt = dtc.AddDays(1);
                        if (DateTime.Compare(date.Date,dtt.Date)==0)
                        {
                            lbl_workerTomorrowContent.Text += "<div class=\"workingSingleItem\">" +
                            "<img src = \"" + img + "\" class=\"workingTodayBoxImage\" />" +
                            "<h3>" + readerWorker["FirstName"] + " " + readerWorker["LastName"] + "</h3>" +
                            "</div>";
                        }
                    }

                    readerWorker.Close();

                }
            }
            readerDuty.Close();
        }
        catch (Exception err3)
        {
            Session["error"] = err3.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        con.Close();
        con.Dispose();

        if (lbl_percentage2.Text.Contains("X%"))
        {
            lbl_percentage2.Text = "0%";
            lbl_amount2.Text = "$0.00";
        }
        if (Convert.ToDecimal(lbl_percentage2.Text.Replace('%', ' ')) <= 0)
        {
            System.Drawing.Color[] gg = new System.Drawing.Color[2];
            gg[0] = System.Drawing.Color.FromArgb(255, 203, 203, 203);
            gg[1] = System.Drawing.Color.FromArgb(255, 255, 0, 0);

            lbl_percentage2.Attributes.Add("style", "color:red");
            Chart1.PaletteCustomColors = gg;
        }
        
        
        
        
        
       
    }

    protected void Chart1_Load(object sender, EventArgs e)
    {

    }
}