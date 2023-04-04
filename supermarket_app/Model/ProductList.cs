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
                            InStock   = int.Parse(line.Split(',').ElementAt(9)),
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
                return;
            }
            
        }

        internal int generateProductID()
        {
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
            return productlistSource;
        }

        internal void saveFile()
        {
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
                    product.InStock +
                    ',' +
                    product.Type +
                    
                    '\r';
            }
            
            System.IO.File.WriteAllText("database.csv", result);
            
        }

        internal void addProduct(int productID, string name, int price, string author, string genre, string format, string language, string platform, string playtime, int inventory, string productType)
        {
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
                InStock = inventory,
                Type = productType
            });
        }

        internal bool isProductIDValid(int productID)
        {
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
            for(int i = 0; i < productList.Count; i++)
            {
                if (productID == productList.ElementAt(i).ProductID)
                {
                    productList.RemoveAt(i);
                }
            }
                

        }

        internal void updateInventory(object item)
        {
            for (int i = 0; i < 10; i++)
            {

            }
        }
    }
}