using Aspose.Cells;
using MyShop.Business;
using MyShop.State;
using MyShop.ValueObject.CategoryVO;
using MyShop.ValueObject.ProductVO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Controls.ListViewItem;
using System.Windows;
using System.Windows.Input;

namespace MyShop.Common
{
    static class FunctionHelper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string generalSKU(string name)
        {
            if (name.Length <= 0)
            {
                throw new SystemException("name is required");
            }
            return $"{name.ToUpper()}-{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Millisecond}";
        }

        public static bool ImportFile(string FileName)
        {
            var dialog = new OpenFileDialog();
            try
            {
                var info = new FileInfo(FileName);
                var imagesFolder = info.Directory + "\\images";
                var excelFile = new Workbook(FileName);
                var tabs = excelFile.Worksheets;

                Category category = new Category();
                foreach (var tab in tabs)
                {
                    var row = 3;
                    var categoryVo = new CategoryCreateVO()
                    {
                        Name = tab.Name
                    };


                    var categoryBus = new CategoryBusiness();
                    var productBus = new ProductBusiness();

                    var categoryF = categoryBus.checkNameExist(categoryVo.Name);



                    var productsVO = new List<ProductCreateVO>();

                    var cell = tab.Cells[$"C3"];
                    while (cell.Value != null || cell.StringValue.IsNotEmpty())
                    {
                        var name = tab.Cells[$"D{row}"].StringValue;
                        var price = tab.Cells[$"E{row}"].IntValue;
                        var quantity = tab.Cells[$"F{row}"].IntValue;
                        var description = tab.Cells[$"G{row}"].StringValue;
                        var image = tab.Cells[$"H{row}"].StringValue;

                        var baseFolder = AppDomain.CurrentDomain.BaseDirectory; // Có dấu \ ở cuối

                        // Kiểm tra thư mục Images có tồn tại hay chưa
                        if (!Directory.Exists(baseFolder + "Assert\\image\\"))
                        {
                            Directory.CreateDirectory(baseFolder + "Assert\\image\\");
                        }
                        string newName = "", extension = "";
                        if (File.Exists(imagesFolder + "\\" + image))
                        {
                            extension = image.Substring(image.Length - 4, 4);
                            newName = Guid.NewGuid().ToString();
                            File.Copy(imagesFolder + "\\" + image, baseFolder + "Assert\\image\\" + newName + extension);
                        }
                        else image = "";
                        var productVO = new ProductCreateVO()
                        {
                            name = name,
                            price = price,
                            quantity = quantity,
                            imagePath = newName + extension,
                            description = description


                        };
                        var checkNameProductExist = productBus.checkNameExist(productVO.name);
                        if (checkNameProductExist == null)
                            productsVO.Add(productVO);
                        row++;
                        cell = tab.Cells[$"C{row}"];
                    }
                    if (categoryF != null)
                        category = categoryBus.InsertData(categoryF.ID, productsVO);
                    else
                    {
                        category = categoryBus.InsertData(categoryVo, productsVO);
                        App.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => CategoryState.categoriesState.Add(category)));
                    }
                }
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public static void CallPaginationFilter(ActionPagination action)
        {
            if (ProductState.categoryId > -1)
                FunctionHelper.Pagination(action, ActionProduct.FilterByCategory);
            else
                FunctionHelper.Pagination(action, ActionProduct.GetAll);
        }

        public static bool IsEmpty(this string data)
        {
            bool result = data.Length == 0;
            return result;
        }

        public static bool IsNotEmpty(this string data)
        {
            bool result = data.Length != 0;
            return result;
        }

        public static void LoadingEvent(Action action)
        {
            using (Loading lw = new Loading(action))
            {
                lw.ShowDialog();
            }
        }

        public static string ConvertToSlug(string str)
        {
            str = str.ToLower();
            for (int i = 32; i < 48; i++)
            {

                str = str.Replace(((char)i).ToString(), " ");

            }
            str = str.Replace(".", "-");

            str = str.Replace(" ", "-");

            str = str.Replace(",", "-");

            str = str.Replace(";", "-");

            str = str.Replace(":", "-");

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = str.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static void Pagination(ActionPagination a, ActionProduct b)
        {

            ProductBusiness productBus = new ProductBusiness();
            switch (a)
            {
                case ActionPagination.Previus:
                    ProductState.pagination.next = true;
                    ProductState.pagination.skip -= ProductState.pagination.limit;
                    break;
                case ActionPagination.Next:
                    ProductState.pagination.previus = true;
                    ProductState.pagination.skip += ProductState.pagination.limit;
                    break;
                case ActionPagination.Load:
                    break;
                case ActionPagination.Import:
                    break;
                case ActionPagination.Delete:
                    if (ProductState.pagination.total == ProductState.pagination.skip + 1 && ProductState.pagination.skip > 0)
                    {
                        ProductState.pagination.skip -= ProductState.pagination.limit;
                    }
                    break;
                case ActionPagination.Create:
                    break;
                case ActionPagination.FindProduct:
                    ProductState.pagination.skip = 0;
                    break;
                default: break;
            }
            List<Product> products = null;
            switch (b)
            {
                case ActionProduct.FilterByCategory:
                    var result = productBus.getProductsByCategoryId(ProductState.pagination.skip, ProductState.pagination.limit, ProductState.categoryId);
                    products = result.products;
                    ProductState.pagination.total = result.count;
                    CalPagination();
                    break;
                case ActionProduct.GetAll:
                    result = productBus.getAll(ProductState.pagination.skip, ProductState.pagination.limit);
                    products = result.products;
                    ProductState.pagination.total = result.count;
                    CalPagination();
                    break;
                case ActionProduct.Reload:
                    ProductState.pagination.skip = 0;
                    result = productBus.getAll(ProductState.pagination.skip, ProductState.pagination.limit);
                    products = result.products;
                    ProductState.pagination.total = result.count;
                    CalPagination();
                    break;
                case ActionProduct.FilterByName:
                    result = productBus.getProductsByName(ProductState.pagination.skip, ProductState.pagination.limit, ProductState.name);
                    products = result.products;
                    ProductState.pagination.total = result.count;
                    CalPagination();
                    break;
                case ActionProduct.FilterByCategoryAndName:
                    result = productBus.getProductsByCategoryIdAndName(ProductState.pagination.skip, ProductState.pagination.limit, ProductState.name, ProductState.categoryId);
                    products = result.products;
                    ProductState.pagination.total = result.count;
                    CalPagination();
                    break;
                default:
                    break;
            }
            ProductState.productsState.Clear();
            for (int i = 0; i < products.Count; i++)
            {
                ProductState.productsState.Add(products[i]);
            }
        }

        public static void CalPagination()
        {
            if (ProductState.pagination.skip == 0)
            {
                ProductState.pagination.previus = false;
            }
            if (ProductState.pagination.skip + ProductState.pagination.limit >= ProductState.pagination.total)
            {
                ProductState.pagination.next = false;
            }
            else ProductState.pagination.next = true;
        }

        public static void Filter()
        {
            ProductBusiness productBus = new ProductBusiness();
            ProductState.pagination.skip = 0;
            ProductPaginationBus result;
            List<Product> products;
            if (ProductState.categoryId > 0)
            {
                result = productBus.getProductsByCategoryId(ProductState.pagination.skip, ProductState.pagination.limit, ProductState.categoryId);
            }
            else
            {
                result = productBus.getAll(ProductState.pagination.skip, ProductState.pagination.limit);
            }
            products = result.products;
            ProductState.pagination.total = result.count;

            ProductState.productsState.Clear();
            for (int i = 0; i < products.Count; i++)
            {
                ProductState.productsState.Add(products[i]);
            }
            CalPagination();
        }

        public static byte[] GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            return imageBytes;
        }
    }

    public class RelativeToAbsoluteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageFile = (string)value;
            imageFile = imageFile != null && imageFile.Length > 0 ? imageFile : "noimage.jpg";
            var currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            string urlImage = $"{currentFolder}Assert\\image\\{imageFile}";
            if (!File.Exists(urlImage))
                urlImage = $"{currentFolder}Assert\\image\\noimage.jpg";
            return urlImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImagePathToBase64 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageFile = (string)value;
            imageFile = imageFile != null && imageFile.Length > 0 ? imageFile : "noimage.jpg";
            var regex = new Regex("/\\/gm");

            if (imageFile == "noimage.jpg" || imageFile.IndexOf(':') == -1)
            {
                var currentFolder = AppDomain.CurrentDomain.BaseDirectory;
                imageFile = $"{currentFolder}Assert\\image\\{imageFile}";
                if (!File.Exists(imageFile))
                    imageFile = $"{currentFolder}Assert\\image\\noimage.jpg";
            }

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(FunctionHelper.GetBase64StringForImage(imageFile));
            bi.EndInit();
            return bi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class PercentageConverter : MarkupExtension, IValueConverter
    {
        private static PercentageConverter _instance;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new PercentageConverter());
        }
    }

    public class NumberToCurrencyVND : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var money = (double)value;
            if (money <= 0) return $"{money} VND";
            return $"{money.ToString("#,###", cul)} VND";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertActionStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
            {
                return "Xem đơn hàng";
            }
            if ((int)value == 2)
            {
                return "Chỉnh sửa đơn hàng";
            }
            if ((int)value == 3)
            {
                return "Tạo đơn hàng mới";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OrdinalConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            ListViewItem lvi = value as ListViewItem;
            int ordinal = 0;

            if (lvi != null)
            {
                ListView lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListView;
                ordinal = lv.ItemContainerGenerator.IndexFromContainer(lvi) + 1;
            }

            return ordinal;

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // This converter does not provide conversion back from ordinal position to list view item
            throw new System.InvalidOperationException();
        }
    }


}
