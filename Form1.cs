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
        private string spielstandName;
        private System.Windows.Forms.Timer autoClickerTimer;
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
            string query = "INSERT INTO Spielstand (Spielstandname, Cookies, cookiesPerClick, clickPowerCost, autoClicker, autoClickerCost, clickPowerUpgrades, rebirths, rebirthCost) " +
            $"VALUES ({spielstandName}, {cookies}, {cookiesPerClick}, {clickPowerCost}, {autoClickers}, {autoClickerCost}, {clickPowerUpgrades}, {rebirths}, {rebirthCost});"; 
            SqliteCommand cmd = new SqliteCommand(query, connection);
            cmd.ExecuteNonQuery();
            
        }

        private void btnload_Click(object sender, EventArgs e)
        {
            string query = "SELECT * CookieClickerdb.db";
            SqliteCommand cmd = new SqliteCommand(query, connection);
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string cookies = reader["Cookies"].ToString();
                string cookiesPerClick = reader["cookiesPerClick"].ToString();
                string clickPowerCost = reader["clickPowerCost"].ToString();
                string autoClicker = reader["autoClicker"].ToString();
                string autoClickerCost = reader["autoClickerCost"].ToString();
                string clickPowerUpgrades = reader["clickPowerUpgrades"].ToString();
                string rebirths = reader["rebirths"].ToString();
                string rebirthCost = reader["rebirthCost"].ToString();
            }
        }
    }
}

