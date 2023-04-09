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
            try
            {
                if (int.Parse(playtime) < 0)
                {
                    MessageBox.Show("PlayTime can not be negative!");
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("PlayTime textbox should be a number!");
                return false;
            }
            playtime += " min";
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
            productlist.SaveSold();
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
            if (cartListBox.Items.Count == 0)
                 return;

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

        internal string top10AndTotalSales(bool yearChecked, bool isTop10)
        {

            List<string> top10 = new List<string>(), tmp = new List<string>();
            top10 = productlist.getAllSoldProducts();
            int changed = 0, numOfSold = 0,mostSold = 0, mostSold_index = -1, arrSize = 10 ;
            string result = "";
            

            // move relevant data to tmp list
            for (int i = 0; i < top10.Count; i++)
            {
                if (yearChecked)
                {
                    // get all the lines for the current year 
                    if (top10[i].Split(',')[0].Contains(DateTime.Now.Year.ToString() ))
                    {
                        tmp.Add(top10[i]);
                    }
                }
                else
                {
                    // Get all the lines for the currnet month
                    if (top10[i].Split(',')[1].Contains(DateTime.Now.Month.ToString()))
                    {
                        tmp.Add(top10[i]);
                    }
                }
                
            }
            // Clear top10 list
            top10.Clear();

            // check for dubblicates
            for (int i = 0; i < tmp.Count; i++)
            {
                for (int j = 0; j < tmp.Count; j++)
                {
                    if (tmp[i].Split(',')[2] == tmp[j].Split(',')[2] && i != j)
                    {
                        numOfSold += int.Parse(tmp[j].Split(',')[4]);
                        tmp.RemoveAt(j);
                        changed = 1;
                    }
                }

                if (changed == 1 && i < tmp.Count)
                {
                    changed = 0;
                    tmp[i] =    tmp[i].Split(',')[0].ToString() +
                                ',' +
                                tmp[i].Split(',')[1].ToString() +
                                ',' +
                                tmp[i].Split(',')[2].ToString() +
                                ',' +
                                tmp[i].Split(',')[3].ToString() +
                                ',' +
                                numOfSold.ToString();
                    numOfSold = 0;
                }
            }

            if (!isTop10)
            {
                arrSize = tmp.Count;
            }

            // Get the values
            for (int i = 0; i < arrSize; i++)
            {
                for (int j = 0; j < tmp.Count; j++)
                {
                    if (int.Parse(tmp[j].Split(',')[4]) > mostSold)
                    {
                        mostSold = int.Parse(tmp[j].Split(',')[4]);
                        mostSold_index = j;
                    }
                }
                if (mostSold_index != -1)
                {
                    result += "Product ID: " + tmp[mostSold_index].Split(',')[2] + " Name: " + tmp[mostSold_index].Split(',')[3] + " Sold: " +tmp[mostSold_index].Split(',')[4] + "\n";
                    tmp.RemoveAt(mostSold_index);
                    mostSold = 0;
                    mostSold_index = -1;
                }
                
            }
           


            return result;

        }
    }
}