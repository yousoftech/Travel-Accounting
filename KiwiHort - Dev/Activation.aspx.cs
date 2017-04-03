using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Activation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM tbl_UserAct WHERE ActCode = @ActivationCode"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {

                        var debug = 0;


                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                        cmd.Connection = con;
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected == 1 || debug == 0)
                        {
                            ltMessage.Text = "<small style='color: #8ab346'>Activation successful.</small>";
                            ltSubmessage.Text = " <a href='login.aspx' class='contact-link'>click here</a> to log in";
                        }
                        else
                        {
                            ltMessage.Text = "<small style='color: red'>Invalid Activation code.</small>";
                            ltSubmessage.Text = "Please re-enter your activation code and try again. If the problem persists please  <a href='Home.aspx#theContact' class='contact-link'>contact us</a>";
                        }
                        sda.Dispose();
                    }

                    cmd.Dispose();
                }
                con.Dispose();
                con.Dispose();
            }
        }
    }
}