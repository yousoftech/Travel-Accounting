using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class Admin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
       
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        if (txtSearch.Text==String.Empty)
        {
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            SqlDataSource1.SelectCommand = "GetWorker";
            SqlDataSource1.DataBind();
        }
        else
        {
            SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectCommand = "GetSearch";

            SqlDataSource1.SelectParameters.Add("search", txtSearch.Text);
            SqlDataSource1.DataBind();
        }
    }
}