using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BLL;
using Common;


public partial class Administrator_User_Controls_MainMenu : System.Web.UI.UserControl
{
    Shoping ac = new Shoping();
    ShopingDatum dm = new ShopingDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("Shoping", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid();
        }
    }

    private void FillGrid()
    {
        PublicClass pc = new PublicClass();
        dm.Register_Date = pc.GetDate();
        dm.Id_User = 0;
        dm.Factor_Code = TextBox3.Text;
            dm.Bank_Name = TextBox4.Text;
        if (TextTitle.Text.Length > 0 && TextBox1.Text.Length > 0 && TextBox2.Text.Length > 0)
        {
            dm.Register_Date = TextTitle.Text + "/" + TextBox1.Text + "/" + TextBox2.Text;
        }
        else if (TextTitle.Text.Length > 0 && TextBox1.Text.Length > 0 && TextBox2.Text.Length == 0)
        {
            dm.Register_Date = TextTitle.Text + "/" + TextBox1.Text;
        }
        else if (TextTitle.Text.Length == 0 && TextBox1.Text.Length > 0 && TextBox2.Text.Length > 0)
        {
            dm.Register_Date =  TextBox1.Text + "/" + TextBox2.Text;
        }
        else if (TextTitle.Text.Length > 0 && TextBox1.Text.Length == 0 && TextBox2.Text.Length == 0)
        {
            dm.Register_Date = TextTitle.Text ;
        }
        if (TextTitle.Text.Length == 0 && TextBox1.Text.Length == 0 && TextBox2.Text.Length > 0)
        {
            dm.Register_Date =  TextBox2.Text;
        }
        if (TextTitle.Text.Length == 0 && TextBox1.Text.Length > 0 && TextBox2.Text.Length == 0)
        {
            dm.Register_Date =TextBox1.Text ;
        }
        if (TextTitle.Text.Length == 0 && TextBox1.Text.Length == 0 && TextBox2.Text.Length == 0)
        {
            dm.Register_Date = "";
        }

        

        if (Request.QueryString["ID_User"] !=null )
        {
            dm.Id_User = decimal.Parse(Request.QueryString["ID_User"]);
        }

        if (Request.QueryString["Kind"] == "NoPayment")
        {
            GridView1.DataSource = ac.select_factor_nopeyment(dm,txtCity.Text,txtCountry.Text);
        }
        else if (Request.QueryString["Kind"] == "Payment")
        {
            GridView1.DataSource = ac.select_factor_peyment(dm,txtCity.Text,txtCountry.Text );
        }
        else if (Request.QueryString["Kind"] == "Sended")
        {
            GridView1.DataSource = ac.select_factor_sended(dm,txtCity.Text,txtCountry.Text );
        }

        //footer
        long sp = 0;
        foreach (DataRow dr in ((DataTable)GridView1.DataSource).Rows)
        {
            sp += long.Parse(dr["sum_price"].ToString());
        }
        GridView1.Columns[2].FooterText =string.Format("جمع کل :{0}", sp.ToString("##,0"));        
        GridView1.DataBind();
        //excel out
        DataTable dt = (DataTable)GridView1.DataSource;
        new DAL.WriteToExcel().WriteTable(dt, Server.MapPath(@"~/Administrator/files/ExcelOut"));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    //---------------------------------------Delete ---------------------------------------------------
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)(GridView1.Rows[e.RowIndex].FindControl("Lblid"))).Text;
        dm.Id_Factor = decimal.Parse(id);
        ac.Delete(dm);
        GridView1.EditIndex = -1;
        FillGrid();


    }
    //----------------------------------------------------------------------------------------------

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

}
