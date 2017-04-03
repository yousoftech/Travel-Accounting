using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data;

public partial class Supervisor_SupervisorAttendance : System.Web.UI.Page
{
    int phaseTime = 15;
    protected void Page_Load(object sender, EventArgs e)
    {



        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }



        ((Label)Master.FindControl("lbl_title")).Text = "Dashboard";

        dateConverter d = new dateConverter();
        DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

        SqlDataSource3.SelectCommand = " SELECT[tbl_duty].[rosterid], [dbo].[tbl_worker].[FirstName],[dbo].[tbl_worker].[LastName], tbl_tempBreak.starttime AS starttime, tbl_tempbreak.endtime AS endtime FROM[dbo].[tbl_worker] Inner join[dbo].[tbl_Duty] on[dbo].[tbl_Duty].[WorkerID] =[dbo].[tbl_worker].[WorkersId]        inner join[dbo].[tbl_Attendance] on[dbo].[tbl_Attendance].[RosterID] =[dbo].[tbl_Duty].[RosterID]       INNER JOIN[dbo].[tbl_tempBreak] ON[dbo].[tbl_tempBreak].rosterid = [dbo].[tbl_Duty].rosterid       WHERE   tbl_duty.supervisorid = @0 and      tbl_duty.day = @1 and[dbo].[tbl_Attendance].[Start_time] is not null and[dbo].[tbl_Attendance].[End_time] is null         UNION  SELECT[tbl_duty].[rosterid], [dbo].[tbl_worker].[FirstName],[dbo].[tbl_worker].[LastName], '' AS starttime, '' AS endtime FROM[dbo].[tbl_worker]        Inner join[dbo].[tbl_Duty] on[dbo].[tbl_Duty].[WorkerID] =[dbo].[tbl_worker].[WorkersId]         inner join[dbo].[tbl_Attendance] on[dbo].[tbl_Attendance].[RosterID] =[dbo].[tbl_Duty].[RosterID]         WHERE  tbl_duty.supervisorid = @0 and      tbl_duty.day = @1 and[dbo].[tbl_Attendance].[Start_time] is not null and[dbo].[tbl_Attendance].[End_time] is null and[tbl_duty].[rosterid] not in (SELECT[tbl_duty].[rosterid]  FROM[dbo].[tbl_worker]  Inner join[dbo].[tbl_Duty] on[dbo].[tbl_Duty].[WorkerID] =[dbo].[tbl_worker].[WorkersId]                           inner join[dbo].[tbl_Attendance] on[dbo].[tbl_Attendance].[RosterID] =[dbo].[tbl_Duty].[RosterID]                          INNER JOIN[dbo].[tbl_tempBreak] ON[dbo].[tbl_tempBreak].rosterid = [dbo].[tbl_Duty].rosterid WHERE    tbl_duty.supervisorid = @0 and      tbl_duty.day = @1 and[dbo].[tbl_Attendance].[Start_time] is not null and[dbo].[tbl_Attendance].[End_time] is null)		";
        SqlDataSource3.SelectParameters.Clear();
        SqlDataSource3.SelectParameters.Add("0", Session["Id"].ToString());
        SqlDataSource3.SelectParameters.Add("1", dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString());
        SqlDataSource3.DataBind();

        SqlDataSource1.SelectParameters.Clear();
        SqlDataSource1.SelectParameters.Add("wday", dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString());
        SqlDataSource1.SelectParameters.Add("name", Session["Id"].ToString());
        SqlDataSource1.DataBind();

        SqlDataSource2.SelectParameters.Clear();
        SqlDataSource2.SelectParameters.Add("wday", dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString());
        SqlDataSource2.SelectParameters.Add("name", Session["Id"].ToString());
        SqlDataSource2.DataBind();

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        //int controw = 0;
        //foreach (var row2 in Gridview40.Rows)
        //{

        //    TextBox st = (TextBox)Gridview40.Rows[controw].FindControl("txt_breakStart");
        //    Button bt = (Button)Gridview40.Rows[controw].FindControl("btnedit");
        //    DateTime d1 = new DateTime(2017, 01, 01, 0, 0, 0);

        //    if (TimeSpan.Compare(d1.TimeOfDay, Convert.ToDateTime(st.Text).TimeOfDay) == 0)
        //    {
        //        bt.Text = "Start";

        //    }
        //    else
        //    {
        //        bt.Text = "End";
        //    }



        //    controw++;

        //}


        try
        {
            con.Open();


            if (!IsPostBack)
            {
                SqlCommand comCheckPhase = new SqlCommand("SELECT phase FROM tbl_phaseCheck WHERE workingDay = @0 and supervisorId = @1", con);
                comCheckPhase.Parameters.AddWithValue("@0", dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString());
                comCheckPhase.Parameters.AddWithValue("@1", Session["Id"].ToString());

                SqlDataReader readerCheckPhase = comCheckPhase.ExecuteReader();
                readerCheckPhase.Read();
                if (readerCheckPhase.HasRows)
                {
                    if (Convert.ToBoolean(readerCheckPhase.GetValue(0)))
                    {
                        chkphase.Checked = true;
                    }
                    else
                    {
                        chkphase.Checked = false;
                    }
                }
                else
                {
                    chkphase.Checked = false;
                }
            }

            SqlCommand com = new SqlCommand("SELECT RosterID from tbl_Attendance", con);
            SqlCommand com2 = new SqlCommand("SELECT End_time FROM tbl_Attendance INNER JOIN tbl_duty ON tbl_attendance.rosterID=tbl_duty.rosterID WHERE tbl_duty.supervisorid = @0 and tbl_duty.day = @1 AND Start_time is not null order by tbl_attendance.rosterId ASC", con);
            com2.Parameters.AddWithValue("@0", Session["Id"].ToString());
            com2.Parameters.AddWithValue("@1", dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString());

            int countHiden = 0;
            int row = 0;
            foreach (var row2 in GridView3.Rows)
            {
                int count = 0;
                SqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        if (reader.GetInt64(0).ToString() == GridView3.DataKeys[row].Values["RosterID"].ToString())
                        {
                            //CheckBox cb = GridView3.Rows[row].FindControl("chkRow") as CheckBox;
                            //cb.Checked = true;
                            GridView3.Rows[row].Visible = false;
                            countHiden++;
                        }
                        count++;
                    }
                }

                reader.Close();
                row++;
            }

            ContentPlaceHolder myContent = (ContentPlaceHolder)this.Master.FindControl("cph_mainSection");
            if (countHiden == GridView3.Rows.Count)
            {
                myContent.FindControl("startTimesContent").Visible = false;
                myContent.FindControl("startTimesEmpty").Visible = true;
            }
            else
            {
                myContent.FindControl("startTimesContent").Visible = true;
                myContent.FindControl("startTimesEmpty").Visible = false;
            }

            SqlDataReader reader2 = com2.ExecuteReader();
            countHiden = 0;
            row = 0;
            foreach (var row2 in GridView4.Rows)
            {
                //int count = 0;

                if (reader2.HasRows)
                {
                    if (reader2.Read())
                    {
                        if (reader2.GetValue(0) != DBNull.Value)
                        {
                            GridView4.Rows[row].Visible = false;
                            countHiden++;
                        }
                    }

                    //while (reader.Read())
                    {

                        //if (reader.GetValue(0) == DBNull.Value)
                        {
                            //CheckBox cb = GridView3.Rows[row].FindControl("chkRow") as CheckBox;
                            //cb.Checked = true;
                            //GridView4.Rows[row].Visible = false;
                        }
                        //count++;
                    }
                }

                row++;
            }

            if (countHiden == GridView4.Rows.Count)
            {
                myContent.FindControl("endTimesContent").Visible = false;
                myContent.FindControl("endTimesEmpty").Visible = true;
            }
            else
            {
                myContent.FindControl("endTimesContent").Visible = true;
                myContent.FindControl("endTimesEmpty").Visible = false;
            }

            reader2.Close();

            if (!IsPostBack)
            {
                for (int row2 = 0; row2 < GridView3.Rows.Count; row2++)
                {

                    SqlCommand comBlock = new SqlCommand("SELECT * FROM tbl_blocks WHERE farmId = (SELECT farmId FROM tbl_shift where shiftid = @0)", con);
                    comBlock.Parameters.AddWithValue("@0", GridView3.DataKeys[row2].Values["ShiftID"].ToString());

                    SqlDataReader reader = comBlock.ExecuteReader();
                    DropDownList cbo_bloc = (DropDownList)GridView3.Rows[row2].Cells[5].Controls[1];
                    cbo_bloc.Items.Clear();
                    //foreach(var rowBlock in )
                    while (reader.Read())
                    {

                        cbo_bloc.Items.Add(new ListItem(reader.GetValue(1).ToString(), reader.GetValue(0).ToString()));
                    }
                    reader.Close();
                    SqlCommand comMcat = new SqlCommand("SELECT * FROM tbl_job_mcat", con);

                    SqlDataReader Mreader = comMcat.ExecuteReader();
                    DropDownList cbo_mcats = (DropDownList)GridView3.Rows[row2].Cells[6].Controls[1];
                    cbo_mcats.Items.Clear();
                    while (Mreader.Read())
                    {
                        cbo_mcats.Items.Add(new ListItem(Mreader.GetValue(1).ToString(), Mreader.GetValue(0).ToString()));
                    }
                    Mreader.Close();
                    SqlCommand comScat = new SqlCommand("SELECT * FROM tbl_job_cat where McatID=@0", con);
                    DropDownList cbo_mmmcats = (DropDownList)GridView3.Rows[row2].Cells[6].Controls[1];
                    DropDownList cbo_mmcats = (DropDownList)GridView3.Rows[row2].Cells[7].Controls[1];
                    comScat.Parameters.AddWithValue("@0", cbo_mmmcats.SelectedValue.ToString());
                    DropDownList cbo_scats = (DropDownList)GridView3.Rows[row2].Cells[7].Controls[1];
                    cbo_scats.Items.Clear();
                    SqlDataReader Sreader = comScat.ExecuteReader();
                    while (Sreader.Read())
                    {


                        cbo_mmcats.Items.Add(new ListItem(Sreader.GetValue(1).ToString(), Sreader.GetValue(0).ToString()));
                    }



                    //reader.Close();
                }
            }


        }
        catch (Exception err2)
        {
            Session["error"] = err2.ToString();
            Response.Redirect("~/Debug.aspx");
        }


        if (String.Equals(Session["shift"], "END"))
        {
            Mv1.ActiveViewIndex = 1;
        }
        else if (String.Equals(Session["shift"], "BREAK"))
        {
            Mv1.ActiveViewIndex = 2;
        }
        else
        {
            Mv1.ActiveViewIndex = 0;
        }

        //GridView3.Columns[0].Visible = false;
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void test(object sender, EventArgs e)
    {
        UploadImage(@"e:/ ");

    }
    protected void btn_startSubmit_Click(object sender, EventArgs e)
    {
        dateConverter d = new dateConverter();
        DateTime dst = d.convertUTCtoNZT(System.DateTime.UtcNow);

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();








        if (chkphase.Checked)
        {
            SqlCommand comCheckRows = new SqlCommand("SELECT count(*) from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
            comCheckRows.Parameters.AddWithValue("@0", Session["Id"].ToString());
            comCheckRows.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

            if ((int)comCheckRows.ExecuteScalar() < 1)
            {
                Session["pnum"] = "0";
                int count = 0;
                foreach (GridViewRow row in GridView3.Rows)
                {
                    TextBox dhu = row.FindControl("txt_time") as TextBox;
                    DropDownList dl = (DropDownList)row.FindControl("cbo_block");
                    DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];
                    SqlCommand coma = null;


                    CheckBox chkC = row.FindControl("chkRow") as CheckBox;
                    if (chkC.Checked)
                    {
                        SqlCommand com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat,phase,[phaseBool]) VALUES (@0, @1, @2, @3,@4,@5,'true') ", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;
                        coma = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM tbl_attendance WHERE RosterID = @0) BEGIN INSERT INTO tbl_attendance (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) END END", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;

                        DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

                        coma.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                        coma.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                        coma.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                        coma.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                        coma.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());




                        coma.ExecuteReader();

                        com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                        com.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                        com.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                        com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                        com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());
                        com.Parameters.AddWithValue("@5", Session["pnum"].ToString());



                        com.ExecuteReader();
                    }
                    count++;
                }
            }
            else
            {
                double binrate = Convert.ToDouble(txtrate.Text);
                double bin = Convert.ToDouble(txtbin.Text);
                SqlCommand mtime = new SqlCommand("Select min([Start_time]) from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                mtime.Parameters.AddWithValue("@0", Session["Id"].ToString());
                mtime.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                SqlDataReader rtime = mtime.ExecuteReader();
                rtime.Read();

                DateTime dbtime = Convert.ToDateTime(rtime[0].ToString());

                rtime.Close();
                mtime.Dispose();

                int checkedIndex = -1;
                //SupervisorHome sm = new global::SupervisorHome();
                //GridView gw = sm.FindControl("Gridview3") as GridView;

                int rowCount = GridView3.Rows.Count;


                DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

                bool[] rowBool = new bool[rowCount];

                try
                {


                    //The section below could do with some imporovement to help relay the correct information to the user, and become more intuitive

                    SqlCommand com, coma = null;
                    int count = 0;

                    foreach (GridViewRow row in GridView3.Rows)
                    {
                        TextBox dhu = row.FindControl("txt_time") as TextBox;
                        DropDownList dl = (DropDownList)row.FindControl("cbo_block");


                        CheckBox chkC = row.FindControl("chkRow") as CheckBox;


                        if (chkC.Checked) //If the checkbox has been checked
                        {
                            TimeSpan ts = Convert.ToDateTime(row.Cells[3].Text).TimeOfDay.Subtract(dbtime.TimeOfDay);

                            DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];
                            coma = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM tbl_attendance WHERE RosterID = @0) BEGIN INSERT INTO tbl_attendance (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) END END", con);
                            //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                            //string str = hd.Value;


                            coma.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                            coma.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                            coma.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                            coma.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                            coma.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());




                            coma.ExecuteReader();


                            if (ts.TotalMinutes > phaseTime)
                            {

                                SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                                getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                                SqlDataReader insertr = getp.ExecuteReader();

                                DataTable tblcsv = new DataTable();
                                tblcsv.Columns.Add("RosterID");
                                tblcsv.Columns.Add("Start_time");
                                tblcsv.Columns.Add("Phase");
                                tblcsv.Columns.Add("phaseBool");
                                while (insertr.Read())
                                {
                                    DateTime ds = Convert.ToDateTime((row.Cells[3].Text));
                                    int i = Convert.ToInt16(insertr[1].ToString());
                                    i = i + 1;
                                    tblcsv.Rows.Add(insertr[0].ToString(), ds.TimeOfDay, i, true);
                                    Session["pnum"] = i.ToString();

                                }
                                insertr.Close();
                                getp.Dispose();



                                SqlBulkCopy bluk = new SqlBulkCopy(con);
                                bluk.DestinationTableName = "[dbo].[temp_attendance]";
                                bluk.ColumnMappings.Add("RosterID", "RosterID");
                                bluk.ColumnMappings.Add("Start_time", "Start_time");
                                bluk.ColumnMappings.Add("Phase", "Phase");
                                bluk.ColumnMappings.Add("phaseBool", "phaseBool");
                                bluk.WriteToServer(tblcsv);



                                SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2, binPay=@4, binCounts=@5 ,phasebool=@6 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[Start_time] <> @3 and [dbo].[temp_attendance].[End_time] is null  ", con);
                                smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                                smax.Parameters.AddWithValue("@2", (row.Cells[3].Text));
                                smax.Parameters.AddWithValue("@3", (row.Cells[3].Text));
                                smax.Parameters.AddWithValue("@4", binrate);
                                smax.Parameters.AddWithValue("@5", bin);
                                smax.Parameters.AddWithValue("@6", true);
                                smax.ExecuteNonQuery();


                                com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat,phase,[phaseBool]) VALUES (@0, @1, @2, @3,@4,@5,@6) ", con);
                                //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                                //string str = hd.Value;


                                com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                                com.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                                com.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                                com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                                com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());
                                com.Parameters.AddWithValue("@5", Session["pnum"].ToString());
                                com.Parameters.AddWithValue("@6", true);


                                com.ExecuteReader();
                                Session["pnum"] = "NULL";

                            }
                            else
                            {

                                com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat,phaseBool) VALUES (@0, @1, @2, @3,@4,@5) ", con);
                                //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                                //string str = hd.Value;


                                com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                                com.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                                com.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                                com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                                com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());
                                com.Parameters.AddWithValue("@5", true);



                                com.ExecuteReader();
                            }







                            checkedIndex = count;
                        }
                        //else if (dhu.Text != "") // if no time value is entered
                        //{
                        //    com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) ", con);
                        //    //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //    //string str = hd.Value;
                        //    com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());



                        //    TextBox time = row.FindControl("txt_time") as TextBox;

                        //    com.Parameters.AddWithValue("@1", time.Text);
                        //    //add 'cbo_block' into insert
                        //    DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];

                        //    com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());

                        //    com.Parameters.AddWithValue("@2", dl.SelectedIndex.ToString());
                        //    com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");

                        //    com.ExecuteReader();

                        //}
                        else if (dhu.Text != "") //If the checkbox has been checked
                        {


                            TextBox time = row.FindControl("txt_time") as TextBox;
                            TimeSpan ts = Convert.ToDateTime(time.Text).TimeOfDay.Subtract(dbtime.TimeOfDay);


                            com = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM tbl_attendance WHERE RosterID = @0) BEGIN INSERT INTO tbl_attendance (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) END END", con);
                            //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                            //string str = hd.Value;
                            com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());



                            com.Parameters.AddWithValue("@1", time.Text);
                            //add 'cbo_block' into insert
                            DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];

                            com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());

                            com.Parameters.AddWithValue("@2", dl.SelectedIndex.ToString());
                            com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");

                            com.ExecuteReader();
                            com.Dispose();





                            // TimeSpan ts = dbtime.TimeOfDay.Subtract(Convert.ToDateTime(time.Text).TimeOfDay);
                            if (ts.TotalMinutes > phaseTime)
                            {

                                SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                                getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                                SqlDataReader insertr = getp.ExecuteReader();

                                DataTable tblcsv = new DataTable();
                                tblcsv.Columns.Add("RosterID");
                                tblcsv.Columns.Add("Start_time");
                                tblcsv.Columns.Add("Phase");

                                while (insertr.Read())
                                {
                                    DateTime ds = Convert.ToDateTime((time.Text));
                                    int i = Convert.ToInt16(insertr[1].ToString());
                                    i = i + 1;
                                    tblcsv.Rows.Add(insertr[0].ToString(), ds.TimeOfDay, i);
                                    Session["pnum"] = i.ToString();

                                }
                                insertr.Close();
                                getp.Dispose();



                                SqlBulkCopy bluk = new SqlBulkCopy(con);
                                bluk.DestinationTableName = "[dbo].[temp_attendance]";
                                bluk.ColumnMappings.Add("RosterID", "RosterID");
                                bluk.ColumnMappings.Add("Start_time", "Start_time");
                                bluk.ColumnMappings.Add("Phase", "Phase");
                                bluk.WriteToServer(tblcsv);



                                SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2, binPay=@4, binCounts=@5 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[Start_time] <> @3 and [dbo].[temp_attendance].[End_time] is null ", con);
                                smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                                smax.Parameters.AddWithValue("@2", (time.Text));
                                smax.Parameters.AddWithValue("@3", (time.Text));
                                smax.Parameters.AddWithValue("@4", binrate);
                                smax.Parameters.AddWithValue("@5", bin);
                                smax.ExecuteNonQuery();


                                com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat,phase) VALUES (@0, @1, @2, @3,@4,@5) ", con);
                                //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                                //string str = hd.Value;


                                com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                                com.Parameters.AddWithValue("@1", (time.Text));
                                com.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                                com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                                com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());
                                com.Parameters.AddWithValue("@5", Session["pnum"].ToString());



                                com.ExecuteReader();
                                Session["pnum"] = "NULL";

                            }
                            else
                            {
                                SqlCommand getp = new SqlCommand("Select max(phase) from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                                getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

                                int i = (int)getp.ExecuteScalar();

                                Session["pnum"] = i.ToString();

                                com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat, phase) VALUES (@0, @1, @2, @3,@4,@5) ", con);
                                //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                                //string str = hd.Value;


                                com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                                com.Parameters.AddWithValue("@1", (time.Text));
                                com.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                                com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");

                                com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());
                                com.Parameters.AddWithValue("@5", Session["pnum"].ToString());




                                com.ExecuteReader();
                                Session["pnum"] = "NULL";
                            }



                        }

                        count++;
                        // com.Dispose();
                    }
                }
                catch (Exception err1)
                {
                    Session["error"] = err1.ToString();
                    Response.Redirect("~/Debug.aspx");
                }
                Session["shift"] = "START";


            }

            SqlCommand sqlphase = new SqlCommand("update temp_attendance set phasebool=@0", con);
            sqlphase.Parameters.AddWithValue("@0", true);
            sqlphase.ExecuteNonQuery();
        }

        else //Box not checked
        {


            int checkedIndex = -1;
            //SupervisorHome sm = new global::SupervisorHome();
            //GridView gw = sm.FindControl("Gridview3") as GridView;

            int rowCount = GridView3.Rows.Count;

            //dateConverter d = new dateConverter();
            DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

            bool[] rowBool = new bool[rowCount];

            try
            {


                //The section below could do with some imporovement to help relay the correct information to the user, and become more intuitive

                SqlCommand com = null;
                SqlCommand comTemp = null;
                int count = 0;

                foreach (GridViewRow row in GridView3.Rows)
                {
                    TextBox dhu = row.FindControl("txt_time") as TextBox;
                    DropDownList dl = (DropDownList)row.FindControl("cbo_block");


                    CheckBox chkC = row.FindControl("chkRow") as CheckBox;


                    if (chkC.Checked) //If the checkbox has been checked
                    {
                        DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];
                        //// <!--==  Incomplete work for today is below and in need of finishing - 13/03/2017  ==--> ////
                        comTemp = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM temp_attendance WHERE RosterID = @0) BEGIN INSERT INTO temp_attendance (RosterID, Start_time, blockid, startsignature,  job_cat,  phaseBool) VALUES(@0, @1, @2, @3, @4, @5) END END", con);
                        comTemp.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());
                        comTemp.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                        comTemp.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());
                        comTemp.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");
                        comTemp.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());

                        comTemp.Parameters.AddWithValue("@5", false);
                        //comTemp.Parameters.AddWithValue("@4", );
                        //comTemp.Parameters.AddWithValue("@5", );
                        //comTemp.Parameters.AddWithValue("@6", );
                        //comTemp.Parameters.AddWithValue("@7", );
                        comTemp.ExecuteReader();

                        com = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM tbl_attendance WHERE RosterID = @0) BEGIN INSERT INTO tbl_attendance (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) END END", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;


                        com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                        com.Parameters.AddWithValue("@1", (row.Cells[3].Text));
                        com.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());

                        com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");


                        com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());




                        com.ExecuteReader();
                        checkedIndex = count;
                    }
                    else if (dhu.Text != "") // if no time value is entered
                    {
                        com = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM tbl_attendance WHERE RosterID = @0) BEGIN INSERT INTO tbl_attendance (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) END END", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;
                        com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());



                        TextBox time = row.FindControl("txt_time") as TextBox;

                        com.Parameters.AddWithValue("@1", time.Text);
                        //add 'cbo_block' into insert
                        DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];

                        com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());

                        com.Parameters.AddWithValue("@2", dl.SelectedIndex.ToString());
                        com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");

                        com.ExecuteReader();
                        com.Dispose();

                        comTemp = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM temp_attendance WHERE RosterID = @0) BEGIN INSERT INTO temp_attendance (RosterID, Start_time, blockid, startsignature,  job_cat,  phaseBool) VALUES(@0, @1, @2, @3, @4, @5) END END", con);
                        comTemp.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());
                        comTemp.Parameters.AddWithValue("@1", (time.Text));
                        comTemp.Parameters.AddWithValue("@2", dl.SelectedValue.ToString());
                        comTemp.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");
                        comTemp.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());

                        comTemp.Parameters.AddWithValue("@5", false);
                        //comTemp.Parameters.AddWithValue("@4", );
                        //comTemp.Parameters.AddWithValue("@5", );
                        //comTemp.Parameters.AddWithValue("@6", );
                        //comTemp.Parameters.AddWithValue("@7", );
                        comTemp.ExecuteReader();
                        comTemp.Dispose();

                    }

                    count++;
                }


                // com.Dispose();
            }
            catch (Exception err1)
            {
                Session["error"] = err1.ToString();
                Response.Redirect("~/Debug.aspx");
            }
            Session["shift"] = "START";

            //SqlCommand com = new SqlCommand("SELECT", con);

        }
        Response.Redirect(Request.RawUrl);
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
        Session["shift"] = "BREAK";


    }

    protected void MultiView1_ActiveViewChanged(object sender, EventArgs e)
    {
        if (Mv1.ActiveViewIndex == 0) //Start
        {
            Session["shift"] = null;
        }
        else if (Mv1.ActiveViewIndex == 1) //End
        {
            Session["shift"] = "END";
        }
        else //break
        {
            Session["shift"] = "BREAK";
        }
    }


    protected void Button3_Click(object sender, EventArgs e)
    {


        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();

        if (chkphase.Checked) //Use phase system
        {
            double binrate = Convert.ToDouble(txtrate.Text);
            double bin = Convert.ToDouble(txtbin.Text);


            dateConverter d = new dateConverter();
            DateTime dst = d.convertUTCtoNZT(System.DateTime.UtcNow);
            SqlCommand mtime = new SqlCommand("Select min([Start_time]) from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
            mtime.Parameters.AddWithValue("@0", Session["Id"].ToString());
            mtime.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
            SqlDataReader rtime = mtime.ExecuteReader();
            rtime.Read();

            DateTime dbtime = Convert.ToDateTime(rtime[0].ToString());

            rtime.Close();
            mtime.Dispose();

            int checkedIndex = -1;
            //SupervisorHome sm = new global::SupervisorHome();
            //GridView gw = sm.FindControl("Gridview3") as GridView;

            int rowCount = GridView4.Rows.Count;


            DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

            bool[] rowBool = new bool[rowCount];

            try
            {


                //The section below could do with some improvement to help relay the correct information to the user, and become more intuitive

                SqlCommand com = null;
                SqlCommand com2 = null;
                int count = 0;

                foreach (GridViewRow row in GridView4.Rows)
                {
                    TextBox dhu = row.FindControl("txt_time") as TextBox;
                    DropDownList dl = (DropDownList)row.FindControl("cbo_block");


                    CheckBox chkC = row.FindControl("chkRow") as CheckBox;


                    if (chkC.Checked) //If the checkbox has been checked
                    {
                        TextBox time = row.FindControl("txt_time") as TextBox;

                        TimeSpan ts = Convert.ToDateTime(row.Cells[3].Text).TimeOfDay.Subtract(dbtime.TimeOfDay);
                        com = new SqlCommand("update [dbo].[temp_attendance] set [End_time] = @1, bincounts=@2, binpay=@3 where rosterid = @0  and end_time is null", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;


                        com.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        com.Parameters.AddWithValue("@1", row.Cells[3].Text);
                        com.Parameters.AddWithValue("@2", bin);
                        com.Parameters.AddWithValue("@3", binrate);




                        com.ExecuteReader();
                        //if (ts.TotalMinutes > phaseTime)
                        //{

                        SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                        getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                        getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                        SqlDataReader insertr = getp.ExecuteReader();

                        DataTable tblcsv = new DataTable();
                        tblcsv.Columns.Add("RosterID");
                        tblcsv.Columns.Add("Start_time");
                        tblcsv.Columns.Add("Phase");
                        tblcsv.Columns.Add("phaseBool");

                        int flag = 0;


                        while (insertr.Read())
                        {
                            DateTime ds = Convert.ToDateTime(row.Cells[3].Text);
                            int i = Convert.ToInt16(insertr[1].ToString());
                            i = i + 1;
                            tblcsv.Rows.Add(insertr[0].ToString(), ds.TimeOfDay, i, true);
                            Session["pnum"] = i.ToString();

                            flag = 1;
                        }
                        insertr.Close();
                        getp.Dispose();

                        if (flag == 0)
                        {
                            Session["pnum"] = "1000";
                        }

                        SqlBulkCopy bluk = new SqlBulkCopy(con);
                        bluk.DestinationTableName = "[dbo].[temp_attendance]";
                        bluk.ColumnMappings.Add("RosterID", "RosterID");
                        bluk.ColumnMappings.Add("Start_time", "Start_time");
                        bluk.ColumnMappings.Add("Phase", "Phase");
                        bluk.ColumnMappings.Add("phaseBool", "phaseBool");
                        bluk.WriteToServer(tblcsv);






                        SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2, binPay=@4, binCounts=@5 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[End_time] is null and phase <> @6", con);
                        smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
                        smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                        smax.Parameters.AddWithValue("@2", row.Cells[3].Text);
                        smax.Parameters.AddWithValue("@4", binrate);
                        smax.Parameters.AddWithValue("@5", bin);
                        smax.Parameters.AddWithValue("@6", Session["pnum"].ToString());
                        smax.ExecuteNonQuery();

                        Session["pnum"] = "NULL";

                        com2 = new SqlCommand("update tbl_attendance set End_time = @1 where rosterid = @0 and end_time is null", con);

                        com2.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        com2.Parameters.AddWithValue("@1", row.Cells[3].Text);

                        com2.ExecuteNonQuery();
                        SqlCommand comGetPay = new SqlCommand("DECLARE @count int DECLARE @dataTbl TABLE(phase int,workers int,bins decimal(4, 2),pay decimal(18, 0)) SET @count = (SELECT MIN(phase) FROM temp_attendance WHERE RosterID = @RosterID) WHILE @count <= (SELECT MAX(phase) FROM temp_attendance WHERE RosterID = @RosterID) BEGIN if (@count IN(SELECT phase FROM temp_attendance WHERE rosterid = @RosterID)) BEGIN INSERT INTO @dataTbl   SELECT DISTINCT (SELECT @count as 'phase'), (SELECT count(DISTINCT RosterID) FROM temp_attendance WHERE phase = @count) AS 'workers', (SELECT bincounts from temp_attendance WHERE phase = @count and RosterID = @RosterID) as 'bins', 		(SELECT binpay from temp_attendance WHERE phase = @count and RosterID = @RosterID) as 'pay'     FROM temp_attendance WHERE 'bins' is not null     GROUP BY RosterID END SET @count = @count + 1;                        END SELECT* FROM @dataTbl", con);
                        comGetPay.Parameters.AddWithValue("@RosterID", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        SqlDataReader readerGetPay = comGetPay.ExecuteReader();

                        while (readerGetPay.Read())
                        {
                            int phase = (int)readerGetPay.GetInt32(0);



                            decimal pay = (Convert.ToDecimal(readerGetPay.GetValue(2).ToString()) * Convert.ToDecimal(readerGetPay.GetValue(3).ToString())) / (int)readerGetPay.GetInt32(1);
                            SqlCommand comUpdate = new SqlCommand("UPDATE temp_attendance SET pay = @0 WHERE RosterID = @1 AND phase = @2", con);
                            comUpdate.Parameters.AddWithValue("@0", pay);
                            comUpdate.Parameters.AddWithValue("@1", GridView4.DataKeys[count].Values["RosterID"].ToString());
                            comUpdate.Parameters.AddWithValue("@2", phase);

                            comUpdate.ExecuteNonQuery();
                        }

                        Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                        //}
                        //else
                        //{





                        //    com = new SqlCommand("update [dbo].[temp_attendance] set [End_time] = @1 where rosterid = @0  and end_time is null", con);
                        //    //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //    //string str = hd.Value;


                        //    com.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        //    com.Parameters.AddWithValue("@1", (row.Cells[3].Text));




                        //    com.ExecuteReader();

                        //    com2 = new SqlCommand("update tbl_attendance set End_time = @1 where rosterid = @0 and end_time is null", con);

                        //    com2.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        //    com2.Parameters.AddWithValue("@1", (row.Cells[3].Text));

                        //    com2.ExecuteNonQuery();
                        //    Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                        //}







                        checkedIndex = count;
                    }
                    //else if (dhu.Text != "") // if no time value is entered
                    //{
                    //    com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1, @2, @3,@4) ", con);
                    //    //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                    //    //string str = hd.Value;
                    //    com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());



                    //    TextBox time = row.FindControl("txt_time") as TextBox;

                    //    com.Parameters.AddWithValue("@1", time.Text);
                    //    //add 'cbo_block' into insert
                    //    DropDownList cbo_mmcats = (DropDownList)row.Cells[7].Controls[1];

                    //    com.Parameters.AddWithValue("@4", cbo_mmcats.SelectedItem.Value.ToString());

                    //    com.Parameters.AddWithValue("@2", dl.SelectedIndex.ToString());
                    //    com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView3.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");

                    //    com.ExecuteReader();

                    //}
                    else if (dhu.Text != "") //If the checkbox has not been checked and some time exists in the textbox
                    {
                        //TimeSpan ts = dbtime.TimeOfDay.Subtract(Convert.ToDateTime(row.Cells[3].Text).TimeOfDay); //problem. It should find the textbox control instead


                        TextBox time = row.FindControl("txt_time") as TextBox;

                        TimeSpan ts = Convert.ToDateTime(time.Text).TimeOfDay.Subtract(dbtime.TimeOfDay);
                        com = new SqlCommand("update [dbo].[temp_attendance] set [End_time] = @1, bincounts=@2, binpay=@3 where rosterid = @0  and end_time is null", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;


                        com.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        com.Parameters.AddWithValue("@1", time.Text);
                        com.Parameters.AddWithValue("@2", bin);
                        com.Parameters.AddWithValue("@3", binrate);




                        com.ExecuteReader();
                        //if (ts.TotalMinutes > phaseTime)
                        //{

                        SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                        getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                        getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                        SqlDataReader insertr = getp.ExecuteReader();

                        DataTable tblcsv = new DataTable();
                        tblcsv.Columns.Add("RosterID");
                        tblcsv.Columns.Add("Start_time");
                        tblcsv.Columns.Add("Phase");
                        tblcsv.Columns.Add("phaseBool");

                        int flag = 0;


                        while (insertr.Read())
                        {
                            DateTime ds = Convert.ToDateTime(time.Text);
                            int i = Convert.ToInt16(insertr[1].ToString());
                            i = i + 1;
                            tblcsv.Rows.Add(insertr[0].ToString(), ds.TimeOfDay, i, true);
                            Session["pnum"] = i.ToString();

                            flag = 1;
                        }
                        insertr.Close();
                        getp.Dispose();

                        if (flag == 0)
                        {
                            Session["pnum"] = "1000";
                        }

                        SqlBulkCopy bluk = new SqlBulkCopy(con);
                        bluk.DestinationTableName = "[dbo].[temp_attendance]";
                        bluk.ColumnMappings.Add("RosterID", "RosterID");
                        bluk.ColumnMappings.Add("Start_time", "Start_time");
                        bluk.ColumnMappings.Add("Phase", "Phase");
                        bluk.ColumnMappings.Add("phaseBool", "phaseBool");
                        bluk.WriteToServer(tblcsv);






                        SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2, binPay=@4, binCounts=@5 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[End_time] is null and phase <> @6", con);
                        smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
                        smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                        smax.Parameters.AddWithValue("@2", (time.Text));
                        smax.Parameters.AddWithValue("@4", binrate);
                        smax.Parameters.AddWithValue("@5", bin);
                        smax.Parameters.AddWithValue("@6", Session["pnum"].ToString());
                        smax.ExecuteNonQuery();

                        Session["pnum"] = "NULL";

                        com2 = new SqlCommand("update tbl_attendance set End_time = @1 where rosterid = @0 and end_time is null", con);

                        com2.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        com2.Parameters.AddWithValue("@1", (time.Text));

                        com2.ExecuteNonQuery();
                        SqlCommand comGetPay = new SqlCommand("DECLARE @count int DECLARE @dataTbl TABLE(phase int,workers int,bins decimal(4, 2),pay decimal(18, 0)) SET @count = (SELECT MIN(phase) FROM temp_attendance WHERE RosterID = @RosterID) WHILE @count <= (SELECT MAX(phase) FROM temp_attendance WHERE RosterID = @RosterID) BEGIN if (@count IN(SELECT phase FROM temp_attendance WHERE rosterid = @RosterID)) BEGIN INSERT INTO @dataTbl   SELECT DISTINCT (SELECT @count as 'phase'), (SELECT count(DISTINCT RosterID) FROM temp_attendance WHERE phase = @count) AS 'workers', (SELECT bincounts from temp_attendance WHERE phase = @count and RosterID = @RosterID) as 'bins', 		(SELECT binpay from temp_attendance WHERE phase = @count and RosterID = @RosterID) as 'pay'     FROM temp_attendance WHERE 'bins' is not null     GROUP BY RosterID END SET @count = @count + 1;                        END SELECT* FROM @dataTbl", con);
                        comGetPay.Parameters.AddWithValue("@RosterID", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        SqlDataReader readerGetPay = comGetPay.ExecuteReader();

                        while (readerGetPay.Read())
                        {
                            int phase = (int)readerGetPay.GetInt32(0);



                            decimal pay = (Convert.ToDecimal(readerGetPay.GetValue(2).ToString()) * Convert.ToDecimal(readerGetPay.GetValue(3).ToString())) / (int)readerGetPay.GetInt32(1);
                            SqlCommand comUpdate = new SqlCommand("UPDATE temp_attendance SET pay = @0 WHERE RosterID = @1 AND phase = @2", con);
                            comUpdate.Parameters.AddWithValue("@0", pay);
                            comUpdate.Parameters.AddWithValue("@1", GridView4.DataKeys[count].Values["RosterID"].ToString());
                            comUpdate.Parameters.AddWithValue("@2", phase);

                            comUpdate.ExecuteNonQuery();
                        }

                        Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                        //}
                        //else
                        //{






                        //    com = new SqlCommand("update [dbo].[temp_attendance] set [End_time] = @1 where rosterid = @0  and end_time is null", con);
                        //    //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //    //string str = hd.Value;


                        //    com.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        //    com.Parameters.AddWithValue("@1", (time.Text));




                        //    com.ExecuteReader();

                        //    com2 = new SqlCommand("update tbl_attendance set End_time = @1 where rosterid = @0 and end_time is null", con);

                        //    com2.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        //    com2.Parameters.AddWithValue("@1", (time.Text));

                        //    com2.ExecuteNonQuery();
                        //    Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                        //}



                    }

                    count++;
                    // com.Dispose();
                }
            }
            catch (Exception err1)
            {
                Session["error"] = err1.ToString();
                Response.Redirect("~/Debug.aspx");
            }
            Session["shift"] = "START";



        }



        else
        {

            int rowCount = GridView4.Rows.Count;
            bool[] rowBool = new bool[rowCount];

            try
            {

                SqlCommand comTemp = null;

                SqlCommand com = null;
                int count = 0;


                dateConverter d = new dateConverter();
                DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

                foreach (GridViewRow row in GridView4.Rows)
                {
                    TextBox dhu = row.FindControl("txt_time") as TextBox;
                    //CheckBox chkC = row.FindControl("chkRow") as CheckBox;

                    CheckBox chkC = row.FindControl("chkRow") as CheckBox;


                    if (chkC.Checked) //if the checkbox is checked
                    {
                        SqlCommand com2 = new SqlCommand("update tbl_attendance set End_time = @1, [Total_hours]= (select sum(DATEDIFF(minute,[Start_time],@1) ) FROM [dbo].[temp_attendance] where [RosterID]=@0 and end_time is not null) where rosterid = @0 and end_time is null", con);

                        com2.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        com2.Parameters.AddWithValue("@1", row.Cells[3].Text.ToString());
                        //com2.ExecuteNonQuery();



                        SqlCommand comPay = new SqlCommand("SELECT pay FROM tbl_pay WHERE payID = (SELECT payrate FROM tbl_worker WHERE WorkersId = (SELECT WorkerID FROM tbl_Duty WHERE RosterID = @0))", con);
                        comPay.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());


                        decimal pay1 = (decimal)comPay.ExecuteScalar();

                        SqlCommand comPayGet = new SqlCommand("select ((sum(DATEDIFF(minute,start_time,@1))/60)*@2) from temp_attendance WHERE phaseBool = 0 AND RosterID = @0", con);
                        comPayGet.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());
                        comPayGet.Parameters.AddWithValue("@1", row.Cells[3].Text.ToString());
                        comPayGet.Parameters.AddWithValue("@2", pay1);

                        decimal pay = (decimal)comPayGet.ExecuteScalar();

                        comTemp = new SqlCommand("UPDATE temp_attendance SET pay=@1,end_time=@2 WHERE phaseBool = 0 AND RosterID = @0", con);
                        comTemp.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());
                        comTemp.Parameters.AddWithValue("@1", pay);
                        comTemp.Parameters.AddWithValue("@2", row.Cells[3].Text.ToString());

                        comTemp.ExecuteNonQuery();


                        com = new SqlCommand("UPDATE tbl_attendance SET End_time = @0, note = @2, endsignature = @3 ,breaktime=@4,[paid_break]=@5,total_hours=@6,pay=@7 WHERE RosterID = @1", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                        //string str = hd.Value;
                        com.Parameters.AddWithValue("@1", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));

                        TextBox timeBreak = row.FindControl("txt_break") as TextBox;

                        com.Parameters.AddWithValue("@0", row.Cells[3].Text.ToString());
                        if (timeBreak.Text != "")
                        {
                            com.Parameters.AddWithValue("@2", timeBreak.Text);
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@2", 0);
                        }

                        com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView4.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");




                        SqlCommand comb = new SqlCommand("select sum(DATEDIFF(minute,[startTime],[endTime]))  FROM[dbo].[tbl_submitBreak] where [RosterID] = @0 and paid_break=0", con);
                        comb.Parameters.AddWithValue("@0", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));

                        SqlDataReader readerComb = comb.ExecuteReader();
                        readerComb.Read();
                        int i;
                        if (readerComb.HasRows && readerComb.GetValue(0) != DBNull.Value)
                        {
                            try
                            {
                                i = Convert.ToInt32(readerComb.GetValue(0).ToString());
                            }
                            catch (Exception error)
                            {
                                i = 0;
                            }
                        }
                        else
                        {
                            i = 0;
                        }
                        comb.Dispose();
                        SqlCommand compb = new SqlCommand("select sum(DATEDIFF(minute,[startTime],[endTime]))  FROM[dbo].[tbl_submitBreak] where [RosterID] = @0 and paid_break=1", con);
                        compb.Parameters.AddWithValue("@0", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));

                        SqlDataReader readerCompb = compb.ExecuteReader();
                        readerCompb.Read();
                        int ip;
                        if (readerCompb.HasRows && readerCompb.GetValue(0) != DBNull.Value)
                        {
                            try
                            {
                                ip = Convert.ToInt32(readerCompb.GetValue(0).ToString());
                            }
                            catch (Exception error)
                            {
                                ip = 0;
                            }
                        }
                        else
                        {
                            ip = 0;
                        }
                        compb.Dispose();
                        com.Parameters.AddWithValue("@4", i);
                        com.Parameters.AddWithValue("@5", ip);

                        SqlCommand comt = new SqlCommand("select sum(DATEDIFF(minute,[Start_time],@1) ) FROM [dbo].[tbl_Attendance] where [RosterID]=@0", con);
                        comt.Parameters.AddWithValue("@1", row.Cells[3].Text.ToString());
                        comt.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"]);
                        int j = (int)comt.ExecuteScalar() - i;
                        com.Parameters.AddWithValue("@6", j);
                        com.Parameters.AddWithValue("@7", pay1 * (j / 60));

                        com.ExecuteReader();





                        //  SqlDataSource2.DataBind();
                        Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                    }

                    else if (dhu.Text != "") //If no time is entered
                    {
                        if (chkC.Checked)
                        {
                            chkC.Checked = false;
                        }

                        TextBox time = row.FindControl("txt_time") as TextBox;

                        SqlCommand com2 = new SqlCommand("update tbl_attendance set End_time = @1, [Total_hours]= (select sum(DATEDIFF(minute,[Start_time],@1) ) FROM [dbo].[temp_attendance] where [RosterID]=@0 and end_time is not null) where rosterid = @0 and end_time is null", con);

                        com2.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        com2.Parameters.AddWithValue("@1", time.Text);
                        //com2.ExecuteNonQuery();


                        SqlCommand comPay = new SqlCommand("SELECT pay FROM tbl_pay WHERE payID = (SELECT payrate FROM tbl_worker WHERE WorkersId = (SELECT WorkerID FROM tbl_Duty WHERE RosterID = @0))", con);
                        comPay.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());


                        decimal pay1 = (decimal)comPay.ExecuteScalar();

                        SqlCommand comPayGet = new SqlCommand("select ((sum(DATEDIFF(minute,start_time,@1))/60)*@2) from temp_attendance WHERE phaseBool = 0 AND RosterID = @0", con);
                        comPayGet.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());
                        comPayGet.Parameters.AddWithValue("@1", time.Text);
                        comPayGet.Parameters.AddWithValue("@2", pay1);

                        decimal pay = (decimal)comPayGet.ExecuteScalar();

                        comTemp = new SqlCommand("UPDATE temp_attendance SET pay=@1,end_time=@2 WHERE phaseBool = 0 AND RosterID = @0", con);
                        comTemp.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"].ToString());
                        comTemp.Parameters.AddWithValue("@1", pay);
                        comTemp.Parameters.AddWithValue("@2", time.Text);

                        comTemp.ExecuteNonQuery();

                        //  com = new SqlCommand("UPDATE tbl_attendance SET End_time = @0, breaktime = (SELECT CONVERT(varchar(5), DATEADD(minute, @2, 0), 114)), total_hours = (SELECT CONVERT(varchar(5), DATEADD(minute, (SELECT @2 * -1), (SELECT CONVERT(varchar(5), DATEADD(minute, DATEDIFF(minute, (SELECT start_time FROM tbl_attendance WHERE rosterID = @1), @0), 0), 114))), 114)) WHERE RosterID = @1", con);
                        //HiddenField hd = row.FindControl("RosterID") as HiddenField;

                        //Label1.Text = DateTime.Now.ToString();
                        com = new SqlCommand("UPDATE tbl_attendance SET End_time = @0, note = @2, endsignature = @3 ,breaktime=@4,[paid_break]=@5,total_hours=@6,pay=@7 WHERE RosterID = @1", con);
                        //string str = hd.Value;
                        com.Parameters.AddWithValue("@1", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));




                        TextBox timeBreak = row.FindControl("txt_break") as TextBox;

                        com.Parameters.AddWithValue("@0", time.Text);
                        if (timeBreak.Text != "")
                        {
                            com.Parameters.AddWithValue("@2", (timeBreak.Text));
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@2", 0);
                        }
                        com.Parameters.AddWithValue("@3", pathimg = "~/signaturePictures/" + GridView4.DataKeys[count].Values["RosterID"].ToString() + dt.ToString().Replace(':', '-').Replace('/', '-').Replace('\\', '-').Replace(' ', '-') + ".png");



                        SqlCommand comb = new SqlCommand("select sum(DATEDIFF(minute,[startTime],[endTime]))  FROM[dbo].[tbl_submitBreak] where[RosterID] = @0 and  paid_break=0", con);
                        comb.Parameters.AddWithValue("@0", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));

                        SqlDataReader readerComb = comb.ExecuteReader();
                        readerComb.Read();
                        int i;
                        if (readerComb.HasRows && readerComb.GetValue(0) != DBNull.Value)
                        {
                            try
                            {
                                i = Convert.ToInt32(readerComb.GetValue(0).ToString());
                            }
                            catch (Exception error)
                            {
                                i = 0;
                            }
                        }
                        else
                        {
                            i = 0;
                        }
                        comb.Dispose();

                        SqlCommand compb = new SqlCommand("select sum(DATEDIFF(minute,[startTime],[endTime]))  FROM[dbo].[tbl_submitBreak] where [RosterID] = @0 and paid_break=1", con);
                        compb.Parameters.AddWithValue("@0", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));

                        SqlDataReader readerCompb = compb.ExecuteReader();
                        readerCompb.Read();
                        int ip;
                        if (readerCompb.HasRows && readerCompb.GetValue(0) != DBNull.Value)
                        {
                            try
                            {
                                ip = Convert.ToInt32(readerCompb.GetValue(0).ToString());
                            }
                            catch (Exception error)
                            {
                                ip = 0;
                            }
                        }
                        else
                        {
                            ip = 0;
                        }
                        compb.Dispose();
                        com.Parameters.AddWithValue("@4", i);
                        com.Parameters.AddWithValue("@5", ip);

                        SqlCommand comt = new SqlCommand("select sum(DATEDIFF(minute,[Start_time],@1) ) FROM [dbo].[tbl_Attendance] where [RosterID]=@0", con);

                        TextBox txttotl = row.FindControl("txt_time") as TextBox;
                        comt.Parameters.AddWithValue("@1", txttotl.Text);
                        comt.Parameters.AddWithValue("@0", GridView4.DataKeys[count].Values["RosterID"]);
                        int j = (int)comt.ExecuteScalar() - i;
                        com.Parameters.AddWithValue("@6", j);
                        com.Parameters.AddWithValue("@7", pay1 * (j / 60));
                        com.ExecuteNonQuery();

                        //Response.Redirect(Request.RawUrl);
                        Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);

                        //Label2.Text = DateTime.Now.ToString();
                        //TimeSpan st = Convert.ToDateTime(Label2.Text).Subtract(Convert.ToDateTime(Label1.Text));
                        //Label2.Text = "total time:" + st.ToString();

                        /// rajan code

                        //Label3.Text = DateTime.Now.ToString();
                        //SqlCommand comR = new SqlCommand("SELECT start_time FROM tbl_attendance WHERE RosterID = @1", con);

                        //comR.Parameters.AddWithValue("@1", GridView4.DataKeys[count].Values["RosterID"].ToString());

                        //SqlDataReader readerR = comR.ExecuteReader();
                        //string str = null;
                        //while (readerR.Read())
                        //{
                        //    str = readerR[0].ToString();
                        //}
                        //DateTime sst = DateTime.Parse(str);
                        //DateTime et = DateTime.Parse(time.Text);
                        //TimeSpan minutes = et.Subtract(sst);
                        //double i = (minutes.TotalMinutes) - Convert.ToInt32(timeBreak.Text);

                        //comR.Dispose();

                        //SqlCommand comRr = new SqlCommand("UPDATE tbl_attendance SET End_time = @0, breaktime = @2, total_hours = @3 WHERE RosterID = @1", con);
                        //comRr.Parameters.AddWithValue("@0", time.Text);
                        //comRr.Parameters.AddWithValue("@1", Convert.ToInt32(GridView4.DataKeys[count].Values["RosterID"]));
                        //if (timeBreak.Text != "")
                        //{
                        //    comRr.Parameters.AddWithValue("@2", Convert.ToInt32(timeBreak.Text));
                        //}
                        //else
                        //{
                        //    comRr.Parameters.AddWithValue("@2", 0);
                        //}
                        //comRr.Parameters.AddWithValue("@3", i);

                        //Label4.Text = DateTime.Now.ToString();
                        //TimeSpan styy = Convert.ToDateTime(Label4.Text).Subtract(Convert.ToDateTime(Label3.Text));

                        //Label4.Text = "total time:" + styy.ToString();

                    }


                    count++;
                }

                if (com != null)
                {

                    com.Dispose();
                }

            }
            catch (Exception err2)
            {
                Session["error"] = err2.ToString();
                Response.Redirect("~/Debug.aspx");
            }
            Session["shift"] = "END";
            //   string test = " (function test() {    var canvas = document.getElementById('thecanvas');var image = document.getElementById(\"thecanvas\").toDataURL(\"image/png\");image = image.replace('data:image/png;base64,', ''); $.ajax({type: 'POST', url: 'SupervisorHome.aspx/UploadImage', data: '{ \"imageData\" : \"' + image + '\" }', contentType: 'application/json; charset=utf-8', dataType: 'json',success: function(msg) { alert('Image saved successfully!');}});});";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "test()", true);
            //     string test = "function test() {alert ('testing');}";
            //     ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", test, true);
            ////     ScriptManager.RegisterClientScriptBlock(this,this.GetType(), "text", test, true);
            //    // Response.Redirect(Request.RawUrl);
        }
    }


    static string pathimg = @"e:/";
    static int counter = 0;
    static string fileNameWitPath = "";
    public static Action NonStatic;
    [WebMethod]

    public static void UploadImage(string imageData)
    {
        if (String.Compare("e:/", pathimg) != 0)
        {
            fileNameWitPath = HttpContext.Current.Server.MapPath(pathimg);

            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {

                using (BinaryWriter bw = new BinaryWriter(fs))

                {

                    byte[] data = Convert.FromBase64String(imageData);

                    bw.Write(data);

                    bw.Close();
                }


            }
            counter++;
        }

    }
    [WebMethod()]

    protected void chkview_CheckedChanged(object sender, EventArgs e)
    {
        //GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
        //int index = row.RowIndex;
        //CheckBox cb1 = (CheckBox)GridView3.Rows[index].FindControl("chkRow");
        //TextBox txt = (TextBox)GridView3.Rows[index].FindControl("txt_time");
        //if (cb1.Checked)
        //{

        //    txt.Text = "--:-- --";

        //    txt.Style.Add("pointer-events", "none");

        //}
        //else
        //{
        //    txt.Style.Add("pointer-events", "auto");
        //}

        //here you can find your control and get value(Id).


    }


    //protected void Button4_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "test()", true);

    //    //string gg = Label1.ClientID.ToString();
    //    string str = fileNameWitPath;

    //    //var sb = new StringBuilder();
    //    //Label l = (Label)this.FindControl(gg);
    //    //l.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
    //    //string str = sb.ToString();

    //         //string fileNameWitPath = path + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";

    //    //using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
    //    //{

    //    //    using (BinaryWriter bw = new BinaryWriter(fs))

    //    //    {

    //    //        byte[] data = Convert.FromBase64String(str);

    //    //        bw.Write(data);

    //    //        bw.Close();
    //    //    }


    //    //}


    //    //string p = Image1.ImageUrl;
    //    //System.Drawing.Image im = System.Drawing.Image.FromFile(p);
    //    //im.Save("e:/test.jpg");

    //}



    protected void Button4_Click(object sender, EventArgs e)
    {

    }


    protected void btn_submitBreak(object sender, EventArgs e)
    {

    }

    protected void txt_break_TextChanged(object sender, EventArgs e)
    {

        foreach (GridViewRow row in GridView4.Rows)
        {
            CheckBox chkC = row.FindControl("chkRow") as CheckBox;


            if (chkC.Checked) //if the checkbox is checked
            {
                chkC.Checked = false;
            }
        }
    }

    protected void Gridview40_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int index = 0;
        GridViewRow row;
        GridView grid = sender as GridView;
        index = Convert.ToInt32(e.CommandArgument);
        row = grid.Rows[index];
        HiddenField hf = (HiddenField)row.FindControl("rosterid");
        CheckBox chk = (CheckBox)row.FindControl("chkpaid");
        TextBox st = (TextBox)row.FindControl("txt_breakStart");
        TextBox et = (TextBox)row.FindControl("txt_breakEnd");
        Button btn = (Button)row.FindControl("btnedit");
        double binrate = Convert.ToDouble(txtrate.Text);
        double bin = Convert.ToDouble(txtbin.Text);
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        switch (e.CommandName)
        {
            case "Start":


                if (chkphase.Checked)
                {


                    dateConverter d = new dateConverter();
                    DateTime dst = d.convertUTCtoNZT(System.DateTime.UtcNow);
                    SqlCommand mtime = new SqlCommand("Select min([Start_time]) from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                    mtime.Parameters.AddWithValue("@0", Session["Id"].ToString());
                    mtime.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                    SqlDataReader rtime = mtime.ExecuteReader();
                    rtime.Read();

                    DateTime dbtime = Convert.ToDateTime(rtime[0].ToString());

                    rtime.Close();
                    mtime.Dispose();

                    //SupervisorHome sm = new global::SupervisorHome();
                    //GridView gw = sm.FindControl("Gridview3") as GridView;

                    int rowCount = GridView4.Rows.Count;


                    DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);

                    bool[] rowBool = new bool[rowCount];




                    //The section below could do with some improvement to help relay the correct information to the user, and become more intuitive

                    SqlCommand com = null;
                    SqlCommand com2 = null;
                   


                    
                    TimeSpan ts = Convert.ToDateTime(st.Text).TimeOfDay.Subtract(dbtime.TimeOfDay);
                    com = new SqlCommand("update [dbo].[temp_attendance] set [End_time] = @1, bincounts=@2, binpay=@3 where rosterid = @0  and end_time is null", con);
                    //string str = hd.Value;


                    com.Parameters.AddWithValue("@0", hf.Value.ToString());

                    com.Parameters.AddWithValue("@1", st.Text);
                    com.Parameters.AddWithValue("@2", bin);
                    com.Parameters.AddWithValue("@3", binrate);




                    com.ExecuteReader();
                    //if (ts.TotalMinutes > phaseTime)
                    //{

                    SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                    getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                    getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                    SqlDataReader insertr = getp.ExecuteReader();

                    DataTable tblcsv = new DataTable();
                    tblcsv.Columns.Add("RosterID");
                    tblcsv.Columns.Add("Start_time");
                    tblcsv.Columns.Add("Phase");
                    tblcsv.Columns.Add("phaseBool");

                    int flag = 0;


                    while (insertr.Read())
                    {
                        DateTime ds = Convert.ToDateTime(st.Text);
                        int i = Convert.ToInt16(insertr[1].ToString());
                        i = i + 1;
                        tblcsv.Rows.Add(insertr[0].ToString(), ds.TimeOfDay, i, true);
                        Session["pnum"] = i.ToString();

                        flag = 1;
                    }
                    insertr.Close();
                    getp.Dispose();

                    if (flag == 0)
                    {
                        Session["pnum"] = "1000";
                    }

                    SqlBulkCopy bluk = new SqlBulkCopy(con);
                    bluk.DestinationTableName = "[dbo].[temp_attendance]";
                    bluk.ColumnMappings.Add("RosterID", "RosterID");
                    bluk.ColumnMappings.Add("Start_time", "Start_time");
                    bluk.ColumnMappings.Add("Phase", "Phase");
                    bluk.ColumnMappings.Add("phaseBool", "phaseBool");
                    bluk.WriteToServer(tblcsv);






                    SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2, binPay=@4, binCounts=@5 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[End_time] is null and phase <> @6", con);
                    smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
                    smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                    smax.Parameters.AddWithValue("@2", st.Text);
                    smax.Parameters.AddWithValue("@4", binrate);
                    smax.Parameters.AddWithValue("@5", bin);
                    smax.Parameters.AddWithValue("@6", Session["pnum"].ToString());
                    smax.ExecuteNonQuery();

                    Session["pnum"] = "NULL";
                    
                    SqlCommand comGetPay = new SqlCommand("DECLARE @count int DECLARE @dataTbl TABLE(phase int,workers int,bins decimal(4, 2),pay decimal(18, 0)) SET @count = (SELECT MIN(phase) FROM temp_attendance WHERE RosterID = @RosterID) WHILE @count <= (SELECT MAX(phase) FROM temp_attendance WHERE RosterID = @RosterID) BEGIN if (@count IN(SELECT phase FROM temp_attendance WHERE rosterid = @RosterID)) BEGIN INSERT INTO @dataTbl   SELECT DISTINCT (SELECT @count as 'phase'), (SELECT count(DISTINCT RosterID) FROM temp_attendance WHERE phase = @count) AS 'workers', (SELECT bincounts from temp_attendance WHERE phase = @count and RosterID = @RosterID) as 'bins', 		(SELECT binpay from temp_attendance WHERE phase = @count and RosterID = @RosterID) as 'pay'     FROM temp_attendance WHERE 'bins' is not null     GROUP BY RosterID END SET @count = @count + 1;                        END SELECT* FROM @dataTbl", con);
                    comGetPay.Parameters.AddWithValue("@RosterID", hf.Value.ToString());

                    SqlDataReader readerGetPay = comGetPay.ExecuteReader();

                    while (readerGetPay.Read())
                    {
                        int phase = (int)readerGetPay.GetInt32(0);



                        decimal pay = (Convert.ToDecimal(readerGetPay.GetValue(2).ToString()) * Convert.ToDecimal(readerGetPay.GetValue(3).ToString())) / (int)readerGetPay.GetInt32(1);
                        SqlCommand comUpdate = new SqlCommand("UPDATE temp_attendance SET pay = @0 WHERE RosterID = @1 AND phase = @2", con);
                        comUpdate.Parameters.AddWithValue("@0", pay);
                        comUpdate.Parameters.AddWithValue("@1", hf.Value.ToString());
                        comUpdate.Parameters.AddWithValue("@2", phase);

                        comUpdate.ExecuteNonQuery();
                    }


                    Session["pnum"] = "NULL";

                    TextBox tsss = (TextBox)row.FindControl("txt_breakStart");
                    TextBox tee = (TextBox)row.FindControl("txt_breakEnd");
                    SqlCommand com4 = new SqlCommand("IF (SELECT TOP (1) breakid FROM tbl_tempBreak WHERE RosterID = @0) is not null UPDATE tbl_tempBreak SET startTime = @1 WHERE breakId = (SELECT TOP (1) breakId FROM tbl_tempBreak WHERE RosterID = @0) ELSE INSERT INTO tbl_tempBreak (rosterId, startTime) VALUES (@0, @1)", con);
                    com4.Parameters.AddWithValue("@0", hf.Value.ToString());
                    com4.Parameters.AddWithValue("@1", tsss.Text);
                    com4.Parameters.AddWithValue("@2", tee.Text);

                    com4.ExecuteNonQuery();
                    com4.Dispose();
                    con.Dispose();
                    Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);



                }



                else
                {
                    int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;


                    TextBox ts = (TextBox)row.FindControl("txt_breakStart");
                    TextBox te = (TextBox)row.FindControl("txt_breakEnd");

                    SqlCommand com = new SqlCommand("IF (SELECT TOP (1) breakid FROM tbl_tempBreak WHERE RosterID = @0) is not null UPDATE tbl_tempBreak SET startTime = @1 WHERE breakId = (SELECT TOP (1) breakId FROM tbl_tempBreak WHERE RosterID = @0) ELSE INSERT INTO tbl_tempBreak (rosterId, startTime) VALUES (@0, @1)", con);
                    com.Parameters.AddWithValue("@0", hf.Value.ToString());
                    com.Parameters.AddWithValue("@1", ts.Text);
                    com.Parameters.AddWithValue("@2", te.Text);

                    com.ExecuteNonQuery();
                    com.Dispose();
                    con.Dispose();
                }


                    break;

            case "End":

                if (chkphase.Checked)
                {

                    {
                        int i = 0;
                        if (chk.Checked)
                        {
                            i = 1;
                        }
                        else
                        {
                            i = 0;
                        }
                        string str = hf.Value.ToString();

                        SqlCommand commm = new SqlCommand("INSERT INTO tbl_submitBreak (RosterID, startTime, endTime,paid_break) VALUES (@0, @1, @2,@3)", con);
                        commm.Parameters.AddWithValue("@0", str);
                        commm.Parameters.AddWithValue("@1", st.Text);
                        commm.Parameters.AddWithValue("@2", et.Text);
                        commm.Parameters.AddWithValue("@3", i);
                        commm.ExecuteNonQuery();
                        SqlCommand comd = new SqlCommand("Delete from tbl_tempBreak where RosterID = @0  ", con);
                        comd.Parameters.AddWithValue("@0", str);
                        comd.ExecuteNonQuery();
                        comd.Dispose();


                        commm.Dispose();



                        dateConverter d = new dateConverter();
                        DateTime dst = d.convertUTCtoNZT(System.DateTime.UtcNow);
                   
                        SqlCommand mtime = new SqlCommand("Select min([Start_time]) from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                        mtime.Parameters.AddWithValue("@0", Session["Id"].ToString());
                        mtime.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                        SqlDataReader rtime = mtime.ExecuteReader();
                        rtime.Read();

                        DateTime dbtime = Convert.ToDateTime(rtime[0].ToString());
                        DateTime dt = d.convertUTCtoNZT(DateTime.UtcNow);
                        rtime.Close();
                        mtime.Dispose();

                        try
                        {
                            SqlCommand com, coma = null;
                            int count = 0;

                            TimeSpan ts = Convert.ToDateTime(et.Text).TimeOfDay.Subtract(dbtime.TimeOfDay);
                            coma = new SqlCommand("BEGIN IF NOT EXISTS (SELECT * FROM tbl_attendance WHERE RosterID = @0) BEGIN INSERT INTO tbl_attendance (RosterID, Start_time) VALUES (@0, @1) END END", con);
                            coma.Parameters.AddWithValue("@0", hf.Value.ToString());
                            coma.Parameters.AddWithValue("@1", et.Text);
                            coma.ExecuteReader();


                            if (ts.TotalMinutes > phaseTime)
                            {

                                SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
                                getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                                SqlDataReader insertr = getp.ExecuteReader();

                                DataTable tblcsv = new DataTable();
                                tblcsv.Columns.Add("RosterID");
                                tblcsv.Columns.Add("Start_time");
                                tblcsv.Columns.Add("Phase");
                                tblcsv.Columns.Add("phaseBool");
                                while (insertr.Read())
                                {
                                    DateTime ds = Convert.ToDateTime(et.Text);
                                    int i2 = Convert.ToInt16(insertr[1].ToString());
                                    i2 = i2 + 1;
                                    tblcsv.Rows.Add(insertr[0].ToString(), ds.TimeOfDay, i2, true);
                                    Session["pnum"] = i2.ToString();

                                }
                                insertr.Close();
                                getp.Dispose();



                                SqlBulkCopy bluk = new SqlBulkCopy(con);
                                bluk.DestinationTableName = "[dbo].[temp_attendance]";
                                bluk.ColumnMappings.Add("RosterID", "RosterID");
                                bluk.ColumnMappings.Add("Start_time", "Start_time");
                                bluk.ColumnMappings.Add("Phase", "Phase");
                                bluk.ColumnMappings.Add("phaseBool", "phaseBool");
                                bluk.WriteToServer(tblcsv);



                                SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2, binPay=@4, binCounts=@5 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[Start_time] <> @3 and [dbo].[temp_attendance].[End_time] is null  ", con);
                                smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
                                smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
                                smax.Parameters.AddWithValue("@2", (et.Text));
                                smax.Parameters.AddWithValue("@3", (et.Text));
                                smax.Parameters.AddWithValue("@4", binrate);
                                smax.Parameters.AddWithValue("@5", bin);
                                smax.ExecuteNonQuery();


                                com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time,phase,[phaseBool]) VALUES (@0, @1, @5,@6) ", con);
                                //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                                //string str = hd.Value;


                                com.Parameters.AddWithValue("@0", hf.Value.ToString());

                                com.Parameters.AddWithValue("@1", et.Text);

                                com.Parameters.AddWithValue("@5", Session["pnum"].ToString());
                                com.Parameters.AddWithValue("@6", true);


                                com.ExecuteReader();
                                Session["pnum"] = "NULL";

                            }

                            else
                            {

                                com = new SqlCommand(" INSERT INTO [dbo].[temp_attendance] (RosterID, Start_time, blockId, startsignature,job_cat) VALUES (@0, @1) ", con);
                                //HiddenField hd = row.FindControl("RosterID") as HiddenField;
                                //string str = hd.Value;


                                com.Parameters.AddWithValue("@0", GridView3.DataKeys[count].Values["RosterID"].ToString());

                                com.Parameters.AddWithValue("@1", (row.Cells[3].Text));





                                com.ExecuteReader();
                            }

                        }
                        catch (Exception exr)
                        {
                            Session["error"] = exr.ToString();
                            Response.Redirect("~/Debug.aspx");
                        }




                    }
                    Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                }
                else
                {

                    if (string.Compare(btn.Text, "End") == 0)
                    {

                        int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;


                        TextBox ts = (TextBox)row.FindControl("txt_breakStart");
                        TextBox te = (TextBox)row.FindControl("txt_breakEnd");

                        SqlCommand com = new SqlCommand("IF (SELECT TOP (1) breakid FROM tbl_tempBreak WHERE RosterID = @0) is not null UPDATE tbl_tempBreak SET startTime = @1 WHERE breakId = (SELECT TOP (1) breakId FROM tbl_tempBreak WHERE RosterID = @0) ELSE INSERT INTO tbl_tempBreak (rosterId, startTime) VALUES (@0, @1)", con);
                        com.Parameters.AddWithValue("@0", hf.Value.ToString());
                        com.Parameters.AddWithValue("@1", ts.Text);
                        com.Parameters.AddWithValue("@2", te.Text);

                        com.ExecuteNonQuery();
                        com.Dispose();
                        con.Dispose();

                    }
                    else
                    {

                    }
                    Response.Redirect("~/Supervisor/SupervisorAttendance.aspx", false);
                }

                break;


        }



    }




    protected void txt_breakStart_TextChanged(object sender, EventArgs e)
    {

        int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;

        GridViewRow row = Gridview40.Rows[RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("rosterid");

    }

    protected void txt_breakEnd_TextChanged(object sender, EventArgs e)
    {
        int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;

        GridViewRow row = Gridview40.Rows[RowIndex];
        HiddenField hf = (HiddenField)row.FindControl("rosterid");
        TextBox ts = (TextBox)row.FindControl("txt_breakStart");
        TextBox te = (TextBox)row.FindControl("txt_breakEnd");
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        SqlCommand com = new SqlCommand("IF (SELECT TOP (1) breakid FROM tbl_tempBreak WHERE RosterID = @0) is not null UPDATE tbl_tempBreak SET endTime = @2 WHERE breakId = (SELECT TOP (1) breakId FROM tbl_tempBreak WHERE RosterID = @0) ELSE INSERT INTO tbl_tempBreak (rosterId, startTime) VALUES (@0, @1)", con);
        com.Parameters.AddWithValue("@0", hf.Value.ToString());
        com.Parameters.AddWithValue("@1", ts.Text);
        com.Parameters.AddWithValue("@2", te.Text);

        com.ExecuteNonQuery();
        com.Dispose();
        con.Dispose();
    }

    protected void cbo_mcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        for (int row2 = 0; row2 < GridView3.Rows.Count; row2++)
        {
            SqlCommand comScat = new SqlCommand("SELECT * FROM tbl_job_cat where McatID=@0", con);
            DropDownList cbo_mmmcats = (DropDownList)GridView3.Rows[row2].Cells[6].Controls[1];
            DropDownList cbo_mmcats = (DropDownList)GridView3.Rows[row2].Cells[7].Controls[1];
            cbo_mmcats.Items.Clear();
            comScat.Parameters.AddWithValue("@0", cbo_mmmcats.SelectedItem.Value.ToString());

            SqlDataReader Sreader = comScat.ExecuteReader();
            while (Sreader.Read())
            {

                DropDownList cbo_scats = (DropDownList)GridView3.Rows[row2].Cells[7].Controls[1];
                cbo_mmcats.Items.Add(new ListItem(Sreader.GetValue(1).ToString(), Sreader.GetValue(0).ToString()));
            }
        }
    }

    protected void chkphase_CheckedChanged(object sender, EventArgs e)
    {
        dateConverter d = new dateConverter();
        DateTime dst = d.convertUTCtoNZT(System.DateTime.UtcNow);

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();

        SqlCommand comCheckPhase = new SqlCommand("select * from tbl_phasecheck where supervisorid = @0 and workingDay = @1", con);
        comCheckPhase.Parameters.AddWithValue("@0", Session["Id"].ToString());
        comCheckPhase.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

        SqlDataReader readerCheckPhase = comCheckPhase.ExecuteReader();


        if (chkphase.Checked)
        {
            if (!readerCheckPhase.HasRows) //Using bins system, if there is no phase data, add some with boolean set to true
            {
                SqlCommand comChangePhase = new SqlCommand("INSERT INTO tbl_phasecheck (supervisorid, workingday, phase) VALUES (@0, @1, 1)", con);
                comChangePhase.Parameters.AddWithValue("@0", Session["Id"].ToString());
                comChangePhase.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

                comChangePhase.ExecuteNonQuery();
            }
            else //Using bins system, if there is phase data, add some with boolean set to true
            {
                SqlCommand comChangePhase = new SqlCommand("UPDATE tbl_phasecheck SET phase = 1 WHERE supervisorid = @0 and workingday = @1", con);
                comChangePhase.Parameters.AddWithValue("@0", Session["Id"].ToString());
                comChangePhase.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

                comChangePhase.ExecuteNonQuery();
            }
            SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
            getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
            getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
            SqlDataReader insertr = getp.ExecuteReader();

            DataTable tblcsv = new DataTable();
            tblcsv.Columns.Add("RosterID");
            tblcsv.Columns.Add("Start_time");
            tblcsv.Columns.Add("Phase");
            tblcsv.Columns.Add("phaseBool");
            while (insertr.Read())
            {
                int i = Convert.ToInt16(insertr[1].ToString());
                i = i + 1;
                tblcsv.Rows.Add(insertr[0].ToString(), dst.TimeOfDay, i, true);
                Session["pnum"] = i.ToString();

            }
            insertr.Close();
            getp.Dispose();



            SqlBulkCopy bluk = new SqlBulkCopy(con);
            bluk.DestinationTableName = "[dbo].[temp_attendance]";
            bluk.ColumnMappings.Add("RosterID", "RosterID");
            bluk.ColumnMappings.Add("Start_time", "Start_time");
            bluk.ColumnMappings.Add("Phase", "Phase");
            bluk.ColumnMappings.Add("phaseBool", "phaseBool");
            bluk.WriteToServer(tblcsv);

            SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[Start_time] <> @3 and   [dbo].[temp_attendance].[End_time] is null ", con);
            smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
            smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
            smax.Parameters.AddWithValue("@2", dst.TimeOfDay);
            smax.Parameters.AddWithValue("@3", dst.TimeOfDay);
            smax.ExecuteNonQuery();


        }
        else
        {
            if (!readerCheckPhase.HasRows) //Using hourly system, if there is no phase data, add some with boolean set to false
            {
                SqlCommand comChangePhase = new SqlCommand("INSERT INTO tbl_phasecheck (supervisorid, workingday, phase) VALUES (@0, @1, 0)", con);
                comChangePhase.Parameters.AddWithValue("@0", Session["Id"].ToString());
                comChangePhase.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

                comChangePhase.ExecuteNonQuery();
            }
            else //Using hourly system, if there is phase data, update it to be set to false
            {
                SqlCommand comChangePhase = new SqlCommand("UPDATE tbl_phasecheck SET phase = 0 WHERE supervisorid = @0 and workingday = @1", con);
                comChangePhase.Parameters.AddWithValue("@0", Session["Id"].ToString());
                comChangePhase.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());

                comChangePhase.ExecuteNonQuery();
            }

            SqlCommand getp = new SqlCommand("Select [dbo].[temp_attendance].RosterID,phase from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1", con);
            getp.Parameters.AddWithValue("@0", Session["Id"].ToString());
            getp.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
            SqlDataReader insertr = getp.ExecuteReader();

            DataTable tblcsv = new DataTable();
            tblcsv.Columns.Add("RosterID");
            tblcsv.Columns.Add("Start_time");
            tblcsv.Columns.Add("Phase");
            tblcsv.Columns.Add("phaseBool");
            while (insertr.Read())
            {
                int i = Convert.ToInt16(insertr[1].ToString());
                i = i + 1;
                tblcsv.Rows.Add(insertr[0].ToString(), dst.TimeOfDay, i, false);
                Session["pnum"] = i.ToString();

            }
            insertr.Close();
            getp.Dispose();



            SqlBulkCopy bluk = new SqlBulkCopy(con);
            bluk.DestinationTableName = "[dbo].[temp_attendance]";
            bluk.ColumnMappings.Add("RosterID", "RosterID");
            bluk.ColumnMappings.Add("Start_time", "Start_time");
            bluk.ColumnMappings.Add("Phase", "Phase");
            bluk.ColumnMappings.Add("phaseBool", "phaseBool");
            bluk.WriteToServer(tblcsv);

            SqlCommand smax = new SqlCommand("update [dbo].[temp_attendance] set [End_time]=@2 where [dbo].[temp_attendance].[RosterID] in (Select [dbo].[temp_attendance].[RosterID] from [dbo].[temp_attendance] inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[RosterID]=[dbo].[temp_attendance].[RosterID] where  [dbo].[tbl_Duty].[supervisorId]=@0 and [dbo].[temp_attendance].[End_time] is null and [dbo].[tbl_Duty].[Day] = @1) and [dbo].[temp_attendance].[Start_time] <> @3 and ", con);
            smax.Parameters.AddWithValue("@0", Session["Id"].ToString());
            smax.Parameters.AddWithValue("@1", dst.Month.ToString() + "/" + dst.Day.ToString() + "/" + dst.Year.ToString());
            smax.Parameters.AddWithValue("@2", dst.TimeOfDay);
            smax.Parameters.AddWithValue("@3", dst.TimeOfDay);
            smax.ExecuteNonQuery();
        }

        readerCheckPhase.Close();
        comCheckPhase.Dispose();
        comCheckPhase.Dispose();
        con.Dispose();
    }
}