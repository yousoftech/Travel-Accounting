using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class temp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.Compare(Session["redirect"].ToString(), "orgSubmit") == 0)
        {
            Session["redirect"] = "message";
            Response.Redirect("Contractor/ContractorOrganiser.aspx");
        }
    }
}