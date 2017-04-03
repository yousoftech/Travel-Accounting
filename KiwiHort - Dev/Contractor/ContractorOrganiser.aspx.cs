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

public partial class ContractorOrganiser : System.Web.UI.Page
{

    //bool flag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.Compare(Session["redirect"].ToString(), "message") == 0)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('The timeslots have been assigned');", true);

            Session["redirect"] = "empty";

            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('The timeslots have been assigned');", true);
        }

        var countChk = 0;

        foreach (ListItem checkbox in chk_workers.Items)
        {
            if (checkbox.Selected)
            {
                Session["checkbox" + countChk.ToString()] = true;
            }
            countChk++;
        }



        if (Session["Id"] == null)
        {

            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your session has expired, you will need to log in again to continue using Kiwihort.'); window.location.href = 'login.aspx';", true);

            Response.Redirect("~/login.aspx");
        }



        if (Convert.ToString(hdf_flag.Value) == "false")
        {
            Session["flag"] = hdf_flag.Value;
        }

        ((Label)Master.FindControl("lbl_title")).Text = "Organiser";




        if (!IsPostBack && Convert.ToBoolean(Session["flag"]))
        {

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

            con.Open();

            if (!IsPostBack)
            {
                SqlCommand com = new SqlCommand("SELECT farm_name, farmId FROM tbl_farms WHERE growerId = @0", con);
                com.Parameters.AddWithValue("@0", Session["Id"]);



                SqlDataReader reader = com.ExecuteReader();

                cbo_mondayFarm.Items.Clear();
                cbo_tuesdayFarm.Items.Clear();
                cbo_wednesdayFarm.Items.Clear();
                cbo_thursdayFarm.Items.Clear();
                cbo_fridayFarm.Items.Clear();
                cbo_saturdayFarm.Items.Clear();
                cbo_sundayFarm.Items.Clear();
                cbo_workerAssignFarm.Items.Clear();
                
                cbo_workerAssignFarm.Items.Add("Select a Farm");
                cbo_farmWorker.Items.Clear();
                cbo_farmWorker.Items.Add("All Farms");
                //cbo_supervisorWorker.Items.Add("All Supervisors");

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        cbo_mondayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_tuesdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_wednesdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_thursdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_fridayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_saturdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_sundayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_workerAssignFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        cbo_farmWorker.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                    }
                }

                com.Dispose();
            }




            DataSourceSelectArguments args = new DataSourceSelectArguments();

            sds_workers.SelectCommand = "SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId], [tbl_login].[type] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] INNER JOIN [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]= [tbl_worker].[WorkersId] where [dbo].[tbl_employees].[growersid]=@0 ORDER BY [tbl_worker].[FirstName]";
            sds_workers.SelectParameters.Clear();
            sds_workers.SelectParameters.Add("0", Session["Id"].ToString());
            DataView view = (DataView)sds_workers.Select(args);
            DataTable dt1 = view.ToTable();

            dt1.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName");

            //if (!IsPostBack)
            //{
            //    chk_workers.Items.Clear();

            //    for (var count = 0; count < dt1.Rows.Count; count++)
            //    {
            //        if (dt1.Rows[count][3].ToString() == "Worker")
            //        {
            //            chk_workers.Items.Add(new ListItem(dt1.Rows[count][0].ToString() + dt1.Rows[count][1].ToString(), dt1.Rows[count][2].ToString()));
            //        }
            //        else
            //        {
            //            chk_workers.Items.Add(new ListItem(dt1.Rows[count][0].ToString() + dt1.Rows[count][1].ToString() + " - Supervisor", dt1.Rows[count][2].ToString()));
            //            chk_workers.Items[count].Attributes.CssStyle.Add("font-weight", "500");
            //        }
            //    }
            //}
            SqlCommand comSupervisor = new SqlCommand("select * from tbl_worker inner join tbl_login on tbl_login.id = tbl_worker.workersid where tbl_login.type='supervisor'", con);

            SqlDataReader readerSupervisor = comSupervisor.ExecuteReader();

            //if(readerSupervisor.HasRows)
            //{
            //    while(readerSupervisor.Read())
            //    {
            //        cbo_supervisorWorker.Items.Add(new ListItem(Convert.ToString(readerSupervisor["FirstName"]) + Convert.ToString(readerSupervisor["LastName"]), Convert.ToString(readerSupervisor["workersId"])));
            //    }
            //}

            

            var weekStart = DateTime.UtcNow;

            var firstRun = true;

            dateConverter d = new dateConverter();

            weekStart = d.convertUTCtoNZT(weekStart).Date; //Not sure if this is right. We're having issues with assigning work.
            

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
                cbo_weekStart.Items.Add(new ListItem(weekStart.Day.ToString() + "/" + weekStart.Month.ToString() + "/" + weekStart.Year.ToString(), d.convertNZTtoUTC(weekStart).ToString()));

            }

            weekStart = DateTime.UtcNow;
            weekStart = d.convertUTCtoNZT(weekStart).Date; 

            for (var count = 0; count < 16; count++, weekStart = weekStart.AddDays(1))
            {
                cbo_weekStart2.Items.Add(new ListItem(weekStart.Day.ToString() + "/" + weekStart.Month.ToString() + "/" + weekStart.Year.ToString() + " - " + weekStart.DayOfWeek.ToString(), weekStart.ToString()));
                cbo_workerAssignDay.Items.Add(new ListItem(weekStart.Day.ToString() + "/" + weekStart.Month.ToString() + "/" + weekStart.Year.ToString() + " - " + weekStart.DayOfWeek.ToString(), weekStart.ToString()));

            }



            SqlCommand comSupervisorSelect = new SqlCommand("SELECT tbl_worker.workersId, tbl_Shift.farmId, tbl_worker.FirstName, tbl_worker.LastName FROM tbl_worker INNER JOIN tbl_login ON tbl_login.Id = tbl_worker.workersId INNER JOIN tbl_Duty ON tbl_Duty.WorkerId = tbl_worker.workersId INNER JOIN tbl_Shift ON tbl_Duty.ShiftID = tbl_Shift.ShiftId WHERE tbl_login.type = 'supervisor' AND tbl_Duty.ShiftID = tbl_Shift.ShiftID AND tbl_Duty.Day = @0 ", con);

            comSupervisorSelect.Parameters.AddWithValue("@0", d.convertNZTtoUTC(Convert.ToDateTime(cbo_workerAssignDay.SelectedValue)));
            //comSupervisorSelect.Parameters.AddWithValue("@0", cbo_workerAssignDay.SelectedItem.Text);
            //comSupervisorSelect.Parameters.AddWithValue("@0", "12/05/2016");

            cbo_workerAssignSupervisor.Items.Clear();

            SqlDataReader readerSupervisorSelect = comSupervisorSelect.ExecuteReader();

            cbo_workerAssignSupervisor.Items.Clear();

            if (readerSupervisorSelect.HasRows)
            {
                while (readerSupervisorSelect.Read())
                {
                    if (cbo_workerAssignFarm.SelectedValue.ToString() == readerSupervisorSelect["farmId"].ToString())
                    {
                        cbo_workerAssignSupervisor.Items.Add(new ListItem(Convert.ToString(readerSupervisorSelect["FirstName"]) + Convert.ToString(readerSupervisorSelect["LastName"]), Convert.ToString(readerSupervisorSelect["workersId"])));
                    }
                    else
                    {
                        cbo_workerAssignSupervisor.Items.Add("There are no supervisors on this farm, for this day");
                    }
                }

            }
            else
            {
                cbo_workerAssignSupervisor.Items.Add("There are no supervisors on this farm, for this day");
            }

            con.Close();

            comSupervisorSelect.Dispose();

            readerSupervisorSelect.Dispose();

            con.Dispose();






        }


        

        if (IsPostBack && !Convert.ToBoolean(Session["flag"]) || !IsPostBack && Convert.ToBoolean(Session["flag"]))
        {
            cbo_weekStart2_SelectedIndexChanged(null, null);
        }

        if (!IsPostBack)
        {

            DataSourceSelectArguments args = new DataSourceSelectArguments();
            sds_workers.SelectCommand = "SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId], [tbl_login].[type], [dbo].[tbl_pay].[pay] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] INNER JOIN [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]= [tbl_worker].[WorkersId] Inner Join [dbo].[tbl_pay] on [dbo].[tbl_pay].[payID]=[dbo].[tbl_worker].[payrate] where [dbo].[tbl_employees].[growersid]=@0 ORDER BY [tbl_worker].[FirstName]";
            sds_workers.SelectParameters.Clear();
            sds_workers.SelectParameters.Add("0", Session["Id"].ToString());
            DataView view = (DataView)sds_workers.Select(args);
            DataTable dt1 = view.ToTable();

            dt1.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName");


            chk_workers.Items.Clear();

            for (var count = 0; count < dt1.Rows.Count; count++)
            {
                if (dt1.Rows[count][3].ToString() == "Worker")
                {
                    chk_workers.Items.Add(new ListItem(dt1.Rows[count][0].ToString() + dt1.Rows[count][1].ToString() + " - $" + Convert.ToDecimal(dt1.Rows[count][4]).ToString("#,##0.00") + "/hr", dt1.Rows[count][2].ToString()));
                }
                else
                {
                    chk_workers.Items.Add(new ListItem(dt1.Rows[count][0].ToString() + dt1.Rows[count][1].ToString() + " - $" + Convert.ToDecimal(dt1.Rows[count][4]).ToString("#,##0.00") + "/hr - Supervisor", dt1.Rows[count][2].ToString()));
                    chk_workers.Items[count].Attributes.CssStyle.Add("font-weight", "500");
                }
            }
        }
        //if (IsPostBack)
        {
            DataSourceSelectArguments args = new DataSourceSelectArguments();
            sds_workers.SelectCommand = "SELECT [tbl_login].[type] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] INNER JOIN [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]= [tbl_worker].[WorkersId] where [dbo].[tbl_employees].[growersid]=@0 ORDER BY [tbl_worker].[FirstName]";
            sds_workers.SelectParameters.Clear();
            sds_workers.SelectParameters.Add("0", Session["Id"].ToString());
            DataView view = (DataView)sds_workers.Select(args);
            DataTable dt1 = view.ToTable();

            for (var count = 0; count < dt1.Rows.Count; count++)
            {
                if (dt1.Rows[count][0].ToString() != "Worker")
                {
                    chk_workers.Items[count].Attributes.CssStyle.Add("font-weight", "500");
                }
            }

            
            
            dateConverter d = new dateConverter();

            
            
            if (DateTime.Compare(d.convertUTCtoNZT(Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString())).Date, d.convertUTCtoNZT(DateTime.UtcNow).Date) <= 0)
            //if (d.convertUTCtoNZT(Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString())) < d.convertUTCtoNZT(DateTime.UtcNow))
            {
                int count = 0;
                DateTime dts = d.convertUTCtoNZT(Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString()));
                for (DateTime dateCounter =dts.Date; DateTime.Compare(dateCounter.Date, d.convertUTCtoNZT(DateTime.UtcNow).Date)<0; dateCounter = dateCounter.AddDays(1), count++)

                //    for (DateTime dateCounter = d.convertUTCtoNZT(Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString())); dateCounter < d.convertUTCtoNZT(DateTime.UtcNow); dateCounter = dateCounter.AddDays(1), count++)
                {
                    if (count == 0)
                    {
                        txt_mondayStart.Enabled = false;
                        txt_mondayStart.Text = "";
                        txt_mondayEnd.Enabled = false;
                        txt_mondayEnd.Text = "";
                        cbo_mondayFarm.Enabled = false;
                        cbo_mondayFarm.Items.Add("N/A");
                        cbo_mondayFarm.Text = "N/A";
                    }
                    else if (count == 1)
                    {
                        txt_tuesdayStart.Enabled = false;
                        txt_tuesdayStart.Text = "";
                        txt_tuesdayEnd.Enabled = false;
                        txt_tuesdayEnd.Text = "";
                        cbo_tuesdayFarm.Enabled = false;
                        cbo_tuesdayFarm.Items.Add("N/A");
                        cbo_tuesdayFarm.Text = "N/A";
                    }
                    else if (count == 2)
                    {
                        txt_wednesdayStart.Enabled = false;
                        txt_wednesdayStart.Text = "";
                        txt_wednesdayEnd.Enabled = false;
                        txt_wednesdayEnd.Text = "";
                        cbo_wednesdayFarm.Enabled = false;
                        cbo_wednesdayFarm.Items.Add("N/A");
                        cbo_wednesdayFarm.Text = "N/A";
                    }
                    else if (count == 3)
                    {
                        txt_thursdayStart.Enabled = false;
                        txt_thursdayStart.Text = "";
                        txt_thursdayEnd.Enabled = false;
                        txt_thursdayEnd.Text = "";
                        cbo_thursdayFarm.Enabled = false;
                        cbo_thursdayFarm.Items.Add("N/A");
                        cbo_thursdayFarm.Text = "N/A";
                    }
                    else if (count == 4)
                    {
                        txt_fridayStart.Enabled = false;
                        txt_fridayStart.Text = "";
                        txt_fridayEnd.Enabled = false;
                        txt_fridayEnd.Text = "";
                        cbo_fridayFarm.Enabled = false;
                        cbo_fridayFarm.Items.Add("N/A");
                        cbo_fridayFarm.Text = "N/A";
                    }
                    else if (count == 5)
                    {
                        txt_saturdayStart.Enabled = false;
                        txt_saturdayStart.Text = "";
                        txt_saturdayEnd.Enabled = false;
                        txt_saturdayEnd.Text = "";
                        cbo_saturdayFarm.Enabled = false;
                        cbo_saturdayFarm.Items.Add("N/A");
                        cbo_saturdayFarm.Text = "N/A";
                    }
                }
            }
            else
            {
                SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

                con.Open();

                SqlCommand com = new SqlCommand("SELECT farm_name, farmId FROM tbl_farms WHERE growerId = @0", con);
                com.Parameters.AddWithValue("@0", Session["Id"]);



                SqlDataReader reader = com.ExecuteReader();

                //cbo_mondayFarm.Items.Clear();
                //cbo_tuesdayFarm.Items.Clear();
                //cbo_wednesdayFarm.Items.Clear();
                //cbo_thursdayFarm.Items.Clear();
                //cbo_fridayFarm.Items.Clear();
                //cbo_saturdayFarm.Items.Clear();
                //cbo_sundayFarm.Items.Clear();
                //cbo_workerAssignFarm.Items.Clear();

                //cbo_farmWorker.Items.Clear();
                //cbo_workerAssignFarm.Items.Add("Select a Farm");
                //cbo_farmWorker.Items.Add("All Farms");
                //cbo_supervisorWorker.Items.Add("All Supervisors");

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        cbo_mondayFarm.Enabled = true;
                        cbo_tuesdayFarm.Enabled = true;
                        cbo_wednesdayFarm.Enabled = true;
                        cbo_thursdayFarm.Enabled = true;
                        cbo_fridayFarm.Enabled = true;
                        cbo_saturdayFarm.Enabled = true;
                        cbo_sundayFarm.Enabled = true;
                        txt_mondayStart.Enabled = true;
                        txt_tuesdayStart.Enabled = true;
                        txt_wednesdayStart.Enabled = true;
                        txt_thursdayStart.Enabled = true;
                        txt_fridayStart.Enabled = true;
                        txt_saturdayStart.Enabled = true;
                        txt_sundayStart.Enabled = true;
                        txt_mondayEnd.Enabled = true;
                        txt_tuesdayEnd.Enabled = true;
                        txt_wednesdayEnd.Enabled = true;
                        txt_thursdayEnd.Enabled = true;
                        txt_fridayEnd.Enabled = true;
                        txt_saturdayEnd.Enabled = true;
                        txt_sundayEnd.Enabled = true;
                        //cbo_mondayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_tuesdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_wednesdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_thursdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_fridayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_saturdayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_sundayFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_workerAssignFarm.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                        //cbo_farmWorker.Items.Add(new ListItem(reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
                    }
                }

                com.Dispose();
            }
        }

        //sds_workers.SelectCommand = "SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId], [tbl_login].[type] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] INNER JOIN [dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]= [tbl_worker].[WorkersId] where [dbo].[tbl_employees].[growersid]=@0 ORDER BY [tbl_worker].[FirstName]";
        //sds_workers.SelectParameters.Clear();
        //sds_workers.SelectParameters.Add("0", Session["Id"].ToString());
        //sds_workers.DataBind();
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        int count1 = 0;
        bool flagt = true;
         dateConverter d = new dateConverter();
        TextBox[] timeInput1 = { txt_mondayStart, txt_mondayEnd, txt_tuesdayStart, txt_tuesdayEnd,txt_wednesdayStart, txt_wednesdayEnd, txt_thursdayStart, txt_thursdayEnd, txt_fridayStart, txt_fridayEnd,
            txt_saturdayStart, txt_saturdayEnd, txt_sundayStart, txt_sundayEnd};
        //int i = 0;
        //for (DateTime dateCounter = d.convertUTCtoNZT(Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString()).Date); DateTime.Compare(dateCounter.Date, d.convertUTCtoNZT(DateTime.UtcNow).Date) < 1; dateCounter = dateCounter.AddDays(1), count1++)
        //{
        //    if (DateTime.Compare(dateCounter.Date, d.convertUTCtoNZT(DateTime.UtcNow).Date) == 0)
        //    {
        //        i = count1 * 2;
        //        DateTime stime = Convert.ToDateTime(timeInput1[i].Text);
        //        DateTime etime = Convert.ToDateTime(timeInput1[i + 1].Text);
        //        if (stime.TimeOfDay > (d.convertUTCtoNZT(DateTime.UtcNow)).TimeOfDay)
        //        {
        //            if (etime.TimeOfDay > (d.convertUTCtoNZT(DateTime.UtcNow)).TimeOfDay)
        //            {
        //                if (stime.TimeOfDay < etime.TimeOfDay)
        //                {
        //                    flagt = true;
        //                }
        //                else
        //                {
        //                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('End time must be > start end');", true);

        //                }
        //            }
        //            else
        //            {
        //                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('End time must be < current time');", true);
        //            }
        //        }
        //        else
        //        {
        //            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('start time must be < current time');", true);

        //        }
        //    }
        //}
        if (flagt==true)
        {
           





        string[,,] str = new string[1000, 9, 4];

        //1st Dimension
        //0 = person 1
        //1 = person 2
        //...
        //2nd Dimension
        //0   = Worker
        //1   = Contractor
        //2-8 = Dates
        //3rd Dimension
        //0   = Email/Date
        //1   = first name/start time
        //2   = last name/end time
        //3   = Farm Name

        //str[0,0,0] = "stephen_l43@otmail.co.nz";
        //str[0, 0,1] = "Stephen";
        //str[0, 0,2] = "Clinton";
        //str[0, 1,0] = "shlok@hypernova.co.nz";
        //str[0, 1,1] = "Shlok";
        //str[0, 1,2] = "Kant";
        //str[0, 2,0] = "12/12/2016";
        //str[0, 2,1] = "9:00 AM";
        //str[0, 2,2] = "5:00 PM";
        //str[0, 2,3] = "Rod's Farm";
        //str[0, 3,0] = "13/12/2016";
        //str[0, 3,1] = "9:00 AM";
        //str[0, 3,2] = "5:00 PM";
        //str[0, 3,3] = "Rod's Farm";
        //str[0, 4,0] = "14/12/2016";
        //str[0, 4,1] = "9:00 AM";
        //str[0, 4,2] = "5:00 PM";
        //str[0, 4,3] = "Rod's Farm";
        //str[0, 5,0] = "15/12/2016";
        //str[0, 5,1] = "9:00 AM";
        //str[0, 5,2] = "5:00 PM";
        //str[0, 5,3] = "Rod's Farm";
        //str[0, 6,0] = "16/12/2016";
        //str[0, 6,1] = "9:00 AM";
        //str[0, 6,2] = "5:00 PM";
        //str[0, 6,3] = "Rod's Farm";
        //str[0, 7,0] = "";
        //str[0, 7,1] = "";
        //str[0, 7,2] = "";
        //str[0, 7,3] = "";
        //str[0, 8,0] = "";
        //str[0, 8,1] = "";
        //str[0, 8,2] = "";
        //str[0, 8,3] = "";

        //Get worker email
        //Get worker names
        //Get work dates
        //Get start and end times for those dates
        //Get farm name
        //Get contractor name & email


        Session["flag"] = true;


        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        try
        {
            con.Open();




            if (txt_mondayStart.Text != "" && txt_mondayEnd.Text != "")
            {
                SqlCommand comGetId = new SqlCommand("IF (SELECT max(CAST(SUBSTRING(Id, 7, 30) AS INT)) FROM tbl_login WHERE SUBSTRING(Id, 1, 6) = 'grower') IS NULL SELECT 0 ELSE SELECT max(CAST(SUBSTRING(Id, 7, 30) AS INT)) FROM tbl_login WHERE SUBSTRING(Id, 1, 6) = 'grower'", con);
                var idNum = (int)comGetId.ExecuteScalar();
                idNum++;
                var idStr = "grower" + idNum.ToString();

                comGetId.Dispose();
            }

                //TimeSpan hours = Convert.ToDateTime(txt_mondayEnd.Text).TimeOfDay.Subtract(Convert.ToDateTime(txt_mondayStart.Text).TimeOfDay);
            }
            catch (Exception err1)
            {
                Session["error"] = "578";
                Response.Redirect("~/Debug.aspx");
            }



            TextBox[] timeInput = { txt_mondayStart, txt_mondayEnd, txt_tuesdayStart, txt_tuesdayEnd,
                    txt_wednesdayStart, txt_wednesdayEnd, txt_thursdayStart, txt_thursdayEnd, txt_fridayStart, txt_fridayEnd,
                    txt_saturdayStart, txt_saturdayEnd, txt_sundayStart, txt_sundayEnd};

        DropDownList[] farmInput = { cbo_mondayFarm, cbo_tuesdayFarm, cbo_wednesdayFarm, cbo_thursdayFarm, cbo_fridayFarm, cbo_saturdayFarm, cbo_sundayFarm };

        DateTime[] start = new DateTime[timeInput.Length];
        DateTime[] end = new DateTime[timeInput.Length];

        bool timeslots = false;

        for (var count = 0; count < timeInput.Length; count++)
        {
            if (timeInput[count].Enabled && !timeInput[count].Text.Equals(""))
            {
                int idNum = 0;
                try
                {
                    SqlCommand comGetId = new SqlCommand("IF(SELECT max(ShiftID) FROM tbl_Shift) IS NULL SELECT 0 ELSE SELECT max(ShiftID) FROM tbl_Shift", con);
                    idNum = (int)comGetId.ExecuteScalar();
                    idNum++;

                    comGetId.Dispose();
                }
                catch (Exception err1)
                {
                    Session["error"] = "(SELECT max(ShiftID) FROM tb";
                    Response.Redirect("~/Debug.aspx");
                }





                if (count % 2 == 0)
                {
                        try
                        {

                            start[count] = Convert.ToDateTime(timeInput[count].Text);
                        }
                        catch (Exception err1)
                        {
                            Session["error"] = "start time"  + timeInput[count].Text;
                            Response.Redirect("~/Debug.aspx");
                        }
                    }
                else if (start[count - 1] != default(DateTime))
                {
                        try
                        {
                            end[count] = Convert.ToDateTime(timeInput[count].Text);




                            TimeSpan hours = end[count].TimeOfDay.Subtract(start[count - 1].TimeOfDay);

                            SqlCommand comShift = new SqlCommand("INSERT INTO tbl_Shift VALUES (@0, @1, @2, @3, @4)", con);
                            comShift.Parameters.AddWithValue("@0", idNum);
                            comShift.Parameters.AddWithValue("@1", start[count - 1].TimeOfDay.ToString());
                            comShift.Parameters.AddWithValue("@2", end[count].TimeOfDay.ToString());
                            comShift.Parameters.AddWithValue("@3", hours.ToString());
                            comShift.Parameters.AddWithValue("@4", farmInput[(count - 1) / 2].SelectedValue.ToString());




                            comShift.ExecuteReader();


                            comShift.Dispose();
                        }
                        catch (Exception err1)
                        {
                            Session["error"] = "end time" + timeInput[count].Text;
                            Response.Redirect("~/Debug.aspx");
                        }
                        var countChk = 0;


                    foreach (ListItem item in chk_workers.Items)
                    {
                        if (Convert.ToBoolean(Session["checkbox" + countChk.ToString()]))
                        {

                              

                                try
                                {
                                    item.Selected = true;
                           DateTime day = Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString()).AddDays(count / 2);
                                    //  DateTime day = Convert.ToDateTime(cbo_weekStart.SelectedItem.Text).AddDays(count/2);
                                    dateConverter dccc = new dateConverter();
                                    day = dccc.convertUTCtoNZT(day);
                                SqlCommand comDutyRemove = new SqlCommand("DELETE FROM tbl_Duty WHERE Day = @0 AND WorkerID = @1 AND GrowerID = @2", con);
                                comDutyRemove.Parameters.AddWithValue("@0", day.Date);
                                //comDutyRemove.Parameters.AddWithValue("@0", day.Month.ToString() + "/" + day.Day.ToString() + "/" + day.Year.ToString());
                                comDutyRemove.Parameters.AddWithValue("@1", item.Value.ToString());
                                comDutyRemove.Parameters.AddWithValue("@2", Session["Id"].ToString());

                                comDutyRemove.ExecuteReader();

                                comDutyRemove.Dispose();
                          
                                SqlCommand comDuty = new SqlCommand("INSERT INTO tbl_Duty (ShiftID, Day, WorkerID, GrowerID) VALUES (@0, @1, @2, @3)", con);
                                comDuty.Parameters.AddWithValue("@0", idNum);
                                //comDuty.Parameters.AddWithValue("@1", cbo_weekStart.SelectedValue.ToString());
                                comDuty.Parameters.AddWithValue("@1", day.Date);
                                //comDuty.Parameters.AddWithValue("@1", day.Month.ToString() + "/" + day.Day.ToString() + "/" + day.Year.ToString());
                                comDuty.Parameters.AddWithValue("@2", item.Value.ToString());
                                comDuty.Parameters.AddWithValue("@3", Session["Id"].ToString());


                                SqlCommand comWorkerSelect = new SqlCommand("SELECT * FROM tbl_login WHERE Id = @0", con);
                                comWorkerSelect.Parameters.AddWithValue("@0", item.Value.ToString());

                                SqlDataReader readerSelect = comWorkerSelect.ExecuteReader();

                                if (readerSelect.HasRows)
                                {
                                    if (readerSelect.Read())
                                    {
                                        str[countChk, 0, 0] = readerSelect.GetValue(1).ToString();
                                    }
                                }

                                readerSelect.Close();



                                comWorkerSelect.Parameters.Clear();
                                comWorkerSelect = new SqlCommand("SELECT * FROM tbl_worker WHERE workersId = @0", con);
                                comWorkerSelect.Parameters.AddWithValue("@0", item.Value.ToString());

                                SqlDataReader readerSelect2 = comWorkerSelect.ExecuteReader();

                                if (readerSelect2.HasRows)
                                {
                                    if (readerSelect2.Read())
                                    {
                                        str[countChk, 0, 1] = readerSelect2.GetValue(1).ToString();
                                        str[countChk, 0, 2] = readerSelect2.GetValue(2).ToString();
                                    }
                                }

                                comWorkerSelect.Dispose();
                                readerSelect2.Close();



                                SqlCommand comGrowerSelect = new SqlCommand("SELECT * FROM tbl_login WHERE Id = @0", con);
                                comGrowerSelect.Parameters.AddWithValue("@0", Session["Id"].ToString());

                                SqlDataReader readerSelectG = comGrowerSelect.ExecuteReader();

                                if (readerSelectG.HasRows)
                                {
                                    if (readerSelectG.Read())
                                    {
                                        str[countChk, 1, 0] = readerSelectG.GetValue(1).ToString();
                                    }
                                }

                                readerSelectG.Close();


                                comGrowerSelect.Parameters.Clear();
                                comGrowerSelect = new SqlCommand("SELECT * FROM tbl_grower WHERE growersId = @0", con);
                                comGrowerSelect.Parameters.AddWithValue("@0", Session["Id"].ToString());

                                SqlDataReader readerSelect2G = comGrowerSelect.ExecuteReader();

                                if (readerSelect2G.HasRows)
                                {
                                    if (readerSelect2G.Read())
                                    {
                                        str[countChk, 1, 1] = readerSelect2G.GetValue(1).ToString();
                                        str[countChk, 1, 2] = readerSelect2G.GetValue(2).ToString();
                                    }
                                }

                                comWorkerSelect.Dispose();
                                readerSelect2G.Close();




                                comDuty.ExecuteReader();

                                comDuty.Dispose();
                            }
                            catch (Exception err3)
                            {
                                Session["error"] = "Count chk";
                                Response.Redirect("~/Debug.aspx");
                            }

                            timeslots = true;
                        }
                        countChk++;
                    }

                        try
                        {
                            str[0, ((count - 1) / 2) + 2, 0] = Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString()).AddDays(count / 2).Date.ToString();
                            str[0, ((count - 1) / 2) + 2, 1] = start[count - 1].TimeOfDay.ToString();
                            str[0, ((count - 1) / 2) + 2, 2] = end[count].TimeOfDay.ToString();
                            str[0, ((count - 1) / 2) + 2, 3] = farmInput[(count - 1) / 2].SelectedItem.ToString();
                        }

                        catch (Exception fdfe)
                        {
                            Session["error"] = str[0, ((count - 1) / 2) + 2, 0] + str[0, ((count - 1) / 2) + 2, 1]+ str[0, ((count - 1) / 2) + 2, 2] + str[0, ((count - 1) / 2) + 2, 3];
                            Response.Redirect("~/Debug.aspx");

                        }

                }



            }
        }


        for (int countChk = 0; countChk < chk_workers.Items.Count; countChk++)
        {
            Session.Remove("checkbox" + countChk.ToString());
        }

            try
            {

                if (timeslots)
                {
                    var count = 0;
                    foreach (ListItem item in chk_workers.Items)
                    {
                        if (item.Selected)
                        {


                            for (int count2 = 2; count2 < 9; count2++)
                            {
                                string input = str[0, count2, 0];
                                if (input != null)
                                {
                                    if (input.IndexOf(" ") != -1)
                                    {
                                        int index = input.IndexOf(" ");
                                        str[0, count2, 0] = str[0, count2, 0].Substring(0, index);
                                    }



                                }

                            }

                            SendEmail se = new SendEmail();

                            se.EmailSend(str[count, 0, 0], "Kiwihort Work Assignment for " + str[count, 1, 1] + " " + str[count, 1, 2], "Hello " + str[count, 0, 1] + " " + str[count, 0, 2] + ",<br><br>This is an automated email that has been sent out by the Kiwihort system to notify you that " + str[count, 1, 1] + " " + str[count, 1, 2] + "has assigned the following hours for you, on the following days<br><br>" +
                                "Monday: " + str[0, 2, 0] + ": " + str[0, 2, 1] + " - " + str[0, 2, 2] + "    Farm: " + str[0, 2, 3] + "<br>" +
                                "Tuesday: " + str[0, 3, 0] + ": " + str[0, 3, 1] + " - " + str[0, 3, 2] + "    Farm: " + str[0, 3, 3] + "<br>" +
                                "Wednesday: " + str[0, 4, 0] + ": " + str[0, 4, 1] + " - " + str[0, 4, 2] + "    Farm: " + str[0, 4, 3] + "<br>" +
                                "Thursday: " + str[0, 5, 0] + ": " + str[0, 5, 1] + " - " + str[0, 5, 2] + "    Farm: " + str[0, 5, 3] + "<br>" +
                                "Friday: " + str[0, 6, 0] + ": " + str[0, 6, 1] + " - " + str[0, 6, 2] + "    Farm: " + str[0, 6, 3] + "<br>" +
                                "Saturday: " + str[0, 7, 0] + ": " + str[0, 7, 1] + " - " + str[0, 7, 2] + "    Farm: " + str[0, 7, 3] + "<br>" +
                                "Sunday: " + str[0, 8, 0] + ": " + str[0, 8, 1] + " - " + str[0, 8, 2] + "    Farm: " + str[0, 8, 3] + "<br>"
                                );

                        }

                        count++;
                    }

                    Session["redirect"] = "orgSubmit";
                    Response.Redirect("../temp.aspx", false);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to select at least one person, and assign at least one timeslot, that has not already been assigned to the person(s)');", true);
                }
            }
            catch(Exception ehgfhr)
            {
                Session["error"] ="ëmail";
                Response.Redirect("~/Debug.aspx");
            }
           
            con.Close();
        con.Dispose();

        cbo_weekStart2_SelectedIndexChanged(null, null);


    }
    }
    //protected void btn_submit_Click(object sender, EventArgs e)
    //{





    //    string[,,] str = new string[1000,9,4];

    //    //1st Dimension
    //    //0 = person 1
    //    //1 = person 2
    //    //...
    //    //2nd Dimension
    //    //0   = Worker
    //    //1   = Contractor
    //    //2-8 = Dates
    //    //3rd Dimension
    //    //0   = Email/Date
    //    //1   = first name/start time
    //    //2   = last name/end time
    //    //3   = Farm Name

    //    //str[0,0,0] = "stephen_l43@otmail.co.nz";
    //    //str[0, 0,1] = "Stephen";
    //    //str[0, 0,2] = "Clinton";
    //    //str[0, 1,0] = "shlok@hypernova.co.nz";
    //    //str[0, 1,1] = "Shlok";
    //    //str[0, 1,2] = "Kant";
    //    //str[0, 2,0] = "12/12/2016";
    //    //str[0, 2,1] = "9:00 AM";
    //    //str[0, 2,2] = "5:00 PM";
    //    //str[0, 2,3] = "Rod's Farm";
    //    //str[0, 3,0] = "13/12/2016";
    //    //str[0, 3,1] = "9:00 AM";
    //    //str[0, 3,2] = "5:00 PM";
    //    //str[0, 3,3] = "Rod's Farm";
    //    //str[0, 4,0] = "14/12/2016";
    //    //str[0, 4,1] = "9:00 AM";
    //    //str[0, 4,2] = "5:00 PM";
    //    //str[0, 4,3] = "Rod's Farm";
    //    //str[0, 5,0] = "15/12/2016";
    //    //str[0, 5,1] = "9:00 AM";
    //    //str[0, 5,2] = "5:00 PM";
    //    //str[0, 5,3] = "Rod's Farm";
    //    //str[0, 6,0] = "16/12/2016";
    //    //str[0, 6,1] = "9:00 AM";
    //    //str[0, 6,2] = "5:00 PM";
    //    //str[0, 6,3] = "Rod's Farm";
    //    //str[0, 7,0] = "";
    //    //str[0, 7,1] = "";
    //    //str[0, 7,2] = "";
    //    //str[0, 7,3] = "";
    //    //str[0, 8,0] = "";
    //    //str[0, 8,1] = "";
    //    //str[0, 8,2] = "";
    //    //str[0, 8,3] = "";

    //    //Get worker email
    //    //Get worker names
    //    //Get work dates
    //    //Get start and end times for those dates
    //    //Get farm name
    //    //Get contractor name & email


    //    Session["flag"] = true;


    //    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

    //    try
    //    {
    //        con.Open();




    //        if (txt_mondayStart.Text != "" && txt_mondayEnd.Text != "")
    //        {
    //            SqlCommand comGetId = new SqlCommand("IF (SELECT max(CAST(SUBSTRING(Id, 7, 30) AS INT)) FROM tbl_login WHERE SUBSTRING(Id, 1, 6) = 'grower') IS NULL SELECT 0 ELSE SELECT max(CAST(SUBSTRING(Id, 7, 30) AS INT)) FROM tbl_login WHERE SUBSTRING(Id, 1, 6) = 'grower'", con);
    //            var idNum = (int)comGetId.ExecuteScalar();
    //            idNum++;
    //            var idStr = "grower" + idNum.ToString();

    //            comGetId.Dispose();
    //        }
    //    }
    //    catch (Exception err1)
    //    {
    //        Session["error"] = err1.ToString();
    //        Response.Redirect("~/Debug.aspx");
    //    }

    //    //TimeSpan hours = Convert.ToDateTime(txt_mondayEnd.Text).TimeOfDay.Subtract(Convert.ToDateTime(txt_mondayStart.Text).TimeOfDay);



    //    TextBox[] timeInput = { txt_mondayStart, txt_mondayEnd, txt_tuesdayStart, txt_tuesdayEnd,
    //        txt_wednesdayStart, txt_wednesdayEnd, txt_thursdayStart, txt_thursdayEnd, txt_fridayStart, txt_fridayEnd,
    //        txt_saturdayStart, txt_saturdayEnd, txt_sundayStart, txt_sundayEnd};

    //    DropDownList[] farmInput = { cbo_mondayFarm, cbo_tuesdayFarm, cbo_wednesdayFarm, cbo_thursdayFarm, cbo_fridayFarm, cbo_saturdayFarm, cbo_sundayFarm };

    //    DateTime[] start = new DateTime[timeInput.Length];
    //    DateTime[] end = new DateTime[timeInput.Length];

    //    bool timeslots = false;

    //    for (var count = 0; count < timeInput.Length; count++)
    //    {
    //        if(timeInput[count].Enabled && !timeInput[count].Text.Equals(""))
    //        {
    //            int idNum = 0;
    //            try
    //            {
    //                SqlCommand comGetId = new SqlCommand("IF(SELECT max(ShiftID) FROM tbl_Shift) IS NULL SELECT 0 ELSE SELECT max(ShiftID) FROM tbl_Shift", con);
    //                idNum = (int)comGetId.ExecuteScalar();
    //                idNum++;

    //                comGetId.Dispose();
    //            }
    //            catch (Exception err1)
    //            {
    //                Session["error"] = err1.ToString();
    //                Response.Redirect("~/Debug.aspx");
    //            }





    //            if (count % 2 == 0)
    //            {
    //                start[count] = Convert.ToDateTime(timeInput[count].Text);
    //            }
    //            else if(start[count - 1] != default(DateTime))
    //            {
    //                end[count] = Convert.ToDateTime(timeInput[count].Text);



    //                TimeSpan hours = end[count].TimeOfDay.Subtract(start[count - 1].TimeOfDay);

    //                SqlCommand comShift = new SqlCommand("INSERT INTO tbl_Shift VALUES (@0, @1, @2, @3, @4)", con);
    //                comShift.Parameters.AddWithValue("@0", idNum);
    //                comShift.Parameters.AddWithValue("@1", start[count - 1].TimeOfDay.ToString());
    //                comShift.Parameters.AddWithValue("@2", end[count].TimeOfDay.ToString());
    //                comShift.Parameters.AddWithValue("@3", hours.ToString());
    //                comShift.Parameters.AddWithValue("@4", farmInput[(count - 1)/2].SelectedValue.ToString());




    //                comShift.ExecuteReader();


    //                comShift.Dispose();

    //                var countChk = 0;


    //                foreach (ListItem item in chk_workers.Items)
    //                {
    //                    if(Convert.ToBoolean(Session["checkbox" + countChk.ToString()]))
    //                    {
    //                        item.Selected = true;
    //                        DateTime day = Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString()).AddDays(count / 2);
    //                        //DateTime day = Convert.ToDateTime(cbo_weekStart.SelectedItem.Text).AddDays(count/2);

    //                        try
    //                        {
    //                            SqlCommand comDutyRemove = new SqlCommand("DELETE FROM tbl_Duty WHERE Day = @0 AND WorkerID = @1 AND GrowerID = @2", con);
    //                            comDutyRemove.Parameters.AddWithValue("@0", day.Date);
    //                            //comDutyRemove.Parameters.AddWithValue("@0", day.Month.ToString() + "/" + day.Day.ToString() + "/" + day.Year.ToString());
    //                            comDutyRemove.Parameters.AddWithValue("@1", item.Value.ToString());
    //                            comDutyRemove.Parameters.AddWithValue("@2", Session["Id"].ToString());

    //                            comDutyRemove.ExecuteReader();

    //                            comDutyRemove.Dispose();
    //                        }
    //                        catch (Exception err2)
    //                        {
    //                            Session["error"] = err2.ToString();
    //                            Response.Redirect("~/Debug.aspx");
    //                        }

    //                        try
    //                        {
    //                            SqlCommand comDuty = new SqlCommand("INSERT INTO tbl_Duty (ShiftID, Day, WorkerID, GrowerID) VALUES (@0, @1, @2, @3)", con);
    //                            comDuty.Parameters.AddWithValue("@0", idNum);
    //                            //comDuty.Parameters.AddWithValue("@1", cbo_weekStart.SelectedValue.ToString());
    //                            comDuty.Parameters.AddWithValue("@1", day.Date);
    //                            //comDuty.Parameters.AddWithValue("@1", day.Month.ToString() + "/" + day.Day.ToString() + "/" + day.Year.ToString());
    //                            comDuty.Parameters.AddWithValue("@2", item.Value.ToString());
    //                            comDuty.Parameters.AddWithValue("@3", Session["Id"].ToString());


    //                            SqlCommand comWorkerSelect = new SqlCommand("SELECT * FROM tbl_login WHERE Id = @0", con);
    //                            comWorkerSelect.Parameters.AddWithValue("@0", item.Value.ToString());

    //                            SqlDataReader readerSelect = comWorkerSelect.ExecuteReader();

    //                            if(readerSelect.HasRows)
    //                            {
    //                                if(readerSelect.Read())
    //                                {
    //                                    str[countChk, 0, 0] = readerSelect.GetValue(1).ToString();
    //                                }
    //                            }

    //                            readerSelect.Close();



    //                            comWorkerSelect.Parameters.Clear();
    //                            comWorkerSelect = new SqlCommand("SELECT * FROM tbl_worker WHERE workersId = @0", con);
    //                            comWorkerSelect.Parameters.AddWithValue("@0", item.Value.ToString());

    //                            SqlDataReader readerSelect2 = comWorkerSelect.ExecuteReader();

    //                            if(readerSelect2.HasRows)
    //                            {
    //                                if(readerSelect2.Read())
    //                                {
    //                                    str[countChk, 0, 1] = readerSelect2.GetValue(1).ToString();
    //                                    str[countChk, 0, 2] = readerSelect2.GetValue(2).ToString();
    //                                }
    //                            }

    //                            comWorkerSelect.Dispose();
    //                            readerSelect2.Close();



    //                            SqlCommand comGrowerSelect = new SqlCommand("SELECT * FROM tbl_login WHERE Id = @0", con);
    //                            comGrowerSelect.Parameters.AddWithValue("@0", Session["Id"].ToString());

    //                            SqlDataReader readerSelectG = comGrowerSelect.ExecuteReader();

    //                            if (readerSelectG.HasRows)
    //                            {
    //                                if (readerSelectG.Read())
    //                                {
    //                                    str[countChk, 1, 0] = readerSelectG.GetValue(1).ToString();
    //                                }
    //                            }

    //                            readerSelectG.Close();


    //                            comGrowerSelect.Parameters.Clear();
    //                            comGrowerSelect = new SqlCommand("SELECT * FROM tbl_grower WHERE growersId = @0", con);
    //                            comGrowerSelect.Parameters.AddWithValue("@0", Session["Id"].ToString());

    //                            SqlDataReader readerSelect2G = comGrowerSelect.ExecuteReader();

    //                            if (readerSelect2G.HasRows)
    //                            {
    //                                if (readerSelect2G.Read())
    //                                {
    //                                    str[countChk, 1, 1] = readerSelect2G.GetValue(1).ToString();
    //                                    str[countChk, 1, 2] = readerSelect2G.GetValue(2).ToString();
    //                                }
    //                            }

    //                            comWorkerSelect.Dispose();
    //                            readerSelect2G.Close();




    //                            comDuty.ExecuteReader();

    //                            comDuty.Dispose();
    //                        }
    //                        catch (Exception err3)
    //                        {
    //                            Session["error"] = err3.ToString();
    //                            Response.Redirect("~/Debug.aspx");
    //                        }

    //                        timeslots = true;
    //                    }
    //                    countChk++;
    //                }


    //                str[0, ((count - 1) / 2) + 2, 0] = Convert.ToDateTime(cbo_weekStart.SelectedValue.ToString()).AddDays(count / 2).Date.ToString();
    //                str[0, ((count - 1) / 2) + 2, 1] = start[count - 1].TimeOfDay.ToString();
    //                str[0, ((count - 1) / 2) + 2, 2] = end[count].TimeOfDay.ToString();
    //                str[0, ((count - 1) / 2) + 2, 3] = farmInput[(count - 1) / 2].SelectedItem.ToString();

    //            }



    //        }
    //    }


    //    for(int countChk = 0; countChk < chk_workers.Items.Count; countChk++)
    //    {
    //        Session.Remove("checkbox" + countChk.ToString());
    //    }


    //    if (timeslots)
    //    {
    //        var count = 0;
    //        foreach(ListItem item in chk_workers.Items)
    //        {
    //            if(item.Selected)
    //            {


    //                for (int count2 = 2; count2 < 9; count2++)
    //                {
    //                    string input = str[0, count2, 0];
    //                    if (input != null)
    //                    {
    //                        if(input.IndexOf(" ") != -1)
    //                        {
    //                            int index = input.IndexOf(" ");
    //                            str[0, count2, 0] = str[0, count2, 0].Substring(0, index);
    //                        }



    //                    }

    //                }

    //                SendEmail se = new SendEmail();

    //                se.EmailSend(str[count, 0, 0], "Kiwihort Work Assignment for " + str[count, 1, 1] + " " + str[count, 1, 2], "Hello " + str[count, 0, 1] + " " + str[count, 0, 2] + ",<br><br>This is an automated email that has been sent out by the Kiwihort system to notify you that " + str[count, 1, 1] + " " + str[count, 1, 2] + "has assigned the following hours for you, on the following days<br><br>" +
    //                    "Monday: " + str[0, 2, 0] + ": " + str[0, 2, 1] + " - " + str[0, 2, 2] + "    Farm: " + str[0, 2, 3] + "<br>" +
    //                    "Tuesday: " + str[0, 3, 0] + ": " + str[0, 3, 1] + " - " + str[0, 3, 2] + "    Farm: " + str[0, 3, 3] + "<br>" +
    //                    "Wednesday: " + str[0, 4, 0] + ": " + str[0, 4, 1] + " - " + str[0, 4, 2] + "    Farm: " + str[0, 4, 3] + "<br>" +
    //                    "Thursday: " + str[0, 5, 0] + ": " + str[0, 5, 1] + " - " + str[0, 5, 2] + "    Farm: " + str[0, 5, 3] + "<br>" +
    //                    "Friday: " + str[0, 6, 0] + ": " + str[0, 6, 1] + " - " + str[0, 6, 2] + "    Farm: " + str[0, 6, 3] + "<br>" +
    //                    "Saturday: " + str[0, 7, 0] + ": " + str[0, 7, 1] + " - " + str[0, 7, 2] + "    Farm: " + str[0, 7, 3] + "<br>" +
    //                    "Sunday: " + str[0, 8, 0] + ": " + str[0, 8, 1] + " - " + str[0, 8, 2] + "    Farm: " + str[0, 8, 3] + "<br>"
    //                    );

    //            }

    //            count++;
    //        }

    //        Session["redirect"] = "orgSubmit";
    //        Response.Redirect("../temp.aspx");
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You need to select at least one person, and assign at least one timeslot, that has not already been assigned to the person(s)');", true);
    //    }

    //    con.Close();
    //    con.Dispose();

    //    cbo_weekStart2_SelectedIndexChanged(null, null);

    //}

    protected void btn_submit2_Click(object sender, EventArgs e)
    {
        Session["flag"] = false;
    }

    //protected void dgd_workers_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    var test = dgd_workers.SelectedIndex.ToString();
    //    //string str = dgd_workers.SelectedRow.Cells[1].ToString();
    //    //txt_fridayStart.Text = dgd_workers.SelectedRow.Cells[0].Text;
    //    //string str = dgd_workers.SelectedValue.ToString();
    //}

    protected void dgd_organiserWorkers_SelectedIndexChanged(object sender, EventArgs e)
    {



        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();


        SqlCommand com3 = new SqlCommand("SELECT [dbo].[tbl_worker].[workersid] FROM tbl_worker where firstName = @0 and lastName = @1", con);

        com3.Parameters.AddWithValue("@0", dgd_organiserWorkers.SelectedRow.Cells[1].Text);
        string s = HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[2].Text);
        com3.Parameters.AddWithValue("@1", s);


       
        SqlDataReader reader3 = com3.ExecuteReader();

        com3.Dispose();

        if (reader3.Read())
        {
            if (reader3.HasRows)
            {




                for (int count = 0; count < chk_workers.Items.Count; count++)
                {
                    if (String.Compare(chk_workers.Items[count].Value, reader3.GetString(0)) == 0)
                    {
                        chk_workers.ClearSelection();
                        chk_workers.Items[count].Selected = true;
                    }
                }
            }
        }

        reader3.Close();
        con.Dispose();





        //old code

        //DateTime dt4 = Convert.ToDateTime(cbo_weekStart2.SelectedValue);


        //SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        //con.Open();
        //SqlCommand com3 = null;

        //var strd = 1;

        //if (cbo_farmWorker.SelectedIndex == 0 && txt_search.Text != "")
        //{

        //    strd = 1;
        //}
        //else if (cbo_farmWorker.SelectedIndex != 0 && txt_search.Text != "")
        //{
        //    strd = 2;
        //}
        //else if (cbo_farmWorker.SelectedIndex == 0 && txt_search.Text == "")
        //{
        //    strd = 3;
        //}
        //else if (cbo_farmWorker.SelectedIndex != 0 && txt_search.Text == "")
        //{
        //    strd = 4;
        //}


        //switch (strd)
        //{
        //    case 1:
        //        com3 = new SqlCommand("SELECT [dbo].[tbl_worker].[workersid] FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_duty.day = @date and ([dbo].[tbl_worker].firstname like '%'+ @search +'%' or [dbo].[tbl_worker].lastname like '%'+ @search +'%') order by [dbo].[tbl_worker].firstname", con);

        //        com3.Parameters.AddWithValue("@search", txt_search.Text);
        //        break;

        //    case 2:
        //        com3 = new SqlCommand("SELECT [dbo].[tbl_worker].[workersid] FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_shift.farmid = @farmid and tbl_duty.day = @date and ([dbo].[tbl_worker].firstname like '%'+ @search +'%' or [dbo].[tbl_worker].lastname like '%'+ @search +'%') order by [dbo].[tbl_worker].firstname", con);

        //        com3.Parameters.AddWithValue("@farmid", cbo_farmWorker.SelectedValue.ToString());
        //        com3.Parameters.AddWithValue("@search", txt_search.Text);
        //        break;
        //    case 3:
        //        com3 = new SqlCommand("SELECT [dbo].[tbl_worker].[workersid] FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_duty.day = @date order by [dbo].[tbl_worker].firstname", con);

        //        break;

        //    case 4:
        //        com3 = new SqlCommand("SELECT [dbo].[tbl_worker].[workersid] FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_shift.farmid = @farmid and tbl_duty.day = @date order by [dbo].[tbl_worker].firstname", con);

        //        com3.Parameters.AddWithValue("@farmid", cbo_farmWorker.SelectedValue.ToString());
        //        break;

        //}

        //com3.Parameters.AddWithValue("@date", dt4.Month.ToString() + "/" + dt4.Day.ToString() + "/" + dt4.Year.ToString());

        //SqlDataReader reader3 = com3.ExecuteReader();

        //com3.Dispose();

        //if(reader3.Read())
        //{
        //    if(reader3.HasRows)
        //    {




        //        for (int count = 0; count < chk_workers.Items.Count; count++)
        //        {
        //            if (String.Compare(chk_workers.Items[count].Value, reader3.GetString(dgd_organiserWorkers.SelectedIndex)) == 0)
        //            {
        //                chk_workers.ClearSelection();
        //                chk_workers.Items[count].Selected = true;
        //            }
        //        }
        //    }
        //}

        //reader3.Close();
        //con.Dispose();


        

        var test = dgd_organiserWorkers.SelectedIndex.ToString();
        string str = dgd_organiserWorkers.SelectedRow.Cells[1].Text.ToString();

        //chk_workers.ClearSelection();
        //chk_workers.SelectedIndex = dgd_organiserWorkers.SelectedIndex;

        //if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Monday")
        if (Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Monday")
        {
            cbo_weekStart.SelectedValue = cbo_weekStart2.SelectedValue;

            txt_mondayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_mondayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
           for (int i=0;i<cbo_mondayFarm.Items.Count;i++)
            {
                
                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_mondayFarm.Items[i].Text))
                {
                    
                    cbo_mondayFarm.SelectedIndex = i;
                }
            }

           

            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";
        }
        //else if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Tuesday")
        else if(Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Tuesday")
        {
            cbo_weekStart.SelectedValue = Convert.ToDateTime(cbo_weekStart2.SelectedValue).AddDays(-1).ToString();

            txt_tuesdayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_tuesdayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
            for (int i = 0; i < cbo_tuesdayFarm.Items.Count; i++)
            {

                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_tuesdayFarm.Items[i].Text))
                {

                    cbo_tuesdayFarm.SelectedIndex = i;
                }
            }
            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";
        }
        //else if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Wednesday")
        else if(Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Wednesday")
        {
            cbo_weekStart.SelectedValue = Convert.ToDateTime(cbo_weekStart2.SelectedValue).AddDays(-2).ToString();

            txt_wednesdayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_wednesdayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
            for (int i = 0; i < cbo_wednesdayFarm.Items.Count; i++)
            {

                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_wednesdayFarm.Items[i].Text))
                {

                    cbo_wednesdayFarm.SelectedIndex = i;
                }
            }
            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";
        }
        //else if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Thursday")
        else if(Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Thursday")
        {

            dateConverter dtss = new dateConverter();
            DateTime dts = Convert.ToDateTime(cbo_weekStart2.SelectedValue.ToString());
          dts=  dtss.convertNZTtoUTC(dts);
            cbo_weekStart.SelectedValue = dts.AddDays(-3).ToString();

            txt_thursdayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_thursdayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
            for (int i = 0; i < cbo_thursdayFarm.Items.Count; i++)
            {

                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_thursdayFarm.Items[i].Text))
                {

                    cbo_thursdayFarm.SelectedIndex = i;
                }
            }
            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";
        }
        //else if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Friday")
        else if(Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Friday")
        {
            dateConverter dtss = new dateConverter();
            DateTime dts = Convert.ToDateTime(cbo_weekStart2.SelectedValue.ToString());
            dts = dtss.convertNZTtoUTC(dts);
            cbo_weekStart.SelectedValue = dts.AddDays(-4).ToString();

            txt_fridayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_fridayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
            for (int i = 0; i < cbo_fridayFarm.Items.Count; i++)
            {

                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_fridayFarm.Items[i].Text))
                {

                    cbo_fridayFarm.SelectedIndex = i;
                }
            }
            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";
        }
        //else if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Saturday")
        else if(Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Saturday")
        {
            cbo_weekStart.SelectedValue = Convert.ToDateTime(cbo_weekStart2.SelectedValue).AddDays(-5).ToString();

            txt_saturdayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_saturdayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
            for (int i = 0; i < cbo_saturdayFarm.Items.Count; i++)
            {

                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_saturdayFarm.Items[i].Text))
                {

                    cbo_saturdayFarm.SelectedIndex = i;
                }
            }
            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
            txt_sundayStart.Text = "--:-- --";
            txt_sundayEnd.Text = "--:-- --";
        }
        //else if(DateTime.ParseExact(cbo_weekStart2.SelectedValue, "M/d/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString() == "Sunday")
        else if(Convert.ToDateTime(cbo_weekStart2.SelectedValue).DayOfWeek.ToString() == "Sunday")
        {
            cbo_weekStart.SelectedValue = Convert.ToDateTime(cbo_weekStart2.SelectedValue).AddDays(-6).ToString();

            txt_sundayStart.Text = dgd_organiserWorkers.SelectedRow.Cells[3].Text.ToString();
            txt_sundayEnd.Text = dgd_organiserWorkers.SelectedRow.Cells[4].Text.ToString();
            for (int i = 0; i < cbo_sundayFarm.Items.Count; i++)
            {

                if (string.Equals(HttpUtility.HtmlDecode(dgd_organiserWorkers.SelectedRow.Cells[5].Text), cbo_sundayFarm.Items[i].Text))
                {

                    cbo_sundayFarm.SelectedIndex = i;
                }
            }
            txt_tuesdayStart.Text = "--:-- --";
            txt_tuesdayEnd.Text = "--:-- --";
            txt_wednesdayStart.Text = "--:-- --";
            txt_wednesdayEnd.Text = "--:-- --";
            txt_thursdayStart.Text = "--:-- --";
            txt_thursdayEnd.Text = "--:-- --";
            txt_fridayStart.Text = "--:-- --";
            txt_fridayEnd.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_saturdayStart.Text = "--:-- --";
            txt_mondayStart.Text = "--:-- --";
            txt_mondayEnd.Text = "--:-- --";
        }
        
    }

    protected void cbo_weekStart2_SelectedIndexChanged(object sender, EventArgs e)
    {
        btn_search_Click(null, null);


        //SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        //SqlDataReader reader = null;

        //try
        //{
        //    con.Open();

        //    SqlCommand comData = new SqlCommand("SELECT * FROM tbl_worker", con);

        //    reader = comData.ExecuteReader();

        //    comData.Dispose();
        //}
        //catch (Exception err4)
        //{
        //    Session["error"] = err4.ToString();
        //    Response.Redirect("~/Debug.aspx");
        //}

        //if (reader.HasRows)
        //{
        //    DataTable dt = new DataTable();
        //    DataRow dr = null;
        //    dt.Columns.Add(new DataColumn("First Name", typeof(string)));
        //    dt.Columns.Add(new DataColumn("Last Name", typeof(string)));
        //    dt.Columns.Add(new DataColumn("Start Time", typeof(string)));
        //    dt.Columns.Add(new DataColumn("End Time", typeof(string)));
        //    dt.Columns.Add(new DataColumn("Farm Name", typeof(string)));

        //    List<string> done = new List<string>();

        //    while (reader.Read())
        //    {

        //        try
        //        {
        //            DateTime dt4 = Convert.ToDateTime(cbo_weekStart2.SelectedValue);



        //            SqlCommand com = new SqlCommand("SELECT * FROM tbl_Duty WHERE WorkerID = @0 AND Day = @1 AND GrowerID = @2", con);
        //            com.Parameters.AddWithValue("@0", reader["workersId"]);
        //            com.Parameters.AddWithValue("@1",dt4.Month.ToString()+"/"+dt4.Day.ToString()+ "/" + dt4.Year.ToString() );
        //            com.Parameters.AddWithValue("@2", Session["Id"]);

        //            SqlDataReader reader2 = com.ExecuteReader();

        //            if (reader2.HasRows)
        //            {
        //                if (reader2.Read())
        //                {
        //                    //try
        //                    //{
        //                    //    dt.Columns.Add(new DataColumn(Convert.ToDateTime(reader2["Day"].ToString()).Day + "/" + Convert.ToDateTime(reader2["Day"].ToString()).Month + "/" + Convert.ToDateTime(reader2["Day"].ToString()).Year + " Start Time", typeof(string)));
        //                    //    dt.Columns.Add(new DataColumn(Convert.ToDateTime(reader2["Day"].ToString()).Day + "/" + Convert.ToDateTime(reader2["Day"].ToString()).Month + "/" + Convert.ToDateTime(reader2["Day"].ToString()).Year + " End Time", typeof(string)));
        //                    //}
        //                    //catch { } //This try catch just exists to stop the thing from creating more than one column.
        //                    try
        //                    {
        //                        SqlCommand com3 = new SqlCommand();
        //                        var str = 1;

        //                        if (cbo_farmWorker.SelectedIndex == 0)
        //                        {

        //                            str = 1;
        //                        }
        //                        else
        //                        {
        //                            str = 2;
        //                        }

        //                        //if (cbo_farmWorker.SelectedIndex == 0 && cbo_supervisorWorker.SelectedIndex == 0)
        //                        //{

        //                        //    str = 1;
        //                        //}
        //                        //else if (cbo_farmWorker.SelectedIndex != 0 && cbo_supervisorWorker.SelectedIndex == 0)
        //                        //{
        //                        //    str = 2;
        //                        //}
        //                        //else if (cbo_farmWorker.SelectedIndex == 0 && cbo_supervisorWorker.SelectedIndex != 0)
        //                        //{
        //                        //    str = 3;
        //                        //}
        //                        //else if(cbo_farmWorker.SelectedIndex != 0 && cbo_supervisorWorker.SelectedIndex != 0)
        //                        //{
        //                        //    str = 4;
        //                        //}
        //                        com3.Parameters.Clear();
        //                        //SqlDataSource1.SelectParameters.Clear();
        //                        switch (str)
        //                        {
        //                            case 1:
        //                                com3 = new SqlCommand("SELECT [ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0", con);
        //                                //SqlDataSource1.SelectCommand = "SELECT[ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms] on[dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0";
        //                                break;

        //                            case 2:
        //                                com3 = new SqlCommand("SELECT [ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0 and [dbo].[tbl_Shift].[farmId] = @1", con);
        //                                com3.Parameters.AddWithValue("@1", cbo_farmWorker.SelectedValue.ToString());
        //                                //SqlDataSource1.SelectCommand = com3.CommandText;
        //                                break;

        //                            //case 3:
        //                            //    com3 = new SqlCommand("SELECT [dbo].[tbl_Shift].[ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] INNER JOIN [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[ShiftID] = [dbo].[tbl_Shift].[ShiftID] WHERE [dbo].[tbl_Shift].[ShiftID] = @0 and [dbo].[tbl_Duty].[supervisorId] = @1", con);
        //                            //    com3.Parameters.AddWithValue("@1", cbo_supervisorWorker.SelectedValue.ToString());
        //                            //    //SqlDataSource1.SelectCommand = com3.CommandText;
        //                            //    break;

        //                            //case 4:
        //                            //    com3 = new SqlCommand("SELECT [tbl_Shift].[ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] INNER JOIN  [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[ShiftID] = [dbo].[tbl_Shift].[ShiftID]   WHERE [tbl_Shift].[ShiftID]   = @0 and [dbo].[tbl_Duty].[supervisorId] = @1 and [dbo].[tbl_Shift].[farmId] = @2", con);
        //                            //    com3.Parameters.AddWithValue("@1", cbo_supervisorWorker.SelectedValue.ToString());
        //                            //    com3.Parameters.AddWithValue("@2", cbo_farmWorker.SelectedValue.ToString());
        //                            //    //SqlDataSource1.SelectCommand = com3.CommandText;
        //                            //    break;
        //                        }

        //                        com3.Parameters.AddWithValue("@0", reader2["ShiftID"]);
        //                        //SqlDataSource1.SelectParameters.Add("0", reader2["ShiftID"].ToString());
        //                        SqlDataReader reader3 = com3.ExecuteReader();
                               
                            
        //                        com3.Dispose();
                                

        //                        if (reader3.HasRows)
        //                        {
        //                            if (reader3.Read())
        //                            {
        //                                ClientScript.RegisterStartupScript(this.GetType(), "myalert", reader["FirstName"].ToString(), true);
                                    
        //                            dr = dt.NewRow();
        //                                dr["First Name"] = reader["FirstName"].ToString();
        //                                dr["Last Name"] = reader["LastName"].ToString();
        //                                dr["Farm Name"] = reader3["Farm_name"].ToString();
        //                                if (Convert.ToInt32(reader3["Shiftstarttime"].ToString().Substring(0, 2)) > 12)
        //                                {
        //                                    if (Convert.ToString(Convert.ToInt32(reader3["Shiftstarttime"].ToString().Substring(0, 2)) - 12).Length == 2)
        //                                    {
        //                                        dr["Start Time"] = Convert.ToString(Convert.ToInt32(reader3["Shiftstarttime"].ToString().Substring(0, 2)) - 12) + ":" + reader3["Shiftstarttime"].ToString().Substring(3, 2) + " PM";
        //                                    }
        //                                    else
        //                                    {
        //                                        dr["Start Time"] = "0" + Convert.ToString(Convert.ToInt32(reader3["Shiftstarttime"].ToString().Substring(0, 2)) - 12) + ":" + reader3["Shiftstarttime"].ToString().Substring(3, 2) + " PM";
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    dr["Start Time"] = reader3["Shiftstarttime"].ToString().Substring(0, 2) + ":" + reader3["Shiftstarttime"].ToString().Substring(3, 2) + " AM";
        //                                }
        //                                if (Convert.ToInt32(reader3["Shiftendtime"].ToString().Substring(0, 2)) > 12)
        //                                {
        //                                    if (Convert.ToString(Convert.ToInt32(reader3["Shiftendtime"].ToString().Substring(0, 2)) - 12).Length == 2)
        //                                    {
        //                                        dr["End Time"] = Convert.ToString(Convert.ToInt32(reader3["Shiftendtime"].ToString().Substring(0, 2)) - 12) + ":" + reader3["Shiftendtime"].ToString().Substring(3, 2) + " PM";
        //                                    }
        //                                    else
        //                                    {
        //                                        dr["End Time"] = "0" + Convert.ToString(Convert.ToInt32(reader3["Shiftendtime"].ToString().Substring(0, 2)) - 12) + ":" + reader3["Shiftendtime"].ToString().Substring(3, 2) + " PM";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    dr["End Time"] = reader3["Shiftendtime"].ToString().Substring(0, 2) + ":" + reader3["Shiftendtime"].ToString().Substring(3, 2) + " AM";
        //                                }
        //                                dt.Rows.Add(dr);
        //                                done.Add(reader["WorkersId"].ToString());
        //                            }
        //                        }
        //                        reader3.Close();
        //                    }
        //                    catch (Exception err6)
        //                    {
        //                        Session["error"] = err6.ToString();
        //                        Response.Redirect("~/Debug.aspx");
        //                    }
        //                }




        //            }
        //            com.Dispose();
        //            reader2.Dispose();
        //            if (!done.Contains(reader["WorkersId"].ToString()))
        //            {
        //                dr = dt.NewRow();
        //                dr["First Name"] = reader["FirstName"].ToString();
        //                dr["Last Name"] = reader["LastName"].ToString();
        //                dr["Start Time"] = "--:-- --";
        //                dr["End Time"] = "--:-- --";
        //                dt.Rows.Add(dr);
        //            }
        //        }
        //        catch (Exception err5)
        //        {
        //            Session["error"] = err5.ToString();
        //            Response.Redirect("~/Debug.aspx");
        //        }

        //        //dr = dt.NewRow();
        //        //dr["First Name"] = reader["FirstName"].ToString();
        //        //dr["Last Name"] = reader["LastName"].ToString();
        //        //for (var count = 0; count < startTimes.Count(); count++)
        //        //{
        //        //    dr[date[count] + " Start Time"] = startTimes[count];
        //        //    dr[date[count] + " End Time"] = endTimes[count];
        //        //}
        //        //dt.Rows.Add(dr);

                  
                
               

        //    }

        //    ViewState["CurrentTable"] = dt;

        //    //if (flag == false)
        //    {
        //        dgd_organiserWorkers.DataSource = dt;
        //        dgd_organiserWorkers.DataBind();
        //        //flag = true;
        //    }
        //}
        ////dgd_organiserWorkers.DataSource = SqlDataSource1;
        //dgd_organiserWorkers.DataBind();
        //reader.Close();

        //con.Close();
        //con.Dispose();
    }

    protected void dgd_workers_Sorting(object sender, GridViewSortEventArgs e)
    {
        cbo_weekStart2_SelectedIndexChanged(null, null);

        dgd_organiserWorkers.DataBind();
    }

    protected void cbo_workerAssignDay_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbo_workerAssignFarm.SelectedIndex = 0;
        cbo_workerAssignSupervisor.SelectedIndex = 0;
        chk_workersForSupervisors.Items.Clear();
    }




    protected void cbo_workerAssignSupervisor_SelectedIndexChanged(object sender, EventArgs e)
    {

        chk_workersForSupervisors.Items.Clear(); 
        
        sds_workersForSupervisors.SelectParameters.Clear();
        sds_workersForSupervisors.SelectCommand = "SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId], [tbl_login].[type] FROM [tbl_worker]" +
            "INNER JOIN[tbl_login] ON[tbl_login].[Id] =[tbl_worker].[WorkersId]" +
            "INNER JOIN[tbl_duty] ON[tbl_Duty].[workerId] =[tbl_worker].[WorkersId]" +
            "INNER JOIN[tbl_Shift] ON[tbl_Shift].[ShiftID] =[tbl_duty].[ShiftId]" +
            "WHERE Day = @0 AND farmId = @1 AND Supervisorid is null";
        sds_workersForSupervisors.SelectParameters.Add("0", Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Month.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Day.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Year.ToString());
        //sds_workersForSupervisors.SelectParameters.Add("0", cbo_workerAssignDay.SelectedValue.ToString());
        sds_workersForSupervisors.SelectParameters.Add("1", cbo_workerAssignFarm.SelectedValue.ToString());
        sds_workersForSupervisors.DataBind();

        DataSourceSelectArguments args = new DataSourceSelectArguments();
        DataView view = (DataView)sds_workersForSupervisors.Select(args);
        DataTable dt1 = view.ToTable();

        dt1.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName");


        for (var count = 0; count < dt1.Rows.Count; count++)
        {
            if (dt1.Rows[count][3].ToString() == "Worker")
            {
                chk_workersForSupervisors.Items.Add(new ListItem(dt1.Rows[count][0].ToString() + dt1.Rows[count][1].ToString(), dt1.Rows[count][2].ToString()));
            }
            else
            {
                chk_workersForSupervisors.Items.Add(new ListItem(dt1.Rows[count][0].ToString() + dt1.Rows[count][1].ToString() + " - Supervisor", dt1.Rows[count][2].ToString()));
                chk_workersForSupervisors.Items[count].Attributes.CssStyle.Add("display", "none");
            }
        }

        if(cbo_workerAssignSupervisor.SelectedIndex == 0)
        {
            chk_workersForSupervisors.Items.Clear();
        }

    }
    
    

    protected void cbo_workerAssignFarm_SelectedIndexChanged(object sender, EventArgs e)
    {
        chk_workersForSupervisors.Items.Clear();
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        SqlCommand comSupervisorSelect = new SqlCommand("SELECT tbl_worker.workersId, tbl_Shift.farmId, tbl_worker.FirstName, tbl_worker.LastName FROM tbl_worker INNER JOIN tbl_login ON tbl_login.Id = tbl_worker.workersId INNER JOIN tbl_Duty ON tbl_Duty.WorkerId = tbl_worker.workersId INNER JOIN tbl_Shift ON tbl_Duty.ShiftID = tbl_Shift.ShiftId WHERE tbl_login.type = 'supervisor' AND tbl_Duty.ShiftID = tbl_Shift.ShiftID AND tbl_Duty.Day = @0 AND [dbo].[tbl_Shift].[farmId]=@1", con);

      //  comSupervisorSelect.Parameters.AddWithValue("@0", Convert.ToDateTime(cbo_workerAssignDay.SelectedValue));
       comSupervisorSelect.Parameters.AddWithValue("@0", Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Month.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Day.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Year.ToString());
        comSupervisorSelect.Parameters.AddWithValue("@1", cbo_workerAssignFarm.SelectedValue.ToString());
        cbo_workerAssignSupervisor.Items.Clear();

        SqlDataReader readerSupervisorSelect = comSupervisorSelect.ExecuteReader();

        if (readerSupervisorSelect.HasRows)
        {
            cbo_workerAssignSupervisor.Items.Add("Select a Supervisor");
            //int count = 0;
            while (readerSupervisorSelect.Read())
            {
                if (cbo_workerAssignFarm.SelectedValue.ToString() == readerSupervisorSelect["farmId"].ToString())
                {
                    cbo_workerAssignSupervisor.Items.Add(new ListItem(Convert.ToString(readerSupervisorSelect["FirstName"]) + Convert.ToString(readerSupervisorSelect["LastName"]), Convert.ToString(readerSupervisorSelect["workersId"])));

                    //count++;
                }
                //else
                //{
                //    cbo_workerAssignSupervisor.Items.Clear();
                //    cbo_workerAssignSupervisor.Items.Add("There are no supervisors on this farm, for this day");
                //}
            }
            //if(count > 0)
            {
                //cbo_workerAssignSupervisor_SelectedIndexChanged(null, null); //No worries, this only runs once.
            }
        }

        else
        {
            cbo_workerAssignSupervisor.Items.Clear();
            cbo_workerAssignSupervisor.Items.Add("There are no supervisors on this farm, for this day");
        }


        comSupervisorSelect.Dispose();

        readerSupervisorSelect.Dispose();

        con.Dispose();
    }

    protected void btn_workerAssign_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        var workerSelected = true;
        
        foreach (ListItem item in chk_workersForSupervisors.Items)
        {
            if (item.Selected)
            {
                SqlCommand comSupervisorSelect = new SqlCommand("UPDATE tbl_duty SET supervisorId = @0 FROM tbl_duty INNER JOIN tbl_shift ON tbl_duty.shiftid = tbl_shift.shiftid WHERE (tbl_duty.workerId = @2 AND tbl_duty.GrowerID = @3 AND tbl_shift.farmid = @4 AND tbl_duty.Day = @1) OR (tbl_duty.workerId = @5 AND tbl_duty.GrowerID = @3 AND tbl_shift.farmid = @4 AND tbl_duty.Day = @1)", con);

                comSupervisorSelect.Parameters.Clear();
                comSupervisorSelect.Parameters.AddWithValue("@0", cbo_workerAssignSupervisor.SelectedValue.ToString());
                


                comSupervisorSelect.Parameters.AddWithValue("@1", Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Month.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Day.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Year.ToString());
                comSupervisorSelect.Parameters.AddWithValue("@2", item.Value.ToString());
                comSupervisorSelect.Parameters.AddWithValue("@3", Session["Id"]);
                comSupervisorSelect.Parameters.AddWithValue("@4", cbo_workerAssignFarm.SelectedValue.ToString());
                comSupervisorSelect.Parameters.AddWithValue("@5", cbo_workerAssignSupervisor.SelectedValue.ToString());

                comSupervisorSelect.ExecuteReader();
                comSupervisorSelect.Dispose();
                workerSelected = false;


                SqlCommand comCheckPhase = new SqlCommand("select * from tbl_phasecheck where supervisorid = @0 and workingDay = @1", con);
                comCheckPhase.Parameters.AddWithValue("@0", cbo_workerAssignSupervisor.SelectedValue.ToString());
                comCheckPhase.Parameters.AddWithValue("@1", Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Month.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Day.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Year.ToString());

                SqlDataReader readerCheckPhase = comCheckPhase.ExecuteReader();

                if(readerCheckPhase.HasRows)
                {

                }
                else
                {
                    SqlCommand comChangePhase = new SqlCommand("INSERT INTO tbl_phasecheck (supervisorid, workingday, phase) VALUES (@0, @1, 0)", con);
                    comChangePhase.Parameters.AddWithValue("@0", cbo_workerAssignSupervisor.SelectedValue.ToString());
                    comChangePhase.Parameters.AddWithValue("@1", Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Month.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Day.ToString() + "/" + Convert.ToDateTime(cbo_workerAssignDay.SelectedValue).Year.ToString());


                    comChangePhase.ExecuteNonQuery();
                }
            }
        }

        if(workerSelected)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('No workers were assigned to any supervisor. You must select a day and farm that have workers and at least one supervisor assigned to them, and then select at least one worker for assignment to the selected supervisor.');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('The worker(s) have been assigned.');", true);
        }

        con.Dispose();


        cbo_workerAssignSupervisor_SelectedIndexChanged(null, null);
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        DateTime dt4 = Convert.ToDateTime(cbo_weekStart2.SelectedValue);

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        
        var str = 1;

        if (cbo_farmWorker.SelectedIndex == 0 && txt_search.Text != "")
        {

            str = 1;
        }
        else if (cbo_farmWorker.SelectedIndex != 0 && txt_search.Text != "")
        {
            str = 2;
        }
        else if (cbo_farmWorker.SelectedIndex == 0 && txt_search.Text == "")
        {
            str = 3;
        }
        else if (cbo_farmWorker.SelectedIndex != 0 && txt_search.Text == "")
        {
            str = 4;
        }

        //com3.Parameters.Clear();
        SqlDataSource1.SelectParameters.Clear();
        switch (str)
        {
            case 1:
                //com3 = new SqlCommand("SELECT [ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0", con);
                SqlDataSource1.SelectCommand = "SELECT [dbo].[tbl_worker].[FirstName] AS 'First Name', [dbo].[tbl_worker].[LastName] AS 'Last Name',[Shiftstarttime] AS 'Start Time',[ShiftendTime] AS 'End Time',[Farm_Name] AS 'Farm Name' FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_duty.GrowerId = @grower and tbl_duty.day = @date and ([dbo].[tbl_worker].firstname like '%'+ @search +'%' or [dbo].[tbl_worker].lastname like '%'+ @search +'%') order by [dbo].[tbl_worker].firstname";
                SqlDataSource1.SelectParameters.Add("search", txt_search.Text);
                break;

            case 2:
                //com3 = new SqlCommand("SELECT [ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0 and [dbo].[tbl_Shift].[farmId] = @1", con);
                //com3.Parameters.AddWithValue("@1", cbo_farmWorker.SelectedValue.ToString());
                //SqlDataSource1.SelectCommand = com3.CommandText;
                SqlDataSource1.SelectCommand = "SELECT [dbo].[tbl_worker].[FirstName] AS 'First Name', [dbo].[tbl_worker].[LastName] AS 'Last Name',[Shiftstarttime] AS 'Start Time',[ShiftendTime] AS 'End Time',[Farm_Name] AS 'Farm Name' FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_duty.GrowerId = @grower and tbl_shift.farmid = @farmid and tbl_duty.day = @date and ([dbo].[tbl_worker].firstname like '%'+ @search +'%' or [dbo].[tbl_worker].lastname like '%'+ @search +'%') order by [dbo].[tbl_worker].firstname";
                SqlDataSource1.SelectParameters.Add("farmid", cbo_farmWorker.SelectedValue.ToString());
                SqlDataSource1.SelectParameters.Add("search", txt_search.Text);
                break;
            case 3:
                //com3 = new SqlCommand("SELECT [ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0", con);
                SqlDataSource1.SelectCommand = "SELECT [dbo].[tbl_worker].[FirstName] AS 'First Name', [dbo].[tbl_worker].[LastName] AS 'Last Name',[Shiftstarttime] AS 'Start Time',[ShiftendTime] AS 'End Time',[Farm_Name] AS 'Farm Name' FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_duty.GrowerId = @grower and tbl_duty.day = @date order by [dbo].[tbl_worker].firstname";
                break;

            case 4:
                //com3 = new SqlCommand("SELECT [ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms]  on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0 and [dbo].[tbl_Shift].[farmId] = @1", con);
                //com3.Parameters.AddWithValue("@1", cbo_farmWorker.SelectedValue.ToString());
                //SqlDataSource1.SelectCommand = com3.CommandText;
                SqlDataSource1.SelectCommand = "SELECT [dbo].[tbl_worker].[FirstName] AS 'First Name', [dbo].[tbl_worker].[LastName] AS 'Last Name',[Shiftstarttime] AS 'Start Time',[ShiftendTime] AS 'End Time',[Farm_Name] AS 'Farm Name' FROM tbl_Shift inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] inner join [dbo].[tbl_duty] on [dbo].[tbl_duty].[ShiftID] = [dbo].[tbl_Shift].[shiftid] inner join [dbo].[tbl_worker] on [dbo].[tbl_worker].workersid = [dbo].[tbl_Duty].[WorkerID] WHERE tbl_duty.GrowerId = @grower and tbl_shift.farmid = @farmid and tbl_duty.day = @date order by [dbo].[tbl_worker].firstname";
                SqlDataSource1.SelectParameters.Add("farmid", cbo_farmWorker.SelectedValue.ToString());
                break;

        }

        //com3.Parameters.AddWithValue("@0", reader2["ShiftID"]);
        SqlDataSource1.SelectParameters.Add("date", dt4.Month.ToString() + "/" + dt4.Day.ToString() + "/" + dt4.Year.ToString());
        SqlDataSource1.SelectParameters.Add("grower", Session["Id"].ToString());
    
    dgd_organiserWorkers.DataSource = SqlDataSource1;
    dgd_organiserWorkers.DataBind();

        

        if(dgd_organiserWorkers.Rows.Count == 0)
        {
            lbl_noContent.Visible = true;
        }
        else
        {
            lbl_noContent.Visible = false;
        }
        

        con.Close();
        con.Dispose();

    }

    protected void cbo_weekStart_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}