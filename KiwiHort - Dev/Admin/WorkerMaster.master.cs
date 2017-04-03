using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class WorkerMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }


        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        try
        {

            con.Open();

            SqlCommand com = new SqlCommand("SELECT * FROM tbl_worker WHERE WorkersId = @0", con);
            com.Parameters.AddWithValue("@0", Session["Id"]);

            SqlDataReader reader = com.ExecuteReader();

            com.Dispose();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    lbl_name.Text = reader["FirstName"] + " " + reader["LastName"];

                    if (reader["Picture"] == DBNull.Value)
                    {
                        img_profile.Attributes["src"] = ResolveUrl("~/img/14456900_1036563233107787_1965655255_o.jpg");
                    }
                    else
                    {
                        img_profile.Attributes["src"] = ResolveUrl(Convert.ToString(reader["Picture"]));
                    }
                }
            }

            reader.Close();

        }
        catch (Exception err1)
        {
            Session["error"] = err1.ToString();
            Response.Redirect("~/Debug.aspx");
        }


        con.Close();
        con.Dispose();
    }

    protected void btn_logOut_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("../home.aspx");
    }
}
