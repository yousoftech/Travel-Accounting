using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text;

public partial class Resetpassword : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (!txtpwd.Text.Equals(null))
        {
           string  email = Request.QueryString["Email"];
            string type= Request.QueryString["Type"];

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
            con.Open();
            SqlCommand com = new SqlCommand("Update tbl_login set password=@0 WHERE password_change_status=1 AND email = @1 AND type=@2", con);
            com.Parameters.AddWithValue("@0", txtpwd.Text);
            com.Parameters.AddWithValue("@1", email);
            com.Parameters.AddWithValue("@2", type);
            int i = com.ExecuteNonQuery();
           
            SqlCommand com1= new SqlCommand("Update tbl_login set password_change_status=0 WHERE  password=@0 AND email = @1 AND type=@2", con);
            com1.Parameters.AddWithValue("@0", txtpwd.Text);
            com1.Parameters.AddWithValue("@1", email);
            com1.Parameters.AddWithValue("@2", type);
            com1.ExecuteNonQuery();
            if (i==1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Hi,<br/> Your password has been successfully updated <br/>");
                sb.Append("thanks");

                SendEmail se = new SendEmail();
                string to = email.Trim();
                se.EmailSend(to, "Password Reset", sb.ToString());
                Response.Write("<script>alert ('your password has been successfully updated'); window.location.href = 'login.aspx';</script>");
                txtpwd.Text = "";
                txtcofrmpwd.Text = "";
            }
            else
            {
                Response.Write("<script>alert ('We can not update your password using this url please reset your password again')</script>");
            }
            com.Dispose();
            com1.Dispose();
            con.Close();
            con.Dispose();
         


        }

    }
    
}