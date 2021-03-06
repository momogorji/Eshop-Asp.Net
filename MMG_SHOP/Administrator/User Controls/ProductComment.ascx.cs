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
    Product_Comment ac = new Product_Comment();
    Product_CommentDatum dm = new Product_CommentDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("ProductComment", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid();
        }
    }

    private void FillGrid()
    {
        dm.Id_Product = 0;

        if (Request.QueryString["ID_Product"] != null)
        {
            dm.Id_Product = decimal.Parse(Request.QueryString["ID_Product"].ToString());
        }
        GridView1.DataSource = ac.select_all(dm);
        GridView1.DataBind();
    }

    //---------------------------------------Delete ---------------------------------------------------
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)(GridView1.Rows[e.RowIndex].FindControl("Lblid"))).Text;
        dm.Id = decimal.Parse(id);
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
    protected void Button2_Click(object sender, EventArgs e)
    {
        int i = 0;
        for (i = 0; i < GridView1.Rows.Count; i++)
        {
            dm.Id = decimal.Parse(((Label)(GridView1.Rows[i].FindControl("Lblid"))).Text);
            dm.Show_Comment = ((CheckBox)(GridView1.Rows[i].FindControl("CheckBox1"))).Checked.ToString();

            ac.Update_product_comment_show(dm);
        }
        GridView1.EditIndex = -1;
        FillGrid();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        FillGrid();
    }
}
