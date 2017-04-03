using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

//public partial class Admin_index : System.Web.UI.Page
//{

//    OleDbConnection Econ;
//    SqlConnection con;

//    string constr, Query, sqlconn;
//    protected void Page_Load(object sender, EventArgs e)
//    {


//    }

//    private void ExcelConn(string FilePath)
//    {

//        constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
//        Econ = new OleDbConnection(constr);

//    }
//    private void connection()
//    {
//        sqlconn = ConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;
//        con = new SqlConnection(sqlconn);

//    }


//    private void InsertExcelRecords(string FilePath)
//    {
//        ExcelConn(FilePath);
//        try
//        {
//            Query = string.Format("Select [First Name],[Last Name],[Dob],[Passport Number],[Visa Status],[Visa Expiry Date]] FROM [{0}]", "Sheet1$");
//            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
//            Econ.Open();

//            DataSet ds = new DataSet();
//            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
//            Econ.Close();
//            oda.Fill(ds);
//            DataTable Exceldt = ds.Tables[0];
//            connection();
//            //creating object of SqlBulkCopy    
//            SqlBulkCopy objbulk = new SqlBulkCopy(con);
//            //assigning Destination table name    
//            objbulk.DestinationTableName = "Employee";
//            //Mapping Table column    
//            objbulk.ColumnMappings.Add("First Name", "First Name");
//            objbulk.ColumnMappings.Add("Last Name", "Last Name");
//            objbulk.ColumnMappings.Add("Dob", "Dob");
//            objbulk.ColumnMappings.Add("Passport Number", "Passport Number");
//            objbulk.ColumnMappings.Add("Visa Status", "Visa Status");
//            objbulk.ColumnMappings.Add("Visa Expiry Date", "Visa Expiry Date");
//            //inserting Datatable Records to DataBase    
//            con.Open();
//            objbulk.WriteToServer(Exceldt);
//        }
//        catch(Exception ex)
//        {

//        }
//        con.Close();

//    }
//    protected void Button1_Click(object sender, EventArgs e)
//    {
//        string CurrentFilePath = Path.GetFullPath(FileUpload1.PostedFile.FileName);
//        InsertExcelRecords(CurrentFilePath);
//    }
//}


public partial class Admin_index : System.Web.UI.Page
{
    SqlConnection conn = new SqlConnection("Data Source=SPIDER;Initial Catalog=Demo;Integrated Security=True");
    DataSet ds;
    DataTable Dt;
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    SqlConnection con;

    string sqlconn;
   


    private void connection()
    {
        sqlconn = ConfigurationManager.ConnectionStrings["KiwihortData"].ConnectionString;
        con = new SqlConnection(sqlconn);

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //Creating object of datatable  
        DataTable tblcsv = new DataTable();
        //creating columns  
        tblcsv.Columns.Add("First Name");
        tblcsv.Columns.Add("Last Name");
        tblcsv.Columns.Add("DOB");
        tblcsv.Columns.Add("Passport Number");
        tblcsv.Columns.Add("Visa Status");
        tblcsv.Columns.Add("Visa Expiry Date");
        //getting full file path of Uploaded file  


        string excelPath = Server.MapPath("~/visa/") + Path.GetFileName(FlUploadcsv.PostedFile.FileName);
        FlUploadcsv.SaveAs(excelPath);
        //Reading All text  
        string ReadCSV = File.ReadAllText(excelPath);
        //spliting row after new line  
        foreach (string csvRow in ReadCSV.Split('\n'))
        {
            if (!string.IsNullOrEmpty(csvRow))
            {
                //Adding each row into datatable  
                tblcsv.Rows.Add();
                int count = 0;
                foreach (string FileRec in csvRow.Split(','))
                {
                    tblcsv.Rows[tblcsv.Rows.Count - 1][count] = FileRec;
                    count++;
                }
            }


        }
        //Calling insert Functions  
        InsertCSVRecords(tblcsv);
    }
    //Function to Insert Records  
    private void InsertCSVRecords(DataTable csvdt)
    {

        connection();

     

        //creating object of SqlBulkCopy    
        SqlBulkCopy objbulk = new SqlBulkCopy(con);
        //assigning Destination table name    
        objbulk.DestinationTableName = "tbl_visaimport";
        //Mapping Table column    
    
        objbulk.ColumnMappings.Add("First Name", "First Name");
        objbulk.ColumnMappings.Add("Last Name", "Last Name");
        objbulk.ColumnMappings.Add("DOB", "DOB");
        objbulk.ColumnMappings.Add("Passport Number", "Passport Number");
        objbulk.ColumnMappings.Add("Visa Status", "Visa Status");
        objbulk.ColumnMappings.Add("Visa Expiry Date", "Visa Expiry Date");
        //inserting Datatable Records to DataBase    
        con.Open();
        objbulk.WriteToServer(csvdt);
        con.Close();


    }

    //private void BindGrid()
    //{
    //    DataSet ds = new DataSet();
    //    conn.Open();
    //    string cmdstr = "Select * from EmpImport";
    //    SqlDataAdapter adp = new SqlDataAdapter(cmdstr, conn);
    //    adp.Fill(ds);
    //    gvEmployee.DataSource = ds;
    //    gvEmployee.DataBind();
    //    ds.Dispose();
    //    conn.Close();
    //}
}
