using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HastaneYonetimSistemi.DataAccess;
using Npgsql;
namespace HastaneYonetimSistemi
{
    public partial class Form1 : Form
    {
        private DBConnection dbConnection = new DBConnection(); // DBConnection sınıfından bir nesne oluştur
        public Form1()
        {
            InitializeComponent();
        }
        private void btnKullaniciEkle_Click(object sender, EventArgs e)
        {
            string adSoyad = txtAdSoyad.Text;
            string telefon = txtTelefon.Text;
            string sifre = txtSifre.Text;
            string rol = cmbRol.SelectedItem.ToString();
            string ekBilgi1 = txtEkBilgi1.Text; // Maaş veya Doğum Tarihi
            string ekBilgi2 = "";

            // Ek Bilgi 2 alanını kontrol et
            if (rol == "Hasta")
            {
                ekBilgi2 = txtEkBilgi2.Text; // Hasta için manuel giriş
            }
            else
            {
                ekBilgi2 = cmbEkBilgi2.SelectedItem.ToString().Split('-')[0].Trim(); // ID kısmını al
            }

            try
            {
                using (var db = new DBConnection())
                {
                    using (var conn = db.Connect())
                    {
                        string query = "SELECT hastane.yenikullaniciekle(@adsoyad, @telefon, @sifre, @rol, @ekbilgi1, @ekbilgi2)";
                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@adsoyad", adSoyad);
                            cmd.Parameters.AddWithValue("@telefon", telefon);
                            cmd.Parameters.AddWithValue("@sifre", sifre);
                            cmd.Parameters.AddWithValue("@rol", rol);
                            cmd.Parameters.AddWithValue("@ekbilgi1", ekBilgi1);
                            cmd.Parameters.AddWithValue("@ekbilgi2", ekBilgi2);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Kullanıcı başarıyla eklendi!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            LaboratuvarTestleriniGetir();

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cmbRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string secilenRol = cmbRol.SelectedItem?.ToString().Trim().ToLower();

           
            string query = ""; // query'yi metodun başında tanımla
            cmbEkBilgi2.Items.Clear();
            txtEkBilgi2.Visible = false;
            cmbEkBilgi2.Visible = true;

            // Label'ları güncelle
            switch (secilenRol)
            {
                case "yonetici":
                    lblEkBilgi1.Text = "Maaş:";
                    lblEkBilgi2.Text = "Departman Seçiniz:";
                    query = "SELECT departmanid, departmanadi FROM hastane.departmanlar";
                    break;

                case "doktor":
                    lblEkBilgi1.Text = "Maaş:";
                    lblEkBilgi2.Text = "Uzmanlık Alanı Seçiniz:";
                    query = "SELECT uzmanlikalanid, adi FROM hastane.uzmanlikalanlari";
                    break;

                case "hemşire":
                    lblEkBilgi1.Text = "Maaş:";
                    lblEkBilgi2.Text = "Servis Seçiniz:";
                    query = "SELECT servisid, servisadi FROM hastane.servisler";

                    break;

                case "hasta":
                    lblEkBilgi1.Text = "Doğum Tarihi (YYYY-MM-DD):";
                    lblEkBilgi2.Text = "Adres:";
                    txtEkBilgi2.Visible = true; // TextBox göster
                    cmbEkBilgi2.Visible = false; // ComboBox gizle
                    return;

                default:
                    lblEkBilgi1.Text = "Ek Bilgi 1:";
                    lblEkBilgi2.Text = "Ek Bilgi 2:";
                    query = "";
                    return;
            }

            // SQL sorgusunu çalıştır
            try
            {
                using (var db = new DBConnection()) // Veritabanı bağlantısı
                using (var conn = db.Connect())     // Bağlantıyı aç

                using (var cmd = new NpgsqlCommand(query, conn)) // Sorgu komutu
                using (var reader = cmd.ExecuteReader()) // Sonuçları okuyucu
                {
                    cmbEkBilgi2.Items.Clear(); // ComboBox'ı temizle

                    
                    while (reader.Read()) // Veri okuma döngüsü
                    {
                        string servis = $"{reader[0]} - {reader[1]}";
                        
                        cmbEkBilgi2.Items.Add(servis); // ComboBox'a ekle
                       
                    }

                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private void KullaniciAra(string aramaMetni)
        {
            try
            {
                using (var db = new DBConnection()) // DBConnection sınıfı
                {
                    using (var connection = db.Connect())
                    {
                        // Kullanıcıları sorgularken silinen hastaları dahil etme
                        string query = @"
                    SELECT k.KullaniciID, k.AdSoyad, k.Telefon, k.Rol 
                    FROM hastane.kullanıcılar k
                    WHERE (k.AdSoyad ILIKE @aramaMetni OR k.Rol ILIKE @aramaMetni)
                    AND k.KullaniciID NOT IN (SELECT KullaniciID FROM Hastalar_Log);";

                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@aramaMetni", "%" + aramaMetni + "%");

                            using (var da = new NpgsqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("Böyle bir kullanıcı bulunmamaktadır.");
                                    return;
                                }

                                dgvKullanicilar.DataSource = dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }


        private void btnKullaniciAra_Click(object sender, EventArgs e)
        {
            string aramaMetni = txtAramaMetni.Text;

            if (string.IsNullOrWhiteSpace(aramaMetni))
            {
                MessageBox.Show("Lütfen bir arama metni giriniz.");
                return;
            }

            try
            {
                using (var db = new DBConnection()) // DBConnection sınıfı
                {
                    using (var connection = db.Connect())
                    {
                        // Fonksiyonu şemasıyla birlikte çağırıyoruz
                        using (var cmd = new NpgsqlCommand("SELECT * FROM hastane.kullaniciara(@aramaMetni)", connection))
                        {
                            cmd.Parameters.AddWithValue("@aramaMetni", aramaMetni);

                            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("Böyle bir kullanıcı bulunmamaktadır.");
                                    return;
                                }

                                dgvKullanicilar.DataSource = dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }


        private void btnReceteEkle_Click(object sender, EventArgs e)
        {
            try
            {
                int hastaID = int.Parse(txtHastaID.Text);
                int doktorID = int.Parse(txtDoktorID.Text);
                int ilacID = int.Parse(txtIlacID.Text);
                int miktar = int.Parse(txtMiktar.Text);

                using (var db = new DBConnection())
                {
                    using (var connection = db.Connect())
                    {
                        // Saklı yordamı çağır
                        using (var cmd = new NpgsqlCommand("SELECT HastaReceteEkle(@p_HastaID, @p_DoktorID, @p_IlacID, @p_Miktar)", connection))
                        {
                            cmd.Parameters.AddWithValue("@p_HastaID", hastaID);
                            cmd.Parameters.AddWithValue("@p_DoktorID", doktorID);
                            cmd.Parameters.AddWithValue("@p_IlacID", ilacID);
                            cmd.Parameters.AddWithValue("@p_Miktar", miktar);

                            cmd.ExecuteNonQuery(); // Yordamı çalıştır
                            MessageBox.Show("Reçete başarıyla eklendi!");
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen tüm alanları doğru formatta doldurunuz (örneğin, ID'ler sayı olmalıdır).");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textHastaID.Text, out int hastaID))
            {
                MessageBox.Show("Lütfen geçerli bir HastaID giriniz.");
                return;
            }

            // 2) Veritabanına bağlan ve HastaReceteGecmisi fonksiyonunu çağır
            using (DBConnection db = new DBConnection())
            {
                using (var conn = db.Connect())
                {
                    // Burada fonksiyonu SELECT ile çağırıyoruz.
                    string query = "SELECT * FROM HastaReceteGecmisi(@p_HastaID);";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("p_HastaID", hastaID);

                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            da.Fill(dt);

                            // 3) Gelen veri DataTable içinde. Bunu DataGridView'e ata
                            dataGridViewReceteGecmisi.DataSource = dt;
                        }
                    }
                }
            }
        }

        
        

        

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btnKullaniciSil_Click(object sender, EventArgs e)
        {
            if (dgvKullanicilar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek bir kullanıcı seçiniz.");
                return;
            }

            int kullaniciID = Convert.ToInt32(dgvKullanicilar.SelectedRows[0].Cells["KullaniciID"].Value);

            try
            {
                using (var db = new DBConnection())
                {
                    using (var connection = db.Connect())
                    {
                        string query = "SELECT hastane.kullanicisil(@p_KullaniciID);";




                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@p_KullaniciID", kullaniciID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Kullanıcı başarıyla silindi.");
                btnKullaniciAra_Click(sender, e); // Listeyi yenile
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }

        }


        private void LaboratuvarTestleriniGetir()
        {
            try
            {
                using (var db = new DBConnection())
                using (var conn = db.Connect())
                {
                    string query = @"
                SELECT 
                    testid AS ""Test ID"",
                    hastaid AS ""Hasta ID"",
                    testsonucu AS ""Test Sonucu"",
                    tarih AS ""Tarih""
                FROM hastane.laboratuvartestleri;
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var da = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewLaboratuvarTestleri.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }
        private int GetSelectedTestID()
        {
            if (dataGridViewLaboratuvarTestleri.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(dataGridViewLaboratuvarTestleri.SelectedRows[0].Cells["Test ID"].Value);
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçiniz!");
                return -1; // Geçersiz ID
            }
        }


        private void txtEkBilgi2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTestSonucuGuncelle_Click(object sender, EventArgs e)
        {
            int selectedTestID = GetSelectedTestID();
            if (selectedTestID == -1) return; // Geçersiz ID varsa işlem yapılmaz

            string yeniTestSonucu = txtTestSonucu.Text.Trim();
            if (string.IsNullOrEmpty(yeniTestSonucu))
            {
                MessageBox.Show("Lütfen yeni bir test sonucu giriniz!");
                return;
            }

            try
            {
                using (var db = new DBConnection())
                using (var conn = db.Connect())
                {
                    string query = @"
                UPDATE hastane.laboratuvartestleri
                SET testsonucu = @YeniTestSonucu
                WHERE testid = @TestID;
            ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@YeniTestSonucu", yeniTestSonucu);
                        cmd.Parameters.AddWithValue("@TestID", selectedTestID);

                        int affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Test sonucu başarıyla güncellendi!");
                            LaboratuvarTestleriniGetir(); // Tabloyu güncelle
                        }
                        else
                        {
                            MessageBox.Show("Test sonucu güncellenemedi. Lütfen tekrar deneyin.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
    }

}

