using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using test1.Models;

namespace test1.Controllers
{
    public class AdminController : Controller
    { 
        public DatabaseDataContext db = new DatabaseDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult tables()
        {
            return View();
        }
        public ActionResult productviews()
        {
            return View();
        }
        public ActionResult dashboard()
        {
            return View();
        }

        //Products
        public ActionResult addproducts()
        {
            return View();
        }
        public ActionResult editproducts()
        {
            return View();
        }
        
        //Categories
        public ActionResult addcategories()
        {
            return View();
        }
        public ActionResult categoryviews()
        {
            return View();
        }
        public ActionResult removecategory()
        {
            RemoveCategory();
            return View();
        }
        public ActionResult editcategories()
        {
            return View();
        }
        //Accounts
        public ActionResult accountviews() 
        { 
            return View();
        }
        public ActionResult editaccounts()
        {
            return View();
        }
        public ActionResult removeaccounts()
        {
            return View();
        }
        public ActionResult addaccounts()
        {
            return View();
        }

        //ACCOUNTS
        public bool IsAccountNameExists(string accountNameToCompare)
        {
            bool usernames = db.Accounts.Any(o => o.UserName == accountNameToCompare);
            return usernames;
        }

        public string Add_acc()
        {
            string ID = Request["a-id"];
            string Username = Request["a-name"];
            Boolean Roles = Convert.ToBoolean(Request["a-roles"]);
            Boolean IsExist = IsAccountNameExists(Username);
            if (!string.IsNullOrEmpty(Username))
            {
                if (IsExist)
                {
                    return "Tài khoản đã tồn tại";
                }
                else
                {
                    try
                    {
                        Account acc = new Account();
                        acc.UserName = Username;
                        acc.IsAdmin = Roles;

                        db.Accounts.InsertOnSubmit(acc);
                        db.SubmitChanges();

                        return "Thêm mới tài khoản thành công";
                    }
                    catch (Exception ex)
                    {
                        return "Thêm mới tài khoản thất bại. Chi tiết lỗi: " + ex.Message;
                    }
                }
            }
            else
            {
                return "Vui lòng điền đầy đủ thông tin";
            }
        }
        public string get_All_acc()
        {
            APIResult_ett<List<Account>> rs = new APIResult_ett<List<Account>>();
            try
            {
                var qr = db.Accounts;
                if (qr.Any())
                {
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Lấy danh sách tài khoản thành công";
                    rs.Data = qr.ToList();
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDesc = "Danh sách tài khoản rỗng";
                    rs.Data = null;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình lấy danh sách tài khoản. Chi tiết lỗi: " + ex.Message;
                rs.Data = null;
            }
            return JsonConvert.SerializeObject(rs);
        }
        //Sua quyen tai khoan
        public string Edit_acc()
        {
            string admin = Request["a-isAdmin"];
            {
                try
                {
                    var qrs = db.Accounts;
                    if (qrs.Any())
                    {
                        Account acc = qrs.SingleOrDefault();
                        acc.IsAdmin = true;
                        db.SubmitChanges();
                        return "Cập nhật thông tin tài khoản thành công";
                    }
                }
                catch (Exception ex)
                {
                    return "Cập nhật thông tin tài khoản thất bại. Chi tiết lỗi: " + ex.Message;
                }
                return "Lỗi";
            }
        }
        public string Remove_acc()
        {
            return "Lỗi";
        }
    //CATEGORY
    public string Add_cate()
        {
            string id = Request["c-id"];
            string catename = Request["c-name"];
            DateTime NOW = DateTime.Now;
            Boolean IsExist = IsCategoryNameExists(catename);
            if (!string.IsNullOrEmpty(catename))
            {
                if (IsExist)
                {
                    return "Danh mục đã tồn tại";
                }
                else
                {
                    try
                    {
                        Category cate = new Category();
                        cate.NameCategory = catename;
                        cate.ShowOnHomePage = true;
                        cate.CreatedOnUtc = NOW;
                        cate.UpdatedOnUtc = NOW;

                        db.Categories.InsertOnSubmit(cate);
                        db.SubmitChanges();

                        return "Thêm mới danh mục thành công";
                    }
                    catch (Exception ex)
                    {
                        return "Thêm mới danh mục thất bại. Chi tiết lỗi: " + ex.Message;
                    }
                }
            }
            else
            {
                return "Mày chơi tao không được đâu";
            }
        }
        public string Edit_cate()
        {
            string id = Request["c-id"];
            string catename = Request["c-name"];
            DateTime NOW = DateTime.Now;
            Boolean IsExist = IsCategoryNameExists(catename);
            int CategoryIntID = int.Parse(id);
            if (!string.IsNullOrEmpty(catename))
            {
                if (IsExist)
                {
                    return "Danh mục đã tồn tại";
                }
                else
                {
                    try
                    {
                        var qrs = db.Categories.Where(o => o.ID == CategoryIntID);
                        if (qrs.Any())
                        {
                            Category cate = qrs.SingleOrDefault();
                            cate.NameCategory = catename;
                            cate.ShowOnHomePage = true;
                            cate.CreatedOnUtc = NOW;
                            cate.UpdatedOnUtc = NOW;

                            db.SubmitChanges();
                            return "Cập nhật thông tin danh mục thành công";
                        }
                    }
                    catch (Exception ex)
                    {
                        return "Cập nhật thông tin danh mục thất bại. Chi tiết lỗi: " + ex.Message;
                    }
                }
            }
            else
            {
                return "Mày chơi tao không được đâu";
            }
            return "Lỗi";
        }
        public void RemoveCategory()
        {
            string id = Request["c-id"];
            int categoryIntID = int.Parse(id);
            var qrs = db.Categories.Where(o => o.ID == categoryIntID);
            if (qrs.Any())
            {
                //xóa (ShowOnHomePage = 0)
                Category sp = qrs.SingleOrDefault();
                sp.ShowOnHomePage = false;
                db.SubmitChanges();
            }
        }
        //CHECK IF CATEGORY NAME ALREADY EXISTS
        public bool IsCategoryNameExists(string categoryNameToCompare)
        {
            bool categoryNames = db.Categories.Any(o => o.NameCategory == categoryNameToCompare);
            return categoryNames;
        }
        //VIEW ALL CATEGORIES
        public string get_All_cate()
        {
            APIResult_ett<List<Category>> rs = new APIResult_ett<List<Category>>();
            try
            {
                var qr = db.Categories.Where(o => o.ShowOnHomePage == true);
                if (qr.Any())
                {
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Lấy danh mục thành công";
                    rs.Data = qr.ToList();
                }
                else
                {
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDesc = "Danh mục rỗng";
                    rs.Data = null;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình lấy danh mục. Chi tiết lỗi: " + ex.Message;
                rs.Data = null;
            }

            return JsonConvert.SerializeObject(rs);
        }
        //PRODUCTS
        public string Add_prd()
        {
            string URL = Request["p-image"];
            string prdname = Request["p-name"];
            string price = Request["p-price"];
            string desc = Request["p-desc"];

            if (!string.IsNullOrEmpty(URL) && !string.IsNullOrEmpty(prdname) && !string.IsNullOrEmpty(price) && !string.IsNullOrEmpty(desc))
            {
                try
                {
                    //trường hợp muốn insert
                    Product prd = new Product();
                    prd.URL = URL;
                    prd.NameProduct = prdname;
                    prd.Price = double.Parse(price);
                    prd.Description = desc;
                    
                    db.Products.InsertOnSubmit(prd);
                    db.SubmitChanges();

                    return "Thêm mới sản phẩm thành công";
                }
                catch (Exception ex)
                {
                    return "Thêm mới sản phẩm thất bại. Chi tiết lỗi: " + ex.Message;
                }
            }
            else
            {
                return "Mày chơi tao không được đâu";
            }
        }
        public string Edit_prd()
        {
            string id = Request["p-id"];
            string URL = Request["p-image"];
            string prdname = Request["p-name"];
            string price = Request["p-price"];
            string desc = Request["p-desc"];
            int productIntID = int.Parse(id);

            if (!string.IsNullOrEmpty(URL) && !string.IsNullOrEmpty(prdname) && !string.IsNullOrEmpty(price) && !string.IsNullOrEmpty(desc))
            {
                try
                {
                    var qrs = db.Products.Where(o => o.ID == productIntID);
                    if (qrs.Any())
                    {
                        //có trả về bản ghi.
                        Product sp = qrs.SingleOrDefault();
                        sp.URL = URL;
                        sp.NameProduct = prdname;
                        sp.Price = double.Parse(price);
                        sp.Description = desc;

                        db.SubmitChanges();

                        return "Cập nhật thông tin sản phẩm thành công";
                    }
                    else
                    {
                        return "Khong tim thay sp";
                    }
                }
                catch (Exception ex)
                {
                    return "Thêm mới sản phẩm thất bại. Chi tiết lỗi: " + ex.Message;
                }

            }
            else
            {
                return "Mày chơi tao không được đâu";
            }
        }

        public string get_All()
        {
            APIResult_ett<List<Product>> rs = new APIResult_ett<List<Product>>();
            try
            {
                //truy vấn db để lấy toàn bộ dữ liệu về ds sản phẩm
                var qr = db.Products;
                if (qr.Any())
                {
                    //có dữ liệu => chính là dssv
                    rs.ErrCode = EnumErrCode.Success;
                    rs.ErrDesc = "Lấy DSSP thành công";
                    rs.Data = qr.ToList();
                }
                else
                {
                    //không có dữ liệu thỏa mãn
                    rs.ErrCode = EnumErrCode.Empty;
                    rs.ErrDesc = "DSSP rỗng";
                    rs.Data = null;
                }
            }
            catch (Exception ex)
            {
                rs.ErrCode = EnumErrCode.Error;
                rs.ErrDesc = "Có lỗi xảy ra trong quá trình lấy về DSSP. Chi tiết lỗi: " + ex.Message;
                rs.Data = null;
            }

            return JsonConvert.SerializeObject(rs);
        }
        public string get_SP_Info()
        {
            string productID = Request["productID"];
            int productIntID = int.Parse(productID);
            try
            {
                var qr = db.Products.Where(o => o.ID == productIntID);
                if (qr.Any())
                {
                    var sp_obj = qr.SingleOrDefault();

                    return JsonConvert.SerializeObject(sp_obj);
                }
                else
                {
                    return "Không tìm thấy SP có ID=" + productID;
                }

            }
            catch (Exception ex)
            {
                return "Lấy thông tin sản phẩm thất bại. Chi tiết lỗi: " + ex.Message;
            }
        }
    }
}