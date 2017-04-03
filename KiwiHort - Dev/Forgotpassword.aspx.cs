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

public partial class Forgotpassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;

    }

    protected void btn_send_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        con.Open();
        SqlCommand com = new SqlCommand("SELECT * FROM tbl_login WHERE type = @0 AND email = @1 ", con);
        com.Parameters.AddWithValue("@0", DropDownList1.Text.ToString());
        com.Parameters.AddWithValue("@1", txt_email.Text.ToString());

        SqlDataReader reader = com.ExecuteReader();

        if (reader.HasRows)
        {
            SqlCommand com1 = new SqlCommand("Update tbl_login set password_change_status=1 WHERE type = @0 AND email = @1 ", con);
            com1.Parameters.AddWithValue("@0", DropDownList1.Text.ToString());
            com1.Parameters.AddWithValue("@1", txt_email.Text.ToString());
            com1.ExecuteNonQuery();
            com1.Dispose();
            StringBuilder sb = new StringBuilder();
            sb.Append("Hi,<br/> Click on below given link to Reset Your Password<br/>");
            sb.Append("<a href='"+Request.Url.AbsoluteUri.Replace("Forgotpassword.aspx", "Resetpassword.aspx?Email=" + txt_email.Text));
            sb.Append("&Type=" + DropDownList1.Text.ToString() + "'> Click here </a> to change your password</a><br/>");
            
            sb.Append("thanks");
            SendEmail se = new SendEmail();
            se.EmailSend(txt_email.Text.Trim(), "Reset Your Password", sb.ToString());
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('successfully sent reset link on  your mail ,please check once! Thank you.');", true);
            
            //Label1.Visible = true;
            txt_email.Text = "";

        }
        else
        {
            Label1.Text = "Invalid Account";
            Label1.Visible = true;
        }
        con.Close();
        con.Dispose();
        com.Dispose();
     }
    
}