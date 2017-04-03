using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class SupervisorMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Id"] == null)
        {
            Response.Redirect("~/login.aspx");
        }

        Button2_Click(null, null);
        ((Label)Master.FindControl("lbl_title")).Text = "Messaging";
        // RequiredFieldValidator2.ControlToValidate = txtsubject;

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:hideIcon(); ", true);
    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownList1_TextChanged(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        try
        {
            SqlDataSource1.SelectCommand = "Select Efrom,Subjects,Content,Sentdate from tbl_messages where eTo= '" + Session["email"].ToString() + "' ORDER BY Sentdate DESC";

            GridView1.DataBind();
        }
        catch (Exception err1)
        {
            Session["error"] = err1.ToString();
            Response.Redirect("~/Debug.aspx");
        }
        //GridView1.Columns[1].HeaderText = "From";
        //GridView1.Columns[2].HeaderText = "Subject";

    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 2;
        try
        {
            SqlDataSource2.SelectCommand = "Select Eto,Subjects,Content,Sentdate from tbl_messages where efrom= '" + Session["email"].ToString() + "' ORDER BY Sentdate DESC";
            GridView2.DataBind();
        }
        catch (Exception err6)
        {
            Session["error"] = err6.ToString();
            Response.Redirect("~/Debug.aspx");
        }
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString);
        try
        {
            con.Open();
            SqlCommand com = new SqlCommand("INSERT INTO tbl_messages (Efrom,Subjects,eTo,SentDate,Content) VALUES (@from,@subject,@to,@date,@content)", con);
            com.Parameters.AddWithValue(@"from", Session["email"].ToString());
            com.Parameters.AddWithValue(@"subject", txtsubject.Text);
            com.Parameters.AddWithValue(@"to", txtto.Text);
            com.Parameters.AddWithValue(@"date", DateTime.UtcNow);
            //if (Convert.ToInt32(DateTime.Now.Hour) > 12)
            //{
            //    com.Parameters.AddWithValue(@"date", DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString() + " " + (Convert.ToInt32(DateTime.Now.Hour) - 12).ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " PM");
            //}
            //else
            //{
            //    com.Parameters.AddWithValue(@"date", DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " AM");
            //}
            com.Parameters.AddWithValue(@"content", txtbody.Text);

            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your message has been sent.');", true);

            com.ExecuteNonQuery();

            com.Dispose();
        }
        catch (Exception err6)
        {
            Session["error"] = err6.ToString();
            Response.Redirect("~/Debug.aspx");
        }
    }
}