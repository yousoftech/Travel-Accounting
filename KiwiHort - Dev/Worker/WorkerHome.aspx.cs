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

public partial class WorkerHome : System.Web.UI.Page
{
  

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }

        ((Label)Master.FindControl("lbl_title")).Text = "Dashboard";

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        try
        {
            dateConverter dc = new dateConverter();
            DateTime dt = dc.convertUTCtoNZT(DateTime.UtcNow);
            con.Open();

            SqlCommand comDuty = new SqlCommand("SELECT * FROM tbl_Duty WHERE Day = @0 AND WorkerID = @1", con);
            comDuty.Parameters.AddWithValue("@0", dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString());
            comDuty.Parameters.AddWithValue("@1", Session["Id"]);

            SqlDataReader readerDuty = comDuty.ExecuteReader();

            comDuty.Dispose();

            if (readerDuty.HasRows)
            {
                if (readerDuty.Read())
                {
                    SqlCommand comGrower = new SqlCommand("SELECT * FROM tbl_grower WHERE GrowersId = @0", con);
                    comGrower.Parameters.AddWithValue("@0", readerDuty["GrowerID"].ToString());

                    SqlDataReader readerGrower = comGrower.ExecuteReader();

                    comGrower.Dispose();

                    if (readerGrower.Read())
                    {
                        lbl_contractorValueToday.Text = readerGrower["FirstName"].ToString() + " " + readerGrower["LastName"].ToString();
                    }

                    readerGrower.Close();
                    
                    SqlCommand comShift = new SqlCommand("SELECT Shiftstarttime, ShiftendTime, [dbo].[tbl_pay].[pay] FROM tbl_Shift INNER JOIN tbl_duty ON tbl_shift.shiftid = tbl_duty.shiftid INNER JOIN tbl_worker ON tbl_duty.workerid=tbl_worker.workersid INNER JOIN [dbo].[tbl_pay] ON [dbo].[tbl_pay].[payID]=[dbo].[tbl_worker].[payrate] WHERE tbl_Shift.ShiftID = @0 AND tbl_duty.workerid = @1", con);
                    comShift.Parameters.AddWithValue("@0", readerDuty["ShiftID"]);
                    comShift.Parameters.AddWithValue("@1", Session["Id"]);

                    SqlDataReader readerShift = comShift.ExecuteReader();

                    comShift.Dispose();

                    if (readerShift.Read())
                    {
                        lbl_startTimeToday.Text = Convert.ToDateTime(readerShift["Shiftstarttime"].ToString().Substring(0, 5)).ToString("h:mm tt");
                        lbl_endTimeToday.Text = Convert.ToDateTime(readerShift["ShiftendTime"].ToString().Substring(0, 5)).ToString("h:mm tt");
                        
                        if(!readerShift["pay"].ToString().Contains('.'))
                        {
                            lbl_hourlyRateValueToday.Text = "$" + readerShift["pay"].ToString() + ".00";
                        }
                        else if(readerShift["pay"].ToString().Length - readerShift["pay"].ToString().IndexOf(".") == 2)
                        {
                            lbl_hourlyRateValueToday.Text = "$" + readerShift["pay"].ToString() + "0";
                        }
                        else
                        {
                            lbl_hourlyRateValueToday.Text = "$" + readerShift["pay"].ToString();
                        }
                        
                    }

                    readerShift.Close();

                }
            }
            SqlCommand comPay = new SqlCommand("SELECT pay FROM [dbo].[tbl_pay] inner join tbl_worker on [dbo].[tbl_pay].payID=tbl_worker.payrate WHERE  workersid = @0", con);
            comPay.Parameters.AddWithValue("@0", Session["Id"]);

            SqlDataReader readerPay = comPay.ExecuteReader();

            if (readerPay.Read())
            {
                decimal d = (decimal)readerPay["pay"];

                lbl_hourlyRateValueToday.Text = "$" + decimal.Round(d, 2, MidpointRounding.AwayFromZero).ToString();
                lbl_hourlyRateValueTomorrow.Text = "$" + decimal.Round(d, 2, MidpointRounding.AwayFromZero).ToString();


            }
            readerPay.Close();



            comDuty = new SqlCommand("SELECT * FROM tbl_Duty WHERE Day = @0 AND WorkerID = @1", con);
            comDuty.Parameters.AddWithValue("@0", dt.AddDays(1).Month.ToString() + "/" + dt.AddDays(1).Day.ToString() + "/" + dt.AddDays(1).Year.ToString());
            comDuty.Parameters.AddWithValue("@1", Session["Id"]);

            readerDuty = comDuty.ExecuteReader();

            comDuty.Dispose();

            if (readerDuty.HasRows)
            {
                if (readerDuty.Read())
                {
                    SqlCommand comGrower = new SqlCommand("SELECT * FROM tbl_grower WHERE GrowersId = @0", con);
                    comGrower.Parameters.AddWithValue("@0", readerDuty["GrowerID"].ToString());

                    SqlDataReader readerGrower = comGrower.ExecuteReader();

                    comGrower.Dispose();

                    if (readerGrower.Read())
                    {
                        lbl_contractorValueTomorrow.Text = readerGrower["FirstName"].ToString() + " " + readerGrower["LastName"].ToString();
                    }

                    readerGrower.Close();

                    SqlCommand comShift = new SqlCommand("SELECT Shiftstarttime, ShiftendTime FROM tbl_Shift INNER JOIN tbl_duty ON tbl_shift.shiftid = tbl_duty.shiftid INNER JOIN tbl_worker ON tbl_duty.workerid=tbl_worker.workersid WHERE tbl_Shift.ShiftID = @0 AND tbl_duty.workerid = @1", con);
                    comShift.Parameters.AddWithValue("@0", readerDuty["ShiftID"]);
                    comShift.Parameters.AddWithValue("@1", Session["Id"]);

                    SqlDataReader readerShift = comShift.ExecuteReader();

                    comShift.Dispose();

                    if (readerShift.Read())
                    {
                        lbl_startTimeTomorrow.Text = Convert.ToDateTime(readerShift["Shiftstarttime"].ToString().Substring(0, 5)).ToString("h:mm tt");
                        lbl_endTimeTomorrow.Text = Convert.ToDateTime(readerShift["ShiftendTime"].ToString().Substring(0, 5)).ToString("h:mm tt");
                        

                    }

                    readerShift.Close();

                }
            }
            


            readerDuty.Close();
        }
        catch (Exception err1)
        {
            Session["error"] = err1.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        try
        {
            SqlCommand comDuty2 = new SqlCommand("SELECT * FROM tbl_grower WHERE GrowersId = @0", con);
            comDuty2.Parameters.AddWithValue("@0", DateTime.Today.AddDays(1).Month.ToString() + "/" + DateTime.Today.AddDays(1).Day.ToString() + "/" + DateTime.Today.AddDays(1).Year.ToString());
            comDuty2.Parameters.AddWithValue("@1", Session["Id"]);

            SqlDataReader readerDuty2 = comDuty2.ExecuteReader();

            comDuty2.Dispose();

            if (readerDuty2.HasRows)
            {
                if (readerDuty2.Read())
                {
                    SqlCommand comGrower = new SqlCommand("SELECT * FROM tbl_grower WHERE GrowersId = @0", con);
                    comGrower.Parameters.AddWithValue("@0", readerDuty2["GrowerID"].ToString());

                    SqlDataReader readerGrower = comGrower.ExecuteReader();

                    comGrower.Dispose();

                    if (readerGrower.Read())
                    {
                        lbl_contractorValueTomorrow.Text = readerGrower["FirstName"].ToString() + " " + readerGrower["LastName"].ToString();
                    }

                    readerGrower.Close();

                    SqlCommand comShift = new SqlCommand("SELECT * FROM tbl_Shift WHERE ShiftID = @0", con);
                    comShift.Parameters.AddWithValue("@0", readerDuty2["ShiftID"]);

                    SqlDataReader readerShift = comShift.ExecuteReader();

                    comShift.Dispose();

                    if (readerShift.Read())
                    {
                        lbl_startTimeTomorrow.Text = Convert.ToDateTime(readerShift["Shiftstarttime"].ToString().Substring(0, 5)).ToString("h:mm tt");
                        lbl_endTimeTomorrow.Text = Convert.ToDateTime(readerShift["ShiftendTime"].ToString().Substring(0, 5)).ToString("h:mm tt");
                    }
                    readerShift.Close();
                }
            }
            readerDuty2.Close();
        }
        catch (Exception err2)
        {
            Session["error"] = err2.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        con.Close();
        con.Dispose();

        if (!IsPostBack)
        {

            

            sds_growers.SelectParameters.Add("0", Session["Id"].ToString());

            DataSourceSelectArguments args = new DataSourceSelectArguments();
            DataView view = (DataView)sds_growers.Select(args);
            DataTable dt = view.ToTable();
            dt.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName");

            cbo_contractors.DataSource = dt;
            cbo_contractors.DataTextField = "FullName";
            cbo_contractors.DataValueField = "GrowersId";
            cbo_contractors.DataBind();

            var weekStart = DateTime.Today.Date;
            var firstRun = true;

            for (var count = 0; count < 8; count++, weekStart = weekStart.AddDays(1))
            {
                while (weekStart.DayOfWeek.ToString() != "Monday")
                {
                    if (firstRun)
                    {
                        weekStart = weekStart.AddDays(-1);
                    }
                    else
                    {
                        weekStart = weekStart.AddDays(1);
                    }
                }
                firstRun = false;
                cbo_weekStart.Items.Add(new ListItem(weekStart.Day.ToString() + "/" + weekStart.Month.ToString() + "/" + weekStart.Year.ToString(), weekStart.Month.ToString() + "/" + weekStart.Day.ToString() + "/" + weekStart.Year.ToString())); //Problems. Big problems? - probably fixed
            }
            
            cbo_weekStart_SelectedIndexChanged(null, null);
        }
        //cbo_weekStart_SelectedIndexChanged(null, null);

    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        
    }

    protected void cbo_weekStart_SelectedIndexChanged(object sender, EventArgs e)
    {
        var wdgy = cbo_weekStart.SelectedIndex;
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        try
        {
            con.Open();

            SqlCommand com = new SqlCommand("SELECT * FROM tbl_Duty WHERE Day > @0 AND Day < @1 AND WorkerID like @2 AND GrowerID like @3", con);
            com.Parameters.AddWithValue("@0", DateTime.ParseExact(cbo_weekStart.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(-1));
            com.Parameters.AddWithValue("@1", DateTime.ParseExact(cbo_weekStart.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(7));
            com.Parameters.AddWithValue("@2", Session["Id"]);
            com.Parameters.AddWithValue("@3", cbo_contractors.SelectedValue);

            SqlDataReader reader = com.ExecuteReader();

            com.Dispose();

            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayEnd.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";

            dateConverter d = new dateConverter();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (var count = 0; count < 7; count++)
                    {
                        //var test2 = reader.GetString(reader.GetOrdinal("Day"));
                        //var test3 = Convert.ToDateTime(reader.GetString(reader.GetOrdinal("Day")));
                        //var test4 = d.convertUTCtoNZT(Convert.ToDateTime(reader["Day"]));
                        //var test5 = DateTime.ParseExact(cbo_weekStart.SelectedValue, "MM/dd/yyyy", CultureInfo.InvariantCulture).AddDays(count);

                        if (Convert.ToDateTime(reader["Day"]) == DateTime.ParseExact(cbo_weekStart.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(count))
                        {
                            SqlCommand com2 = new SqlCommand("SELECT Shiftstarttime FROM tbl_Shift WHERE ShiftID = @0", con);
                            com2.Parameters.AddWithValue("@0", Convert.ToString(reader["ShiftID"]));
                            SqlCommand com3 = new SqlCommand("SELECT ShiftendTime FROM tbl_Shift WHERE ShiftID = @0", con);
                            com3.Parameters.AddWithValue("@0", Convert.ToString(reader["ShiftID"]));

                            switch (count)
                            {
                                case 0:
                                    txt_mondayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_mondayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                                case 1:
                                    txt_tuesdayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_tuesdayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                                case 2:
                                    txt_wednesdayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_wednesdayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                                case 3:
                                    txt_thursdayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_thursdayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                                case 4:
                                    txt_fridayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_fridayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                                case 5:
                                    txt_saturdayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_saturdayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                                case 6:
                                    txt_sundayStart.Text = Convert.ToString(com2.ExecuteScalar());
                                    txt_sundayEnd.Text = Convert.ToString(com3.ExecuteScalar());
                                    break;
                            }
                            com2.Dispose();
                            com3.Dispose();
                        }
                    }
                }


                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('The work for the selected week has been listed.');", true);
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You have no work for the selected week.');", true);
            }
            //reader.Close();
        }
        catch (Exception err3)
        {
            Session["error"] = err3.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        con.Close();
        con.Dispose();
    }
    
}