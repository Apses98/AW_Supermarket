using System.ComponentModel.DataAnnotations;

namespace supermarket_app
{
    internal class Controller
    {
        private mainForm mainForm;
        ProductList productlist;
        public Controller(mainForm mainForm)
        {
            this.mainForm = mainForm;
            productlist = new ProductList();
        }

        internal bool addProductButtonPressed(string productIDString, string name, string stringPrice, string author, string genre, string format, string language, string platform, string playtime, string productType, string inStock)
        {
            int productID = 0, inventory, price;
            if (productIDString == "")
                productID = productlist.generateProductID();
            if (name == "" || stringPrice == "")
                return false;

            try
            {
                if (productIDString != "")
                {
                    productID = int.Parse(productIDString);
                    if (!productlist.isProductIDValid(productID))
                        return false;
                }
                
                inventory = int.Parse(inStock);
                price = int.Parse(stringPrice);
            }
            catch (Exception)
            {
                return false;
            }
            
          
            productlist.addProduct(
            productID,
            name,
            price,
            author,
            genre,
            format,
            language,
            platform,
            playtime,
            inventory,
            productType
            );
            
            return true;
        }


        internal void deleteProductButtonPressed(DataGridViewSelectedRowCollection selectedRows)
        {
            int productID;
            foreach (DataGridViewRow row in selectedRows)
            {
                try
                {
                    productID = int.Parse(row.Cells[0].Value.ToString());
                }
                catch (Exception)
                {
                    return;
                }
                productlist.deleteProduct(productID);
            }
        }

        internal void FormColsing()
        {
            productlist.saveFile();
        }

        internal object getDataSource()
        {
            return productlist.getDataSource();
        }
    }
}