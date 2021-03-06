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
    Product_Picture ac = new Product_Picture();
    Product_PictureDatum dm = new Product_PictureDatum();


    //<ذخيره فايل در مکان فيزيکي>
    public void Save_File(System.Web.UI.WebControls.FileUpload File_Upload, string Address)
    {
        string str = "";
        str = Server.MapPath(Address);
        File_Upload.PostedFile.SaveAs(str);
    }
    //<بررسي مي کند که نام فايل مورد نظر داخل هاست هست اگر باشد نام جديد را انتخاب مي کند و آن را برمي گرداند>
    string check_name_File(string Folder, string File_Name)
    {
        while (System.IO.File.Exists(Server.MapPath(Folder + File_Name)))
            File_Name = 'a' + File_Name;
        return Folder + File_Name;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("ProductPicture", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid();
        }
    }


    private void FillGrid()
    {
        if (Request.QueryString["ID_Product"] != null)
        {
            dm.Id_Product = decimal.Parse(Request.QueryString["ID_Product"].ToString());
        }
        GridView1.DataSource = ac.select(dm);
        GridView1.DataBind();
    }
    private void Cancel()
    {
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Cancel();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string Folder = "~\\Administrator\\files\\ProductPic\\";
        string File_Name = FileUpload1.FileName.ToString().Trim();
        string Address_Full = check_name_File(Folder, File_Name);

        if (Request.QueryString["ID_Product"] != null)
        {
            dm.Id_Product = decimal.Parse(Request.QueryString["ID_Product"].ToString());
        }

        int i = 0;
        //<اضافه کردن مشخصات آدرس فايل>
        if (FileUpload1.HasFile)
        {
            if (FileUpload1.PostedFile.ContentLength < 5120000)
            {
                dm.Pic = Address_Full;
            }
            else
            {
                i++;
                Response.Write("<script>alert('حجم بايد کمتر از 5000 کيلو بايت باشد')</script>");
            }
        }

        dm.Id_Admin = 1;
        ac.Insert(dm);
        if (FileUpload1.HasFile)
        {//<فايل را در فضاي هاست ذخيره مي کند>
            Save_File(FileUpload1, Address_Full);
        }

        FillGrid();
        Cancel();
    }

    //---------------------------------------Delete ---------------------------------------------------
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)(GridView1.Rows[e.RowIndex].FindControl("Lblid"))).Text;
        string File = ((HyperLink)(GridView1.Rows[e.RowIndex].FindControl("HyperLink1"))).NavigateUrl;
        if (System.IO.File.Exists(Server.MapPath(File)))
        {
            System.IO.File.Delete(Server.MapPath(File));
        } 
        dm.Id = decimal.Parse(id);
        ac.Delete(dm);
        Cancel();
        FillGrid();

    }
    //----------------------------------------------------------------------------------------------

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        FillGrid();
        Cancel();
    }
}
