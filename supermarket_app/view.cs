using System.Security.Cryptography.X509Certificates;

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
                quantityTextBox.Enabled = true;
                platformtextBox.Text = "";
                playtimetextBox.Text = "";
                quantityTextBox.Text = "";
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
                quantityTextBox.Enabled = true;
                quantityTextBox.Text = "";
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
                quantityTextBox.Enabled = true;
                quantityTextBox.Text = "";
            }
        }

        private void updateDataGridView()
        {            
            dataGridView1.DataSource = controller.getDataSource();
            dataGridView2.DataSource = controller.getDataSource();
            dataGridView1.Refresh();
            dataGridView2.Refresh();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.FormColsing();
        }

        private void updateCart()
        {
            if (int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString()) <= 0)
            {
                MessageBox.Show("Product out of stock!");
                return;
            }
            int x = 1;
            if (cartListBox.Items.Count == 0)
            {
                cartListBox.Items.Add(dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + '\t' + dataGridView1.SelectedRows[0].Cells[3].Value.ToString() + "\t" + dataGridView1.SelectedRows[0].Cells[4].Value + " x " + '1');
                updateTotalPrice(); 
                return;
            }
            for (int i = 0; i < cartListBox.Items.Count; i++)
            {
                if (cartListBox.Items[i].ToString().Split('\t')[1] == dataGridView1.SelectedRows[0].Cells[3].Value.ToString())
                {
                    x = int.Parse(cartListBox.Items[i].ToString().Split('\t')[2].Split('x')[1].ToString()) + 1;
                    if (x <= int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString()))
                    {
                        cartListBox.Items.Add(dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + '\t' + dataGridView1.SelectedRows[0].Cells[3].Value + "\t" + dataGridView1.SelectedRows[0].Cells[4].Value + " x " + x.ToString());
                        cartListBox.Items.RemoveAt(i);
                    }
                    else
                    {
                        x--;
                        MessageBox.Show($"Can't add more!\nThere is only {x} piece of this product in the inventory!");
                        return;
                    }
                    
                        
                }
                else
                {
                    x = 1;
                }
            }
            if (x == 1)
            {
                cartListBox.Items.Add(dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + '\t' + dataGridView1.SelectedRows[0].Cells[3].Value + "\t" + dataGridView1.SelectedRows[0].Cells[4].Value + " x " + x.ToString());
            }
            
            updateTotalPrice();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            updateCart();
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
                    totalPrice += int.Parse(product.Split('\t')[2].Split("x")[0]) * int.Parse(product.Split('\t')[2].Split("x")[1]);
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
            if (controller.addProductButtonPressed(
                productIDtextBox.Text,
                nametextBox.Text,
                pricetextBox.Text,
                authortextBox.Text,
                genretextBox.Text,
                formattextBox.Text,
                languagetextBox.Text,
                platformtextBox.Text,
                playtimetextBox.Text,
                productTypeComboBox.SelectedItem.ToString(),
                quantityTextBox.Text
                ))
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

        private void addToOrderButton_Click(object sender, EventArgs e)
        {
            updateNewOrderList();
        }

        private void sellButton_Click(object sender, EventArgs e)
        {
            if (cartListBox.Items.Count == 0)
                return;
            controller.sellButtonpressed(cartListBox);
            cartListBox.Items.Clear();
            updateDataGridView();
        }


        private void updateNewOrderList()
        {
            int x = 1, noMatch = 0;
            if (orderListBox.Items.Count == 0)
            {
                for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {
                    orderListBox.Items.Add(dataGridView2.SelectedRows[i].Cells[0].Value.ToString() + '\t' + dataGridView2.SelectedRows[i].Cells[3].Value.ToString() + "\t" + " x " + '1');
                }
                return;
            }
            for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
            {
                
                for (int j = 0; j < orderListBox.Items.Count; j++)
                {
                    x = 1;
                    if (int.Parse(dataGridView2.SelectedRows[i].Cells[0].Value.ToString()) == int.Parse(orderListBox.Items[j].ToString().Split('\t')[0]))
                    {
                        x += int.Parse(orderListBox.Items[j].ToString().Split('\t')[2].Split('x')[1]);
                        orderListBox.Items.Add(dataGridView2.SelectedRows[i].Cells[0].Value.ToString() + '\t' + dataGridView2.SelectedRows[i].Cells[3].Value.ToString() + "\t" + " x " + x.ToString());
                        orderListBox.Items.RemoveAt(j);
                        noMatch = 1;
                        break;
                    }
                }
                if (noMatch == 0)
                {
                    orderListBox.Items.Add(dataGridView2.SelectedRows[i].Cells[0].Value.ToString() + '\t' + dataGridView2.SelectedRows[i].Cells[3].Value.ToString() + "\t" + " x " + '1');
                }
                
            }

        }

        private void RemoveFromOrderButton_Click(object sender, EventArgs e)
        {
            orderListBox.Items.Clear();
        }
    }
}