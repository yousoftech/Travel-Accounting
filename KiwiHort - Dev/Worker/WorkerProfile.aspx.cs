using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class Worker_WorkerProfile : System.Web.UI.Page
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



            com = new SqlCommand("SELECT * FROM tbl_worker WHERE WorkersId = @0", con);

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

            com = new SqlCommand("SELECT * FROM tbl_worker WHERE WorkersId = @0", con);
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
                        SqlCommand comUpdate = new SqlCommand("UPDATE tbl_worker SET " + property[count] + " = @0 WHERE WorkersId = @1", con);
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

        if (FileUpload1.HasFile)
        {
            SqlCommand com2 = new SqlCommand("UPDATE [tbl_visa] SET [passporturl] = @0 WHERE [VisaId] = @1", con);
            com2.Parameters.AddWithValue("@0", "~/visa/"+ FileUpload1.FileName );
            com2.Parameters.AddWithValue("@1", Session["Id"].ToString());

            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/profilePictures/") + FileUpload1.FileName);
            com2.ExecuteReader();

            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your passport picture has been submited!');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your account has not been updated, as none of the fields were filled in.');", true);
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

        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your profile settings have been updated.'); window.location.href = 'WorkerHome.aspx';", true);
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {

    }
}