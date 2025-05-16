using System;
using System.Numerics;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Data.Sqlite;

namespace CookieClicker
{
    public partial class Form1 : Form
    {
        private int cookies = 0;
        private int cookiesPerClick = 1;
        private int clickPowerCost = 100;
        private int autoClickers = 0;
        private int autoClickerCost = 500;
        private int clickPowerUpgrades = 0;
        private int rebirths = 0;
        private int rebirthCost = 1500000;
        private System.Windows.Forms.Timer autoClickerTimer;
        private string spielstandName = "";
        private SqliteConnection connection;



        public Form1()
        {
            InitializeComponent();
            SetupAutoClicker();
            UpdateLabels();
            connection = new SqliteConnection("Data Source=..\\..\\..\\CookieCLickerdb.db");
            connection.Open();

        }
        private void SetupAutoClicker()
        {
            autoClickerTimer = new System.Windows.Forms.Timer();
            autoClickerTimer.Interval = 500;
            autoClickerTimer.Tick += (s, e) =>
            {
                cookies += autoClickers;
                UpdateLabels();
            };
            autoClickerTimer.Start();
        }


        private void btnCookie_Click(object sender, EventArgs e)
        {
            cookies += cookiesPerClick;
            UpdateLabels();
        }


        private void btnUpgradeClickpower_Click(object sender, EventArgs e)
        {
            if (cookies >= clickPowerCost)
            {
                cookies -= clickPowerCost;
                if (rebirths > 0)
                {
                    cookiesPerClick += 2 * rebirths;
                }
                else
                {
                    cookiesPerClick += 1;
                }
                clickPowerUpgrades++;
                clickPowerCost += 50;
                UpdateLabels();
            }
        }

        private void btnAutoclicker_Click(object sender, EventArgs e)
        {
            if (cookies >= autoClickerCost)
            {
                cookies -= autoClickerCost;
                autoClickers++;
                autoClickerCost += 250;
                UpdateLabels();
            }
        }

        private void UpdateLabels()
        {
            lblCookies.Text = $"Cookies: {cookies}";
            lblUpgradeCost.Text = $"ClickpowerCost: {clickPowerCost} | AutoclickerCost: {autoClickerCost} | RebirthCost: {rebirthCost} ";
            lblUpgradeCount.Text = $"Clickpower lvl: {clickPowerUpgrades} | Autoclicker lvl: {autoClickers} | Rebirths: {rebirths} ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookies += 1500000;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cookies >= rebirthCost)
            {
                cookies = 0;
                cookiesPerClick = 1;
                clickPowerUpgrades = 0;
                autoClickers = 0;
                clickPowerCost = 100;
                autoClickerCost = 500;
                rebirthCost += 500000;
                rebirths++;
                UpdateLabels();
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            spielstandName = txtSpielstandName.Text;

            if (string.IsNullOrWhiteSpace(spielstandName))
            {
                MessageBox.Show("Bitte gib einen Namen für den Spielstand ein.");
                return;
            }

            string query = @"
        INSERT OR REPLACE INTO Spielstand 
        (Spielstandname, Cookies, cookiesPerClick, clickPowerCost, autoClicker, autoClickerCost, clickPowerUpgrades, rebirths, rebirthCost) 
        VALUES 
        (@Spielstandname, @Cookies, @CookiesPerClick, @ClickPowerCost, @AutoClicker, @AutoClickerCost, @ClickPowerUpgrades, @Rebirths, @RebirthCost);";

            using (SqliteCommand cmd = new SqliteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Spielstandname", spielstandName);
                cmd.Parameters.AddWithValue("@Cookies", cookies);
                cmd.Parameters.AddWithValue("@CookiesPerClick", cookiesPerClick);
                cmd.Parameters.AddWithValue("@ClickPowerCost", clickPowerCost);
                cmd.Parameters.AddWithValue("@AutoClicker", autoClickers);
                cmd.Parameters.AddWithValue("@AutoClickerCost", autoClickerCost);
                cmd.Parameters.AddWithValue("@ClickPowerUpgrades", clickPowerUpgrades);
                cmd.Parameters.AddWithValue("@Rebirths", rebirths);
                cmd.Parameters.AddWithValue("@RebirthCost", rebirthCost);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Spielstand gespeichert!");
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            spielstandName = txtSpielstandName.Text;

            if (string.IsNullOrWhiteSpace(spielstandName))
            {
                MessageBox.Show("Bitte gib den Namen des Spielstands ein.");
                return;
            }

            string query = "SELECT * FROM Spielstand WHERE Spielstandname = @Spielstandname LIMIT 1";
            using (SqliteCommand cmd = new SqliteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Spielstandname", spielstandName);

                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cookies = Convert.ToInt32(reader["Cookies"]);
                        cookiesPerClick = Convert.ToInt32(reader["cookiesPerClick"]);
                        clickPowerCost = Convert.ToInt32(reader["clickPowerCost"]);
                        autoClickers = Convert.ToInt32(reader["autoClicker"]);
                        autoClickerCost = Convert.ToInt32(reader["autoClickerCost"]);
                        clickPowerUpgrades = Convert.ToInt32(reader["clickPowerUpgrades"]);
                        rebirths = Convert.ToInt32(reader["rebirths"]);
                        rebirthCost = Convert.ToInt32(reader["rebirthCost"]);

                        UpdateLabels();
                        MessageBox.Show("Spielstand geladen!");
                    }
                    else
                    {
                        MessageBox.Show("Kein Spielstand mit diesem Namen gefunden.");
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Du kannst deinen Spielstand speichern in dem du in dem Textfeld unten einen Namen für den Spielstand eingibst und dann auf safe drückst." +
                " Um einen alten Spielstand zu laden musst du den Namen des Spielstands wieder in die Textbox eingeben und dann auf load klicken.");
        }

        private void txtSpielstandName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

