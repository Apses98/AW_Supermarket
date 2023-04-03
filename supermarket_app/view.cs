namespace supermarket_app
{
    public partial class mainForm : Form
    {
        Controller controller;

        public mainForm()
        {
            InitializeComponent();
            controller = new Controller(this);
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            productTypeComboBox.SelectedIndex= 0;
            updateDataGridView();
        }

        private void productTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTextboxesEnabledStatus(productTypeComboBox.SelectedIndex);
            
        }

        // Updates the Enabled Status of the textboxes in the inventory tab
        private void updateTextboxesEnabledStatus(int selectedIndex)
        {
            if (selectedIndex == 0)
            {
                productIDtextBox.Enabled= true;
                nametextBox.Enabled     = true;
                pricetextBox.Enabled    = true;
                authortextBox.Enabled   = true;
                genretextBox.Enabled    = true;
                formattextBox.Enabled   = true;
                languagetextBox.Enabled = true;
                platformtextBox.Enabled = false;
                playtimetextBox.Enabled = false;
                platformtextBox.Text = "";
                playtimetextBox.Text = "";
            }
            else if (selectedIndex == 1)
            {
                productIDtextBox.Enabled= true;
                nametextBox.Enabled     = true;
                pricetextBox.Enabled    = true;
                authortextBox.Enabled   = false;
                genretextBox.Enabled    = false;
                authortextBox.Text = "";
                genretextBox.Text = "";
                formattextBox.Enabled   = true;
                languagetextBox.Enabled = false;
                platformtextBox.Enabled = false;
                languagetextBox.Text = "";
                platformtextBox.Text = "";
                playtimetextBox.Enabled = true;
            }
            else if (selectedIndex == 2)
            {
                productIDtextBox.Enabled= true;
                nametextBox.Enabled     = true;
                pricetextBox.Enabled    = true;
                authortextBox.Enabled   = false;
                genretextBox.Enabled    = false;
                formattextBox.Enabled   = false;
                languagetextBox.Enabled = false;
                authortextBox.Text = "";
                genretextBox.Text = "";
                formattextBox.Text = "";
                languagetextBox.Text = "";
                platformtextBox.Enabled = true;
                playtimetextBox.Enabled = false;
                playtimetextBox.Text = "";
            }
        }

        private void updateDataGridView()
        {
            dataGridView1.DataSource = controller.getDataSource();
            dataGridView2.DataSource = controller.getDataSource();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.FormColsing();
        }

        private void addSelectionToCart()
        {
            cartListBox.Items.Add(dataGridView1.SelectedRows[0].Cells[2].Value + "\t" + dataGridView1.SelectedRows[0].Cells[3].Value);
            updateTotalPrice();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            addSelectionToCart();
        }

        private void removeFromCartButton_Click(object sender, EventArgs e)
        {
            cartListBox.Items.Remove(cartListBox.SelectedItem);
            updateTotalPrice();
        }

        private void updateTotalPrice()
        {
            int totalPrice = 0;
            if (cartListBox.Items.Count != 0)
            {
                foreach (string product in cartListBox.Items)
                {
                    totalPrice += int.Parse(product.Split('\t')[1]);
                }
                TotalLabel.Text = "Total: " + totalPrice.ToString();
            }
            else
            {
                TotalLabel.Text = "Total: 0"; 
            }
            
        }

        private void addProductButton_Click(object sender, EventArgs e)
        {
            if (!controller.addProductButtonPressed(
                productIDtextBox.Text,
                nametextBox.Text,
                pricetextBox.Text,
                authortextBox.Text,
                genretextBox.Text,
                formattextBox.Text,
                languagetextBox.Text,
                platformtextBox.Text,
                playtimetextBox.Text,
                inStockTextBox.Text,
                productTypeComboBox.SelectedItem.ToString()
                ))
            {
                MessageBox.Show("Error!\nCheck your inputs!!\nMake sure that your product Name and Price are not empty!\nAnd productID is not already in use.");
            }
            else
            {
                clearTextBoxes();
            }
            
        }

        private void clearTextBoxes()
        {
            productIDtextBox.Text  = string.Empty;
            pricetextBox.Text = string.Empty;
            authortextBox.Text = string.Empty;
            nametextBox.Text = string.Empty;
            genretextBox.Text = string.Empty;
            formattextBox.Text = string.Empty;
            languagetextBox.Text = string.Empty;
            platformtextBox.Text = string.Empty;
            playtimetextBox.Text = string.Empty;

        }

        private void deleteProductButton_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                controller.deleteProductButtonPressed(dataGridView2.SelectedRows);
            }
            else
            {
                MessageBox.Show("Please select a product to delete!");
            }
            
            
        }
    }
}