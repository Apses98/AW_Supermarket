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

        internal bool addProductButtonPressed(string productIDString, string name, string stringPrice, string author, string genre, string format, string language, string platform, string playtime, string productType, string stringQuantity)
        {
            int productID = 0, inventory, price;
            if (productIDString == "")
            {
                productID = productlist.generateProductID();
            }
            else
            {
                try
                {
                    productID = int.Parse(productIDString);

                    if (!productlist.isProductIDValid(productID))
                    {
                        MessageBox.Show("Product ID is already used by another product!\nTry another product ID!");
                        return false;
                    }
                    if (productID < 0)
                    {
                        MessageBox.Show("Product ID can not be negative!");
                        return false;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Product id should be a number!");
                    return false;
                }
            }
                
            
            if (name == "")
            {
                MessageBox.Show("Name can not be empty!");
                return false;
            }
            if (stringPrice == "")
            {
                MessageBox.Show("Price can not be empty!");
                return false;
            }
            if (stringQuantity == "")
            {
                MessageBox.Show("Quantity textbox can not be empty!");
                return false;
            }

            try
            {
                price = int.Parse(stringPrice);
                if (price < 0)
                {
                    MessageBox.Show("Price can not be negative!");
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Price should be a number!");
                return false;
            }

            try
            {
                inventory = int.Parse(stringQuantity);
                if (inventory < 0 )
                {
                    MessageBox.Show("Quantity can not be negative!");
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Quantity textbox should be a number!");
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

        internal void orderNowButtonPressed(ListBox orderListBox)
        {
            foreach (var item in orderListBox.Items)
            {
                productlist.updateQuantity(item, "return");
            }
        }

        internal void sellButtonpressed(ListBox cartListBox)
        {
            foreach (var item in cartListBox.Items)
            {
                productlist.updateQuantity(item, "sell");
            }
        }
    }
}