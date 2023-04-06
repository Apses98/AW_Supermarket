using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        internal void sell_returnButtonPressed(ListBox cartListBox, string operation)
        {
            foreach (var item in cartListBox.Items)
            {
                productlist.updateQuantity(item, operation);
                productlist.updateSold(item, operation);
            }
        }

        internal int getQuantity(object item)
        {
            int productID = int.Parse(item.ToString().Split('\t')[0]);
            return productlist.getQuantity(productID);
        }

        internal object searchFor(string text)
        {
            BindingList<Product> tmpProductList = new BindingList<Product>();
            BindingSource tmpDataSource = new BindingSource();
            tmpDataSource.DataSource = tmpProductList;
            foreach (var product in productlist.getProducts())
            {
                if (product.Name.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.Type.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.Author.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.Genre.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.Format.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.Language.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.Platform.ToLower().Contains(text.ToLower()))
                {
                    tmpProductList.Add(product);
                }
                else if (product.ProductID.ToString().Contains(text))
                {
                    tmpProductList.Add(product);
                }
            }


            return tmpDataSource;
        }

        internal string getTop10()
        {
            List<Product> top10 = new List<Product>(), tmp = new List<Product>();
            tmp = productlist.getProducts();
            int mostSold = 0, mostSold_index = 0;
            string result = "";
            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < tmp.Count; j++)
                {
                    if (tmp[j].sold > mostSold)
                    {
                        mostSold = tmp[j].sold;
                        mostSold_index = j;
                    }
                }
                try
                {
                    top10.Add(tmp.ElementAt(mostSold_index));
                    tmp.RemoveAt(mostSold_index);
                    result += $"{top10[i].Name} \n";
                }
                catch (Exception)
                {
                    
                }

            }
            return result;
        }
    }
}