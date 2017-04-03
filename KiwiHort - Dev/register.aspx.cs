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

public partial class register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btn_submit.Enabled = false;
        if (cbo_registerAs.SelectedValue == "Worker" || cbo_registerAs.SelectedValue == "Supervisor")
        {
            txt_code.Visible = true;
           
            txt_passport.Visible = true;
        }
        else
        {
            txt_code.Visible = false;
            txt_passport.Visible = false;
        }
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);

        con.Open();

        SqlCommand comEmail = new SqlCommand(" SELECT * FROM tbl_login WHERE type = @0 AND email = @1", con);
        comEmail.Parameters.AddWithValue("@0", cbo_registerAs.Text.ToString());
        comEmail.Parameters.AddWithValue("@1", txt_email.Text.ToString());

        string email =(string) comEmail.ExecuteScalar();

        bool validCode = false;

        if(String.Equals(null,email))
        {
            int idNum;
            string idStr;
            string type; //Type can equal "worker", "grower", "superv"?, "monito"

            if (cbo_registerAs.SelectedValue == "Worker" || cbo_registerAs.SelectedValue == "Supervisor")
            {
                type = "worker";
                SqlCommand comCode2 = new SqlCommand("SELECT growersId FROM [tbl_growercode] WHERE code = @0", con);
                comCode2.Parameters.AddWithValue("@0", txt_code.Text);

                SqlDataReader readerCode = comCode2.ExecuteReader();
                comCode2.Dispose();

                if (readerCode.HasRows)
                {
                    readerCode.Read();
                    validCode = true;

                    Session["gId"] = readerCode.GetString(0).ToString();

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please enter a valid code');", true);
                }
            }
            else if(cbo_registerAs.SelectedValue == "Contractor")
            {
                type = "grower";
                validCode = true;
            }
            else
            {
                type = "monitr";
                validCode = true;
            }

            if (validCode)
            {
                SqlCommand comGetId = new SqlCommand("IF (SELECT max(CAST(SUBSTRING(Id, 7, 30) AS INT)) FROM tbl_login WHERE SUBSTRING(Id, 1, 6) = @0) IS NULL SELECT 0 ELSE SELECT max(CAST(SUBSTRING(Id, 7, 30) AS INT)) FROM tbl_login WHERE SUBSTRING(Id, 1, 6) = @0", con);
                comGetId.Parameters.AddWithValue("@0", type);
                idNum = (int)comGetId.ExecuteScalar();
                idNum++;
                idStr = type + idNum.ToString();

                SqlCommand comLogin = new SqlCommand("INSERT INTO tbl_login (Id, email, password, type) VALUES (@0, @1, @2, @3)", con);
                comLogin.Parameters.AddWithValue("@0", idStr);
                comLogin.Parameters.AddWithValue("@1", txt_email.Text.ToString());
                comLogin.Parameters.AddWithValue("@2", txt_password.Text.ToString());
                comLogin.Parameters.AddWithValue("@3", cbo_registerAs.Text.ToString());

                if (type == "monitr")
                {
                    type = "monitor";
                }
                SqlCommand comAccount = null;
                if (type == "worker")
                {
                    SqlCommand comPay = new SqlCommand("INSERT INTO tbl_pay (pay, lastUpdate) VALUES (15.75, @0)", con);
                    comPay.Parameters.AddWithValue("@0", DateTime.UtcNow);
                    comPay.ExecuteScalar();

                    comPay.Parameters.Clear();
                    comPay = new SqlCommand("SELECT max(payID) FROM tbl_pay", con);


                    comAccount = new SqlCommand("INSERT INTO tbl_" + type + " (" + type + "sId, FirstName, LastName, payrate,DOB,[Passport Number]) VALUES (@0, @1, @2, @3,@4,@5)", con);
                    comAccount.Parameters.AddWithValue("@3", comPay.ExecuteScalar().ToString());
                    comAccount.Parameters.AddWithValue("@4", txt_DOB.Text);
                    comAccount.Parameters.AddWithValue("@5", txt_passport.Text);

                    comPay.Dispose();
                }
                else
                {
                    comAccount = new SqlCommand("INSERT INTO tbl_" + type + " (" + type + "sId, FirstName, LastName) VALUES (@0, @1, @2)", con);
                }
                comAccount.Parameters.AddWithValue("@0", idStr);
                comAccount.Parameters.AddWithValue("@1", txt_firstName.Text.ToString());
                comAccount.Parameters.AddWithValue("@2", txt_lastName.Text.ToString());
               


                if (type == "worker")
                {
                    SqlCommand comCode = new SqlCommand("INSERT INTO tbl_employees VALUES (@0, @1)", con);
                    comCode.Parameters.AddWithValue("@0", idStr);
                    comCode.Parameters.AddWithValue("@1", Session["gId"].ToString());
                    Session.Remove("gId");
                    comCode.ExecuteReader();
                    comCode.Dispose();
                }

                
                comLogin.ExecuteReader();
                comAccount.ExecuteReader();
                comLogin.Dispose();
                comAccount.Dispose();
                
                SqlCommand comAccount1 = new SqlCommand("INSERT INTO tbl_UserAct (Id,ActCode) VALUES (@0, @1)", con);
                comAccount1.Parameters.AddWithValue("@0", idStr);

                string activationCode = Guid.NewGuid().ToString();
                Session["Id"] = idStr;
                Session["Email"] = txt_email.Text.ToString();
                //Rajan Code Start
                comAccount1.Parameters.AddWithValue("@1", activationCode);
                comAccount1.ExecuteReader();
                comAccount1.Dispose();
                string subj = "Account Activation";
                string body = "Hello " + txt_email.Text.Trim() + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("register.aspx", "Activation.aspx?ActivationCode=" + activationCode) + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";
                string to = txt_email.Text;
                SendEmail se = new SendEmail();
                se.EmailSend(to, subj, body);
                // Rajan Code End

                if (type == "worker")
                {
                    SqlCommand comVisa = new SqlCommand("INSERT INTO tbl_visa (VisaId) VALUES (@0)", con);
                    comVisa.Parameters.AddWithValue("@0", idStr);

                    comVisa.ExecuteReader();
                    comVisa.Dispose();


                }
                Session["registerNew"] = "true";
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('The email address you are trying to use is already registered with another account');", true);
        }

        con.Close();
        con.Dispose();
    }




    //Rajan code end


    protected void cbo_registerAs_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbo_registerAs.SelectedValue == "Worker" || cbo_registerAs.SelectedValue == "Supervisor")
        {
            txt_code.Visible = true;
        }
        else
        {
            txt_code.Visible = false;
        }
    }
}