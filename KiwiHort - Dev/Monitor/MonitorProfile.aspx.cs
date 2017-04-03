using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class Monitor_MonitorProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btn_cancel.Attributes.Add("onClick", "javascript:history.back(); return false;");

        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        SqlCommand com = null;
        SqlDataReader reader = null;

        try
        {
            con.Open();



            com = new SqlCommand("SELECT * FROM tbl_monitor WHERE MonitorsId = @0", con);

            com.Parameters.AddWithValue("@0", Session["Id"]);

            reader = com.ExecuteReader();

            com.Dispose();

        }
        catch (Exception err1)
        {
            Session["error"] = err1.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        if (reader.HasRows && !IsPostBack)
        {
            if (reader.Read())
            {
                txt_firstName.Text = Convert.ToString(reader["FirstName"]);
                txt_middleName.Text = Convert.ToString(reader["MiddleName"]);
                txt_lastName.Text = Convert.ToString(reader["LastName"]);
                txt_email.Text = Convert.ToString(Session["email"]);
            }
        }

        try
        {
            SqlCommand comAd = new SqlCommand("SELECT * FROM tbl_address WHERE AddressId = @0", con);
            comAd.Parameters.AddWithValue("@0", Session["Id"]);

            reader = comAd.ExecuteReader();

            comAd.Dispose();

            if (reader.HasRows && !IsPostBack)
            {
                if (reader.Read())
                {
                    txt_address1.Text = Convert.ToString(reader["Address1"]);
                    txt_address2.Text = Convert.ToString(reader["Address2"]);
                    txt_city.Text = Convert.ToString(reader["City"]);
                    txt_region.Text = Convert.ToString(reader["Region"]);
                }
            }

        }
        catch (Exception err2)
        {
            Session["error"] = err2.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        reader.Close();




        txt_firstName.Text = txt_firstName.Text.TrimEnd();
        txt_middleName.Text = txt_middleName.Text.TrimEnd();
        txt_lastName.Text = txt_lastName.Text.TrimEnd();
        txt_email.Text = txt_email.Text.TrimEnd();
        txt_address1.Text = txt_address1.Text.TrimEnd();
        txt_address2.Text = txt_address2.Text.TrimEnd();
        txt_city.Text = txt_city.Text.TrimEnd();
        txt_region.Text = txt_region.Text.TrimEnd();

        con.Close();
        con.Dispose();
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {


        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        SqlCommand com = null;
        SqlDataReader reader = null;

        try
        {

            con.Open();

            com = new SqlCommand("SELECT * FROM tbl_monitor WHERE monitorsId = @0", con);
            com.Parameters.AddWithValue("@0", Session["Id"]);


            reader = com.ExecuteReader();

            com.Dispose();
        }
        catch (Exception err3)
        {
            Session["error"] = err3.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        if (reader.HasRows)
        {
            if (reader.Read())
            {
                List<string> property = new List<string>();
                List<string> value = new List<string>();
                int count = 0;

                if (txt_firstName.Text != Convert.ToString(reader["FirstName"]))
                {
                    count++;
                    property.Add("FirstName");
                    value.Add(txt_firstName.Text);
                }

                if (txt_middleName.Text != Convert.ToString(reader["MiddleName"]))
                {
                    count++;
                    property.Add("MiddleName");
                    value.Add(txt_middleName.Text);
                }

                if (txt_lastName.Text != Convert.ToString(reader["LastName"]))
                {
                    count++;
                    property.Add("LastName");
                    value.Add(txt_lastName.Text);
                }

                if (fup_picture.HasFile)
                {
                    count++;
                    property.Add("Picture");
                    value.Add("~/profilePictures/" + fup_picture.FileName);
                    fup_picture.PostedFile.SaveAs(Server.MapPath("~/profilePictures/") + fup_picture.FileName);
                }

                if (txt_email.Text != Convert.ToString(Session["email"]))
                {
                    try
                    {
                        SqlCommand comUpdate = new SqlCommand("UPDATE tbl_login SET email = @0 WHERE Id = @1", con);
                        comUpdate.Parameters.AddWithValue("@0", txt_email.Text);
                        comUpdate.Parameters.AddWithValue("@1", Session["Id"]);
                        comUpdate.ExecuteReader();
                        comUpdate.Dispose();
                    }
                    catch (Exception err4)
                    {
                        Session["error"] = err4.ToString();
                        Response.Redirect("~/Debug.aspx");
                    }
                    Session["email"] = txt_email.Text;
                }

                //Do we want them changing their password? Because so far they can only go as far as updating their email... which I think is fair enough.

                int max = count;
                count = 0;

                while (count < max)
                {
                    try
                    {
                        SqlCommand comUpdate = new SqlCommand("UPDATE tbl_monitor SET " + property[count] + " = @0 WHERE monitorsId = @1", con);
                        comUpdate.Parameters.AddWithValue("@0", value[count]);
                        comUpdate.Parameters.AddWithValue("@1", Session["Id"]);

                        comUpdate.ExecuteReader();
                        comUpdate.Dispose();
                    }
                    catch (Exception err5)
                    {
                        Session["error"] = err5.ToString();
                        Response.Redirect("~/Debug.aspx");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your " + property[count] + " has been updated!');", true);

                    count++;
                }
            }
        }

        reader.Close();

        try
        {
            com = new SqlCommand("SELECT * FROM tbl_address WHERE AddressId = @0", con);
            com.Parameters.AddWithValue("@0", Session["Id"]);

            reader = com.ExecuteReader();
        }
        catch (Exception err6)
        {
            Session["error"] = err6.ToString();
            Response.Redirect("~/Debug.aspx");
        }

        if (!reader.HasRows)
        {
            try
            {
                SqlCommand comAdd = new SqlCommand("INSERT INTO tbl_address (AddressId) VALUES (@0)", con);
                comAdd.Parameters.AddWithValue("@0", Session["Id"]);

                comAdd.ExecuteReader();

                comAdd.Dispose();
            }
            catch (Exception err7)
            {
                Session["error"] = err7.ToString();
                Response.Redirect("~/Debug.aspx");
            }
        }

        reader.Close();

        reader = com.ExecuteReader();

        com.Dispose();

        if (reader.HasRows)
        {
            if (reader.Read())
            {
                List<string> property = new List<string>();
                List<string> value = new List<string>();
                int count = 0;

                if (txt_address1.Text != Convert.ToString(reader["Address1"]))
                {
                    count++;
                    property.Add("Address1");
                    value.Add(txt_address1.Text);
                }

                if (txt_address2.Text != Convert.ToString(reader["Address2"]))
                {
                    count++;
                    property.Add("Address2");
                    value.Add(txt_address2.Text);
                }

                if (txt_city.Text != Convert.ToString(reader["City"]))
                {
                    count++;
                    property.Add("City");
                    value.Add(txt_city.Text);
                }

                if (txt_region.Text != Convert.ToString(reader["Region"]))
                {
                    count++;
                    property.Add("Region");
                    value.Add(txt_region.Text);
                }
                reader.Close();

                int max = count;
                count = 0;

                while (count < max)
                {
                    try
                    {
                        SqlCommand comUpdate = new SqlCommand("UPDATE tbl_address SET " + property[count] + " = @0 WHERE AddressId = @1", con);
                        comUpdate.Parameters.AddWithValue("@0", value[count]);
                        comUpdate.Parameters.AddWithValue("@1", Session["Id"]);

                        comUpdate.ExecuteReader();
                        comUpdate.Dispose();
                    }
                    catch (Exception err8)
                    {
                        Session["error"] = err8.ToString();
                        Response.Redirect("~/Debug.aspx");
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your " + property[count] + " has been updated!');", true);

                    count++;
                }
            }
        }

        //if (fup_picture.HasFile)
        //{
        //    SqlCommand com2 = new SqlCommand("UPDATE tbl_grower SET Picture = @0 WHERE GrowersId = @1", con);
        //    com2.Parameters.AddWithValue("@0", "~/profilePictures/" + fup_picture.FileName);
        //    com2.Parameters.AddWithValue("@1", Session["Id"]);

        //    fup_picture.PostedFile.SaveAs(Server.MapPath("~/profilePictures/") + fup_picture.FileName);
        //    com2.ExecuteReader();

        //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your picture has been updated!');", true);
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your account has not been updated, as none of the fields were filled in.');", true);
        //}

        con.Close();
        con.Dispose();

        //Response.Redirect("ContractorOrganiser.aspx");

        Response.Redirect("MonitorHome.aspx");
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MonitorHome.aspx");
    }



    //Button_submitfarm_Click() starts here

    protected void btn_submitFarm_Click(object sender, EventArgs e)
    {
        btn_cancel.Text = "Return";

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        SqlCommand comAddress = null;
        SqlCommand comFarm = null;
        SqlCommand comSelectMax = null;

        con.Open();

        comSelectMax = new SqlCommand("IF (SELECT max(CAST(SUBSTRING(farmId, 7, 30) AS INT)) FROM tbl_farms WHERE SUBSTRING(farmId, 1, 6) = 'farmid') IS NULL SELECT 0 ELSE SELECT max(CAST(SUBSTRING(farmId, 7, 30) AS INT)) FROM tbl_farms WHERE SUBSTRING(farmId, 1, 6) = 'farmid'", con);

        int max = (int)comSelectMax.ExecuteScalar();

        max++;


        comFarm = new SqlCommand("INSERT INTO tbl_farms (FarmId, farm_name, growerId) VALUES (@0, @1, @2)", con);
        comFarm.Parameters.AddWithValue("@0", "farmid" + max);
        comFarm.Parameters.AddWithValue("@1", txt_farmName.Text);
        comFarm.Parameters.AddWithValue("@2", Session["Id"]);

        comAddress = new SqlCommand("INSERT INTO tbl_address (AddressId, Address1, Address2, City, Region, postcode) VALUES (@0, @1, @2, @3, @4, @5)", con);
        comAddress.Parameters.AddWithValue("@0", "farmid" + max);
        comAddress.Parameters.AddWithValue("@1", txt_addressFarm.Text);
        comAddress.Parameters.AddWithValue("@2", txt_address2Farm.Text);
        comAddress.Parameters.AddWithValue("@3", txt_cityFarm.Text);
        comAddress.Parameters.AddWithValue("@4", txt_regionFarm.Text);
        comAddress.Parameters.AddWithValue("@5", txt_postcodeFarm.Text);

        comFarm.ExecuteReader();
        comAddress.ExecuteReader();

        comFarm.Dispose();
        comAddress.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('You have registered a farm with the system.');", true);




        // GridView1.DataBind();
        txt_addressFarm.Text = "";
        txt_address2Farm.Text = "";
        txt_cityFarm.Text = "";
        txt_farmName.Text = "";
        txt_postcodeFarm.Text = "";
        txt_regionFarm.Text = "";

        con.Dispose();

    }


    //BUtton_submitefarm_Click() ends here


    //Button Edit in View/update farm starts here
    protected void Button1_Click(object sender, EventArgs e)
    {
        //Named it wrong, didn't I?

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        SqlCommand com = new SqlCommand("SELECT tbl_farms.farmId, tbl_farms.farm_name, tbl_address.address1, tbl_address.address2, tbl_address.city, tbl_address.region, tbl_address.postcode FROM tbl_farms INNER JOIN tbl_address ON tbl_farms.FarmId = tbl_address.AddressId WHERE FarmId = @0", con);
        com.Parameters.AddWithValue("@0", cbo_selectFarm.SelectedValue.ToString());


        SqlDataReader reader = com.ExecuteReader();

        Session["SelectedFarm"] = String.Empty;

        if (reader.HasRows)
        {
            if (reader.Read())
            {
                Session["SelectedFarm"] = reader[0].ToString().TrimEnd();
                txt_farmName.Text = reader[1].ToString().TrimEnd();
                txt_addressFarm.Text = reader[2].ToString().TrimEnd();
                txt_address2Farm.Text = reader[3].ToString().TrimEnd();
                txt_cityFarm.Text = reader[4].ToString().TrimEnd();
                txt_regionFarm.Text = reader[5].ToString().TrimEnd();
                txt_postcodeFarm.Text = reader[6].ToString().TrimEnd();
            }
        }
        else
        {
            txt_farmName.Text = "";
            txt_addressFarm.Text = "";
            txt_address2Farm.Text = "";
            txt_cityFarm.Text = "";
            txt_regionFarm.Text = "";
            txt_postcodeFarm.Text = "";
        }
    }

    //button Eidt in View/Update Farm Ends Here

    //button Add block code starts Here
    protected void Button2_Click(object sender, EventArgs e)
    {
        // string str = DropDownList1.SelectedValue.ToString();
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        SqlCommand com = new SqlCommand("insert Into tbl_blocks([Block_Name],[FarmId]) values (@0,@1)", con);
        com.Parameters.AddWithValue("@0", txtblockname.Text);
        com.Parameters.AddWithValue("@1", txtfarmid.Text);
        com.ExecuteNonQuery();
        com.Dispose();
        con.Dispose();


    }
    //button add block Code Ends Here


    // button edit block code starts here

    protected void btneditblock_Click(object sender, EventArgs e)
    {
        btn_cancel.Text = "Return";

        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        SqlCommand com = new SqlCommand("SELECT tbl_blocks.BlockId, tbl_blocks.Block_Name from tbl_blocks where tbl_blocks.Block_Name= @0", con);
        com.Parameters.AddWithValue("@0", DropDownList2.SelectedValue.ToString());


        SqlDataReader reader = com.ExecuteReader();

        Session["SelectedBlock"] = String.Empty;

        if (reader.HasRows)
        {
            if (reader.Read())
            {
                Session["SelectedBlock"] = reader[0].ToString().TrimEnd();
                txtblockname.Text = reader[1].ToString().TrimEnd();

                con.Close();

            }
        }
        else
        {
            txtblockname.Text = "";
            con.Close();
        }
    }

    //button edit block code ends here



    //GV view blocks update and edit starts here

    protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        //NewEditIndex property used to determine the index of the row being edited.  
        GridView3.EditIndex = e.NewEditIndex;
        GridView3.DataBind();
    }
    protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        //Finding the controls from Gridview for the row which is going to update  
        string cs =  WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;
        SqlConnection con;
        Label id = GridView3.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        TextBox Bname = GridView3.Rows[e.RowIndex].FindControl("txt_blockName") as TextBox;
        TextBox Fid = GridView3.Rows[e.RowIndex].FindControl("txt_farmid") as TextBox;
        con = new SqlConnection(cs);
        con.Open();
        //updating the record  
        SqlCommand cmd = new SqlCommand("Update tbl_blocks set Block_Name=@Bname,FarmId=@Fid where BlockId=@id", con);
        cmd.Parameters.AddWithValue("@Bname", Bname.Text);
        cmd.Parameters.AddWithValue("@Fid", Fid.Text);
        cmd.Parameters.AddWithValue("@id", id.Text);
        cmd.ExecuteNonQuery();
        con.Close();
        //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
        GridView3.EditIndex = -1;
        //Call ShowData method for displaying updated data  
        GridView3.DataBind();
    }
    protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
        GridView3.EditIndex = -1;
        GridView3.DataBind();
    }


    //protected void ShowData()
    //{
    //    string cs = WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;
    //    SqlConnection con;
    //    DataTable dt = new DataTable();
    //    con = new SqlConnection(cs);
    //    con.Open();
    //    SqlDataAdapter adapt;
    //    adapt = new SqlDataAdapter("Select ID,Name,City from tbl_Employee", con);
    //    adapt.Fill(dt);
    //    if (dt.Rows.Count > 0)
    //    {
    //        GridView1.DataSource = dt;
    //        GridView1.DataBind();
    //    }
    //    con.Close();
    //}
    //GV view blocks update and edit ends here
}