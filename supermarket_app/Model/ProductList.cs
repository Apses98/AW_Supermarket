using System.ComponentModel;

namespace supermarket_app
{
    internal class ProductList
    {
        BindingList<Product> productList;
        BindingSource productlistSource;
        private List<string>? csvFile;
        private const int MIN_PRODUCT_ID = 0, MAX_PRODUCT_ID = 99999;
        public ProductList()
        {
            productList = new BindingList<Product> ();
            productlistSource = new BindingSource();
            productlistSource.DataSource = productList;
            try
            {
                loadCSVFile();
            }
            catch (Exception)
            {
                return;
            }
            
        }

        
        private void loadCSVFile()
        {
            /* loadCSVFile function -> loads a csv file (database.csv) which includes all information about the products in the supermarket.
            * The function loads the info to the product list and checks if a product does not have a product id (may be cased be adding the products manualy to the csv file) it requests a 
            * new unique product ID. 
            * If the file is missing a new file gets created!
            */
            if (File.Exists("database.csv"))
            {
                csvFile = new List<string>();
                csvFile = System.IO.File.ReadAllText("database.csv").Split('\r').ToList();
                csvFile.RemoveAt(csvFile.Count() - 1);
                
                foreach (string line in csvFile)
                {
                    List<string> test = line.Split(',').ToList();
                        productList.Add(new Product { 
                            ProductID   = int.Parse(line.Split(',').ElementAt(0)), 
                            Name        = line.Split(',').ElementAt(1), 
                            Price       = int.Parse(line.Split(',').ElementAt(2)), 
                            Author      = line.Split(',').ElementAt(3),
                            Genre       = line.Split(',').ElementAt(4),
                            Format      = line.Split(',').ElementAt(5),
                            Language    = line.Split(',').ElementAt(6),
                            Platform    = line.Split(',').ElementAt(7),
                            PlayTime    = line.Split(',').ElementAt(8),
                            Quantity    = int.Parse(line.Split(',').ElementAt(9)),
                            sold        = int.Parse(line.Split(',').ElementAt(10)),
                            Type = line.Split(',').Last()
                        });

                    if (line.Split(',').ElementAt(0) == "")
                    {
                        productList.ElementAt(productList.Count).ProductID = generateProductID();
                    }
                    
                }
                
            }
            else
            {
                File.Create("database.csv");
            }
        }

        internal int generateProductID()
        {
            /* This function generates a new random product ID and to make it unique it compares it to all other product id's */
            Random rand = new Random();
            int productID = rand.Next(MIN_PRODUCT_ID, MAX_PRODUCT_ID);
            for (int i = 0; i < productList.Count; i++)
            {
                if (productList.ElementAt(i).ProductID == productID)
                {
                    productID = rand.Next(MIN_PRODUCT_ID, MAX_PRODUCT_ID);
                    i = 0;
                }
            }
            return productID;
        }

        internal object getDataSource()
        {
            // Returns the dataSource 
            return productlistSource;
        }

        internal void saveFile()
        {
            /* Saves all the data in the productList to the file/database (database.csv) */
            string result = "";
            
            foreach (var product in productList)
            {
                result +=

                    product.ProductID.ToString() +
                    ',' +
                    product.Name +
                    ',' +
                    product.Price.ToString() +
                    ',' +
                    product.Author +
                    ',' +
                    product.Genre +
                    ',' +
                    product.Format +
                    ',' +
                    product.Language +
                    ',' +
                    product.Platform +
                    ',' +
                    product.PlayTime +
                    ',' +
                    product.Quantity +
                    ',' +
                    product.sold +
                    ',' +
                    product.Type +
                    
                    '\r';
            }
            
            System.IO.File.WriteAllText("database.csv", result);
            
        }

        internal void addProduct(int productID, string name, int price, string author, string genre, string format, string language, string platform, string playtime, int inventory, string productType)
        {
            /* Adds a new Product to the productList */
            productList.Add(new Product
            {
                ProductID = productID,
                Name = name,
                Price = price,
                Author = author,
                Genre = genre,
                Format = format,
                Language = language,
                Platform = platform,
                PlayTime = playtime,
                Quantity = inventory,
                Type = productType,
                sold = 0
            }); ;
        }

        internal bool isProductIDValid(int productID)
        {
            /* Checks if the product ID is used by another product */
            foreach (var product in productList)
            {
                if (productID == product.ProductID)
                {
                    return false;
                }
            }
            return true;
        }

        internal void deleteProduct(int productID)
        {
            /* Deletes a product from the productList */
            for(int i = 0; i < productList.Count; i++)
            {
                if (productID == productList.ElementAt(i).ProductID)
                {
                    productList.RemoveAt(i);
                }
            }
                

        }

        internal void updateQuantity(object item, string operation)
        {
            /* Edits the Quantity of a product */
            if (operation == "sell")
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    if (productList.ElementAt(i).ProductID == int.Parse(item.ToString().Split('\t')[0]))
                    {
                        productList.ElementAt(i).Quantity -= int.Parse(item.ToString().Split('\t')[2].Split('x')[1]);
                    }
                }
            }
            else if (operation == "return")
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    if (productList.ElementAt(i).ProductID == int.Parse(item.ToString().Split('\t')[0]))
                    {
                        productList.ElementAt(i).Quantity += int.Parse(item.ToString().Split('\t')[2].Split('x')[1]);
                    }
                }
            }
            
        }

        internal int getQuantity(int productID)
        {
            foreach (var product in productList)
            {
                if (product.ProductID == productID)
                {
                    return product.Quantity;
                }
            }
            return 0;
        }

        internal void updateSold(object item, string operation)
        {
            if (operation == "sell")
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    if (productList.ElementAt(i).ProductID == int.Parse(item.ToString().Split('\t')[0]))
                    {
                        productList.ElementAt(i).sold += int.Parse(item.ToString().Split('\t')[2].Split('x')[1]);
                    }
                }
            }
            else if (operation == "return")
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    if (productList.ElementAt(i).ProductID == int.Parse(item.ToString().Split('\t')[0]))
                    {
                        productList.ElementAt(i).sold -= int.Parse(item.ToString().Split('\t')[2].Split('x')[1]);
                    }
                }
            }
        }

        internal int getSold(int productID)
        {
            foreach (var product in productList)
            {
                if (product.ProductID == productID)
                {
                    return product.sold;
                }
            }
            return 0;
        }

    }
}