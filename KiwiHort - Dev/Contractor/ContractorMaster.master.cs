using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class ContractorMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if(Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }


        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();
        if (!IsPostBack)
        {
            try
            {
                SqlCommand com = new SqlCommand("select * from tbl_monitor inner join tbl_employees on tbl_monitor.monitorsid = [dbo].[tbl_employees].[growersid] where [dbo].[tbl_employees].[workersid] = @0", con);
                com.Parameters.AddWithValue("@0", Session["Id"].ToString());

                SqlDataReader reader = com.ExecuteReader();

                com.Dispose();

                //if (reader.HasRows) //This should be uncommented to enable this feature.
                if(false) //Comment this to enable this feature
                {
                    cbo_monitor.Items.Clear();
                    while (reader.Read())
                    {
                        cbo_monitor.Items.Add(new ListItem(reader["FirstName"].ToString() + reader["LastName"].ToString(), reader["MonitorsId"].ToString()));
                    }
                }
                else
                {
                    cbo_monitor.Items.Add("None");
                }
            }
            catch (Exception err2)
            {
                Session["error"] = err2.ToString();
                Response.Redirect("~/Debug.aspx");
            }



            if (!string.IsNullOrEmpty(Session["monitorIndex"] as string))
            {
                cbo_monitor.SelectedIndex = Convert.ToInt32(Session["monitorIndex"].ToString());
                
            }
        }
        

        try
        {

            SqlCommand com = new SqlCommand("SELECT * FROM tbl_grower WHERE GrowersId = @0", con);
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

    protected void btn_signOut_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("../home.aspx");
    }

    protected void cbo_monitor_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["monitorIndex"] = cbo_monitor.SelectedIndex.ToString();
    }
}
